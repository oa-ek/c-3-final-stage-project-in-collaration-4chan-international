import { useCallback, useEffect, useMemo, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/contexts/auth-context'
import type { ApiBuild, Build, Game, SupportedGameDto } from '@/types/home'

export function useHomePage() {
  const router = useRouter()
  const { user, isLoading, isAdmin, logout } = useAuth()
  const [games, setGames] = useState<Game[]>([])
  const [builds, setBuilds] = useState<Build[]>([])
  const [isCreatingBuild, setIsCreatingBuild] = useState(false)
  const [activeGame, setActiveGame] = useState('all')
  const [selectedBuild, setSelectedBuild] = useState<Build | null>(null)

  const createEmptyEquipment = () => ({
    weapons: [null, null, null],
    shields: [null, null, null],
    arrows: [null, null, null, null],
    armor: {
      head: null,
      chest: null,
      hands: null,
      legs: null,
    },
    talismans: [null, null, null, null],
    consumables: [null, null, null, null, null, null, null, null, null, null],
  })

  useEffect(() => {
    const fetchBuilds = async () => {
      try {
        const response = await fetch('/api/builds/Builds')
        if (!response.ok) {
          setBuilds([])
          return
        }

        const apiBuilds = (await response.json()) as ApiBuild[]
        const mappedBuilds = apiBuilds.map((build) => ({
          id: build.id,
          game: 'er',
          name: build.name,
          level: build.stats.level,
          stats: {
            vig: build.stats.vigor,
            end: build.stats.endurance,
            str: build.stats.strength,
            dex: build.stats.dexterity,
          },
          sets: [
            {
              id: build.id,
              name: 'Main Setup',
              icon: '',
            },
          ],
        }))

        setBuilds(mappedBuilds)
      } catch {
        setBuilds([])
      }
    }

    fetchBuilds()
  }, [])

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

  const handleCreateBuild = useCallback(async () => {
    if (!user || isCreatingBuild) {
      return
    }

    setIsCreatingBuild(true)
    try {
      const payload = {
        name: 'New Build',
        userId: user.id,
        characterName: user.userName,
        stats: {
          level: 1,
          runesHeld: 0,
          vigor: 10,
          mind: 10,
          endurance: 10,
          strength: 10,
          dexterity: 10,
          intelligence: 10,
          faith: 10,
          arcane: 10,
        },
        equipment: createEmptyEquipment(),
      }

      const response = await fetch('/api/builds/Builds', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      })

      if (!response.ok) {
        return
      }

      const createdBuild = (await response.json()) as ApiBuild
      const nextBuild: Build = {
        id: createdBuild.id,
        game: 'er',
        name: createdBuild.name,
        level: createdBuild.stats.level,
        stats: {
          vig: createdBuild.stats.vigor,
          end: createdBuild.stats.endurance,
          str: createdBuild.stats.strength,
          dex: createdBuild.stats.dexterity,
        },
        sets: [
          {
            id: createdBuild.id,
            name: 'Main Setup',
            icon: '',
          },
        ],
      }

      setBuilds((prev) => [nextBuild, ...prev])
      router.push(`/editor/${createdBuild.id}`)
    } catch {
      // noop
    } finally {
      setIsCreatingBuild(false)
    }
  }, [isCreatingBuild, router, user])

  const filteredBuilds = useMemo(() => (
    activeGame === 'all'
      ? builds
      : builds.filter((build) => build.game === activeGame)
  ), [activeGame, builds])

  return {
    user,
    isLoading,
    isAdmin,
    games,
    activeGame,
    selectedBuild,
    filteredBuilds,
    isCreatingBuild,
    setActiveGame,
    setSelectedBuild,
    handleLogout,
    handleCreateBuild,
  }
}
