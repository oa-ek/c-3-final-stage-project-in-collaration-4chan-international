import type { UserDTO } from './users';

export interface RefreshTokenDTO {
    token?: string | null;
    expires?: string | null; // DateTime
    isRevoked: boolean;
    createdAt?: string | null; // DateTime
    userId?: string | null;
    user?: UserDTO | null;
}