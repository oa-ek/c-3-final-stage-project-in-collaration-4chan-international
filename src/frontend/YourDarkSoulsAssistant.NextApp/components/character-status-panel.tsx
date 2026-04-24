"use client"

import { User } from "lucide-react"
import type { CharacterStats } from "@/types/equipment"

interface StatRowProps {
  label: string
  value: number
  onChange?: (value: number) => void
  editable?: boolean
  maxValue?: number
  highlight?: boolean
}

function StatRow({ label, value, onChange, editable = false, maxValue, highlight = false }: StatRowProps) {
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = parseInt(e.target.value) || 0
    onChange?.(Math.max(1, Math.min(99, newValue)))
  }

  return (
    <div className="flex justify-between items-center py-[1px]">
      <span className="text-[#8a8070] text-[13px]">{label}</span>
      {editable ? (
        <input
          type="number"
          value={value}
          onChange={handleChange}
          min={1}
          max={99}
          className="w-12 bg-transparent text-right text-[13px] text-[#d4cfc5] border-b border-[#3a352c] focus:border-[#c4a456] outline-none"
        />
      ) : (
        <span className={`text-[13px] ${highlight ? "text-[#d4cfc5]" : "text-[#a09080]"}`}>
          {maxValue ? `${value} / ${maxValue}` : value}
        </span>
      )}
    </div>
  )
}

interface CharacterStatusPanelProps {
  stats: CharacterStats
  onStatsChange?: (stats: CharacterStats) => void
  editable?: boolean
}

export function CharacterStatusPanel({ stats, onStatsChange, editable = true }: CharacterStatusPanelProps) {
  const handleStatChange = (key: keyof CharacterStats, value: number) => {
    if (onStatsChange) {
      onStatsChange({ ...stats, [key]: value })
    }
  }

  // Calculate derived stats
  const hp = Math.floor(300 + stats.vigor * 25 + stats.level * 2)
  const fp = Math.floor(50 + stats.mind * 6)
  const stamina = Math.floor(80 + stats.endurance * 1.5)
  const equipLoad = (stats.endurance * 2.5).toFixed(1)

  return (
    <div className="flex-1 flex flex-col">
      {/* Header */}
      <div className="flex items-center gap-2 mb-3">
        <User className="w-4 h-4 text-[#c4a456]" />
        <span className="text-[#c4a456] text-sm uppercase tracking-wider font-serif">
          Character Status
        </span>
      </div>

      {/* Level & Runes */}
      <div className="mb-3 pb-2 border-b border-[#3a352c]/50">
        <StatRow 
          label="Level" 
          value={stats.level} 
          onChange={(v) => handleStatChange("level", v)}
          editable={editable}
          highlight 
        />
        <StatRow 
          label="Runes Held" 
          value={stats.runesHeld}
          onChange={(v) => handleStatChange("runesHeld", v)}
          editable={editable}
        />
      </div>

      {/* Attributes */}
      <div className="mb-3 pb-2 border-b border-[#3a352c]/50 space-y-[1px]">
        <StatRow 
          label="Vigor" 
          value={stats.vigor}
          onChange={(v) => handleStatChange("vigor", v)}
          editable={editable}
        />
        <StatRow 
          label="Mind" 
          value={stats.mind}
          onChange={(v) => handleStatChange("mind", v)}
          editable={editable}
        />
        <StatRow 
          label="Endurance" 
          value={stats.endurance}
          onChange={(v) => handleStatChange("endurance", v)}
          editable={editable}
        />
        <StatRow 
          label="Strength" 
          value={stats.strength}
          onChange={(v) => handleStatChange("strength", v)}
          editable={editable}
        />
        <StatRow 
          label="Dexterity" 
          value={stats.dexterity}
          onChange={(v) => handleStatChange("dexterity", v)}
          editable={editable}
        />
        <StatRow 
          label="Intelligence" 
          value={stats.intelligence}
          onChange={(v) => handleStatChange("intelligence", v)}
          editable={editable}
        />
        <StatRow 
          label="Faith" 
          value={stats.faith}
          onChange={(v) => handleStatChange("faith", v)}
          editable={editable}
        />
        <StatRow 
          label="Arcane" 
          value={stats.arcane}
          onChange={(v) => handleStatChange("arcane", v)}
          editable={editable}
        />
      </div>

      {/* Resources (calculated) */}
      <div className="mb-3 pb-2 border-b border-[#3a352c]/50">
        <StatRow label="HP" value={hp} maxValue={hp} highlight />
        <StatRow label="FP" value={fp} maxValue={fp} />
        <StatRow label="Stamina" value={stamina} />
      </div>

      {/* Equipment Stats */}
      <div className="mb-3">
        <div className="flex justify-between items-center py-[1px]">
          <span className="text-[#8a8070] text-[13px]">Equip Load</span>
          <span className="text-[#a09080] text-[13px]">0.0 / {equipLoad}</span>
        </div>
        <div className="flex justify-end">
          <span className="text-[#6a6050] text-[11px]">Light Load</span>
        </div>
      </div>
    </div>
  )
}
