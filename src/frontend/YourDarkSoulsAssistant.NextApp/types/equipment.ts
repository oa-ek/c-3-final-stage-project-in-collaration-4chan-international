export interface ItemData {
  id?: string
  name: string
  type: string
  category?: "weapon" | "armor" | "talisman" | "consumable" | "arrow" | "shield"
  attackType?: string
  fpCost?: string
  weight: string
  image?: string
  attack: {
    physical: string
    magic: string
    fire: string
    lightning: string
    holy: string
    critical: string
  }
  guard: {
    physical: string
    magic: string
    fire: string
    lightning: string
    holy: string
    boost: string
  }
  scaling: {
    str: string
    dex: string
    int: string
    fai: string
    arc: string
  }
  required: {
    str: string
    dex: string
    int: string
    fai: string
    arc: string
  }
  passiveEffects?: string[]
}

export interface CharacterStats {
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

export interface EquipmentSlots {
  weapons: (ItemData | null)[]
  shields: (ItemData | null)[]
  arrows: (ItemData | null)[]
  armor: {
    head: ItemData | null
    chest: ItemData | null
    hands: ItemData | null
    legs: ItemData | null
  }
  talismans: (ItemData | null)[]
  consumables: (ItemData | null)[]
}

export interface BuildData {
  id: string
  name: string
  stats: CharacterStats
  equipment: EquipmentSlots
  createdAt: string
  updatedAt: string
}

export const DEFAULT_STATS: CharacterStats = {
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
}

export const DEFAULT_EQUIPMENT: EquipmentSlots = {
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
}
