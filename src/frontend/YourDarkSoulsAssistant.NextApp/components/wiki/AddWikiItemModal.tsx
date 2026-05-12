import { useState } from 'react'
import { Search, X, Plus, Download, Shield, Sword, Skull } from 'lucide-react'

// Пропси для модалки
interface AddWikiItemModalProps {
    isOpen: boolean;
    onClose: () => void;
    onItemAdded: () => void; // Щоб оновити список на головній сторінці
}

export default function AddWikiItemModal({ isOpen, onClose, onItemAdded }: AddWikiItemModalProps) {
    const [category, setCategory] = useState('bosses')
    const [searchQuery, setSearchQuery] = useState('')
    const [searchResults, setSearchResults] = useState<any[]>([])
    const [isSearching, setIsSearching] = useState(false)
    const [isSaving, setIsSaving] = useState<string | null>(null) // ID елемента, який зараз зберігається
    const [error, setError] = useState('')

    if (!isOpen) return null;

    // 1. Пошук у зовнішньому API (Elden Ring API)
    const handleSearch = async () => {
        if (!searchQuery.trim()) return;
        setIsSearching(true)
        setError('')

        try {
            // Зверни увагу на шлях - використовуємо твій налаштований проксі /api/articles/wiki
            const response = await fetch(`/api/articles/wiki/external-search?category=${category}&query=${encodeURIComponent(searchQuery)}`)

            if (!response.ok) throw new Error('Помилка пошуку в зовнішньому API')

            const result = await response.json()
            // Elden Ring Fan API повертає дані в полі "data"
            setSearchResults(result.data || [])

            if (result.data?.length === 0) {
                setError('Нічого не знайдено в базі Elden Ring')
            }
        } catch (err) {
            setError('Не вдалося зв\'язатися з API')
            setSearchResults([])
        } finally {
            setIsSearching(false)
        }
    }

    // 2. Індексація (Збереження в нашу локальну БД)
    const handleSaveToDb = async (item: any) => {
        setIsSaving(item.id)
        try {
            // Витягуємо стати для збереження у наш jsonb
            const rawStats = {
                hp: item.healthPoints,
                defense: item.defense,
                attackPower: item.attackPower,
                weight: item.weight,
                scalesWith: item.scalesWith
            }

            const payload = {
                title: item.name,
                subtitle: item.location || 'Unknown Location',
                category: category,
                description: item.description || `An entity from the lands between.`,
                rawStatsJson: JSON.stringify(rawStats)
            }

            const response = await fetch('/api/articles/wiki/index', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            })

            if (!response.ok) {
                const errorData = await response.text()
                if (response.status === 409) throw new Error('Ця стаття вже є у вашій базі!')
                throw new Error(errorData || 'Помилка збереження')
            }

            // Успіх! Закриваємо модалку і оновлюємо головний список
            onItemAdded()
            onClose()
            setSearchResults([])
            setSearchQuery('')
        } catch (err: any) {
            alert(err.message)
        } finally {
            setIsSaving(null)
        }
    }

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/80 backdrop-blur-sm p-4">
            <div className="bg-[#0f0e0c] border border-[#3a352c] rounded-xl w-full max-w-2xl overflow-hidden flex flex-col max-h-[90vh]">

                {/* Header */}
                <div className="flex items-center justify-between p-4 border-b border-[#3a352c]/50 bg-[#1a1714]">
                    <h2 className="text-xl font-serif text-[#D4AF37] flex items-center gap-2">
                        <Download className="w-5 h-5" />
                        Import to Archives
                    </h2>
                    <button onClick={onClose} className="text-gray-500 hover:text-gray-300">
                        <X className="w-5 h-5" />
                    </button>
                </div>

                {/* Search Form */}
                <div className="p-4 border-b border-[#3a352c]/50 flex gap-2">
                    <select
                        value={category}
                        onChange={(e) => setCategory(e.target.value)}
                        className="bg-[#1a1714] border border-[#3a352c] rounded-lg px-3 py-2 text-gray-300 focus:outline-none focus:border-[#D4AF37]"
                    >
                        <option value="bosses">Bosses</option>
                        <option value="weapons">Weapons</option>
                        <option value="items">Items</option>
                        <option value="armors">Armors</option>
                    </select>

                    <div className="flex-1 relative">
                        <input
                            type="text"
                            placeholder="Enter name (e.g. Radahn)..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
                            className="w-full bg-[#1a1714] border border-[#3a352c] rounded-lg pl-3 pr-10 py-2 text-gray-300 focus:outline-none focus:border-[#D4AF37]"
                        />
                    </div>

                    <button
                        onClick={handleSearch}
                        disabled={isSearching}
                        className="bg-gradient-to-br from-[#D4AF37] to-[#8B6914] text-black px-4 py-2 rounded-lg font-medium hover:from-[#E5C767] transition-all disabled:opacity-50 flex items-center gap-2"
                    >
                        {isSearching ? <div className="w-4 h-4 border-2 border-black border-t-transparent rounded-full animate-spin" /> : <Search className="w-4 h-4" />}
                        Search API
                    </button>
                </div>

                {/* Results Area */}
                <div className="p-4 flex-1 overflow-y-auto bg-[#0a0908]">
                    {error && <p className="text-red-400 text-center py-4">{error}</p>}

                    <div className="space-y-3">
                        {searchResults.map((item) => (
                            <div key={item.id} className="flex items-center justify-between p-3 border border-[#3a352c] bg-[#1a1714] rounded-lg hover:border-[#D4AF37]/50 transition-colors">
                                <div className="flex items-center gap-4">
                                    {item.image && (
                                        <img src={item.image} alt={item.name} className="w-12 h-12 object-cover rounded bg-black" />
                                    )}
                                    <div>
                                        <h3 className="text-[#D4AF37] font-medium">{item.name}</h3>
                                        <p className="text-xs text-gray-500 line-clamp-1">{item.description}</p>
                                    </div>
                                </div>
                                <button
                                    onClick={() => handleSaveToDb(item)}
                                    disabled={isSaving === item.id}
                                    className="p-2 bg-green-900/30 text-green-500 border border-green-900/50 rounded hover:bg-green-900/50 transition-all flex items-center gap-2"
                                    title="Add to Local Database"
                                >
                                    {isSaving === item.id ? (
                                        <div className="w-4 h-4 border-2 border-green-500 border-t-transparent rounded-full animate-spin" />
                                    ) : (
                                        <Plus className="w-4 h-4" />
                                    )}
                                </button>
                            </div>
                        ))}

                        {!isSearching && searchResults.length === 0 && !error && (
                            <div className="text-center text-gray-600 py-8">
                                <Search className="w-8 h-8 mx-auto mb-2 opacity-50" />
                                <p>Search the external API to add items to your local wiki.</p>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}
