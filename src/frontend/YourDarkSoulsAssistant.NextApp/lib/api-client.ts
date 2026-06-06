import type { AuthResponseDTO, AuthTokensDTO } from '@/types/dto/auth';

const API_BASE_URL = "/api/users";

export const tokenStorage = {
    getTokens: (): { accessToken: string; refreshToken: string } | null => {
        if (typeof window === "undefined") return null;
        try {
            const tokens = localStorage.getItem("auth_tokens");
            return tokens ? JSON.parse(tokens) : null;
        } catch {
            localStorage.removeItem("auth_tokens");
            return null;
        }
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
    },
};

let isRefreshing = false;
let refreshSubscribers: ((token: string | null) => void)[] = [];

const subscribeTokenRefresh = (cb: (token: string | null) => void) => {
    refreshSubscribers.push(cb);
};

const onRefreshed = (token: string | null) => {
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
            const result: AuthResponseDTO = await response.json();
            const data = result.data as AuthTokensDTO;
            if (result.isSuccess && data?.accessToken && data?.refreshToken) {
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

export async function apiClient(endpoint: string, options: RequestInit = {}): Promise<Response> {
    const tokens = tokenStorage.getTokens();
    const headers = new Headers(options.headers);

    if (tokens?.accessToken) {
        headers.set("Authorization", `Bearer ${tokens.accessToken}`);
    }

    if (!headers.has("Content-Type") && options.body && !(options.body instanceof FormData)) {
        headers.set("Content-Type", "application/json");
    }

    const config: RequestInit = { ...options, headers };

    const response = await fetch(`${API_BASE_URL}${endpoint}`, config);

    if (response.status === 401 && tokens?.refreshToken) {
        if (!isRefreshing) {
            isRefreshing = true;
            let newToken: string | null = null;

            try {
                newToken = await refreshTokensApi();
            } catch (error) {
                console.error("Critical error during token refresh:", error);
            } finally {
                isRefreshing = false;
                onRefreshed(newToken);
            }

            if (newToken) {
                headers.set("Authorization", `Bearer ${newToken}`);
                return fetch(`${API_BASE_URL}${endpoint}`, { ...config, headers });
            }
            return response;
        }

        return new Promise((resolve) => {
            subscribeTokenRefresh((newToken) => {
                if (newToken) {
                    headers.set("Authorization", `Bearer ${newToken}`);
                    resolve(fetch(`${API_BASE_URL}${endpoint}`, { ...config, headers }));
                } else {
                    resolve(response);
                }
            });
        });
    }

    return response;
}
