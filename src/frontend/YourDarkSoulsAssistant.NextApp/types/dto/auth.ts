export interface LoginRequestDTO {
    login: string;
    password?: string;
    rememberMe?: boolean;
}

export interface AuthTokensDTO {
    accessToken: string;
    refreshToken: string;
    userName?: string;
    role?: string[];
}

export interface AuthResponseDTO {
    isSuccess: boolean;
    errorMessage?: string | null;
    data?: AuthTokensDTO | null;
}

export interface RefreshTokenRequestDTO {
    accessToken: string;
    refreshToken: string;
}