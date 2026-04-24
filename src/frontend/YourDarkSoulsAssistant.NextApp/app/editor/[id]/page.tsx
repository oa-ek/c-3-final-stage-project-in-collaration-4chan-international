"use client"

import { useState, useEffect } from "react"
import { useRouter, useParams } from "next/navigation"
import Link from "next/link"
import { EquipmentPanel } from "@/components/equipment-panel"
import { ItemDetailsPanel } from "@/components/item-details-panel"
import { CharacterStatusPanel } from "@/components/character-status-panel"
import { InventoryModal, type InventoryCategory, type ArmorSlot } from "@/components/inventory-modal"
import { Sword, ArrowLeft, Save, Check } from "lucide-react"
import type { 
  ItemData, 
  CharacterStats, 
  EquipmentSlots, 
  BuildData,
  DEFAULT_STATS,
  DEFAULT_EQUIPMENT 
} from "@/types/equipment"
import {getImageUrl} from "@/lib/content-utils";

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

export default function EditorPage() {
  const router = useRouter()
  const params = useParams()
  const buildId = params.id as string

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

  // Load build from localStorage on mount
  useEffect(() => {
    const savedBuild = localStorage.getItem(`build-${buildId}`)
    if (savedBuild) {
      try {
        const parsed: BuildData = JSON.parse(savedBuild)
        setBuildName(parsed.name)
        setStats(parsed.stats)
        setEquipment(parsed.equipment)
      } catch (e) {
        console.error("Failed to load build:", e)
      }
    }
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

  // Save build to localStorage
  const handleSave = async () => {
    setIsSaving(true)
    
    const buildData: BuildData = {
      id: buildId,
      name: buildName,
      stats,
      equipment,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
    }

    // Save to localStorage
    localStorage.setItem(`build-${buildId}`, JSON.stringify(buildData))
    
    // Also save to builds list
    const buildsListStr = localStorage.getItem("builds-list") || "[]"
    const buildsList = JSON.parse(buildsListStr)
    const existingIndex = buildsList.findIndex((b: { id: string }) => b.id === buildId)
    
    if (existingIndex >= 0) {
      buildsList[existingIndex] = { id: buildId, name: buildName, updatedAt: buildData.updatedAt }
    } else {
      buildsList.push({ id: buildId, name: buildName, updatedAt: buildData.updatedAt })
    }
    localStorage.setItem("builds-list", JSON.stringify(buildsList))

    // Simulate save delay
    await new Promise(resolve => setTimeout(resolve, 500))
    
    setIsSaving(false)
    setSaveSuccess(true)
    setTimeout(() => setSaveSuccess(false), 2000)
  }

  // Default icon paths - customize these with your actual paths
  const defaultIcons = {
    weapon: "/icons/weapon-placeholder.png",
    shield: "/icons/shield-placeholder.png",
    arrow: "/icons/arrow-placeholder.png",
    armor: "/icons/armor-placeholder.png",
    talisman: "/icons/talisman-placeholder.png",
    consumable: "/icons/consumable-placeholder.png",
  }

  return (
      <div className="min-h-screen flex-1 flex flex-col relative overflow-hidden bg-cover bg-center bg-no-repeat"
           style={{ backgroundImage: `url('${getImageUrl('editor/wallpaper')}')` }}>
        <div className="absolute inset-0 bg-black/60 z-0"></div>
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
              href="/builds"
              className="px-4 py-2 text-sm text-gray-400 hover:text-[#C89B64] border border-gray-700 hover:border-[#C89B64] transition-all uppercase tracking-wider"
            >
              Back to Builds
            </Link>
          </div>
        </div>
      </header>

      <main className="relative z-10 flex flex-1 gap-8 p-8 min-h-0">
        <div className="flex-1 min-w-[340px] max-w-[400px]">
          <EquipmentPanel
            equipment={equipment}
            onItemHover={setHoveredItem}
            onSlotClick={handleSlotClick}
            selectedSlot={selectedSlot}
            defaultIcons={defaultIcons}
          />
        </div>

        <div className="flex-[2] min-w-[380px]">
          <ItemDetailsPanel item={hoveredItem} />
        </div>

        <div className="flex-1 min-w-[200px] max-w-[280px]">
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
        selectedItem={hoveredItem}
      />
    </div>
  )
}
