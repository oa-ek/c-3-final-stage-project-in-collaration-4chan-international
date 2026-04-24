export interface LoginRequestDTO {
    email: string;
    password?: string;
}

export interface RegisterRequestDTO {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password?: string;
    confirmPassword?: string;
}

export interface AuthResponseDTO {
    isSuccess: boolean;
    accessToken?: string | null;
    refreshToken?: string | null;
    errorMessage?: string | null;
    userName?: string | null;
    role?: string | null;
}

export interface RefreshTokenRequestDTO {
    accessToken: string;
    refreshToken: string;
}