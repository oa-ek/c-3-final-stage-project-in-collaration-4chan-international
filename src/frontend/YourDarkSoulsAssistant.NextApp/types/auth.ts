export type UserRole = "admin" | "user"

export interface User {
  id: string
  email: string
  userName: string
  firstName: string
  lastName: string
  role: UserRole
  avatarPath?: string
  joinDate: string
  isAdmin: boolean
  level: number
  covenant?: string
  buildsCount?: number
}

export interface AuthTokens {
  accessToken: string
  refreshToken: string
}

export interface AuthState {
  user: User | null
  tokens: AuthTokens | null
  isAuthenticated: boolean
  isLoading: boolean
}

export interface LoginCredentials {
  email: string
  password: string
}

export interface RegisterData {
  firstName: string
  lastName: string
  userName: string
  email: string
  password: string
  confirmPassword: string
}

export interface UpdateUserData {
  id?: string
  firstName?: string
  lastName?: string
  userName?: string
  email?: string
  covenant?: string
}

export interface RefreshTokenRequest {
  accessToken: string
  refreshToken: string
}

// API Response types
export interface LoginResponse {
  accessToken: string
  refreshToken: string
  user: User
}

export interface RegisterResponse {
  accessToken: string
  refreshToken: string
  user: User
}
