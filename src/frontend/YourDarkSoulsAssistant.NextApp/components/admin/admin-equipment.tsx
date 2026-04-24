"use client"

import { useState } from "react"
import { 
  Sword, 
  Shield, 
  Gem,
  Shirt,
  Search, 
  Plus,
  Edit,
  Trash2,
  MoreVertical
} from "lucide-react"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { ScrollArea } from "@/components/ui/scroll-area"
import type { ItemData } from "@/types/equipment"

type EquipmentCategory = "weapons" | "armor" | "talismans" | "consumables"

const categoryIcons: Record<EquipmentCategory, React.ReactNode> = {
  weapons: <Sword className="w-4 h-4" />,
  armor: <Shirt className="w-4 h-4" />,
  talismans: <Gem className="w-4 h-4" />,
  consumables: <Shield className="w-4 h-4" />,
}

const initialEquipment: Record<EquipmentCategory, ItemData[]> = {
  weapons: [
    {
      id: "w1",
      name: "Black Knife+7",
      type: "Dagger",
      category: "weapon",
      weight: "2.0",
      attack: { physical: "132", magic: "0", fire: "0", lightning: "0", holy: "130", critical: "110" },
      guard: { physical: "26.0", magic: "15.0", fire: "15.0", lightning: "15.0", holy: "42.0", boost: "16" },
      scaling: { str: "E", dex: "C", int: "-", fai: "D", arc: "-" },
      required: { str: "8", dex: "12", int: "0", fai: "18", arc: "0" },
    },
    {
      id: "w2",
      name: "Moonveil+10",
      type: "Katana",
      category: "weapon",
      weight: "6.5",
      attack: { physical: "198", magic: "162", fire: "0", lightning: "0", holy: "0", critical: "100" },
      guard: { physical: "46.0", magic: "52.0", fire: "31.0", lightning: "31.0", holy: "31.0", boost: "31" },
      scaling: { str: "E", dex: "D", int: "B", fai: "-", arc: "-" },
      required: { str: "12", dex: "18", int: "23", fai: "0", arc: "0" },
    },
  ],
  armor: [
    {
      id: "a1",
      name: "Black Knife Armor",
      type: "Chest Armor",
      category: "armor",
      weight: "6.6",
      attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
      guard: { physical: "10.6", magic: "11.4", fire: "10.8", lightning: "10.1", holy: "12.2", boost: "0" },
      scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
      required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
    },
  ],
  talismans: [
    {
      id: "t1",
      name: "Erdtree's Favor +2",
      type: "Talisman",
      category: "talisman",
      weight: "1.5",
      attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
      guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
      scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
      required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
      passiveEffects: ["Raises max HP by 4%", "Raises max stamina by 9.5%"],
    },
  ],
  consumables: [
    {
      id: "c1",
      name: "Flask of Crimson Tears",
      type: "Consumable",
      category: "consumable",
      weight: "0.0",
      attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
      guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
      scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
      required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
      passiveEffects: ["Restores HP"],
    },
  ],
}

const emptyItem: Omit<ItemData, "id"> = {
  name: "",
  type: "",
  weight: "0.0",
  attack: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", critical: "0" },
  guard: { physical: "0", magic: "0", fire: "0", lightning: "0", holy: "0", boost: "0" },
  scaling: { str: "-", dex: "-", int: "-", fai: "-", arc: "-" },
  required: { str: "0", dex: "0", int: "0", fai: "0", arc: "0" },
}

