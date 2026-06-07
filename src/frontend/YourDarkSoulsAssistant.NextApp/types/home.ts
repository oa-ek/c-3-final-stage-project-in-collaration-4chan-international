export interface Game {
  id: string
  name: string
  icon: string
}

export interface SupportedGameDto {
  id: number
  code: string
  name: string
  iconPath: string
}

export interface BuildSet {
  id: number
  name: string
  icon: string
}

export interface BuildStats {
  vig: number
  end: number
  str: number
  dex: number
}

export interface Build {
  id: number
  game: string
  name: string
  level: number
  image: string
  stats: BuildStats
  sets: BuildSet[]
}

export const buildsData: Build[] = [
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