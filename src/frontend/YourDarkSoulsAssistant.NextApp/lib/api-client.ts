// lib/api-client.ts
import { AuthResponseDTO } from '@/types/dto';

const API_BASE_URL = "/api/user";

// Допоміжні функції для роботи з localStorage
export const tokenStorage = {
    getTokens: () => {
        if (typeof window === "undefined") return null;
        const tokens = localStorage.getItem("auth_tokens");
        return tokens ? JSON.parse(tokens) : null;
    },
    setTokens: (tokens: { accessToken: string; refreshToken: string }) => {
        if (typeof window !== "undefined") {
            localStorage.setItem("auth_tokens", JSON.stringify(tokens));
        }
    },
    clearAuth: () => {
        if (typeof window !== "undefined") {
            localStorage.removeItem("auth_tokens");
            localStorage.removeItem("auth_user");
        }
    }
};

// Функція оновлення токена
let isRefreshing = false;
let refreshSubscribers: ((token: string) => void)[] = [];

const subscribeTokenRefresh = (cb: (token: string) => void) => {
    refreshSubscribers.push(cb);
};

const onRefreshed = (token: string) => {
    refreshSubscribers.forEach((cb) => cb(token));
    refreshSubscribers = [];
};

export async function refreshTokensApi(): Promise<string | null> {
    const tokens = tokenStorage.getTokens();
    if (!tokens?.accessToken || !tokens?.refreshToken) return null;

    try {
        const response = await fetch(`${API_BASE_URL}/Auth/refresh`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                accessToken: tokens.accessToken,
                refreshToken: tokens.refreshToken,
            }),
        });

        if (response.ok) {
            const data: AuthResponseDTO = await response.json();
            if (data.isSuccess && data.accessToken && data.refreshToken) {
                tokenStorage.setTokens({
                    accessToken: data.accessToken,
                    refreshToken: data.refreshToken,
                });
                return data.accessToken;
            }
        }
    } catch (error) {
        console.error("Token refresh failed:", error);
    }

    tokenStorage.clearAuth();
    return null;
}

// Головна обгортка над fetch
export async function apiClient(endpoint: string, options: RequestInit = {}): Promise<Response> {
    const tokens = tokenStorage.getTokens();
    const headers = new Headers(options.headers);

    if (tokens?.accessToken) {
        headers.set("Authorization", `Bearer ${tokens.accessToken}`);
    }

    if (!headers.has("Content-Type") && options.body && !(options.body instanceof FormData)) {
        headers.set("Content-Type", "application/json");
    }

    const config: RequestInit = {
        ...options,
        headers,
    };

    let response = await fetch(`${API_BASE_URL}${endpoint}`, config);

    // Обробка 401 Unauthorized (Expired Token)
    if (response.status === 401 && tokens?.refreshToken) {
        if (!isRefreshing) {
            isRefreshing = true;
            const newToken = await refreshTokensApi();
            isRefreshing = false;

            if (newToken) {
                onRefreshed(newToken);
            } else {
                // Якщо рефреш не вдався, очищаємо дані та повертаємо 401
                return response;
            }
        }

        // Чекаємо, поки токен оновиться іншим запитом, і повторюємо
        return new Promise((resolve) => {
            subscribeTokenRefresh((newToken) => {
                headers.set("Authorization", `Bearer ${newToken}`);
                resolve(fetch(`${API_BASE_URL}${endpoint}`, { ...config, headers }));
            });
        });
    }

    return response;
}