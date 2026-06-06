"use client"

import { useState, useEffect } from 'react'
import Link from 'next/link'
import { useAuth } from '@/contexts/auth-context'
import {
    Search,
    BookOpen,
    Home,
    User,
    X,
    Image as ImageIcon,
    Download,
    Heart,
    Eye,
    ChevronLeft,
    ChevronRight,
    Grid3X3,
    LayoutGrid,
    Maximize2,
    Filter,
    Play,
    Library
} from 'lucide-react'

// Types
interface GalleryImage {
    id: string
    title: string
    description?: string
    imageUrl: string
    thumbnailUrl?: string
    category: 'screenshot' | 'artwork' | 'wallpaper' | 'concept' | 'community'
    game: 'dark-souls' | 'dark-souls-2' | 'dark-souls-3' | 'elden-ring' | 'bloodborne' | 'sekiro'
    tags: string[]
    author?: string
    likes: number
    views: number
    uploadedAt: string
}

// Category configuration
const categoryFilters = [
    { id: 'all', label: 'All' },
    { id: 'screenshot', label: 'Screenshots' },
    { id: 'artwork', label: 'Artwork' },
    { id: 'wallpaper', label: 'Wallpapers' },
    { id: 'concept', label: 'Concept Art' },
    { id: 'community', label: 'Community' },
]

const gameFilters = [
    { id: 'all', label: 'All Games' },
    { id: 'elden-ring', label: 'Elden Ring' },
    { id: 'dark-souls-3', label: 'Dark Souls III' },
    { id: 'dark-souls-2', label: 'Dark Souls II' },
    { id: 'dark-souls', label: 'Dark Souls' },
    { id: 'bloodborne', label: 'Bloodborne' },
    { id: 'sekiro', label: 'Sekiro' },
]

