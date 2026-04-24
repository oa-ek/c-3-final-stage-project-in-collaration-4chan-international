"use client"

import { useState, useEffect } from "react"
import type { ItemData } from "@/types/equipment"
import { cn } from "@/lib/utils"
import { Sword, Shield, CircleDot, Shirt, Gem, FlaskConical } from "lucide-react"

// Slot types for different equipment categories
export type SlotType = "weapon" | "shield" | "arrow" | "armor" | "talisman" | "consumable"

// Icon components for each slot type when empty
const SLOT_ICONS: Record<SlotType, React.ReactNode> = {
  weapon: <Sword className="w-6 h-6" />,
  shield: <Shield className="w-6 h-6" />,
  arrow: <CircleDot className="w-5 h-5" />,
  armor: <Shirt className="w-6 h-6" />,
  talisman: <Gem className="w-5 h-5" />,
  consumable: <FlaskConical className="w-5 h-5" />,
}

interface EquipSlotProps {
  item?: ItemData | null
  onHover: (item: ItemData | null) => void
  onClick?: () => void
  hasItem?: boolean
  selected?: boolean
  count?: number
  icon?: React.ReactNode
  imageSrc?: string
  slotType?: SlotType
  defaultIcon?: string
}

export function EquipSlot({
  item,
  onHover,
  onClick,
  hasItem = false,
  selected = false,
  count,
  icon,
  imageSrc,
  slotType = "weapon",
  defaultIcon,
}: EquipSlotProps) {
  const [imageError, setImageError] = useState(false)
  const [imageLoaded, setImageLoaded] = useState(false)
  
  // Reset states when item changes
  useEffect(() => {
    setImageError(false)
    setImageLoaded(false)
  }, [item?.image, imageSrc])
  
  // Determine what image to show
  const hasRealItem = item !== null && item !== undefined
  const itemImage = item?.image || imageSrc
  const shouldShowImage = itemImage && !imageError

  return (
    <div
      className={cn(
        "w-16 h-16 relative cursor-pointer transition-all duration-150",
        "rounded-[4px] overflow-hidden",
        // Base gradient - dark stone look
        "bg-gradient-to-br from-[#3d3a35] via-[#2d2a26] to-[#1f1d1a]",
        // Inner shadow for depth
        "shadow-[inset_2px_2px_8px_rgba(0,0,0,0.7),inset_-1px_-1px_3px_rgba(90,85,75,0.12)]",
        // Border
        "border border-[#4a4540]/70",
        // Hover state
        "hover:border-[#8a7a55]/80 hover:shadow-[inset_2px_2px_8px_rgba(0,0,0,0.7),inset_-1px_-1px_3px_rgba(90,85,75,0.12),0_0_6px_rgba(138,122,85,0.2)]",
        // Selected state
        selected && "border-[#c4a456] border-2 shadow-[inset_2px_2px_8px_rgba(0,0,0,0.5),0_0_12px_rgba(196,164,86,0.4)]"
      )}
      onClick={onClick}
      onMouseEnter={() => {
        if (item) {
          onHover({ ...item, image: item.image || imageSrc })
        }
      }}
      onMouseLeave={() => onHover(null)}
    >
      {/* Stone texture pattern overlay */}
      <div 
        className="absolute inset-0 opacity-40 pointer-events-none mix-blend-overlay"
        style={{
          backgroundImage: `
            radial-gradient(ellipse at 20% 30%, rgba(80,75,65,0.3) 0%, transparent 50%),
            radial-gradient(ellipse at 80% 70%, rgba(60,55,45,0.2) 0%, transparent 50%),
            url("data:image/svg+xml,%3Csvg viewBox='0 0 100 100' xmlns='http://www.w3.org/2000/svg'%3E%3Cfilter id='n'%3E%3CfeTurbulence type='fractalNoise' baseFrequency='0.75' numOctaves='3'/%3E%3C/filter%3E%3Crect width='100%25' height='100%25' filter='url(%23n)' opacity='0.5'/%3E%3C/svg%3E")
          `,
        }}
      />

      {/* Subtle inner glow at top */}
      <div className="absolute inset-x-0 top-0 h-4 bg-gradient-to-b from-[#5a5550]/10 to-transparent pointer-events-none" />

      {/* Content area */}
      <div className="absolute inset-0 flex items-center justify-center p-1.5">
        {shouldShowImage ? (
          // Show item image
          <img
            src={itemImage}
            alt={item?.name || "Equipment"}
            className={cn(
              "w-full h-full object-contain",
              "drop-shadow-[0_2px_3px_rgba(0,0,0,0.6)]",
              !imageLoaded && "opacity-0"
            )}
            onLoad={() => setImageLoaded(true)}
            onError={() => setImageError(true)}
          />
        ) : (
          // Show placeholder icon for empty slot
          <div className="text-[#5a554a]/50">
            {icon || SLOT_ICONS[slotType]}
          </div>
        )}
      </div>

      {/* Item count badge */}
      {count !== undefined && count > 0 && (
        <span className="absolute bottom-0.5 right-1 text-[11px] text-[#d4c8b0] font-serif drop-shadow-[0_1px_2px_rgba(0,0,0,0.9)]">
          {count}
        </span>
      )}

      {/* Golden highlight overlay when selected */}
      {selected && (
        <div className="absolute inset-0 bg-gradient-to-br from-[#c4a456]/15 via-transparent to-[#c4a456]/5 pointer-events-none" />
      )}

      {/* Bottom edge highlight */}
      <div className="absolute inset-x-0 bottom-0 h-px bg-gradient-to-r from-transparent via-[#5a5550]/20 to-transparent pointer-events-none" />
    </div>
  )
}
