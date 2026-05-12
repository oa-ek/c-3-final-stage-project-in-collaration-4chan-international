import { clsx, type ClassValue } from 'clsx'
import { twMerge } from 'tailwind-merge'

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function checkIsAdmin(roles?: string[] | { name: string }[] | null): boolean {
  if (!roles || !Array.isArray(roles)) return false;

  return roles.some(role => {

    if (typeof role === 'string') {
      return role.toLowerCase() === 'admin';
    }

    return role?.name?.toLowerCase() === 'admin';
  });
}