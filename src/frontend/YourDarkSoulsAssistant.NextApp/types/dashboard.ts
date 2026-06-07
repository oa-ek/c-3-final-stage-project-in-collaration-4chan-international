import type { UserResponseDTO } from '@/types/dto/users'
import type { Build, Game } from '@/types/home'

export interface DashboardUserCardProps {
  user: UserResponseDTO
  isAdmin: boolean
}

export interface DashboardResourcesProps {
  onLogout: () => void
}

export interface DashboardGamesProps {
  games: Game[]
  activeGame: string
  setActiveGame: (id: string) => void
}

export interface DashboardBuildsProps {
  builds: Build[]
  onSelectBuild: (build: Build) => void
}