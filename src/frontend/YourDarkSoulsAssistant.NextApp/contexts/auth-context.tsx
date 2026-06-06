"use client"

import {
  createContext,
  useContext,
  useState,
  useEffect,
  useCallback,
  useMemo,
  type ReactNode,
} from "react"
import { apiClient, tokenStorage } from "@/lib/api-client"
import { checkIsAdmin } from "@/lib/utils"
import type { LoginRequestDTO, AuthTokensDTO } from "@/types/dto/auth"
import type { UserResponseDTO, UpdateUserRequestDTO, CreateUserRequestDTO } from "@/types/dto/users"

interface AuthTokens {
  accessToken: string
  refreshToken: string
}

interface AuthContextType {
  user: UserResponseDTO | null
  tokens: AuthTokens | null
  isAuthenticated: boolean
  isLoading: boolean
  isAdmin: boolean
  login: (credentials: LoginRequestDTO) => Promise<{ success: boolean; error?: string }>
  register: (data: CreateUserRequestDTO) => Promise<{ success: boolean; error?: string }>
  logout: () => Promise<void>
  updateProfile: (data: UpdateUserRequestDTO) => Promise<{ success: boolean; error?: string }>
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserResponseDTO | null>(null)
  const [tokens, setTokens] = useState<AuthTokens | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  const fetchProfile = useCallback(async (): Promise<UserResponseDTO | null> => {
    try {
      const response = await apiClient("/Account/profile")
      if (response.ok) {
        const userData: UserResponseDTO = await response.json()
        setUser(userData)
        if (typeof window !== "undefined") {
          localStorage.setItem("auth_user", JSON.stringify(userData))
        }
        return userData
      }
    } catch (error) {
      console.error("Failed to fetch profile:", error)
    }
    return null
  }, [])

  useEffect(() => {
    const initAuth = async () => {
      try {
        const storedUser = typeof window !== "undefined" ? localStorage.getItem("auth_user") : null
        const storedTokens = tokenStorage.getTokens()

        if (storedUser && storedTokens) {
          setUser(JSON.parse(storedUser))
          setTokens(storedTokens)

          const freshUser = await fetchProfile()
          if (!freshUser) {
            setUser(null)
            setTokens(null)
            tokenStorage.clearAuth()
          }
        }
      } catch (error) {
        console.error("Session initialization error:", error)
        setUser(null)
        setTokens(null)
        tokenStorage.clearAuth()
      } finally {
        setIsLoading(false)
      }
    }

    void initAuth()
  }, [fetchProfile])

  const login = useCallback(
    async (credentials: LoginRequestDTO) => {
      setIsLoading(true)
      try {
        const response = await fetch("/api/users/Auth/login", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(credentials),
        })

        const responseData = await response.json().catch(() => null)

        if (!responseData) {
          return { success: false, error: "Server error. Invalid response." }
        }

        if (response.status === 400 && responseData.errors) {
          const errorMessages = Object.values(responseData.errors).flat().join(" ")
          return { success: false, error: errorMessages }
        }

        if (responseData.isSuccess && responseData.data) {
          const { accessToken, refreshToken } = responseData.data as AuthTokensDTO
          const newTokens = { accessToken, refreshToken }
          setTokens(newTokens)
          tokenStorage.setTokens(newTokens)
          await fetchProfile()
          return { success: true }
        }

        return { success: false, error: responseData.errorMessage || "Authorization failed" }
      } catch {
        return { success: false, error: "Network error." }
      } finally {
        setIsLoading(false)
      }
    },
    [fetchProfile],
  )

  const register = useCallback(
    async (data: CreateUserRequestDTO) => {
      setIsLoading(true)
      try {
        const response = await fetch("/api/users/Auth/register", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(data),
        })

        const responseData = await response.json().catch(() => null)

        if (!responseData) {
          return { success: false, error: "Server error. Invalid response." }
        }

        if (response.status === 400 && responseData.errors) {
          const errorMessages = Object.values(responseData.errors).flat().join(" ")
          return { success: false, error: errorMessages }
        }

        if (responseData.isSuccess && responseData.data) {
          const { accessToken, refreshToken } = responseData.data as AuthTokensDTO
          const newTokens = { accessToken, refreshToken }
          setTokens(newTokens)
          tokenStorage.setTokens(newTokens)
          await fetchProfile()
          return { success: true }
        }

        return { success: false, error: responseData.errorMessage || "Registration failed" }
      } catch {
        return { success: false, error: "Network error." }
      } finally {
        setIsLoading(false)
      }
    },
    [fetchProfile],
  )

  const logout = useCallback(async () => {
    // 1. Запам'ятовуємо токен до того, як очистити стан
    const tokenToRevoke = tokens?.accessToken;

    // 2. ОДРАЗУ очищаємо стан (синхронно). Це миттєво зробить isAuthenticated = false.
    setUser(null);
    setTokens(null);
    tokenStorage.clearAuth();

    // 3. Відправляємо запит на сервер для інвалідації токена у фоні
    if (tokenToRevoke) {
      try {
        // Використовуємо звичайний fetch, щоб вручну додати щойно видалений зі сховища токен
        await fetch("/api/users/Auth/logout", {
          method: "POST",
          headers: {
            "Authorization": `Bearer ${tokenToRevoke}`
          }
        });
      } catch (error) {
        console.error("Logout API call failed:", error)
      }
    }
  }, [tokens])

  const updateProfile = useCallback(
    async (data: UpdateUserRequestDTO) => {
      try {
        const response = await apiClient("/Account/profile", {
          method: "PUT",
          body: JSON.stringify(data),
        })

        if (response.ok) {
          await fetchProfile()
          return { success: true }
        }

        const errorData = await response.json().catch(() => ({}))
        return { success: false, error: errorData.errorMessage || errorData.message || "Failed to update profile" }
      } catch {
        return { success: false, error: "Network error." }
      }
    },
    [fetchProfile],
  )

  const contextValue = useMemo<AuthContextType>(
    () => ({
      user,
      tokens,
      isAuthenticated: !!user && !!tokens,
      isLoading,
      isAdmin: checkIsAdmin(user?.roles),
      login,
      register,
      logout,
      updateProfile,
    }),
    [user, tokens, isLoading, login, register, logout, updateProfile],
  )

  return <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const context = useContext(AuthContext)
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider")
  }
  return context
}