export default function ArchivePage() {
    const { user } = useAuth()
    const [searchQuery, setSearchQuery] = useState('')
    const [activeCategory, setActiveCategory] = useState('all')
    const [activeGame, setActiveGame] = useState('all')
    const [images, setImages] = useState<GalleryImage[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [selectedImage, setSelectedImage] = useState<GalleryImage | null>(null)
    const [viewMode, setViewMode] = useState<'grid' | 'masonry'>('grid')
    const [currentPage, setCurrentPage] = useState(1)
    const imagesPerPage = 12

    // Filter images
    const filteredImages = images.filter(img => {
        const matchesSearch = img.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
            img.tags.some(tag => tag.toLowerCase().includes(searchQuery.toLowerCase()))
        const matchesCategory = activeCategory === 'all' || img.category === activeCategory
        const matchesGame = activeGame === 'all' || img.game === activeGame
        return matchesSearch && matchesCategory && matchesGame
    })

    // Pagination
    const totalPages = Math.ceil(filteredImages.length / imagesPerPage)
    const paginatedImages = filteredImages.slice(
        (currentPage - 1) * imagesPerPage,
        currentPage * imagesPerPage
    )

    useEffect(() => {
        setCurrentPage(1)
    }, [searchQuery, activeCategory, activeGame])

    // Fetch gallery images from the Content Delivery API.
    // If the API returns nothing (or is unavailable), the gallery simply
    // renders its empty state instead of any mock content.
    useEffect(() => {
        const fetchImages = async () => {
            setIsLoading(true)
            setError(null)
            try {
                const response = await fetch('/api/content/ContentItem/gallery')
                if (!response.ok) throw new Error('Failed to load gallery')

                const data = await response.json()
                const list: any[] = Array.isArray(data) ? data : (data.items ?? [])

                const mapped: GalleryImage[] = list.map((img: any) => ({
                    id: String(img.id),
                    title: img.title ?? img.name ?? 'Untitled',
                    description: img.description ?? undefined,
                    imageUrl: img.imageUrl ?? (img.publicRoute ? `/api/content/ContentItem/${img.publicRoute}` : ''),
                    thumbnailUrl: img.thumbnailUrl ?? undefined,
                    category: (img.category ?? 'community') as GalleryImage['category'],
                    game: (img.game ?? 'elden-ring') as GalleryImage['game'],
                    tags: Array.isArray(img.tags) ? img.tags : [],
                    author: img.author ?? undefined,
                    likes: img.likes ?? 0,
                    views: img.views ?? 0,
                    uploadedAt: img.uploadedAt ?? img.createdAt ?? '',
                }))

                setImages(mapped)
            } catch (error) {
                console.error('Failed to fetch images:', error)
                setImages([])
                setError(error instanceof Error ? error.message : 'Failed to load gallery')
            } finally {
                setIsLoading(false)
            }
        }

        fetchImages()
    }, [])

    const getGameColor = (game: string) => {
        const colors: Record<string, string> = {
            'dark-souls': '#C89B64',
            'dark-souls-2': '#8B7355',
            'dark-souls-3': '#9B7ED9',
            'elden-ring': '#D4AF37',
            'bloodborne': '#DC3545',
            'sekiro': '#28A745'
        }
        return colors[game] || '#D4AF37'
    }

    const formatNumber = (num: number) => {
        if (num >= 1000) return (num / 1000).toFixed(1) + 'K'
        return num.toString()
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
                            <div className="w-10 h-10 rounded-lg bg-gradient-to-br from-[#6F42C1] to-[#4A2C7A] flex items-center justify-center shadow-lg shadow-[#6F42C1]/20">
                                <Library className="w-5 h-5 text-white" />
                            </div>
                            <div>
                                <h1 className="font-serif text-xl text-[#D4AF37] group-hover:text-[#E5C767] transition-colors">
                                    The Grand Archives
                                </h1>
                                <p className="text-[10px] text-gray-500 uppercase tracking-[0.3em]">Image Gallery</p>
                            </div>
                        </Link>

                        {/* Search Bar */}
                        <div className="flex-1 max-w-xl mx-8">
                            <div className="relative">
                                <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500" />
                                <input
                                    type="text"
                                    placeholder="Search images, artwork, wallpapers..."
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
                            <Link href="/wiki" className="flex items-center gap-2 px-4 py-2 text-gray-400 hover:text-[#D4AF37] transition-colors">
                                <BookOpen className="w-4 h-4" />
                                <span className="text-sm">Wiki</span>
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
                                </Link>
                            )}
                        </div>
                    </div>
                </div>
            </header>

            {/* Hero Section */}
            <section className="relative z-10 py-12 border-b border-[#3a352c]/30">
                <div className="max-w-7xl mx-auto px-4 text-center">
                    <h2 className="text-4xl md:text-5xl font-serif text-[#D4AF37] mb-4">The Grand Archives</h2>
                    <p className="text-gray-400 max-w-2xl mx-auto">
                        A vast collection of screenshots, artwork, wallpapers, and concept art from the worlds of Dark Souls,
                        Elden Ring, Bloodborne, and Sekiro. Explore the beauty and horror of FromSoftware&apos;s creations.
                    </p>
                </div>
            </section>

            {/* Filter Bar */}
            <div className="relative z-10 border-b border-[#3a352c]/30 bg-[#0f0e0c]/80 backdrop-blur-sm sticky top-0">
                <div className="max-w-7xl mx-auto px-4 py-4">
                    <div className="flex flex-wrap items-center justify-between gap-4">
                        {/* Category Filters */}
                        <div className="flex items-center gap-2 overflow-x-auto">
                            {categoryFilters.map((cat) => (
                                <button
                                    key={cat.id}
                                    onClick={() => setActiveCategory(cat.id)}
                                    className={`px-4 py-2 rounded-lg text-sm font-medium whitespace-nowrap transition-all ${
                                        activeCategory === cat.id
                                            ? 'bg-gradient-to-br from-[#2a2520] to-[#1a1714] border border-[#D4AF37]/50 text-[#D4AF37]'
                                            : 'bg-[#1a1714]/50 border border-transparent text-gray-400 hover:text-gray-200'
                                    }`}
                                >
                                    {cat.label}
                                </button>
                            ))}
                        </div>

                        {/* Game Filter & View Mode */}
                        <div className="flex items-center gap-3">
                            <select
                                value={activeGame}
                                onChange={(e) => setActiveGame(e.target.value)}
                                className="px-4 py-2 bg-[#1a1714] border border-[#3a352c] rounded-lg text-gray-200 text-sm focus:border-[#D4AF37]/50 focus:outline-none"
                            >
                                {gameFilters.map((game) => (
                                    <option key={game.id} value={game.id}>{game.label}</option>
                                ))}
                            </select>

                            <div className="flex items-center border border-[#3a352c] rounded-lg overflow-hidden">
                                <button
                                    onClick={() => setViewMode('grid')}
                                    className={`p-2 transition-colors ${viewMode === 'grid' ? 'bg-[#D4AF37]/20 text-[#D4AF37]' : 'text-gray-400 hover:text-gray-200'}`}
                                >
                                    <Grid3X3 className="w-5 h-5" />
                                </button>
                                <button
                                    onClick={() => setViewMode('masonry')}
                                    className={`p-2 transition-colors ${viewMode === 'masonry' ? 'bg-[#D4AF37]/20 text-[#D4AF37]' : 'text-gray-400 hover:text-gray-200'}`}
                                >
                                    <LayoutGrid className="w-5 h-5" />
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Main Content */}
            <main className="relative z-10 max-w-7xl mx-auto px-4 py-8">
                {/* Results Header */}
                <div className="flex items-center justify-between mb-6">
                    <p className="text-sm text-gray-500">
                        {filteredImages.length} {filteredImages.length === 1 ? 'image' : 'images'} found
                    </p>
                </div>

                {/* Image Grid */}
                {isLoading ? (
                    <div className="flex items-center justify-center h-64">
                        <div className="w-8 h-8 border-2 border-[#D4AF37] border-t-transparent rounded-full animate-spin" />
                    </div>
                ) : error ? (
                    <div className="flex flex-col items-center justify-center h-64 text-center">
                        <X className="w-12 h-12 text-red-500 mb-4" />
                        <h3 className="text-xl font-serif text-red-400">Failed to load gallery</h3>
                        <p className="text-gray-500 mt-2">{error}</p>
                        <button
                            onClick={() => window.location.reload()}
                            className="mt-4 px-4 py-2 bg-[#D4AF37]/20 text-[#D4AF37] rounded hover:bg-[#D4AF37]/30 transition-colors"
                        >
                            Try Again
                        </button>
                    </div>
                ) : paginatedImages.length === 0 ? (
                    <div className="flex flex-col items-center justify-center h-64 text-center">
                        <ImageIcon className="w-12 h-12 text-gray-600 mb-4" />
                        <h3 className="text-xl font-serif text-gray-400">No images found</h3>
                        <p className="text-gray-500 mt-2">Try adjusting your filters</p>
                    </div>
                ) : (
                    <div className={`grid gap-4 ${viewMode === 'grid' ? 'grid-cols-2 md:grid-cols-3 lg:grid-cols-4' : 'columns-2 md:columns-3 lg:columns-4 space-y-4'}`}>
                        {paginatedImages.map((image) => (
                            <button
                                key={image.id}
                                onClick={() => setSelectedImage(image)}
                                className={`group relative overflow-hidden rounded-xl border border-[#3a352c]/50 hover:border-[#D4AF37]/50 transition-all duration-300 text-left ${viewMode === 'masonry' ? 'break-inside-avoid mb-4' : ''}`}
                            >
                                {/* Image */}
                                <div className={`relative bg-[#1a1714] overflow-hidden ${viewMode === 'grid' ? 'aspect-[4/3]' : ''}`}>
                                    <div className="absolute inset-0 flex items-center justify-center">
                                        <ImageIcon className="w-12 h-12 text-gray-700" />
                                    </div>
                                    {image.imageUrl && (
                                        <img
                                            src={image.imageUrl}
                                            alt={image.title}
                                            className="relative z-10 w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                                            onError={(e) => {
                                                (e.target as HTMLImageElement).style.display = 'none'
                                            }}
                                        />
                                    )}

                                    {/* Overlay on hover */}
                                    <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/20 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300 z-20">
                                        <div className="absolute bottom-0 left-0 right-0 p-4">
                                            <h3 className="font-serif text-lg text-[#D4AF37] truncate">{image.title}</h3>
                                            <div className="flex items-center justify-between mt-2">
                                                <span
                                                    className="text-xs px-2 py-1 rounded uppercase"
                                                    style={{ backgroundColor: getGameColor(image.game) + '30', color: getGameColor(image.game) }}
                                                >
                                                    {image.game.replace('-', ' ')}
                                                </span>
                                                <div className="flex items-center gap-3 text-xs text-gray-400">
                                                    <span className="flex items-center gap-1">
                                                        <Heart className="w-3 h-3" />
                                                        {formatNumber(image.likes)}
                                                    </span>
                                                    <span className="flex items-center gap-1">
                                                        <Eye className="w-3 h-3" />
                                                        {formatNumber(image.views)}
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    {/* Expand icon */}
                                    <div className="absolute top-3 right-3 p-2 bg-black/50 rounded-lg opacity-0 group-hover:opacity-100 transition-opacity z-20">
                                        <Maximize2 className="w-4 h-4 text-white" />
                                    </div>
                                </div>
                            </button>
                        ))}
                    </div>
                )}

                {/* Pagination */}
                {totalPages > 1 && (
                    <div className="flex items-center justify-center gap-2 mt-8">
                        <button
                            onClick={() => setCurrentPage(p => Math.max(1, p - 1))}
                            disabled={currentPage === 1}
                            className="p-2 bg-[#1a1714] border border-[#3a352c] rounded-lg text-gray-400 hover:text-[#D4AF37] disabled:opacity-50 disabled:cursor-not-allowed"
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
                                        : 'bg-[#1a1714] border border-[#3a352c] text-gray-400 hover:text-[#D4AF37]'
                                }`}
                            >
                                {page}
                            </button>
                        ))}

                        <button
                            onClick={() => setCurrentPage(p => Math.min(totalPages, p + 1))}
                            disabled={currentPage === totalPages}
                            className="p-2 bg-[#1a1714] border border-[#3a352c] rounded-lg text-gray-400 hover:text-[#D4AF37] disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            <ChevronRight className="w-5 h-5" />
                        </button>
                    </div>
                )}
            </main>

            {/* Lightbox Modal */}
            {selectedImage && (
                <div
                    className="fixed inset-0 z-50 flex items-center justify-center bg-black/95 backdrop-blur-sm"
                    onClick={() => setSelectedImage(null)}
                >
                    <button
                        onClick={() => setSelectedImage(null)}
                        className="absolute top-4 right-4 p-3 text-gray-400 hover:text-white transition-colors z-50"
                    >
                        <X className="w-6 h-6" />
                    </button>

                    <div
                        className="relative max-w-6xl max-h-[90vh] mx-4"
                        onClick={(e) => e.stopPropagation()}
                    >
                        {/* Image */}
                        <div className="relative bg-[#1a1714] rounded-xl overflow-hidden">
                            <div className="flex items-center justify-center min-h-[300px]">
                                <ImageIcon className="w-16 h-16 text-gray-700" />
                            </div>
                            {selectedImage.imageUrl && (
                                <img
                                    src={selectedImage.imageUrl}
                                    alt={selectedImage.title}
                                    className="absolute inset-0 w-full h-full object-contain"
                                    onError={(e) => {
                                        (e.target as HTMLImageElement).style.display = 'none'
                                    }}
                                />
                            )}
                        </div>

                        {/* Info Panel */}
                        <div className="mt-4 p-4 bg-[#1a1714] border border-[#3a352c] rounded-xl">
                            <div className="flex items-start justify-between gap-4">
                                <div>
                                    <h2 className="text-2xl font-serif text-[#D4AF37]">{selectedImage.title}</h2>
                                    {selectedImage.description && (
                                        <p className="text-gray-400 mt-2">{selectedImage.description}</p>
                                    )}
                                    <div className="flex items-center gap-4 mt-3">
                                        <span
                                            className="text-xs px-3 py-1 rounded uppercase font-medium"
                                            style={{ backgroundColor: getGameColor(selectedImage.game) + '30', color: getGameColor(selectedImage.game) }}
                                        >
                                            {selectedImage.game.replace('-', ' ')}
                                        </span>
                                        <span className="text-xs text-gray-500 capitalize">{selectedImage.category}</span>
                                        {selectedImage.author && (
                                            <span className="text-xs text-gray-500">by {selectedImage.author}</span>
                                        )}
                                    </div>
                                </div>
                                <div className="flex items-center gap-3">
                                    <button className="flex items-center gap-2 px-4 py-2 bg-[#0a0908] border border-[#3a352c] rounded-lg text-gray-400 hover:text-red-400 hover:border-red-400/30 transition-colors">
                                        <Heart className="w-4 h-4" />
                                        <span className="text-sm">{formatNumber(selectedImage.likes)}</span>
                                    </button>
                                    <button className="flex items-center gap-2 px-4 py-2 bg-[#D4AF37] text-black rounded-lg hover:bg-[#E5C767] transition-colors">
                                        <Download className="w-4 h-4" />
                                        <span className="text-sm font-medium">Download</span>
                                    </button>
                                </div>
                            </div>

                            {/* Tags */}
                            {selectedImage.tags.length > 0 && (
                                <div className="flex flex-wrap gap-2 mt-4">
                                    {selectedImage.tags.map((tag) => (
                                        <span key={tag} className="px-2 py-1 bg-[#0a0908] rounded text-xs text-gray-400">
                                            #{tag}
                                        </span>
                                    ))}
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}
