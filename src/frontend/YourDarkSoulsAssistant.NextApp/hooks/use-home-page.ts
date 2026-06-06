import { useCallback, useEffect, useMemo, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/contexts/auth-context'

type BuildSet = {
  id: number
  name: string
  icon: string
}

export type Build = {
  id: number
  game: string
  name: string
  level: number
  image: string
  stats: {
    vig: number
    end: number
    str: number
    dex: number
  }
  sets: BuildSet[]
}

export const games = [
  { id: 'all', name: 'All Games', icon: '/game-icons/all.jpg' },
  { id: 'ds1', name: 'Dark Souls 1', icon: '/game-icons/ds1.jpg' },
  { id: 'ds2', name: 'Dark Souls 2', icon: '/game-icons/ds2.jpg' },
  { id: 'ds3', name: 'Dark Souls 3', icon: '/game-icons/ds3.jpg' },
  { id: 'er', name: 'Elden Ring', icon: '/game-icons/er.png' },
]

const buildsData: Build[] = [
  {
    id: 1, game: 'er', name: 'Giant Dad', level: 99,
    image: '/home/giant.webp',
    stats: { vig: 48, end: 66, str: 16, dex: 10 },
    sets: [
      { id: 101, name: 'PVP Invasions', icon: '/sets/invasion.webp' },
      { id: 102, name: 'PVE Boss Rush', icon: '/sets/rush.jpg' },
      { id: 103, name: 'Farming Set', icon: '/sets/bull-goat.png' },
    ],
  },
  {
    id: 2, game: 'er', name: 'Moonveil Samurai', level: 150,
    image: '/home/samurai.webp',
    stats: { vig: 60, end: 30, str: 12, dex: 30 },
    sets: [
      { id: 201, name: 'Glass Cannon', icon: '/sets/radahn-armor.jpg' },
      { id: 202, name: 'Balanced Poise', icon: '/sets/crucible-helm.webp' },
    ],
  },
  {
    id: 3, game: 'er', name: 'Hollow Bleed', level: 120,
    image: '/home/malenia.webp',
    stats: { vig: 40, end: 40, str: 40, dex: 40 },
    sets: [
      { id: 301, name: 'Main Setup', icon: '/sets/blood.png' },
      { id: 302, name: 'Parry God', icon: '/sets/parry.jpg' },
      { id: 303, name: 'Bow Only', icon: '/sets/bow.png' },
    ],
  },
]

export function useHomePage() {
  const router = useRouter()
  const { user, isLoading, isAdmin, logout } = useAuth()
  const [activeGame, setActiveGame] = useState('all')
  const [selectedBuild, setSelectedBuild] = useState<Build | null>(null)

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
    activeGame,
    selectedBuild,
    filteredBuilds,
    setActiveGame,
    setSelectedBuild,
    handleLogout,
  }
}
