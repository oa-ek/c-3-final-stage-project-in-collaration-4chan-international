"use client"

import { useState, useEffect } from 'react'
import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/contexts/auth-context'
import {
    Search,
    Sword,
    Shield,
    Gem,
    Skull,
    BookOpen,
    MapPin,
    Sparkles,
    Filter,
    ChevronDown,
    ChevronLeft,
    ChevronRight,
    Home,
    User,
    X,
    Play,
    Library, Download
} from 'lucide-react'
import AddWikiItemModal from "@/components/wiki/AddWikiItemModal";

// Category types for filtering
type CategoryType = 'all' | 'weapons' | 'armor' | 'talismans' | 'bosses' | 'guides' | 'locations' | 'magic'

interface WikiItem {
    id: string
    name: string
    category: CategoryType
    subcategory?: string
    image?: string
    description: string
    stats?: Record<string, string | number>
    location?: string
    difficulty?: string
}

// Category configuration with icons and colors
const categories: { id: CategoryType; label: string; icon: React.ReactNode; color: string }[] = [
    { id: 'all', label: 'All', icon: <Sparkles className="w-5 h-5" />, color: '#D4AF37' },
    { id: 'weapons', label: 'Weapons', icon: <Sword className="w-5 h-5" />, color: '#C89B64' },
    { id: 'armors', label: 'Armor', icon: <Shield className="w-5 h-5" />, color: '#8B7355' },
    { id: 'talismans', label: 'Talismans', icon: <Gem className="w-5 h-5" />, color: '#9B7ED9' },
    { id: 'bosses', label: 'Bosses', icon: <Skull className="w-5 h-5" />, color: '#DC3545' },
    { id: 'guides', label: 'Guides', icon: <BookOpen className="w-5 h-5" />, color: '#28A745' },
    { id: 'locations', label: 'Locations', icon: <MapPin className="w-5 h-5" />, color: '#17A2B8' },
    { id: 'magic', label: 'Magic', icon: <Sparkles className="w-5 h-5" />, color: '#6F42C1' },
]

