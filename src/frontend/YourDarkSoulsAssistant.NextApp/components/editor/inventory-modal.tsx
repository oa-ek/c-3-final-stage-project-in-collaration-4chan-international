"use client"

import * as React from "react"
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { cn } from "@/lib/utils"
import { ScrollArea } from "@/components/ui/scroll-area"
import { getImageUrl } from "@/lib/content-utils"
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
    Loader2,
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
    onRemoveItem?: () => void
    selectedItem?: ItemData | null
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

const mapCategoryToCatalog = (category: InventoryCategory): ItemData["category"][] => {
    switch (category) {
        case "weapons":
            return ["weapon"]
        case "shields":
            return ["shield"]
        case "arrows":
            return ["arrow"]
        case "armor":
            return ["armor"]
        case "talismans":
            return ["talisman"]
        case "consumables":
            return ["consumable"]
        default:
            return []
    }
}

const normalizeArmorType = (type: string) => type.trim().toLowerCase()

const armorSlotAliases: Record<ArmorSlot, string[]> = {
    head: ["helm", "helmet", "hood", "crown", "mask", "head"],
    chest: ["chest", "armor", "robe", "coat", "garb"],
    hands: ["gauntlet", "glove", "bracer", "hand"],
    legs: ["leg", "greave", "boot", "trouser"],
}

const matchesArmorSlot = (itemType: string, slot: ArmorSlot) => {
    const normalized = normalizeArmorType(itemType)
    const exactType = normalizeArmorType(armorTypeMap[slot])

    if (normalized === exactType) {
        return true
    }

    return armorSlotAliases[slot].some(alias => normalized.includes(alias))
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
    const itemVisual = item.image || item.icon
    const itemVisualUrl = itemVisual ? getImageUrl(itemVisual) : null

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
                {itemVisualUrl ? (
                    <Image
                        src={itemVisualUrl}
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
                                   onRemoveItem,
                                   selectedItem
                               }: InventoryModalProps) {
    const [hoveredItem, setHoveredItem] = React.useState<ItemData | null>(null)
    const [allItems, setAllItems] = React.useState<ItemData[]>([])
    const [isLoading, setIsLoading] = React.useState(false)
    const [loadError, setLoadError] = React.useState<string | null>(null)

    React.useEffect(() => {
        if (!open) {
            return
        }

        const controller = new AbortController()

        const fetchItems = async () => {
            setIsLoading(true)
            setLoadError(null)

            try {
                const response = await fetch("/api/catalog/GameItems/equipments", { signal: controller.signal })

                if (!response.ok) {
                    throw new Error("Failed to load equipment")
                }

                const data: ItemData[] = await response.json()
                setAllItems(Array.isArray(data) ? data : [])
            } catch (error) {
                if (error instanceof DOMException && error.name === "AbortError") {
                    return
                }

                setLoadError("Failed to load equipment")
            } finally {
                setIsLoading(false)
            }
        }

        fetchItems()

        return () => {
            controller.abort()
        }
    }, [open])

    // Filter items by category and armor slot if applicable
    const items = React.useMemo(() => {
        const validCategories = mapCategoryToCatalog(category)
        let categoryItems = allItems.filter(item => !!item.category && validCategories.includes(item.category))

        if (category === "armor" && armorSlot) {
            categoryItems = categoryItems.filter(item => matchesArmorSlot(item.type, armorSlot))
        }

        return categoryItems
    }, [allItems, category, armorSlot])

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
                                        onRemoveItem?.()
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

                                {isLoading && (
                                    <div className="text-center py-12 flex flex-col items-center gap-2">
                                        <Loader2 className="w-5 h-5 text-[#8a8070] animate-spin" />
                                        <p className="text-[#8a8070] text-sm">Loading equipment...</p>
                                    </div>
                                )}

                                {!isLoading && loadError && (
                                    <div className="text-center py-12">
                                        <p className="text-[#b06f6f] text-sm">{loadError}</p>
                                    </div>
                                )}

                                {!isLoading && !loadError && items.length === 0 && (
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
                                    {(displayItem.image || displayItem.icon) ? (
                                        <Image
                                            src={getImageUrl(displayItem.image || displayItem.icon)}
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

