"use client"

import { useState, useEffect } from "react"
import { useRouter, useParams } from "next/navigation"
import Link from "next/link"
import { EquipmentPanel } from "@/components/editor/equipment-panel"
import { ItemDetailsPanel } from "@/components/editor/item-details-panel"
import { CharacterStatusPanel } from "@/components/editor/character-status-panel"
import { InventoryModal, type InventoryCategory, type ArmorSlot } from "@/components/editor/inventory-modal"
import { Sword, ArrowLeft, Save, Check } from "lucide-react"
import type { 
  ItemData, 
  CharacterStats, 
  EquipmentSlots
} from "@/types/equipment"
import {getImageUrl} from "@/lib/content-utils";
import { useAuth } from "@/contexts/auth-context"

// Re-export ItemData for backward compatibility
export type { ItemData } from "@/types/equipment"

const INITIAL_STATS: CharacterStats = {
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

const INITIAL_EQUIPMENT: EquipmentSlots = {
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

interface ApiBuildEquipmentIds {
  weapons: (string | null)[]
  shields: (string | null)[]
  arrows: (string | null)[]
  armor: {
    head: string | null
    chest: string | null
    hands: string | null
    legs: string | null
  }
  talismans: (string | null)[]
  consumables: (string | null)[]
}

interface ApiBuild {
  id: string
  name: string
  stats: CharacterStats
  equipment: ApiBuildEquipmentIds
}

const mapItemIds = (
  ids: (string | null)[] | undefined,
  size: number,
  itemsById: Map<string, ItemData>
): (ItemData | null)[] => {
  const mapped = Array.from({ length: size }, (_, index) => {
    const id = ids?.[index]
    return id ? itemsById.get(id) ?? null : null
  })

  return mapped
}

const mapEquipmentFromApi = (sourceEquipment: ApiBuildEquipmentIds | undefined, allItems: ItemData[]): EquipmentSlots => {
  if (!sourceEquipment) {
    return INITIAL_EQUIPMENT
  }

  const itemsById = new Map(allItems.filter((item) => !!item.id).map((item) => [item.id as string, item]))

  return {
    weapons: mapItemIds(sourceEquipment.weapons, INITIAL_EQUIPMENT.weapons.length, itemsById),
    shields: mapItemIds(sourceEquipment.shields, INITIAL_EQUIPMENT.shields.length, itemsById),
    arrows: mapItemIds(sourceEquipment.arrows, INITIAL_EQUIPMENT.arrows.length, itemsById),
    armor: {
      head: sourceEquipment.armor.head ? itemsById.get(sourceEquipment.armor.head) ?? null : null,
      chest: sourceEquipment.armor.chest ? itemsById.get(sourceEquipment.armor.chest) ?? null : null,
      hands: sourceEquipment.armor.hands ? itemsById.get(sourceEquipment.armor.hands) ?? null : null,
      legs: sourceEquipment.armor.legs ? itemsById.get(sourceEquipment.armor.legs) ?? null : null,
    },
    talismans: mapItemIds(sourceEquipment.talismans, INITIAL_EQUIPMENT.talismans.length, itemsById),
    consumables: mapItemIds(sourceEquipment.consumables, INITIAL_EQUIPMENT.consumables.length, itemsById),
  }
}

export default function EditorPage() {
  const router = useRouter()
  const params = useParams()
  const buildId = params.id as string
  const { user } = useAuth()

  // State for the build
  const [buildName, setBuildName] = useState("New Build")
  const [stats, setStats] = useState<CharacterStats>(INITIAL_STATS)
  const [equipment, setEquipment] = useState<EquipmentSlots>(INITIAL_EQUIPMENT)
  const [hoveredItem, setHoveredItem] = useState<ItemData | null>(null)
  const [isSaving, setIsSaving] = useState(false)
  const [saveSuccess, setSaveSuccess] = useState(false)

  // Inventory modal state
  const [inventoryOpen, setInventoryOpen] = useState(false)
  const [inventoryCategory, setInventoryCategory] = useState<InventoryCategory>("weapons")
  const [armorSlot, setArmorSlot] = useState<ArmorSlot | undefined>(undefined)
  const [selectedSlotIndex, setSelectedSlotIndex] = useState<number | undefined>(undefined)
  const [selectedSlot, setSelectedSlot] = useState<{
    category: InventoryCategory
    index?: number
    armorSlot?: ArmorSlot
  } | undefined>(undefined)

  const getItemId = (item: ItemData | null) => item?.id ?? null

  const toEquipmentPayload = (sourceEquipment: EquipmentSlots) => ({
    weapons: sourceEquipment.weapons.map(getItemId),
    shields: sourceEquipment.shields.map(getItemId),
    arrows: sourceEquipment.arrows.map(getItemId),
    armor: {
      head: getItemId(sourceEquipment.armor.head),
      chest: getItemId(sourceEquipment.armor.chest),
      hands: getItemId(sourceEquipment.armor.hands),
      legs: getItemId(sourceEquipment.armor.legs),
    },
    talismans: sourceEquipment.talismans.map(getItemId),
    consumables: sourceEquipment.consumables.map(getItemId),
  })

  // Load build by id
  useEffect(() => {
    if (buildId === "new") {
      return
    }

    const fetchBuild = async () => {
      try {
        const [buildsResponse, itemsResponse] = await Promise.all([
          fetch("/api/builds/Builds"),
          fetch("/api/catalog/GameItems/equipments"),
        ])

        if (!buildsResponse.ok || !itemsResponse.ok) {
          return
        }

        const builds = await buildsResponse.json() as ApiBuild[]
        const allItems = await itemsResponse.json() as ItemData[]
        const currentBuild = builds.find((build) => build.id === buildId)
        if (!currentBuild) {
          return
        }
        
        setBuildName(currentBuild.name)
        setStats(currentBuild.stats)
        setEquipment(mapEquipmentFromApi(currentBuild.equipment, Array.isArray(allItems) ? allItems : []))
      } catch (e) {
        console.error("Failed to load build:", e)
      }
    }

    void fetchBuild()
  }, [buildId])

  // Handle slot click to open inventory
  const handleSlotClick = (category: InventoryCategory, slot?: ArmorSlot, slotIndex?: number) => {
    setInventoryCategory(category)
    setArmorSlot(slot)
    setSelectedSlotIndex(slotIndex)
    setSelectedSlot({ category, index: slotIndex, armorSlot: slot })
    setInventoryOpen(true)
  }

  // Handle item selection from inventory
  const handleSelectItem = (item: ItemData) => {
    const newEquipment = { ...equipment }

    if (inventoryCategory === "weapons" && selectedSlotIndex !== undefined) {
      newEquipment.weapons = [...equipment.weapons]
      newEquipment.weapons[selectedSlotIndex] = item
    } else if (inventoryCategory === "shields" && selectedSlotIndex !== undefined) {
      newEquipment.shields = [...equipment.shields]
      newEquipment.shields[selectedSlotIndex] = item
    } else if (inventoryCategory === "arrows" && selectedSlotIndex !== undefined) {
      newEquipment.arrows = [...equipment.arrows]
      newEquipment.arrows[selectedSlotIndex] = item
    } else if (inventoryCategory === "armor" && armorSlot) {
      newEquipment.armor = { ...equipment.armor }
      newEquipment.armor[armorSlot] = item
    } else if (inventoryCategory === "talismans" && selectedSlotIndex !== undefined) {
      newEquipment.talismans = [...equipment.talismans]
      newEquipment.talismans[selectedSlotIndex] = item
    } else if (inventoryCategory === "consumables" && selectedSlotIndex !== undefined) {
      newEquipment.consumables = [...equipment.consumables]
      newEquipment.consumables[selectedSlotIndex] = item
    }

    setEquipment(newEquipment)
    setHoveredItem(item)
    setInventoryOpen(false)
  }

  const handleRemoveItem = () => {
    const newEquipment = { ...equipment }

    if (inventoryCategory === "weapons" && selectedSlotIndex !== undefined) {
      newEquipment.weapons = [...equipment.weapons]
      newEquipment.weapons[selectedSlotIndex] = null
    } else if (inventoryCategory === "shields" && selectedSlotIndex !== undefined) {
      newEquipment.shields = [...equipment.shields]
      newEquipment.shields[selectedSlotIndex] = null
    } else if (inventoryCategory === "arrows" && selectedSlotIndex !== undefined) {
      newEquipment.arrows = [...equipment.arrows]
      newEquipment.arrows[selectedSlotIndex] = null
    } else if (inventoryCategory === "armor" && armorSlot) {
      newEquipment.armor = { ...equipment.armor }
      newEquipment.armor[armorSlot] = null
    } else if (inventoryCategory === "talismans" && selectedSlotIndex !== undefined) {
      newEquipment.talismans = [...equipment.talismans]
      newEquipment.talismans[selectedSlotIndex] = null
    } else if (inventoryCategory === "consumables" && selectedSlotIndex !== undefined) {
      newEquipment.consumables = [...equipment.consumables]
      newEquipment.consumables[selectedSlotIndex] = null
    }

    setEquipment(newEquipment)
    setHoveredItem(null)
  }

  const handleSave = async () => {
    setIsSaving(true)

    try {
      const payload = {
        name: buildName,
        userId: user?.id ?? null,
        stats,
        equipment: toEquipmentPayload(equipment),
      }

      const response = await fetch(`/api/builds/Builds/${buildId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      })

      if (!response.ok) {
        setIsSaving(false)
        return
      }

      setSaveSuccess(true)
      setTimeout(() => setSaveSuccess(false), 2000)
    } catch (e) {
      console.error("Failed to save build:", e)
    } finally {
      setIsSaving(false)
    }
  }

  return (
    <div className="min-h-screen flex-1 flex flex-col relative overflow-hidden bg-cover bg-center bg-no-repeat"
         style={{ backgroundImage: `url('${getImageUrl('editor/wallpaper')}')` }}
    >
      <div className="absolute inset-0 bg-black/60 z-0" />

      <header className="sticky top-0 z-20 px-6 py-3 border-b border-[#3a352c]/50 bg-[#1a1815]/95 backdrop-blur-sm">
        <div className="flex items-center gap-4">
          <button
            onClick={() => router.back()}
            className="text-gray-400 hover:text-[#c4a456] transition-colors"
          >
            <ArrowLeft className="w-5 h-5" />
          </button>
          <div className="flex items-center gap-3">
            <Sword className="w-5 h-5 text-[#c4a456]" />
            <input
              type="text"
              value={buildName}
              onChange={(e) => setBuildName(e.target.value)}
              className="text-[#c4a456] text-lg tracking-[0.2em] uppercase font-serif bg-transparent border-b border-transparent hover:border-[#3a352c] focus:border-[#c4a456] outline-none"
            />
          </div>
          <div className="ml-auto flex items-center gap-4">
            {/* Save Button */}
            <button
              onClick={handleSave}
              disabled={isSaving}
              className={`
                flex items-center gap-2 px-4 py-2 
                text-sm uppercase tracking-wider
                border transition-all duration-300
                ${saveSuccess 
                  ? "bg-green-900/30 border-green-600 text-green-400" 
                  : "border-[#c4a456] text-[#c4a456] hover:bg-[#c4a456]/10"
                }
                disabled:opacity-50
              `}
            >
              {saveSuccess ? (
                <>
                  <Check className="w-4 h-4" />
                  Saved
                </>
              ) : (
                <>
                  <Save className="w-4 h-4" />
                  {isSaving ? "Saving..." : "Save Build"}
                </>
              )}
            </button>
            <Link
              href="/home"
              className="px-4 py-2 text-sm text-gray-400 hover:text-[#C89B64] border border-gray-700 hover:border-[#C89B64] transition-all uppercase tracking-wider"
            >
              Back to Home
            </Link>
          </div>
        </div>
      </header>

      <main className="relative z-10 flex flex-1 gap-8 p-8 min-h-0">
        <div className="flex-1 min-w-85 max-w-100">
          <EquipmentPanel
            equipment={equipment}
            onItemHover={setHoveredItem}
            onSlotClick={handleSlotClick}
            selectedSlot={selectedSlot}
          />
        </div>

        <div className="flex-2 min-w-95">
          <ItemDetailsPanel item={hoveredItem} />
        </div>

        <div className="flex-1 min-w-50 max-w-70">
          <CharacterStatusPanel 
            stats={stats}
            onStatsChange={setStats}
            editable={true}
          />
        </div>
      </main>

      <InventoryModal
        open={inventoryOpen}
        onClose={() => {
          setInventoryOpen(false)
          setSelectedSlot(undefined)
        }}
        category={inventoryCategory}
        armorSlot={armorSlot}
        onSelectItem={handleSelectItem}
        onRemoveItem={handleRemoveItem}
        selectedItem={hoveredItem}
      />
    </div>
  )
}
