"use client"

import {createContext, useContext, useState, useEffect, useCallback, type ReactNode, useMemo} from "react"
import { apiClient, tokenStorage } from "@/lib/api-client"
import { checkIsAdmin } from "@/lib/utils"
import type { LoginRequestDTO, AuthResponseDTO } from "@/types/dto/auth"
import type { UserResponseDTO, UpdateUserRequestDTO, CreateUserRequestDTO } from "@/types/dto/users"

interface AuthTokens {
  accessToken: string;
  refreshToken: string;
}

interface AuthContextType {
  user: UserResponseDTO | null;
  tokens: AuthTokens | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  isAdmin: boolean;
  login: (credentials: LoginRequestDTO) => Promise<{ success: boolean; error?: string }>;
  register: (data: CreateUserRequestDTO) => Promise<{ success: boolean; error?: string }>;
  logout: () => Promise<void>;
  updateProfile: (data: UpdateUserRequestDTO) => Promise<{ success: boolean; error?: string }>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserResponseDTO | null>(null)
  const [tokens, setTokens] = useState<AuthTokens | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  const fetchProfile = useCallback(async (): Promise<UserResponseDTO | null> => {
    try {
      const response = await apiClient("/Account/profile");
      if (response.ok) {
        const userData: UserResponseDTO = await response.json();
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

  useEffect(() => {
    const initAuth = async () => {
      try {
        const storedUser = localStorage.getItem("auth_user");
        const storedTokens = tokenStorage.getTokens();

        if (storedUser && storedTokens) {
          const parsedUser = JSON.parse(storedUser);
          setUser(parsedUser);
          setTokens(storedTokens);

          const freshUser = await fetchProfile();
          if (!freshUser) {
            setUser(null);
            setTokens(null);
            tokenStorage.clearAuth();
          }
        }
      } catch (error) {
        console.error("Критична помилка ініціалізації сесії:", error);
        setUser(null);
        setTokens(null);
        tokenStorage.clearAuth();
      } finally {
        setIsLoading(false);
      }
    };

    void initAuth();
  }, [fetchProfile]);

  const login = async (credentials: LoginRequestDTO) => {
    setIsLoading(true);
    try {
      const response = await fetch("/api/users/Auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(credentials),
      });

      const responseData = await response.json().catch(() => null);

      if (!responseData) {
        setIsLoading(false);
        return { success: false, error: "Помилка сервера. Некоректна відповідь." };
      }

      if (response.status === 400 && responseData.errors) {
        const errorMessages = Object.values(responseData.errors).flat().join(" ");
        setIsLoading(false);
        return { success: false, error: errorMessages };
      }

      if (responseData.isSuccess && responseData.data) {
        const { accessToken, refreshToken } = responseData.data;
        const newTokens = { accessToken, refreshToken };

        setTokens(newTokens);
        tokenStorage.setTokens(newTokens);

        await fetchProfile();
        setIsLoading(false);
        return { success: true };
      } else {
        setIsLoading(false);

        return { success: false, error: responseData.errorMessage || "Помилка авторизації" };
      }
    } catch (error) {
      setIsLoading(false);
      return { success: false, error: "Помилка мережі." };
    }
  };

  const register = async (data: CreateUserRequestDTO) => {
    setIsLoading(true);
    try {
      const response = await fetch("/api/users/Auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });

      const responseData = await response.json().catch(() => null);

      if (!responseData) {
        setIsLoading(false);
        return { success: false, error: "Помилка сервера. Некоректна відповідь." };
      }

      if (response.status === 400 && responseData.errors) {
        const errorMessages = Object.values(responseData.errors).flat().join(" ");
        setIsLoading(false);
        return { success: false, error: errorMessages };
      }

      if (responseData.isSuccess && responseData.data) {
        const { accessToken, refreshToken } = responseData.data;
        const newTokens = { accessToken, refreshToken };

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
      return { success: false, error: "Помилка мережі." };
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

  const updateProfile = async (data: UpdateUserRequestDTO) => {
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

// Загортаємо методи, щоб вони не перестворювались
  const memoizedLogin = useCallback(login, [fetchProfile]);
  const memoizedRegister = useCallback(register, [fetchProfile]);
  const memoizedLogout = useCallback(logout, []);
  const memoizedUpdateProfile = useCallback(updateProfile, [fetchProfile]);

  // Мемоїзуємо весь контекст
  const contextValue = useMemo(() => ({
    user,
    tokens,
    isAuthenticated: !!user && !!tokens,
    isLoading,
    isAdmin: checkIsAdmin(user?.roles),
    login: memoizedLogin,
    register: memoizedRegister,
    logout: memoizedLogout,
    updateProfile: memoizedUpdateProfile,
  }), [user, tokens, isLoading, memoizedLogin, memoizedRegister, memoizedLogout, memoizedUpdateProfile]);

  return (
      <AuthContext.Provider value={contextValue}>
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
