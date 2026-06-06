import { clsx, type ClassValue } from 'clsx'
import { twMerge } from 'tailwind-merge'

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

interface RoleLike {
  name?: string | null
  normalizedName?: string | null
}

// Determines whether a user has the Admin role.
// Accepts either an array of role objects (UserResponseDTO.roles) or string roles.
export function checkIsAdmin(roles?: Array<RoleLike | string> | null): boolean {
  if (!roles || roles.length === 0) return false
  return roles.some((role) => {
    const value = typeof role === "string" ? role : role?.normalizedName || role?.name
    return value?.toUpperCase() === "ADMIN"
  })
}
