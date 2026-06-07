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

export interface ApiBuildStats {
  level: number
  runesHeld: number
  vigor: number
  mind: number
  endurance: number
  strength: number
  dexterity: number
  intelligence: number
  faith: number
  arcane: number
}

export interface ApiBuild {
  id: string
  name: string
  stats: ApiBuildStats
}

export interface BuildSet {
  id: string
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
  id: string
  game: string
  name: string
  level: number
  stats: BuildStats
  sets: BuildSet[]
}