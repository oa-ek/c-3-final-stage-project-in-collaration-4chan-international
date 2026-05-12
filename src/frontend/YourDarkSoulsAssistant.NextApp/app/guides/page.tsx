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
    Play,
    Clock,
    Eye,
    ThumbsUp,
    ChevronLeft,
    ChevronRight,
    Filter,
    Sword,
    Shield,
    Skull,
    MapPin,
    Sparkles,
    Library,
    ExternalLink,
    Calendar
} from 'lucide-react'

// Types
interface VideoGuide {
    id: string
    title: string
    description: string
    thumbnailUrl: string
    videoUrl: string
    duration: string
    category: 'boss' | 'build' | 'location' | 'lore' | 'pvp' | 'tips' | 'walkthrough'
    game: 'dark-souls' | 'dark-souls-2' | 'dark-souls-3' | 'elden-ring' | 'bloodborne' | 'sekiro'
    author: string
    authorAvatar?: string
    views: number
    likes: number
    publishedAt: string
    tags: string[]
}

// Category configuration
const categoryFilters = [
    { id: 'all', label: 'All Guides', icon: <Sparkles className="w-4 h-4" /> },
    { id: 'boss', label: 'Boss Guides', icon: <Skull className="w-4 h-4" /> },
    { id: 'build', label: 'Build Guides', icon: <Sword className="w-4 h-4" /> },
    { id: 'location', label: 'Area Guides', icon: <MapPin className="w-4 h-4" /> },
    { id: 'walkthrough', label: 'Walkthroughs', icon: <BookOpen className="w-4 h-4" /> },
    { id: 'lore', label: 'Lore', icon: <BookOpen className="w-4 h-4" /> },
    { id: 'pvp', label: 'PvP', icon: <Shield className="w-4 h-4" /> },
    { id: 'tips', label: 'Tips & Tricks', icon: <Sparkles className="w-4 h-4" /> },
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

export default function GuidesPage() {
    const { user } = useAuth()
    const [searchQuery, setSearchQuery] = useState('')
    const [activeCategory, setActiveCategory] = useState('all')
    const [activeGame, setActiveGame] = useState('all')
    const [videos, setVideos] = useState<VideoGuide[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [selectedVideo, setSelectedVideo] = useState<VideoGuide | null>(null)
    const [currentPage, setCurrentPage] = useState(1)
    const [expandedDescriptions, setExpandedDescriptions] = useState<Set<string>>(new Set())

    // Додаємо стан для загальної кількості сторінок
    const [totalPages, setTotalPages] = useState(1)

    useEffect(() => {
        const fetchVideos = async () => {
            setIsLoading(true)
            try {
                // Будуємо URL з параметрами
                const url = new URL('/api/guides/YouTubeGuides/youtube-guides')
                url.searchParams.append('page', currentPage.toString())
                url.searchParams.append('pageSize', videosPerPage.toString())
                url.searchParams.append('category', activeCategory)

                if (searchQuery.trim()) {
                    url.searchParams.append('searchQuery', searchQuery)
                }

                const response = await fetch(url.toString())
                if (!response.ok) throw new Error('Помилка завантаження')

                const data = await response.json()

                // data - це наш PagedResult<VideoGuideEntity>
                setVideos(data.items)
                setTotalPages(data.totalPages) // Беремо кількість сторінок з бекенду

            } catch (error) {
                console.error('Failed to fetch videos:', error)
                setVideos([])
            }
            setIsLoading(false)
        }

        const timeoutId = setTimeout(() => {
            fetchVideos()
        }, 500)

        return () => clearTimeout(timeoutId)
    }, [searchQuery, activeCategory, currentPage]) // Перезапускаємо при зміні будь-якого фільтра
    const toggleDescription = (videoId: string) => {
        setExpandedDescriptions(prev => {
            const newSet = new Set(prev)
            if (newSet.has(videoId)) {
                newSet.delete(videoId)
            } else {
                newSet.add(videoId)
            }
            return newSet
        })
    }
    const videosPerPage = 8

    // Filter videos
    const filteredVideos = videos.filter(video => {
        const matchesSearch = video.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
            video.description.toLowerCase().includes(searchQuery.toLowerCase()) ||
            video.tags.some(tag => tag.toLowerCase().includes(searchQuery.toLowerCase()))
        const matchesCategory = activeCategory === 'all' || video.category === activeCategory
        const matchesGame = activeGame === 'all' || video.game === activeGame
        return matchesSearch && matchesCategory && matchesGame
    })

    // Pagination
    const paginatedVideos = filteredVideos.slice(
        (currentPage - 1) * videosPerPage,
        currentPage * videosPerPage
    )

    useEffect(() => {
        setCurrentPage(1)
    }, [searchQuery, activeCategory, activeGame])

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

    const getCategoryIcon = (category: string) => {
        const cat = categoryFilters.find(c => c.id === category)
        return cat?.icon || <Sparkles className="w-4 h-4" />
    }

    const formatNumber = (num: number) => {
        if (num >= 1000000) return (num / 1000000).toFixed(1) + 'M'
        if (num >= 1000) return (num / 1000).toFixed(1) + 'K'
        return num.toString()
    }

    const formatDate = (dateStr: string) => {
        const date = new Date(dateStr)
        return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' })
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
                            <div className="w-10 h-10 rounded-lg bg-gradient-to-br from-[#DC3545] to-[#8B2232] flex items-center justify-center shadow-lg shadow-[#DC3545]/20">
                                <Play className="w-5 h-5 text-white" />
                            </div>
                            <div>
                                <h1 className="font-serif text-xl text-[#D4AF37] group-hover:text-[#E5C767] transition-colors">
                                    Video Guides
                                </h1>
                                <p className="text-[10px] text-gray-500 uppercase tracking-[0.3em]">Learn from the Masters</p>
                            </div>
                        </Link>

                        {/* Search Bar */}
                        <div className="flex-1 max-w-xl mx-8">
                            <div className="relative">
                                <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500" />
                                <input
                                    type="text"
                                    placeholder="Search boss guides, builds, walkthroughs..."
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
                            <Link href="/archive" className="flex items-center gap-2 px-4 py-2 text-gray-400 hover:text-[#D4AF37] transition-colors">
                                <Library className="w-4 h-4" />
                                <span className="text-sm">Archive</span>
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
                    <h2 className="text-4xl md:text-5xl font-serif text-[#D4AF37] mb-4">Video Guides</h2>
                    <p className="text-gray-400 max-w-2xl mx-auto">
                        Master every boss, discover hidden secrets, and perfect your builds with our curated collection
                        of video guides from the Souls community&apos;s best content creators.
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
                                    className={`flex items-center gap-2 px-4 py-2 rounded-lg text-sm font-medium whitespace-nowrap transition-all ${
                                        activeCategory === cat.id
                                            ? 'bg-gradient-to-br from-[#2a2520] to-[#1a1714] border border-[#D4AF37]/50 text-[#D4AF37]'
                                            : 'bg-[#1a1714]/50 border border-transparent text-gray-400 hover:text-gray-200'
                                    }`}
                                >
                                    {cat.icon}
                                    {cat.label}
                                </button>
                            ))}
                        </div>

                        {/* Game Filter */}
                        <select
                            value={activeGame}
                            onChange={(e) => setActiveGame(e.target.value)}
                            className="px-4 py-2 bg-[#1a1714] border border-[#3a352c] rounded-lg text-gray-200 text-sm focus:border-[#D4AF37]/50 focus:outline-none"
                        >
                            {gameFilters.map((game) => (
                                <option key={game.id} value={game.id}>{game.label}</option>
                            ))}
                        </select>
                    </div>
                </div>
            </div>

            {/* Main Content */}
            <main className="relative z-10 max-w-7xl mx-auto px-4 py-8">
                {/* Results Header */}
                <div className="flex items-center justify-between mb-6">
                    <p className="text-sm text-gray-500">
                        {filteredVideos.length} {filteredVideos.length === 1 ? 'video' : 'videos'} found
                    </p>
                </div>

                {/* Video Grid */}
                {isLoading ? (
                    <div className="flex items-center justify-center h-64">
                        <div className="w-8 h-8 border-2 border-[#D4AF37] border-t-transparent rounded-full animate-spin" />
                    </div>
                ) : error ? (
                    <div className="flex flex-col items-center justify-center h-64 text-center">
                        <X className="w-12 h-12 text-red-500 mb-4" />
                        <h3 className="text-xl font-serif text-red-400">Error loading videos</h3>
                        <p className="text-gray-500 mt-2">{error}</p>
                        <button
                            onClick={() => window.location.reload()}
                            className="mt-4 px-4 py-2 bg-[#D4AF37]/20 text-[#D4AF37] rounded hover:bg-[#D4AF37]/30 transition-colors"
                        >
                            Try Again
                        </button>
                    </div>
                ) : paginatedVideos.length === 0 ? (
                    <div className="flex flex-col items-center justify-center h-64 text-center">
                        <Play className="w-12 h-12 text-gray-600 mb-4" />
                        <h3 className="text-xl font-serif text-gray-400">No videos found</h3>
                        <p className="text-gray-500 mt-2">Try adjusting your filters</p>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                        {paginatedVideos.map((video) => (
                            <article
                                key={video.id}
                                className="group flex flex-col bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl overflow-hidden hover:border-[#D4AF37]/50 transition-all duration-300 h-full"
                            >
                                {/* Thumbnail */}
                                <div className="relative aspect-video bg-[#0a0908] overflow-hidden">
                                    <div className="absolute inset-0 flex items-center justify-center">
                                        <Play className="w-12 h-12 text-gray-700" />
                                    </div>
                                    {video.thumbnailUrl && (
                                        <img
                                            src={video.thumbnailUrl}
                                            alt={video.title}
                                            className="relative z-10 w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                                            onError={(e) => {
                                                (e.target as HTMLImageElement).style.display = 'none'
                                            }}
                                        />
                                    )}

                                    {/* Play overlay */}
                                    <div className="absolute inset-0 flex items-center justify-center bg-black/30 opacity-0 group-hover:opacity-100 transition-opacity z-20">
                                        <div className="w-16 h-16 rounded-full bg-[#D4AF37] flex items-center justify-center">
                                            <Play className="w-8 h-8 text-black ml-1" fill="currentColor" />
                                        </div>
                                    </div>

                                    {/* Duration badge */}
                                    <div className="absolute bottom-2 right-2 px-2 py-1 bg-black/80 rounded text-xs text-white z-20">
                                        {video.duration}
                                    </div>

                                    {/* Category badge */}
                                    <div
                                        className="absolute top-2 left-2 px-2 py-1 rounded text-[10px] uppercase tracking-wider font-medium z-20 flex items-center gap-1"
                                        style={{ backgroundColor: getGameColor(video.game) + '90', color: '#fff' }}
                                    >
                                        {getCategoryIcon(video.category)}
                                        {video.category}
                                    </div>
                                </div>

                                {/* Content */}
                                <div className="flex-1 flex flex-col p-4">
                                    <h3 className="font-serif text-lg text-[#D4AF37] group-hover:text-[#E5C767] transition-colors line-clamp-2 mb-2">
                                        {video.title}
                                    </h3>

                                    <div className="mb-3">
                                        <p className={`text-sm text-gray-400 ${expandedDescriptions.has(video.id) ? '' : 'line-clamp-2'}`}>
                                            {video.description}
                                        </p>
                                        {video.description.length > 100 && (
                                            <button
                                                onClick={() => toggleDescription(video.id)}
                                                className="text-xs text-[#D4AF37] hover:text-[#E5C767] mt-1 transition-colors"
                                            >
                                                {expandedDescriptions.has(video.id) ? 'Show less' : '...Show more'}
                                            </button>
                                        )}
                                    </div>

                                    {/* Author */}
                                    <div className="flex items-center justify-between">
                                        <span className="text-xs text-gray-500">{video.author}</span>
                                        <span
                                            className="text-xs px-2 py-1 rounded"
                                            style={{ backgroundColor: getGameColor(video.game) + '20', color: getGameColor(video.game) }}
                                        >
                                            {video.game.replace(/-/g, ' ')}
                                        </span>
                                    </div>

                                    {/* Stats - push to bottom */}
                                    <div className="flex items-center gap-4 mt-auto pt-3 border-t border-[#3a352c]/30">
                                        <span className="flex items-center gap-1 text-xs text-gray-500">
                                            <Eye className="w-3 h-3" />
                                            {formatNumber(video.views)}
                                        </span>
                                        <span className="flex items-center gap-1 text-xs text-gray-500">
                                            <ThumbsUp className="w-3 h-3" />
                                            {formatNumber(video.likes)}
                                        </span>
                                        <span className="flex items-center gap-1 text-xs text-gray-500 ml-auto">
                                            <Calendar className="w-3 h-3" />
                                            {formatDate(video.publishedAt)}
                                        </span>
                                    </div>
                                </div>

                                {/* Watch button - always at bottom */}
                                <div className="px-4 pb-4 mt-auto">
                                    <a
                                        href={video.videoUrl}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="flex items-center justify-center gap-2 w-full py-2 bg-[#DC3545] text-white rounded-lg hover:bg-[#c82333] transition-colors text-sm font-medium"
                                    >
                                        <Play className="w-4 h-4" />
                                        Watch Video
                                        <ExternalLink className="w-3 h-3" />
                                    </a>
                                </div>
                            </article>
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
        </div>
    )
}
