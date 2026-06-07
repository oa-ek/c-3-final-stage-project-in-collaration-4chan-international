import { useCallback, useEffect, useMemo, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/contexts/auth-context'
import { buildsData } from '@/types/home'
import type { Build, Game, SupportedGameDto } from '@/types/home'

export function useHomePage() {
  const router = useRouter()
  const { user, isLoading, isAdmin, logout } = useAuth()
  const [games, setGames] = useState<Game[]>([])
  const [activeGame, setActiveGame] = useState('all')
  const [selectedBuild, setSelectedBuild] = useState<Build | null>(null)

  useEffect(() => {
    const fetchSupportedGames = async () => {
      try {
        const response = await fetch('/api/catalog/GameItems/supported-games')
        if (!response.ok) {
          return
        }

        const supportedGames = (await response.json()) as SupportedGameDto[]
        const mappedGames = supportedGames.map((game) => ({
          id: game.code,
          name: game.name,
          icon: game.iconPath,
        }))

        setGames(mappedGames)
      } catch {
        setGames([])
      }
    }

    fetchSupportedGames()
  }, [])

  useEffect(() => {
    if (!isLoading && !user) {
      router.push('/')
    }
  }, [isLoading, router, user])

  const handleLogout = useCallback(() => {
    logout()
    router.push('/')
  }, [logout, router])

  const filteredBuilds = useMemo(() => (
    activeGame === 'all'
      ? buildsData
      : buildsData.filter((build) => build.game === activeGame)
  ), [activeGame])

  return {
    user,
    isLoading,
    isAdmin,
    games,
    activeGame,
    selectedBuild,
    filteredBuilds,
    setActiveGame,
    setSelectedBuild,
    handleLogout,
  }
}
