import type { RoleDTO } from './roles';

export interface UserResponseDTO {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    buildCounts: number;
    avatarPath: string;
    joinDate: string;
    roles: RoleDTO[];
    level: number;
    covenant: string;
}

export interface UpdateUserRequestDTO {
    id: string;
    firstName?: string | null;
    lastName?: string | null;
    userName?: string | null;
    email?: string | null;
    avatarPath: string;
    covenant?: string | null;
}

export interface CreateUserRequestDTO {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password?: string;
    confirmPassword?: string;
}

export interface SmallUserResponseDTO {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    roles: string[];
}
