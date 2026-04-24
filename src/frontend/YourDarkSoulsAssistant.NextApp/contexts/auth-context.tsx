"use client"

import { createContext, useContext, useState, useEffect, useCallback, type ReactNode } from "react"
import { apiClient, tokenStorage } from "@/lib/api-client"
import type {
  UserDTO,
  LoginRequestDTO,
  RegisterRequestDTO,
  AuthResponseDTO,
  UpdateUserDTO
} from "@/types/dto"

// Типи для стану токенів
interface AuthTokens {
  accessToken: string;
  refreshToken: string;
}

interface AuthContextType {
  user: UserDTO | null;
  tokens: AuthTokens | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  isAdmin: boolean;
  login: (credentials: LoginRequestDTO) => Promise<{ success: boolean; error?: string }>;
  register: (data: RegisterRequestDTO) => Promise<{ success: boolean; error?: string }>;
  logout: () => Promise<void>;
  updateProfile: (data: UpdateUserDTO) => Promise<{ success: boolean; error?: string }>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserDTO | null>(null)
  const [tokens, setTokens] = useState<AuthTokens | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  // Завантаження профілю через наш новий apiClient
  const fetchProfile = useCallback(async (): Promise<UserDTO | null> => {
    try {
      const response = await apiClient("/Account/profile");
      if (response.ok) {
        const userData: UserDTO = await response.json();
        setUser(userData);
        if (typeof window !== "undefined") {
          localStorage.setItem("auth_user", JSON.stringify(userData));
        }
        return userData;
      }
    } catch (error) {
      console.error("Failed to fetch profile:", error);
    }
    return null;
  }, []);

  // Ініціалізація при завантаженні сторінки
  useEffect(() => {
    const initAuth = async () => {
      const storedUser = localStorage.getItem("auth_user");
      const storedTokens = tokenStorage.getTokens();

      if (storedUser && storedTokens) {
        try {
          setUser(JSON.parse(storedUser));
          setTokens(storedTokens);

          // Асинхронно перевіряємо валідність на бекенді
          const freshUser = await fetchProfile();
          if (!freshUser) {
            setUser(null);
            setTokens(null);
            tokenStorage.clearAuth();
          }
        } catch {
          tokenStorage.clearAuth();
        }
      }
      setIsLoading(false);
    };

    initAuth();
  }, [fetchProfile]);

  const login = async (credentials: LoginRequestDTO) => {
    setIsLoading(true);
    try {
      const response = await fetch("/api/user/Auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(credentials),
      });

      const data: AuthResponseDTO = await response.json();

      if (data.isSuccess && data.accessToken && data.refreshToken) {
        const newTokens = { accessToken: data.accessToken, refreshToken: data.refreshToken };
        setTokens(newTokens);
        tokenStorage.setTokens(newTokens);

        await fetchProfile(); // Підтягуємо повний профіль
        setIsLoading(false);
        return { success: true };
      } else {
        setIsLoading(false);
        return { success: false, error: data.errorMessage || "Неправильний email або пароль" };
      }
    } catch (error) {
      setIsLoading(false);
      return { success: false, error: "Помилка мережі. Спробуйте пізніше." };
    }
  };

  const register = async (data: RegisterRequestDTO) => {
    setIsLoading(true);
    try {
      const response = await fetch("/api/user/Auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });

      const responseData: AuthResponseDTO = await response.json();

      if (responseData.isSuccess && responseData.accessToken && responseData.refreshToken) {
        const newTokens = { accessToken: responseData.accessToken, refreshToken: responseData.refreshToken };
        setTokens(newTokens);
        tokenStorage.setTokens(newTokens);

        await fetchProfile();
        setIsLoading(false);
        return { success: true };
      } else {
        setIsLoading(false);
        return { success: false, error: responseData.errorMessage || "Помилка реєстрації" };
      }
    } catch (error) {
      setIsLoading(false);
      return { success: false, error: "Помилка мережі. Спробуйте пізніше." };
    }
  };

  const logout = async () => {
    if (tokens?.accessToken) {
      try {
        await apiClient("/Auth/logout", { method: "POST" });
      } catch (error) {
        console.error("Logout API call failed:", error);
      }
    }
    setUser(null);
    setTokens(null);
    tokenStorage.clearAuth();
  };

  const updateProfile = async (data: UpdateUserDTO) => {
    try {
      const response = await apiClient("/Account/profile", {
        method: "PUT",
        body: JSON.stringify(data),
      });

      if (response.ok) {
        await fetchProfile();
        return { success: true };
      } else {
        const errorData = await response.json().catch(() => ({}));
        return { success: false, error: errorData.errorMessage || "Не вдалося оновити профіль" };
      }
    } catch (error) {
      return { success: false, error: "Помилка мережі." };
    }
  };

  return (
      <AuthContext.Provider
          value={{
            user,
            tokens,
            isAuthenticated: !!user && !!tokens,
            isLoading,
            isAdmin: user?.isAdmin ?? false, // Використовуємо поле isAdmin з нашого UserDTO
            login,
            register,
            logout,
            updateProfile,
          }}
      >
        {children}
      </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
