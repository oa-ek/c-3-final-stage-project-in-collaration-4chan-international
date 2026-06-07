"use client"

import type { ItemData } from "@/types/equipment"
import { Sword, Shield } from "lucide-react"
import Image from "next/image"
import { getImageUrl } from "@/lib/content-utils"

interface ItemDetailsPanelProps {
  item: ItemData | null
}

function StatRow({
                   label,
                   value,
                   secondary
                 }: {
  label: string
  value: string
  secondary?: string
}) {
  return (
      <div className="flex justify-between items-center py-[2px]">
        <span className="text-[#8a8070] text-[13px]">{label}</span>
        <div className="flex items-center gap-2">
          <span className="text-[#d4cfc5] text-[13px]">{value}</span>
          {secondary && (
              <span className="text-[#8a8070] text-[13px]">{secondary}</span>
          )}
        </div>
      </div>
  )
}

function ScalingRow({
                      stats
                    }: {
  stats: { label: string; value: string }[]
}) {
  return (
      <div className="flex gap-6">
        {stats.map((stat) => (
            <div key={stat.label} className="flex items-center gap-2">
              <span className="text-[#8a8070] text-[13px]">{stat.label}</span>
              <span className="text-[#d4cfc5] text-[13px]">{stat.value}</span>
            </div>
        ))}
      </div>
  )
}

export function ItemDetailsPanel({ item }: ItemDetailsPanelProps) {
  if (!item) {
    return (
        <div className="flex-1 flex items-center justify-center">
          <p className="text-[#6a6050] text-sm">Select equipment to see details</p>
        </div>
    )
  }

  const displayItem = item
  const itemVisual = displayItem.image || displayItem.icon
  const itemVisualUrl = itemVisual ? getImageUrl(itemVisual) : null

  return (
      <div className="flex-1 flex flex-col">
        {/* Item Header */}
        <div className="mb-4">
          <h2 className="text-[#d4cfc5] text-xl font-serif tracking-wide">
            {displayItem.name}
          </h2>
          <div className="mt-1 space-y-[2px]">
            <p className="text-[#8a8070] text-sm">{displayItem.type}</p>
            {displayItem.attackType && (
                <p className="text-[#8a8070] text-sm">{displayItem.attackType}</p>
            )}
          </div>
        </div>

        {/* Special Properties */}
        <div className="mb-4 py-2 border-t border-b border-[#3a352c]/50">
          <div className="flex justify-between mt-1">
            <span className="text-[#8a8070] text-[13px]">FP Cost</span>
            <span className="text-[#d4cfc5] text-[13px]">{displayItem.fpCost || "-"}</span>
          </div>
          <div className="flex justify-between">
            <span className="text-[#8a8070] text-[13px]">Weight</span>
            <span className="text-[#d4cfc5] text-[13px]">{displayItem.weight}</span>
          </div>
        </div>

        {/* Main Stats Grid */}
        <div className="flex gap-4">
          {/* Left Column - Attack Power */}
          <div className="flex-1">
            <div className="flex items-center gap-2 mb-2">
              <Sword className="w-4 h-4 text-[#8a8070]" />
              <span className="text-[#c4a456] text-[13px] uppercase tracking-wider">Attack Power</span>
            </div>
            <div className="space-y-[1px]">
              <StatRow label="Physical" value={displayItem.attack.physical} secondary="73" />
              <StatRow label="Magic" value={displayItem.attack.magic} />
              <StatRow label="Fire" value={displayItem.attack.fire} />
              <StatRow label="Lightning" value={displayItem.attack.lightning} />
              <StatRow label="Holy" value={displayItem.attack.holy} secondary="58" />
              <StatRow label="Critical" value={displayItem.attack.critical} />
            </div>
          </div>

          {/* Right Column - Item Image */}
          <div className="w-[140px] h-[140px] bg-[#1a1815] border border-[#3a352c] flex items-center justify-center relative overflow-hidden">
            {itemVisualUrl ? (
                <Image
                    src={itemVisualUrl}
                    alt={displayItem.name}
                    fill
                    className="object-contain p-2"
                    sizes="140px"
                />
            ) : (
                <Sword className="w-16 h-16 text-[#6a6050] rotate-[-45deg]" />
            )}
          </div>
        </div>

        {/* Guarded Damage Negation */}
        <div className="mt-4">
          <div className="flex items-center gap-2 mb-2">
            <Shield className="w-4 h-4 text-[#8a8070]" />
            <span className="text-[#c4a456] text-[13px] uppercase tracking-wider">Guarded Damage Negation</span>
          </div>
          <div className="grid grid-cols-2 gap-x-4">
            <StatRow label="Physical" value={displayItem.guard.physical} />
            <StatRow label="Magic" value={displayItem.guard.magic} />
            <StatRow label="Fire" value={displayItem.guard.fire} />
            <StatRow label="Lightning" value={displayItem.guard.lightning} />
            <StatRow label="Holy" value={displayItem.guard.holy} />
            <StatRow label="Guard Boost" value={displayItem.guard.boost} />
          </div>
        </div>

        {/* Scaling & Requirements Grid */}
        <div className="mt-4 grid grid-cols-2 gap-4">
          {/* Attribute Scaling */}
          <div>
            <div className="flex items-center gap-2 mb-2">
              <span className="text-[#c4a456] text-[12px] uppercase tracking-wider">Attribute Scaling</span>
            </div>
            <div className="space-y-1">
              <ScalingRow stats={[
                { label: "Str", value: displayItem.scaling.str },
                { label: "Dex", value: displayItem.scaling.dex },
              ]} />
              <ScalingRow stats={[
                { label: "Int", value: displayItem.scaling.int },
                { label: "Fai", value: displayItem.scaling.fai },
              ]} />
              <ScalingRow stats={[
                { label: "Arc", value: displayItem.scaling.arc },
              ]} />
            </div>
          </div>

          {/* Attributes Required */}
          <div>
            <div className="flex items-center gap-2 mb-2">
              <span className="text-[#c4a456] text-[12px] uppercase tracking-wider">Attributes Required</span>
            </div>
            <div className="space-y-1">
              <ScalingRow stats={[
                { label: "Str", value: displayItem.required.str },
                { label: "Dex", value: displayItem.required.dex },
              ]} />
              <ScalingRow stats={[
                { label: "Int", value: displayItem.required.int },
                { label: "Fai", value: displayItem.required.fai },
              ]} />
              <ScalingRow stats={[
                { label: "Arc", value: displayItem.required.arc },
              ]} />
            </div>
          </div>
        </div>

        {/* Passive Effects */}
        <div className="mt-4">
          <div className="flex items-center gap-2 mb-2">
            <span className="text-[#c4a456] text-[12px] uppercase tracking-wider">Passive Effects</span>
          </div>
          <div className="space-y-[2px]">
            {(displayItem.passiveEffects || ["-", "-", "-"]).map((effect, i) => (
                <p key={i} className="text-[#8a8070] text-[13px]">{effect}</p>
            ))}
          </div>
        </div>
      </div>
  )
}
