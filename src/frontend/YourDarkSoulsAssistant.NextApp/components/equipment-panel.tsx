"use client"

import type { ItemData, EquipmentSlots } from "@/types/equipment"
import { EquipSlot, type SlotType } from "./equip-slot"
import type { InventoryCategory, ArmorSlot } from "./inventory-modal"

interface EquipmentPanelProps {
  equipment: EquipmentSlots
  onItemHover: (item: ItemData | null) => void
  onSlotClick?: (category: InventoryCategory, armorSlot?: ArmorSlot, slotIndex?: number) => void
  selectedSlot?: { category: InventoryCategory; index?: number; armorSlot?: ArmorSlot }
  // Default icon paths for empty slots
  defaultIcons?: {
    weapon?: string
    shield?: string
    arrow?: string
    armor?: string
    talisman?: string
    consumable?: string
  }
}

export function EquipmentPanel({ 
  equipment, 
  onItemHover, 
  onSlotClick,
  selectedSlot,
  defaultIcons = {}
}: EquipmentPanelProps) {
  
  const handleSlotClick = (category: InventoryCategory, slotIndex?: number, armorSlot?: ArmorSlot) => {
    if (onSlotClick) {
      onSlotClick(category, armorSlot, slotIndex)
    }
  }

  const isSelected = (category: InventoryCategory, index?: number, armorSlot?: ArmorSlot) => {
    if (!selectedSlot) return false
    if (selectedSlot.category !== category) return false
    if (armorSlot) return selectedSlot.armorSlot === armorSlot
    return selectedSlot.index === index
  }

  // Get the currently selected weapon for the header display
  const getSelectedWeaponName = () => {
    const firstWeapon = equipment.weapons[0]
    if (firstWeapon) return firstWeapon.name
    return "Empty"
  }

  return (
    <div className="flex-shrink-0 flex flex-col">
      {/* Header */}
      <div className="mb-3">
        <p className="text-[#a09888] text-sm tracking-wide">
          Right Hand Armament 1
        </p>
        <p className="text-[#e8e4dc] text-base">
          {getSelectedWeaponName()}
        </p>
      </div>

      {/* Weapons Grid - 5x2 */}
      <div className="grid grid-cols-5 gap-1.5 w-fit">
        {/* Row 1 - Weapons (3) + Arrows (2) */}
        {equipment.weapons.map((item, i) => (
          <EquipSlot
            key={`weapon-${i}`}
            item={item}
            onHover={onItemHover}
            onClick={() => handleSlotClick("weapons", i)}
            slotType="weapon"
            defaultIcon={defaultIcons.weapon}
            selected={isSelected("weapons", i)}
          />
        ))}
        {equipment.arrows.slice(0, 2).map((item, i) => (
          <EquipSlot
            key={`arrow-${i}`}
            item={item}
            onHover={onItemHover}
            onClick={() => handleSlotClick("arrows", i)}
            slotType="arrow"
            defaultIcon={defaultIcons.arrow}
            count={item ? 99 : undefined}
            selected={isSelected("arrows", i)}
          />
        ))}

        {/* Row 2 - Shields (3) + Bolts (2) */}
        {equipment.shields.map((item, i) => (
          <EquipSlot
            key={`shield-${i}`}
            item={item}
            onHover={onItemHover}
            onClick={() => handleSlotClick("shields", i)}
            slotType="shield"
            defaultIcon={defaultIcons.shield}
            selected={isSelected("shields", i)}
          />
        ))}
        {equipment.arrows.slice(2, 4).map((item, i) => (
          <EquipSlot
            key={`bolt-${i}`}
            item={item}
            onHover={onItemHover}
            onClick={() => handleSlotClick("arrows", i + 2)}
            slotType="arrow"
            defaultIcon={defaultIcons.arrow}
            count={item ? 99 : undefined}
            selected={isSelected("arrows", i + 2)}
          />
        ))}
      </div>

      {/* Armor Grid - 4 slots */}
      <div className="grid grid-cols-4 gap-1.5 w-fit mt-3">
        <EquipSlot
          item={equipment.armor.head}
          onHover={onItemHover}
          onClick={() => handleSlotClick("armor", 0, "head")}
          slotType="armor"
          defaultIcon={defaultIcons.armor}
          selected={isSelected("armor", undefined, "head")}
        />
        <EquipSlot
          item={equipment.armor.chest}
          onHover={onItemHover}
          onClick={() => handleSlotClick("armor", 1, "chest")}
          slotType="armor"
          defaultIcon={defaultIcons.armor}
          selected={isSelected("armor", undefined, "chest")}
        />
        <EquipSlot
          item={equipment.armor.hands}
          onHover={onItemHover}
          onClick={() => handleSlotClick("armor", 2, "hands")}
          slotType="armor"
          defaultIcon={defaultIcons.armor}
          selected={isSelected("armor", undefined, "hands")}
        />
        <EquipSlot
          item={equipment.armor.legs}
          onHover={onItemHover}
          onClick={() => handleSlotClick("armor", 3, "legs")}
          slotType="armor"
          defaultIcon={defaultIcons.armor}
          selected={isSelected("armor", undefined, "legs")}
        />
      </div>

      {/* Talismans Grid - 4 slots */}
      <div className="grid grid-cols-4 gap-1.5 w-fit mt-3">
        {equipment.talismans.map((item, i) => (
          <EquipSlot
            key={`talisman-${i}`}
            item={item}
            onHover={onItemHover}
            onClick={() => handleSlotClick("talismans", i)}
            slotType="talisman"
            defaultIcon={defaultIcons.talisman}
            selected={isSelected("talismans", i)}
          />
        ))}
      </div>

      {/* Consumables Grid - 2 rows of 5 */}
      <div className="grid grid-cols-5 gap-1.5 w-fit mt-3">
        {equipment.consumables.map((item, i) => (
          <EquipSlot
            key={`consumable-${i}`}
            item={item}
            onHover={onItemHover}
            onClick={() => handleSlotClick("consumables", i)}
            slotType="consumable"
            defaultIcon={defaultIcons.consumable}
            count={item ? 1 : undefined}
            selected={isSelected("consumables", i)}
          />
        ))}
      </div>
    </div>
  )
}