export function AdminEquipment() {
  const [equipment, setEquipment] = useState(initialEquipment)
  const [selectedCategory, setSelectedCategory] = useState<EquipmentCategory>("weapons")
  const [searchQuery, setSearchQuery] = useState("")
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)
  const [editingItem, setEditingItem] = useState<ItemData | null>(null)
  const [isCreating, setIsCreating] = useState(false)

  const filteredItems = equipment[selectedCategory].filter(item =>
    item.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    item.type.toLowerCase().includes(searchQuery.toLowerCase())
  )

  const handleCreateItem = () => {
    setEditingItem({ ...emptyItem, id: `new-${Date.now()}` } as ItemData)
    setIsCreating(true)
    setIsEditDialogOpen(true)
  }

  const handleEditItem = (item: ItemData) => {
    setEditingItem({ ...item })
    setIsCreating(false)
    setIsEditDialogOpen(true)
  }

  const handleSaveItem = () => {
    if (!editingItem) return

    setEquipment(prev => {
      const category = selectedCategory
      if (isCreating) {
        return {
          ...prev,
          [category]: [...prev[category], editingItem]
        }
      } else {
        return {
          ...prev,
          [category]: prev[category].map(item => 
            item.id === editingItem.id ? editingItem : item
          )
        }
      }
    })

    setIsEditDialogOpen(false)
    setEditingItem(null)
    setIsCreating(false)
  }

  const handleDeleteItem = (itemId: string) => {
    if (confirm("Are you sure you want to delete this item?")) {
      setEquipment(prev => ({
        ...prev,
        [selectedCategory]: prev[selectedCategory].filter(item => item.id !== itemId)
      }))
    }
  }

  const updateEditingItem = (field: string, value: string) => {
    if (!editingItem) return
    
    if (field.includes(".")) {
      const [parent, child] = field.split(".")
      setEditingItem({
        ...editingItem,
        [parent]: {
          ...(editingItem[parent as keyof ItemData] as Record<string, string>),
          [child]: value
        }
      })
    } else {
      setEditingItem({ ...editingItem, [field]: value })
    }
  }

  return (
    <div className="space-y-4">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-2">
          <Sword className="w-5 h-5 text-[#c4a456]" />
          <h2 className="text-lg font-serif text-[#c4a456]">Equipment Database</h2>
        </div>
        <Button 
          onClick={handleCreateItem}
          className="bg-[#c4a456] text-black hover:bg-[#d4b466] h-8 text-xs"
        >
          <Plus className="w-3 h-3 mr-1" />
          Add Item
        </Button>
      </div>

      {/* Category Tabs */}
      <div className="flex gap-1 border-b border-[#3a352c]">
        {(Object.keys(categoryIcons) as EquipmentCategory[]).map(category => (
          <button
            key={category}
            onClick={() => setSelectedCategory(category)}
            className={`flex items-center gap-1 px-3 py-2 text-xs uppercase tracking-wider transition-colors border-b-2 -mb-px ${
              selectedCategory === category
                ? "text-[#c4a456] border-[#c4a456]"
                : "text-gray-500 border-transparent hover:text-gray-300"
            }`}
          >
            {categoryIcons[category]}
            {category}
          </button>
        ))}
      </div>

      {/* Search */}
      <div className="relative">
        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500" />
        <Input
          placeholder="Search items..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          className="pl-10 bg-[#1a1815] border-[#3a352c] text-gray-300 placeholder:text-gray-600 h-9 text-sm"
        />
      </div>

      {/* Items Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-3 max-h-[350px] overflow-y-auto pr-1">
        {filteredItems.map(item => (
          <div
            key={item.id}
            className="bg-[#2a2520] border border-[#3a352c] rounded-lg p-3 hover:border-[#c4a456]/50 transition-colors"
          >
            <div className="flex items-start justify-between">
              <div className="flex items-center gap-2">
                <div className="w-10 h-10 bg-[#1a1815] border border-[#3a352c] rounded flex items-center justify-center">
                  {categoryIcons[selectedCategory]}
                </div>
                <div>
                  <h3 className="text-gray-200 font-serif text-sm">{item.name}</h3>
                  <p className="text-gray-500 text-xs">{item.type}</p>
                </div>
              </div>
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <button className="p-1 hover:bg-[#3a352c] rounded transition-colors">
                    <MoreVertical className="w-4 h-4 text-gray-500" />
                  </button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="bg-[#1a1815] border-[#3a352c]">
                  <DropdownMenuItem 
                    onClick={() => handleEditItem(item)}
                    className="text-gray-300 hover:text-white hover:bg-[#2a2520] cursor-pointer text-sm"
                  >
                    <Edit className="w-3 h-3 mr-2" />
                    Edit
                  </DropdownMenuItem>
                  <DropdownMenuItem 
                    onClick={() => handleDeleteItem(item.id!)}
                    className="text-red-400 hover:text-red-300 hover:bg-red-900/20 cursor-pointer text-sm"
                  >
                    <Trash2 className="w-3 h-3 mr-2" />
                    Delete
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </div>
            
            <div className="mt-2 grid grid-cols-3 gap-1 text-xs">
              <div>
                <span className="text-gray-500">Wt:</span>
                <span className="text-gray-400 ml-1">{item.weight}</span>
              </div>
              <div>
                <span className="text-gray-500">Phy:</span>
                <span className="text-gray-400 ml-1">{item.attack.physical}</span>
              </div>
              {item.attack.magic !== "0" && (
                <div>
                  <span className="text-blue-400/70">Mag:</span>
                  <span className="text-blue-400 ml-1">{item.attack.magic}</span>
                </div>
              )}
            </div>
          </div>
        ))}
      </div>

      {filteredItems.length === 0 && (
        <div className="text-center py-8 text-gray-500 text-sm">
          No items found. Click &quot;Add Item&quot; to create one.
        </div>
      )}

      {/* Edit/Create Dialog */}
      <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
        <DialogContent className="bg-[#1a1815] border-[#3a352c] max-w-lg max-h-[80vh]">
          <DialogHeader>
            <DialogTitle className="text-[#c4a456] text-base">
              {isCreating ? "Create New Item" : "Edit Item"}
            </DialogTitle>
          </DialogHeader>
          <ScrollArea className="max-h-[50vh] pr-4">
            <div className="space-y-3 py-2">
              {/* Basic Info */}
              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="text-xs text-gray-400 mb-1 block">Name</label>
                  <Input
                    value={editingItem?.name || ""}
                    onChange={(e) => updateEditingItem("name", e.target.value)}
                    className="bg-[#2a2520] border-[#3a352c] text-gray-300 h-8 text-sm"
                  />
                </div>
                <div>
                  <label className="text-xs text-gray-400 mb-1 block">Type</label>
                  <Input
                    value={editingItem?.type || ""}
                    onChange={(e) => updateEditingItem("type", e.target.value)}
                    className="bg-[#2a2520] border-[#3a352c] text-gray-300 h-8 text-sm"
                  />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="text-xs text-gray-400 mb-1 block">Weight</label>
                  <Input
                    value={editingItem?.weight || ""}
                    onChange={(e) => updateEditingItem("weight", e.target.value)}
                    className="bg-[#2a2520] border-[#3a352c] text-gray-300 h-8 text-sm"
                  />
                </div>
                <div>
                  <label className="text-xs text-gray-400 mb-1 block">Image Path</label>
                  <Input
                    value={editingItem?.image || ""}
                    onChange={(e) => updateEditingItem("image", e.target.value)}
                    className="bg-[#2a2520] border-[#3a352c] text-gray-300 h-8 text-sm"
                    placeholder="/images/weapons/..."
                  />
                </div>
              </div>

              {/* Attack Stats */}
              <div>
                <h4 className="text-[#c4a456] text-xs uppercase tracking-wider mb-2">Attack</h4>
                <div className="grid grid-cols-3 gap-2">
                  {["physical", "magic", "fire", "lightning", "holy", "critical"].map(stat => (
                    <div key={stat}>
                      <label className="text-[10px] text-gray-500 capitalize">{stat}</label>
                      <Input
                        value={editingItem?.attack[stat as keyof typeof editingItem.attack] || "0"}
                        onChange={(e) => updateEditingItem(`attack.${stat}`, e.target.value)}
                        className="bg-[#2a2520] border-[#3a352c] text-gray-300 h-7 text-xs"
                      />
                    </div>
                  ))}
                </div>
              </div>

              {/* Scaling */}
              <div>
                <h4 className="text-[#c4a456] text-xs uppercase tracking-wider mb-2">Scaling</h4>
                <div className="grid grid-cols-5 gap-2">
                  {["str", "dex", "int", "fai", "arc"].map(stat => (
                    <div key={stat}>
                      <label className="text-[10px] text-gray-500 uppercase">{stat}</label>
                      <Input
                        value={editingItem?.scaling[stat as keyof typeof editingItem.scaling] || "-"}
                        onChange={(e) => updateEditingItem(`scaling.${stat}`, e.target.value)}
                        className="bg-[#2a2520] border-[#3a352c] text-gray-300 h-7 text-xs text-center"
                      />
                    </div>
                  ))}
                </div>
              </div>

              {/* Requirements */}
              <div>
                <h4 className="text-[#c4a456] text-xs uppercase tracking-wider mb-2">Requirements</h4>
                <div className="grid grid-cols-5 gap-2">
                  {["str", "dex", "int", "fai", "arc"].map(stat => (
                    <div key={stat}>
                      <label className="text-[10px] text-gray-500 uppercase">{stat}</label>
                      <Input
                        value={editingItem?.required[stat as keyof typeof editingItem.required] || "0"}
                        onChange={(e) => updateEditingItem(`required.${stat}`, e.target.value)}
                        className="bg-[#2a2520] border-[#3a352c] text-gray-300 h-7 text-xs text-center"
                      />
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </ScrollArea>
          <DialogFooter>
            <Button 
              variant="outline" 
              onClick={() => setIsEditDialogOpen(false)}
              className="border-[#3a352c] text-gray-400 hover:text-white h-8 text-sm"
            >
              Cancel
            </Button>
            <Button 
              onClick={handleSaveItem}
              className="bg-[#c4a456] text-black hover:bg-[#d4b466] h-8 text-sm"
            >
              {isCreating ? "Create" : "Save"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}
