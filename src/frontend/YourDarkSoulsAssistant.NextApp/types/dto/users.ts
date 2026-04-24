import type { RoleDTO } from './roles';

export interface UserDTO {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    buildCounts: number;
    avatarPath: string;
    joinDate: string;
    roles: RoleDTO[];
    isAdmin: boolean;
    level: number;
    covenant: string;
}

export interface UpdateUserDTO {
    id: string;
    firstName?: string | null;
    lastName?: string | null;
    userName?: string | null;
    email?: string | null;
    avatarPath: string;
    covenant?: string | null;
}

export interface CreateUserDTO {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password?: string;
    confirmPassword?: string;
}

export interface SmallUserDTO {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    isAdmin: boolean;
    roles: string[];
}