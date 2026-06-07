"use client"

import * as React from "react"
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { cn } from "@/lib/utils"
import { ScrollArea } from "@/components/ui/scroll-area"
import type { ItemData } from "@/types/equipment"
import Image from "next/image"
import {
    Sword,
    Shield,
    ArrowUp,
    HardHat,
    Shirt,
    Hand,
    Footprints,
    X
} from "lucide-react"

export type InventoryCategory = "weapons" | "shields" | "arrows" | "armor" | "talismans" | "consumables"
export type ArmorSlot = "head" | "chest" | "hands" | "legs"

interface InventoryModalProps {
    open: boolean
    onClose: () => void
    category: InventoryCategory
    armorSlot?: ArmorSlot
    onSelectItem: (item: ItemData) => void
    selectedItem?: ItemData | null
}

// Extended mock inventory data for different categories
const inventoryData: Record<InventoryCategory, ItemData[]> = {
    weapons: [
        {
            name: "Black Knife+7",
            type: "Dagger",
            attackType: "Slash/Pierce",
            fpCost: "25 ( - )",
            weight: "2.0",
            image: "/black_knife.png",
            attack: { physical: "132 +", magic: "0", fire: "0", lightning: "0", holy: "130 +", critical: "110" },
            guard: { physical: "26.0", magic: "15.0", fire: "15.0", lightning: "15.0", holy: "42.0", boost: "16" },
            scaling: { str: "E", dex: "C", int: "-", fai: "D", arc: "-" },
            required: { str: "8", dex: "12", int: "0", fai: "18", arc: "0" },
            passiveEffects: ["-", "-", "-"],
        },
        {
            name: "Uchigatana+25",
            type: "Katana",
            attackType: "Slash/Pierce",
            weight: "5.5",
            attack: { physical: "245 +", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "45.0", magic: "30.0", fire: "30.0", lightning: "30.0", holy: "30.0", boost: "30" },
            scaling: { str: "D", dex: "B", int: "-", fai: "-", arc: "-" },
            required: { str: "11", dex: "15", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Moonveil+10",
            type: "Katana",
            attackType: "Slash/Pierce",
            weight: "6.5",
            attack: { physical: "198 +", magic: "162 +", fire: "0", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "46.0", magic: "52.0", fire: "31.0", lightning: "31.0", holy: "31.0", boost: "31" },
            scaling: { str: "E", dex: "D", int: "B", fai: "-", arc: "-" },
            required: { str: "12", dex: "18", int: "23", fai: "0", arc: "0" },
        },
        {
            name: "Rivers of Blood+10",
            type: "Katana",
            attackType: "Slash/Pierce",
            weight: "6.5",
            attack: { physical: "200 +", magic: "0", fire: "130 +", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "46.0", magic: "31.0", fire: "52.0", lightning: "31.0", holy: "31.0", boost: "31" },
            scaling: { str: "E", dex: "D", int: "-", fai: "-", arc: "D" },
            required: { str: "12", dex: "18", int: "0", fai: "0", arc: "20" },
            passiveEffects: ["Causes blood loss buildup (73)"],
        },
        {
            name: "Bloodhound's Fang+10",
            type: "Curved Greatsword",
            attackType: "Slash/Pierce",
            weight: "11.5",
            attack: { physical: "292 +", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "55.0", magic: "35.0", fire: "35.0", lightning: "35.0", holy: "35.0", boost: "42" },
            scaling: { str: "C", dex: "B", int: "-", fai: "-", arc: "-" },
            required: { str: "18", dex: "17", int: "0", fai: "0", arc: "0" },
            passiveEffects: ["Causes blood loss buildup (55)"],
        },
        {
            name: "Blasphemous Blade+10",
            type: "Greatsword",
            attackType: "Standard/Pierce",
            weight: "13.5",
            attack: { physical: "224 +", magic: "0", fire: "146 +", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "62.0", magic: "37.0", fire: "57.0", lightning: "37.0", holy: "37.0", boost: "47" },
            scaling: { str: "D", dex: "D", int: "-", fai: "D", arc: "-" },
            required: { str: "22", dex: "15", int: "0", fai: "21", arc: "0" },
        },
    ],
    shields: [
        {
            name: "Brass Shield+15",
            type: "Medium Shield",
            attackType: "Strike",
            weight: "7.0",
            attack: { physical: "112 +", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "100.0", magic: "56.0", fire: "44.0", lightning: "38.0", holy: "44.0", boost: "56" },
            scaling: { str: "D", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "16", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Erdtree Greatshield+10",
            type: "Greatshield",
            attackType: "Strike",
            weight: "18.0",
            attack: { physical: "125 +", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "100.0", magic: "71.0", fire: "63.0", lightning: "56.0", holy: "92.0", boost: "72" },
            scaling: { str: "D", dex: "-", int: "-", fai: "D", arc: "-" },
            required: { str: "30", dex: "0", int: "0", fai: "12", arc: "0" },
        },
        {
            name: "Carian Knight's Shield",
            type: "Medium Shield",
            attackType: "Strike",
            weight: "5.0",
            attack: { physical: "85 +", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "100.0", magic: "73.0", fire: "35.0", lightning: "35.0", holy: "35.0", boost: "50" },
            scaling: { str: "D", dex: "-", int: "D", fai: "-", arc: "-" },
            required: { str: "10", dex: "0", int: "15", fai: "0", arc: "0" },
        },
    ],
    arrows: [
        {
            name: "Arrow",
            type: "Arrow",
            weight: "0.0",
            attack: { physical: "45", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Fire Arrow",
            type: "Arrow",
            weight: "0.0",
            attack: { physical: "15", magic: "0", fire: "75", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Lightning Arrow",
            type: "Arrow",
            weight: "0.0",
            attack: { physical: "15", magic: "0", fire: "0", lightning: "75", holy: "0", critical: "100" },
            guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Bolt",
            type: "Bolt",
            weight: "0.0",
            attack: { physical: "50", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "100" },
            guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
    ],
    armor: [
        // Head
        {
            name: "Black Knife Hood",
            type: "Helm",
            weight: "2.0",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "2.5", magic: "5.0", fire: "4.6", lightning: "4.4", holy: "5.3", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Bull-Goat Helm",
            type: "Helm",
            weight: "11.6",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "7.3", magic: "5.1", fire: "5.8", lightning: "4.7", holy: "5.3", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        // Chest
        {
            name: "Black Knife Armor",
            type: "Chest Armor",
            weight: "6.6",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "10.6", magic: "11.4", fire: "10.8", lightning: "10.1", holy: "12.2", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Veteran's Armor",
            type: "Chest Armor",
            weight: "16.4",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "14.4", magic: "9.9", fire: "11.7", lightning: "9.0", holy: "10.3", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        // Hands
        {
            name: "Black Knife Gauntlets",
            type: "Gauntlets",
            weight: "2.0",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "2.3", magic: "2.8", fire: "2.5", lightning: "2.3", holy: "3.0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Radahn's Gauntlets",
            type: "Gauntlets",
            weight: "5.5",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "3.8", magic: "2.8", fire: "3.0", lightning: "2.5", holy: "2.8", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        // Legs
        {
            name: "Black Knife Greaves",
            type: "Leg Armor",
            weight: "3.9",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "5.7", magic: "6.6", fire: "6.0", lightning: "5.5", holy: "7.0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
        {
            name: "Bull-Goat Greaves",
            type: "Leg Armor",
            weight: "14.8",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "10.3", magic: "7.2", fire: "8.1", lightning: "6.7", holy: "7.5", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
        },
    ],
    talismans: [
        {
            name: "Erdtree's Favor +2",
            type: "Talisman",
            weight: "1.5",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
            passiveEffects: ["Raises max HP by 4%", "Raises max stamina by 9.5%", "Raises equip load by 8%"],
        },
        {
            name: "Dragoncrest Greatshield Talisman",
            type: "Talisman",
            weight: "1.0",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
            passiveEffects: ["Reduces physical damage taken by 20%"],
        },
    ],
    consumables: [
        {
            name: "Flask of Crimson Tears",
            type: "Consumable",
            weight: "0.0",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
            passiveEffects: ["Restores HP"],
        },
        {
            name: "Flask of Cerulean Tears",
            type: "Consumable",
            weight: "0.0",
            attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
            guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
            scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
            required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
            passiveEffects: ["Restores FP"],
        },
    ],
}

const categoryTitles: Record<InventoryCategory, string> = {
    weapons: "Weapons",
    shields: "Shields",
    arrows: "Arrows & Bolts",
    armor: "Armor",
    talismans: "Talismans",
    consumables: "Quick Items",
}

const armorSlotTitles: Record<ArmorSlot, string> = {
    head: "Head Armor",
    chest: "Chest Armor",
    hands: "Gauntlets",
    legs: "Leg Armor",
}

const armorTypeMap: Record<ArmorSlot, string> = {
    head: "Helm",
    chest: "Chest Armor",
    hands: "Gauntlets",
    legs: "Leg Armor",
}

function getCategoryIcon(category: InventoryCategory, armorSlot?: ArmorSlot) {
    if (category === "armor" && armorSlot) {
        switch (armorSlot) {
            case "head": return <HardHat className="w-5 h-5" />
            case "chest": return <Shirt className="w-5 h-5" />
            case "hands": return <Hand className="w-5 h-5" />
            case "legs": return <Footprints className="w-5 h-5" />
        }
    }

    switch (category) {
        case "weapons": return <Sword className="w-5 h-5" />
        case "shields": return <Shield className="w-5 h-5" />
        case "arrows": return <ArrowUp className="w-5 h-5" />
        default: return <Sword className="w-5 h-5" />
    }
}

function InventoryItem({
                           item,
                           isSelected,
                           onClick,
                           onHover
                       }: {
    item: ItemData
    isSelected: boolean
    onClick: () => void
    onHover: (item: ItemData | null) => void
}) {
    return (
        <div
            onClick={onClick}
            onMouseEnter={() => onHover(item)}
            onMouseLeave={() => onHover(null)}
            className={cn(
                "flex items-center gap-3 p-3 cursor-pointer transition-all duration-150",
                "border border-[#3a352c]/50 rounded-md",
                "bg-gradient-to-br from-[#2a2825] via-[#242220] to-[#1e1c1a]",
                "hover:border-[#c4a456]/50 hover:bg-[#2a2825]",
                isSelected && "border-[#c4a456] bg-[#2a2825] shadow-[0_0_8px_rgba(196,164,86,0.2)]"
            )}
        >
            {/* Item Icon */}
            <div className="w-14 h-14 relative flex-shrink-0 bg-[#1a1815] border border-[#3a352c] rounded flex items-center justify-center overflow-hidden">
                {item.image ? (
                    <Image
                        src={item.image}
                        alt={item.name}
                        fill
                        className="object-contain p-1"
                        sizes="56px"
                    />
                ) : (
                    <Sword className="w-7 h-7 text-[#6a6050] rotate-[-45deg]" />
                )}
            </div>

            {/* Item Info */}
            <div className="flex-1 min-w-0">
                <p className="text-[#e8e4dc] text-sm font-serif truncate">{item.name}</p>
                <p className="text-[#8a8070] text-xs truncate">{item.type}</p>
                <div className="flex gap-3 mt-1">
                    {item.attack.physical !== "0" && (
                        <span className="text-[#a09080] text-xs">Phys: {item.attack.physical}</span>
                    )}
                    {item.attack.magic !== "0" && (
                        <span className="text-[#7090c0] text-xs">Mag: {item.attack.magic}</span>
                    )}
                    {item.attack.fire !== "0" && (
                        <span className="text-[#c07050] text-xs">Fire: {item.attack.fire}</span>
                    )}
                </div>
            </div>

            {/* Weight */}
            <div className="text-right flex-shrink-0">
                <p className="text-[#8a8070] text-xs">Wgt</p>
                <p className="text-[#a09080] text-sm">{item.weight}</p>
            </div>
        </div>
    )
}

export function InventoryModal({
                                   open,
                                   onClose,
                                   category,
                                   armorSlot,
                                   onSelectItem,
                                   selectedItem
                               }: InventoryModalProps) {
    const [hoveredItem, setHoveredItem] = React.useState<ItemData | null>(null)

    // Filter items by category and armor slot if applicable
    const items = React.useMemo(() => {
        let categoryItems = inventoryData[category] || []

        if (category === "armor" && armorSlot) {
            const targetType = armorTypeMap[armorSlot]
            categoryItems = categoryItems.filter(item => item.type === targetType)
        }

        return categoryItems
    }, [category, armorSlot])

    const title = category === "armor" && armorSlot
        ? armorSlotTitles[armorSlot]
        : categoryTitles[category]

    const displayItem = hoveredItem || selectedItem

    return (
        <Dialog open={open} onOpenChange={(isOpen) => !isOpen && onClose()}>
            <DialogContent className="max-w-4xl h-[80vh] bg-[#1a1815] border-[#3a352c] p-0 overflow-hidden" showCloseButton={false}>
                {/* Header */}
                <DialogHeader className="px-6 py-4 border-b border-[#3a352c]/50 bg-gradient-to-r from-[#2a2520] to-[#1a1815]">
                    <DialogTitle className="flex items-center gap-3">
                        <span className="text-[#c4a456]">{getCategoryIcon(category, armorSlot)}</span>
                        <span className="text-[#c4a456] text-lg tracking-[0.15em] uppercase font-serif">
              {title}
            </span>
                    </DialogTitle>
                    <button
                        onClick={onClose}
                        className="absolute right-4 top-4 text-[#8a8070] hover:text-[#c4a456] transition-colors"
                    >
                        <X className="w-5 h-5" />
                    </button>
                </DialogHeader>

                <div className="flex flex-1 overflow-hidden">
                    {/* Items List */}
                    <div className="flex-1 border-r border-[#3a352c]/50">
                        <ScrollArea className="h-[calc(80vh-80px)]">
                            <div className="p-4 space-y-2">
                                {/* Unequip option */}
                                <div
                                    onClick={() => {
                                        onClose()
                                    }}
                                    className={cn(
                                        "flex items-center gap-3 p-3 cursor-pointer transition-all duration-150",
                                        "border border-[#3a352c]/50 border-dashed rounded-md",
                                        "bg-[#1a1815]",
                                        "hover:border-[#c4a456]/30 hover:bg-[#222018]"
                                    )}
                                >
                                    <div className="w-14 h-14 border border-[#3a352c] border-dashed rounded flex items-center justify-center">
                                        <X className="w-6 h-6 text-[#5a5040]" />
                                    </div>
                                    <p className="text-[#8a8070] text-sm">Remove Equipment</p>
                                </div>

                                {items.map((item, index) => (
                                    <InventoryItem
                                        key={`${item.name}-${index}`}
                                        item={item}
                                        isSelected={selectedItem?.name === item.name}
                                        onClick={() => onSelectItem(item)}
                                        onHover={setHoveredItem}
                                    />
                                ))}

                                {items.length === 0 && (
                                    <div className="text-center py-12">
                                        <p className="text-[#6a6050] text-sm">No items available</p>
                                    </div>
                                )}
                            </div>
                        </ScrollArea>
                    </div>

                    {/* Item Preview Panel */}
                    <div className="w-[320px] p-4 bg-[#0f0e0c]">
                        {displayItem ? (
                            <div className="space-y-4">
                                {/* Item Image */}
                                <div className="w-full aspect-square bg-[#1a1815] border border-[#3a352c] rounded flex items-center justify-center relative overflow-hidden">
                                    {displayItem.image ? (
                                        <Image
                                            src={displayItem.image}
                                            alt={displayItem.name}
                                            fill
                                            className="object-contain p-4"
                                            sizes="288px"
                                        />
                                    ) : (
                                        <Sword className="w-24 h-24 text-[#4a4540] rotate-[-45deg]" />
                                    )}
                                </div>

                                {/* Item Name & Type */}
                                <div>
                                    <h3 className="text-[#e8e4dc] text-lg font-serif">{displayItem.name}</h3>
                                    <p className="text-[#8a8070] text-sm">{displayItem.type}</p>
                                    {displayItem.attackType && (
                                        <p className="text-[#6a6050] text-xs mt-1">{displayItem.attackType}</p>
                                    )}
                                </div>

                                {/* Stats Preview */}
                                <div className="space-y-1 pt-2 border-t border-[#3a352c]/50">
                                    <div className="flex justify-between text-xs">
                                        <span className="text-[#8a8070]">Weight</span>
                                        <span className="text-[#a09080]">{displayItem.weight}</span>
                                    </div>
                                    {displayItem.attack.physical !== "0" && (
                                        <div className="flex justify-between text-xs">
                                            <span className="text-[#8a8070]">Physical</span>
                                            <span className="text-[#a09080]">{displayItem.attack.physical}</span>
                                        </div>
                                    )}
                                    {displayItem.attack.magic !== "0" && (
                                        <div className="flex justify-between text-xs">
                                            <span className="text-[#7090c0]">Magic</span>
                                            <span className="text-[#7090c0]">{displayItem.attack.magic}</span>
                                        </div>
                                    )}
                                </div>

                                {/* Passive Effects */}
                                {displayItem.passiveEffects && displayItem.passiveEffects[0] !== "-" && (
                                    <div className="pt-2 border-t border-[#3a352c]/50">
                                        <p className="text-[#c4a456] text-xs uppercase tracking-wide mb-1">Effects</p>
                                        {displayItem.passiveEffects.map((effect, i) => (
                                            effect !== "-" && (
                                                <p key={i} className="text-[#8a8070] text-xs">{effect}</p>
                                            )
                                        ))}
                                    </div>
                                )}
                            </div>
                        ) : (
                            <div className="h-full flex items-center justify-center">
                                <p className="text-[#5a5040] text-sm">Hover over an item to preview</p>
                            </div>
                        )}
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}