export default function WikiPage() {
    const router = useRouter()
    const { user, isLoading: authLoading } = useAuth()
    const [searchQuery, setSearchQuery] = useState('')
    const [activeCategory, setActiveCategory] = useState<CategoryType>('all')
    const [isAddModalOpen, setIsAddModalOpen] = useState(false)

    const [items, setItems] = useState<WikiItem[]>([])
    const [isLoading, setIsLoading] = useState(false)
    const [showFilters, setShowFilters] = useState(false)

    const [currentPage, setCurrentPage] = useState(1)
    const [totalPages, setTotalPages] = useState(1)
    const itemsPerPage = 12

    useEffect(() => {
        setCurrentPage(1)
    }, [searchQuery, activeCategory])

    // Отримання даних з нашого ASP.NET API
    useEffect(() => {
        const fetchItems = async () => {
            setIsLoading(true)
            try {
                // Збираємо параметри запиту
                const params = new URLSearchParams({
                    page: currentPage.toString(),
                    pageSize: itemsPerPage.toString(),
                    category: activeCategory
                });

                if (searchQuery.trim()) {
                    params.append('query', searchQuery);
                }

                // Використовуємо відносний шлях.
                // Зверни увагу: якщо твій контролер називається WikiController,
                // але в next.config.mjs налаштовано проксі для '/api/articles',
                // переконайся, що шлях збігається.
                // Припускаю, що ендпоінт лежить за адресою /api/articles/wiki
                const response = await fetch(`/api/articles/wiki?${params.toString()}`)

                if (!response.ok) throw new Error('Помилка завантаження')

                const data = await response.json()

                // Мапимо EnrichedWikiItemDto з бекенду на наш фронтенд-інтерфейс WikiItem
                const mappedItems: WikiItem[] = data.items.map((item: any) => ({
                    id: item.id,
                    name: item.name,
                    category: item.category as CategoryType,
                    description: item.description,
                    image: item.image, // Динамічна картинка з Elden Ring API!
                    stats: item.stats ? {
                        'HP/Damage': item.stats.HP || item.stats.AttackPower || '???',
                        'Defense': item.stats.Defense || '???'
                    } : undefined
                }))

                setItems(mappedItems)
                // Серверна пагінація: бекенд знає загальну кількість сторінок
                setTotalPages(data.totalPages || 1)
            } catch (error) {
                console.error('Failed to fetch items:', error)
                setItems([])
            } finally {
                setIsLoading(false)
            }
        }

        // Debounce: чекаємо 500мс після останнього натискання клавіші, перш ніж робити запит
        const timeoutId = setTimeout(() => {
            fetchItems()
        }, 500)

        return () => clearTimeout(timeoutId)
    }, [searchQuery, activeCategory, currentPage])

    const getCategoryIcon = (category: CategoryType) => {
        const cat = categories.find(c => c.id === category)
        return cat?.icon || <Sparkles className="w-5 h-5" />
    }

    const getCategoryColor = (category: CategoryType) => {
        const cat = categories.find(c => c.id === category)
        return cat?.color || '#D4AF37'
    }

    return (
        <div className="min-h-screen bg-[#0a0908] text-gray-200">
            {/* Background texture */}
            <div className="fixed inset-0 pointer-events-none">
                <div className="absolute inset-0 bg-gradient-to-b from-[#1a1714]/50 via-transparent to-[#0a0908]" />
                <div
                    className="absolute inset-0 opacity-[0.03]"
                    style={{
                        backgroundImage: `url("data:image/svg+xml,%3Csvg viewBox='0 0 256 256' xmlns='http://www.w3.org/2000/svg'%3E%3Cfilter id='noise'%3E%3CfeTurbulence type='fractalNoise' baseFrequency='0.9' numOctaves='4' stitchTiles='stitch'/%3E%3C/filter%3E%3Crect width='100%25' height='100%25' filter='url(%23noise)'/%3E%3C/svg%3E")`,
                    }}
                />
            </div>

            {/* Header */}
            <header className="relative z-10 border-b border-[#3a352c]/50">
                <div className="max-w-7xl mx-auto px-4 py-4">
                    <div className="flex items-center justify-between">
                        {/* Logo */}
                        <Link href="/builds" className="flex items-center gap-3 group">
                            <div className="w-10 h-10 rounded-lg bg-gradient-to-br from-[#D4AF37] to-[#8B6914] flex items-center justify-center shadow-lg shadow-[#D4AF37]/20">
                                <BookOpen className="w-5 h-5 text-black" />
                            </div>
                            <div>
                                <h1 className="font-serif text-xl text-[#D4AF37] group-hover:text-[#E5C767] transition-colors">
                                    Tarnished Wiki
                                </h1>
                                <p className="text-[10px] text-gray-500 uppercase tracking-[0.3em]">Knowledge Archive</p>
                            </div>
                        </Link>

                        {/* Search Bar */}
                        <div className="flex-1 max-w-xl mx-8">
                            <div className="relative">
                                <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500" />
                                <input
                                    type="text"
                                    placeholder="Search weapons, armor, bosses, guides..."
                                    value={searchQuery}
                                    onChange={(e) => setSearchQuery(e.target.value)}
                                    className="w-full pl-12 pr-4 py-3 bg-[#1a1714] border border-[#3a352c] rounded-lg text-gray-200 placeholder-gray-500 focus:border-[#D4AF37]/50 focus:outline-none focus:ring-1 focus:ring-[#D4AF37]/30 transition-all"
                                />
                                {searchQuery && (
                                    <button
                                        onClick={() => setSearchQuery('')}
                                        className="absolute right-4 top-1/2 -translate-y-1/2 text-gray-500 hover:text-gray-300"
                                    >
                                        <X className="w-4 h-4" />
                                    </button>
                                )}
                            </div>
                        </div>

                        {/* Navigation */}
                        <div className="flex items-center gap-4">
                            <Link href="/archive" className="flex items-center gap-2 px-4 py-2 text-gray-400 hover:text-[#D4AF37] transition-colors">
                                <Library className="w-4 h-4" />
                                <span className="text-sm">Archive</span>
                            </Link>
                            <Link href="/guides" className="flex items-center gap-2 px-4 py-2 text-gray-400 hover:text-[#D4AF37] transition-colors">
                                <Play className="w-4 h-4" />
                                <span className="text-sm">Videos</span>
                            </Link>
                            <Link href="/builds" className="flex items-center gap-2 px-4 py-2 text-gray-400 hover:text-[#D4AF37] transition-colors">
                                <Home className="w-4 h-4" />
                                <span className="text-sm">Builds</span>
                            </Link>
                            {user && (
                                <Link href="/profile" className="flex items-center gap-2 px-4 py-2 text-gray-400 hover:text-[#D4AF37] transition-colors">
                                    <User className="w-4 h-4" />
                                    <span className="text-sm">{user.firstName || 'Profile'}</span>
                                </Link>
                            )}
                        </div>
                    </div>
                </div>
            </header>

            {/* Category Filter Bar */}
            <div className="relative z-10 border-b border-[#3a352c]/30 bg-[#0f0e0c]/80 backdrop-blur-sm">
                <div className="max-w-7xl mx-auto px-4">
                    <div className="flex items-center gap-2 py-3 overflow-x-auto scrollbar-hide">
                        {categories.map((category) => (
                            <button
                                key={category.id}
                                onClick={() => setActiveCategory(category.id)}
                                className={`flex items-center gap-2 px-4 py-2 rounded-lg text-sm font-medium whitespace-nowrap transition-all ${
                                    activeCategory === category.id
                                        ? 'bg-gradient-to-br from-[#2a2520] to-[#1a1714] border border-[#D4AF37]/50 text-[#D4AF37] shadow-lg shadow-[#D4AF37]/10'
                                        : 'bg-[#1a1714]/50 border border-transparent text-gray-400 hover:text-gray-200 hover:bg-[#1a1714]'
                                }`}
                                style={{
                                    borderColor: activeCategory === category.id ? category.color + '50' : undefined,
                                    color: activeCategory === category.id ? category.color : undefined,
                                }}
                            >
                                {category.icon}
                                {category.label}
                            </button>
                        ))}
                    </div>
                </div>
            </div>

            {/* Main Content */}
            <main className="relative z-10 max-w-7xl mx-auto px-4 py-8">
                {/* Results Header */}
                <div className="flex items-center justify-between mb-6">
                    <div>
                        <h2 className="text-2xl font-serif text-[#D4AF37]">
                            {activeCategory === 'all' ? 'All Items' : categories.find(c => c.id === activeCategory)?.label}
                        </h2>
                    </div>
                    <div className="flex gap-3">
                        {/* НОВА КНОПКА ДЛЯ ІМПОРТУ */}
                        <button
                            onClick={() => setIsAddModalOpen(true)}
                            className="flex items-center gap-2 px-4 py-2 bg-gradient-to-br from-[#D4AF37] to-[#8B6914] text-black rounded-lg font-medium hover:from-[#E5C767] transition-all"
                        >
                            <Download className="w-4 h-4" />
                            <span className="hidden sm:inline">Import from API</span>
                        </button>

                        <button
                            onClick={() => setShowFilters(!showFilters)}
                            className="flex items-center gap-2 px-4 py-2 bg-[#1a1714] border border-[#3a352c] rounded-lg text-gray-400 hover:text-[#D4AF37] hover:border-[#D4AF37]/30 transition-all"
                        >
                            <Filter className="w-4 h-4" />
                            <span className="hidden sm:inline">Filters</span>
                            <ChevronDown className={`w-4 h-4 transition-transform ${showFilters ? 'rotate-180' : ''}`} />
                        </button>
                    </div>
                </div>

                {/* Items Grid */}
                {isLoading ? (
                    <div className="flex items-center justify-center h-64">
                        <div className="w-8 h-8 border-2 border-[#D4AF37] border-t-transparent rounded-full animate-spin" />
                    </div>
                ) : items.length === 0 ? (
                    <div className="flex flex-col items-center justify-center h-64 text-center">
                        <Search className="w-12 h-12 text-gray-600 mb-4" />
                        <h3 className="text-xl font-serif text-gray-400">No items found</h3>
                        <p className="text-gray-500 mt-2">Try adjusting your search or filters</p>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                        {items.map((item) => (
                            <Link
                                key={item.id}
                                href={`/wiki/${item.category}/${item.id}`}
                                className="group relative bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-lg overflow-hidden hover:border-[#D4AF37]/50 transition-all duration-300 text-left block"
                            >
                                {/* Item Image */}
                                <div className="relative h-40 bg-[#0a0908] overflow-hidden">
                                    {item.image ? (
                                        <img
                                            src={item.image}
                                            alt={item.name}
                                            className="w-full h-full object-contain p-4 group-hover:scale-110 transition-transform duration-500"
                                            onError={(e) => {
                                                (e.target as HTMLImageElement).style.display = 'none'
                                            }}
                                        />
                                    ) : null}
                                    {/* Placeholder icon if no image */}
                                    <div className="absolute inset-0 flex items-center justify-center opacity-20">
                                        {getCategoryIcon(item.category)}
                                    </div>
                                    {/* Category badge */}
                                    <div
                                        className="absolute top-3 right-3 px-2 py-1 rounded text-[10px] uppercase tracking-wider font-medium"
                                        style={{
                                            backgroundColor: getCategoryColor(item.category) + '20',
                                            color: getCategoryColor(item.category)
                                        }}
                                    >
                                        {item.category}
                                    </div>
                                    {/* Gradient overlay */}
                                    <div className="absolute inset-x-0 bottom-0 h-20 bg-gradient-to-t from-[#1a1714] to-transparent" />
                                </div>

                                {/* Item Info */}
                                <div className="p-4">
                                    <h3 className="font-serif text-lg text-[#D4AF37] group-hover:text-[#E5C767] transition-colors truncate">
                                        {item.name}
                                    </h3>
                                    {item.subcategory && (
                                        <p className="text-xs text-gray-500 uppercase tracking-wider mt-1">{item.subcategory}</p>
                                    )}
                                    <p className="text-sm text-gray-400 mt-2 line-clamp-2">{item.description}</p>

                                    {/* Quick stats */}
                                    {item.stats && (
                                        <div className="flex flex-wrap gap-2 mt-3">
                                            {Object.entries(item.stats).slice(0, 2).map(([key, value]) => (
                                                <span key={key} className="text-xs px-2 py-1 bg-[#0a0908] rounded text-gray-400">
                                                    {key}: <span className="text-[#D4AF37]">{value}</span>
                                                </span>
                                            ))}
                                        </div>
                                    )}

                                    {item.location && (
                                        <div className="flex items-center gap-1 mt-3 text-xs text-gray-500">
                                            <MapPin className="w-3 h-3" />
                                            {item.location}
                                        </div>
                                    )}
                                </div>

                                {/* Hover glow effect */}
                                <div
                                    className="absolute inset-0 opacity-0 group-hover:opacity-100 transition-opacity duration-300 pointer-events-none"
                                    style={{
                                        boxShadow: `inset 0 0 30px ${getCategoryColor(item.category)}10`
                                    }}
                                />
                            </Link>
                        ))}
                    </div>
                )}

                {/* Server-side Pagination */}
                {totalPages > 1 && (
                    <div className="flex items-center justify-center gap-2 mt-8">
                        <button
                            onClick={() => setCurrentPage(p => Math.max(1, p - 1))}
                            disabled={currentPage === 1}
                            className="p-2 bg-[#1a1714] border border-[#3a352c] rounded-lg text-gray-400 hover:text-[#D4AF37] hover:border-[#D4AF37]/30 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            <ChevronLeft className="w-5 h-5" />
                        </button>

                        {Array.from({ length: totalPages }, (_, i) => i + 1).map(page => (
                            <button
                                key={page}
                                onClick={() => setCurrentPage(page)}
                                className={`w-10 h-10 rounded-lg font-medium transition-all ${
                                    currentPage === page
                                        ? 'bg-gradient-to-br from-[#D4AF37] to-[#8B6914] text-black'
                                        : 'bg-[#1a1714] border border-[#3a352c] text-gray-400 hover:text-[#D4AF37] hover:border-[#D4AF37]/30'
                                }`}
                            >
                                {page}
                            </button>
                        ))}

                        <button
                            onClick={() => setCurrentPage(p => Math.min(totalPages, p + 1))}
                            disabled={currentPage === totalPages}
                            className="p-2 bg-[#1a1714] border border-[#3a352c] rounded-lg text-gray-400 hover:text-[#D4AF37] hover:border-[#D4AF37]/30 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            <ChevronRight className="w-5 h-5" />
                        </button>
                    </div>
                )}
                <AddWikiItemModal
                    isOpen={isAddModalOpen}
                    onClose={() => setIsAddModalOpen(false)}
                    onItemAdded={() => {
                        setCurrentPage(1);
                    }}
                />
            </main>
        </div>
    )
}
