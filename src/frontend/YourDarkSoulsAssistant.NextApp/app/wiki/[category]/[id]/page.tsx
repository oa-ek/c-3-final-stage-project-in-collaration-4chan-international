"use client"

import { useState, useEffect } from 'react'
import Link from 'next/link'
import { useParams, useRouter } from 'next/navigation'
import { useAuth } from '@/contexts/auth-context'
import {
    ChevronLeft,
    Sword,
    Shield,
    Gem,
    Skull,
    BookOpen,
    MapPin,
    Sparkles,
    Home,
    User,
    ExternalLink,
    Share2,
    Bookmark,
    ChevronRight
} from 'lucide-react'

// Types
type CategoryType = 'weapons' | 'armor' | 'talismans' | 'bosses' | 'guides' | 'locations' | 'magic'

interface WikiItemDetail {
    id: string
    name: string
    category: CategoryType
    subcategory?: string
    image?: string
    description: string
    lore?: string

    // Common stats
    weight?: number

    // Weapon specific
    attackPower?: {
        physical: number
        magic: number
        fire: number
        lightning: number
        holy: number
        critical: number
    }
    guardedDamageNegation?: {
        physical: number
        magic: number
        fire: number
        lightning: number
        holy: number
        guardBoost: number
    }
    attributeScaling?: {
        str: string
        dex: string
        int: string
        fai: string
        arc: string
    }
    attributesRequired?: {
        str: number
        dex: number
        int: number
        fai: number
        arc: number
    }
    passiveEffects?: string[]
    skillName?: string
    skillFpCost?: number

    // Armor specific
    damageNegation?: {
        physical: number
        strike: number
        slash: number
        pierce: number
        magic: number
        fire: number
        lightning: number
        holy: number
    }
    resistance?: {
        immunity: number
        robustness: number
        focus: number
        vitality: number
        poise: number
    }

    // Boss specific
    hp?: number
    defense?: number
    stance?: number
    difficulty?: string
    rewards?: string[]
    drops?: { name: string; dropRate: string }[]
    weaknesses?: string[]
    resistances?: string[]
    phases?: { name: string; description: string }[]
    strategies?: string[]

    // Guide specific
    sections?: { title: string; content: string }[]
    tips?: string[]
    videoUrl?: string

    // Location specific
    enemies?: string[]
    items?: string[]
    npcs?: string[]
    connectedAreas?: string[]

    // Magic specific
    fpCost?: number
    slots?: number
    effects?: string[]

    // General
    location?: string
    howToObtain?: string
    relatedItems?: { id: string; name: string; category: CategoryType }[]
}

const categoryConfig: Record<CategoryType, { icon: React.ReactNode; color: string; label: string }> = {
    weapons: { icon: <Sword className="w-5 h-5" />, color: '#C89B64', label: 'Weapon' },
    armor: { icon: <Shield className="w-5 h-5" />, color: '#8B7355', label: 'Armor' },
    talismans: { icon: <Gem className="w-5 h-5" />, color: '#9B7ED9', label: 'Talisman' },
    bosses: { icon: <Skull className="w-5 h-5" />, color: '#DC3545', label: 'Boss' },
    guides: { icon: <BookOpen className="w-5 h-5" />, color: '#28A745', label: 'Guide' },
    locations: { icon: <MapPin className="w-5 h-5" />, color: '#17A2B8', label: 'Location' },
    magic: { icon: <Sparkles className="w-5 h-5" />, color: '#6F42C1', label: 'Magic' },
}

export default function WikiDetailPage() {
    const params = useParams()
    const router = useRouter()
    const { user } = useAuth()
    const [item, setItem] = useState<WikiItemDetail | null>(null)
    const [isLoading, setIsLoading] = useState(true)
    const [isSaved, setIsSaved] = useState(false)

    const category = params.category as CategoryType
    const id = params.id as string

    useEffect(() => {
        const fetchItem = async () => {
            setIsLoading(true)

            try {
                // Викликаємо наш новий ASP.NET ендпоінт
                const response = await fetch(`/api/articles/wiki/${category}/${id}`)

                if (!response.ok) {
                    throw new Error('Item not found')
                }

                const data = await response.json()

                // Мапимо дані з бекенду (EnrichedWikiItemDto) у формат, який чекає інтерфейс
                const mappedItem: WikiItemDetail = {
                    id: data.id,
                    name: data.name,
                    category: data.category as CategoryType,
                    description: data.description,
                    lore: data.lore || 'No lore found in the archives...',
                    image: data.image || '/placeholder-image.png',

                    // ASP.NET автоматично перетворить наш jsonb (Stats) у звичайний JS об'єкт!
                    // Тому ми можемо просто присвоїти ці поля, якщо вони є в БД
                    hp: data.stats?.HP,
                    defense: data.stats?.Defense,
                    stance: data.stats?.Stance,

                    // Відео можна вивести в окремий компонент або секцію
                    videoUrl: data.videos?.[0]?.videoUrl
                }

                setItem(mappedItem)
            } catch (error) {
                console.error("Failed to load Wiki info:", error)
                setItem(null)
            } finally {
                setIsLoading(false)
            }
        }

        fetchItem()
    }, [category, id])

    const config = categoryConfig[category] || categoryConfig.weapons

    if (isLoading) {
        return (
            <div className="min-h-screen bg-[#0a0908] flex items-center justify-center">
                <div className="w-8 h-8 border-2 border-[#D4AF37] border-t-transparent rounded-full animate-spin" />
            </div>
        )
    }

    if (!item) {
        return (
            <div className="min-h-screen bg-[#0a0908] flex flex-col items-center justify-center text-center p-4">
                <Skull className="w-16 h-16 text-gray-600 mb-4" />
                <h1 className="text-2xl font-serif text-[#D4AF37]">Item Not Found</h1>
                <p className="text-gray-500 mt-2">The requested item could not be found in the archives.</p>
                <Link
                    href="/wiki"
                    className="mt-6 px-6 py-3 bg-gradient-to-br from-[#D4AF37] to-[#8B6914] text-black font-medium rounded-lg hover:from-[#E5C767] hover:to-[#A68B1B] transition-all"
                >
                    Return to Wiki
                </Link>
            </div>
        )
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
            <header className="relative z-10 border-b border-[#3a352c]/50 bg-[#0f0e0c]/80 backdrop-blur-sm sticky top-0">
                <div className="max-w-6xl mx-auto px-4 py-4">
                    <div className="flex items-center justify-between">
                        <div className="flex items-center gap-4">
                            <button
                                onClick={() => router.back()}
                                className="flex items-center gap-2 text-gray-400 hover:text-[#D4AF37] transition-colors"
                            >
                                <ChevronLeft className="w-5 h-5" />
                                <span className="text-sm">Back</span>
                            </button>
                            <div className="w-px h-6 bg-[#3a352c]" />
                            <Link href="/wiki" className="flex items-center gap-2 text-gray-400 hover:text-[#D4AF37] transition-colors">
                                <BookOpen className="w-4 h-4" />
                                <span className="text-sm">Wiki</span>
                            </Link>
                            <ChevronRight className="w-4 h-4 text-gray-600" />
                            <span className="text-sm text-gray-500 capitalize">{category}</span>
                        </div>

                        <div className="flex items-center gap-3">
                            <button
                                onClick={() => setIsSaved(!isSaved)}
                                className={`p-2 rounded-lg transition-colors ${isSaved ? 'text-[#D4AF37] bg-[#D4AF37]/10' : 'text-gray-400 hover:text-[#D4AF37]'}`}
                            >
                                <Bookmark className="w-5 h-5" fill={isSaved ? 'currentColor' : 'none'} />
                            </button>
                            <button className="p-2 text-gray-400 hover:text-[#D4AF37] rounded-lg transition-colors">
                                <Share2 className="w-5 h-5" />
                            </button>
                            {user && (
                                <Link
                                    href="/profile"
                                    className="flex items-center gap-2 px-3 py-2 text-gray-400 hover:text-[#D4AF37] transition-colors"
                                >
                                    <User className="w-4 h-4" />
                                </Link>
                            )}
                        </div>
                    </div>
                </div>
            </header>

            {/* Hero Section */}
            <section className="relative z-10">
                <div className="relative h-80 bg-gradient-to-b from-[#1a1714] to-[#0a0908] overflow-hidden">
                    {/* Large background icon */}
                    <div className="absolute inset-0 flex items-center justify-center opacity-5">
                        <div className="w-96 h-96" style={{ color: config.color }}>
                            {config.icon}
                        </div>
                    </div>

                    {/* Item image */}
                    {item.image && (
                        <div className="absolute inset-0 flex items-center justify-center">
                            <img
                                src={item.image}
                                alt={item.name}
                                className="max-h-64 object-contain drop-shadow-2xl"
                                onError={(e) => {
                                    (e.target as HTMLImageElement).style.display = 'none'
                                }}
                            />
                        </div>
                    )}

                    {/* Gradient overlay */}
                    <div className="absolute inset-x-0 bottom-0 h-40 bg-gradient-to-t from-[#0a0908] to-transparent" />
                </div>
            </section>

            {/* Main Content */}
            <main className="relative z-10 max-w-6xl mx-auto px-4 -mt-20">
                {/* Title Section */}
                <div className="mb-8">
                    <div
                        className="inline-flex items-center gap-2 px-4 py-2 rounded-full text-sm font-medium mb-4"
                        style={{
                            backgroundColor: config.color + '20',
                            color: config.color
                        }}
                    >
                        {config.icon}
                        {config.label}
                        {item.subcategory && (
                            <>
                                <span className="opacity-50">•</span>
                                {item.subcategory}
                            </>
                        )}
                    </div>

                    <h1 className="text-4xl md:text-5xl font-serif text-[#D4AF37] mb-4">{item.name}</h1>

                    {item.location && (
                        <div className="flex items-center gap-2 text-gray-400">
                            <MapPin className="w-4 h-4" />
                            <span>{item.location}</span>
                        </div>
                    )}
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                    {/* Left Column - Main Content */}
                    <div className="lg:col-span-2 space-y-8">
                        {/* Description */}
                        <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                            <h2 className="text-xl font-serif text-[#D4AF37] mb-4">Description</h2>
                            <p className="text-gray-300 leading-relaxed">{item.description}</p>
                        </section>

                        {/* Lore */}
                        {item.lore && (
                            <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                <h2 className="text-xl font-serif text-[#D4AF37] mb-4">Lore</h2>
                                <p className="text-gray-400 leading-relaxed italic">{item.lore}</p>
                            </section>
                        )}

                        {/* Weapon Stats */}
                        {category === 'weapons' && item.attackPower && (
                            <>
                                {/* Attack Power Table */}
                                <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                    <h2 className="text-xl font-serif text-[#D4AF37] mb-4">Attack Power</h2>
                                    <div className="grid grid-cols-3 md:grid-cols-6 gap-4">
                                        {Object.entries(item.attackPower).map(([stat, value]) => (
                                            <div key={stat} className="text-center p-3 bg-[#0a0908] rounded-lg">
                                                <p className="text-xs text-gray-500 uppercase tracking-wider mb-1">{stat}</p>
                                                <p className="text-lg font-medium text-[#D4AF37]">{value}</p>
                                            </div>
                                        ))}
                                    </div>
                                </section>

                                {/* Guard Stats */}
                                {item.guardedDamageNegation && (
                                    <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                        <h2 className="text-xl font-serif text-[#D4AF37] mb-4">Guarded Damage Negation</h2>
                                        <div className="grid grid-cols-3 md:grid-cols-6 gap-4">
                                            {Object.entries(item.guardedDamageNegation).map(([stat, value]) => (
                                                <div key={stat} className="text-center p-3 bg-[#0a0908] rounded-lg">
                                                    <p className="text-xs text-gray-500 uppercase tracking-wider mb-1">{stat.replace(/([A-Z])/g, ' $1').trim()}</p>
                                                    <p className="text-lg font-medium text-gray-300">{value}</p>
                                                </div>
                                            ))}
                                        </div>
                                    </section>
                                )}

                                {/* Scaling & Requirements */}
                                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                    {item.attributeScaling && (
                                        <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                            <h2 className="text-lg font-serif text-[#D4AF37] mb-4">Attribute Scaling</h2>
                                            <div className="grid grid-cols-5 gap-2">
                                                {Object.entries(item.attributeScaling).map(([attr, grade]) => (
                                                    <div key={attr} className="text-center p-2 bg-[#0a0908] rounded-lg">
                                                        <p className="text-xs text-gray-500 uppercase">{attr}</p>
                                                        <p className={`text-lg font-bold ${grade === '-' ? 'text-gray-600' : 'text-[#D4AF37]'}`}>{grade}</p>
                                                    </div>
                                                ))}
                                            </div>
                                        </section>
                                    )}

                                    {item.attributesRequired && (
                                        <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                            <h2 className="text-lg font-serif text-[#D4AF37] mb-4">Attributes Required</h2>
                                            <div className="grid grid-cols-5 gap-2">
                                                {Object.entries(item.attributesRequired).map(([attr, value]) => (
                                                    <div key={attr} className="text-center p-2 bg-[#0a0908] rounded-lg">
                                                        <p className="text-xs text-gray-500 uppercase">{attr}</p>
                                                        <p className={`text-lg font-medium ${value === 0 ? 'text-gray-600' : 'text-gray-300'}`}>{value || '-'}</p>
                                                    </div>
                                                ))}
                                            </div>
                                        </section>
                                    )}
                                </div>
                            </>
                        )}

                        {/* Armor Stats */}
                        {category === 'armor' && (
                            <>
                                {item.damageNegation && (
                                    <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                        <h2 className="text-xl font-serif text-[#D4AF37] mb-4">Damage Negation</h2>
                                        <div className="grid grid-cols-4 md:grid-cols-8 gap-3">
                                            {Object.entries(item.damageNegation).map(([stat, value]) => (
                                                <div key={stat} className="text-center p-3 bg-[#0a0908] rounded-lg">
                                                    <p className="text-[10px] text-gray-500 uppercase tracking-wider mb-1">{stat}</p>
                                                    <p className="text-lg font-medium text-[#D4AF37]">{value}</p>
                                                </div>
                                            ))}
                                        </div>
                                    </section>
                                )}

                                {item.resistance && (
                                    <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                        <h2 className="text-xl font-serif text-[#D4AF37] mb-4">Resistance</h2>
                                        <div className="grid grid-cols-5 gap-3">
                                            {Object.entries(item.resistance).map(([stat, value]) => (
                                                <div key={stat} className="text-center p-3 bg-[#0a0908] rounded-lg">
                                                    <p className="text-[10px] text-gray-500 uppercase tracking-wider mb-1">{stat}</p>
                                                    <p className={`text-lg font-medium ${stat === 'poise' ? 'text-[#D4AF37]' : 'text-gray-300'}`}>{value}</p>
                                                </div>
                                            ))}
                                        </div>
                                    </section>
                                )}
                            </>
                        )}

                        {/* Boss Content */}
                        {category === 'bosses' && (
                            <>
                                {/* Boss Phases */}
                                {item.phases && (
                                    <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                        <h2 className="text-xl font-serif text-[#D4AF37] mb-4">Phases</h2>
                                        <div className="space-y-4">
                                            {item.phases.map((phase, i) => (
                                                <div key={i} className="p-4 bg-[#0a0908] rounded-lg border-l-2 border-[#DC3545]">
                                                    <h3 className="font-medium text-gray-200 mb-2">{phase.name}</h3>
                                                    <p className="text-gray-400 text-sm">{phase.description}</p>
                                                </div>
                                            ))}
                                        </div>
                                    </section>
                                )}

                                {/* Strategies */}
                                {item.strategies && (
                                    <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                        <h2 className="text-xl font-serif text-[#D4AF37] mb-4">Strategies</h2>
                                        <ul className="space-y-3">
                                            {item.strategies.map((strategy, i) => (
                                                <li key={i} className="flex gap-3 text-gray-300">
                                                    <span className="text-[#D4AF37] font-medium">{i + 1}.</span>
                                                    {strategy}
                                                </li>
                                            ))}
                                        </ul>
                                    </section>
                                )}
                            </>
                        )}

                        {/* How to Obtain */}
                        {item.howToObtain && (
                            <section className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                <h2 className="text-xl font-serif text-[#D4AF37] mb-4">How to Obtain</h2>
                                <p className="text-gray-300">{item.howToObtain}</p>
                            </section>
                        )}
                    </div>

                    {/* Right Column - Sidebar */}
                    <div className="space-y-6">
                        {/* Quick Stats Card */}
                        <div className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6 sticky top-24">
                            <h3 className="text-lg font-serif text-[#D4AF37] mb-4">Quick Info</h3>

                            <div className="space-y-3">
                                {item.weight !== undefined && (
                                    <div className="flex justify-between py-2 border-b border-[#3a352c]/30">
                                        <span className="text-gray-500">Weight</span>
                                        <span className="text-gray-300">{item.weight}</span>
                                    </div>
                                )}

                                {item.skillName && (
                                    <div className="flex justify-between py-2 border-b border-[#3a352c]/30">
                                        <span className="text-gray-500">Skill</span>
                                        <span className="text-[#D4AF37]">{item.skillName}</span>
                                    </div>
                                )}

                                {item.skillFpCost && (
                                    <div className="flex justify-between py-2 border-b border-[#3a352c]/30">
                                        <span className="text-gray-500">FP Cost</span>
                                        <span className="text-gray-300">{item.skillFpCost}</span>
                                    </div>
                                )}

                                {item.difficulty && (
                                    <div className="flex justify-between py-2 border-b border-[#3a352c]/30">
                                        <span className="text-gray-500">Difficulty</span>
                                        <span className="text-[#DC3545] font-medium">{item.difficulty}</span>
                                    </div>
                                )}

                                {item.hp && (
                                    <div className="flex justify-between py-2 border-b border-[#3a352c]/30">
                                        <span className="text-gray-500">HP</span>
                                        <span className="text-gray-300">{item.hp.toLocaleString()}</span>
                                    </div>
                                )}
                            </div>

                            {/* Passive Effects */}
                            {item.passiveEffects && item.passiveEffects.length > 0 && (
                                <div className="mt-4 pt-4 border-t border-[#3a352c]/30">
                                    <p className="text-xs text-gray-500 uppercase tracking-wider mb-2">Passive Effects</p>
                                    <ul className="space-y-1">
                                        {item.passiveEffects.map((effect, i) => (
                                            <li key={i} className="text-sm text-[#9B7ED9]">{effect}</li>
                                        ))}
                                    </ul>
                                </div>
                            )}

                            {/* Boss Weaknesses/Resistances */}
                            {item.weaknesses && (
                                <div className="mt-4 pt-4 border-t border-[#3a352c]/30">
                                    <p className="text-xs text-gray-500 uppercase tracking-wider mb-2">Weaknesses</p>
                                    <div className="flex flex-wrap gap-2">
                                        {item.weaknesses.map((weakness, i) => (
                                            <span key={i} className="text-xs px-2 py-1 bg-green-900/30 text-green-400 rounded">{weakness}</span>
                                        ))}
                                    </div>
                                </div>
                            )}

                            {item.resistances && (
                                <div className="mt-4 pt-4 border-t border-[#3a352c]/30">
                                    <p className="text-xs text-gray-500 uppercase tracking-wider mb-2">Resistances</p>
                                    <div className="flex flex-wrap gap-2">
                                        {item.resistances.map((res, i) => (
                                            <span key={i} className="text-xs px-2 py-1 bg-red-900/30 text-red-400 rounded">{res}</span>
                                        ))}
                                    </div>
                                </div>
                            )}

                            {/* Rewards/Drops */}
                            {item.rewards && (
                                <div className="mt-4 pt-4 border-t border-[#3a352c]/30">
                                    <p className="text-xs text-gray-500 uppercase tracking-wider mb-2">Rewards</p>
                                    <ul className="space-y-1">
                                        {item.rewards.map((reward, i) => (
                                            <li key={i} className="text-sm text-[#D4AF37]">{reward}</li>
                                        ))}
                                    </ul>
                                </div>
                            )}
                        </div>

                        {/* Related Items */}
                        {item.relatedItems && item.relatedItems.length > 0 && (
                            <div className="bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c]/50 rounded-xl p-6">
                                <h3 className="text-lg font-serif text-[#D4AF37] mb-4">Related Items</h3>
                                <div className="space-y-2">
                                    {item.relatedItems.map((related) => (
                                        <Link
                                            key={related.id}
                                            href={`/wiki/${related.category}/${related.id}`}
                                            className="flex items-center gap-3 p-3 bg-[#0a0908] rounded-lg hover:bg-[#1a1714] transition-colors group"
                                        >
                                            <div
                                                className="w-8 h-8 rounded-lg flex items-center justify-center"
                                                style={{ backgroundColor: categoryConfig[related.category]?.color + '20' }}
                                            >
                                                {categoryConfig[related.category]?.icon}
                                            </div>
                                            <span className="text-gray-300 group-hover:text-[#D4AF37] transition-colors">{related.name}</span>
                                            <ExternalLink className="w-4 h-4 text-gray-600 ml-auto" />
                                        </Link>
                                    ))}
                                </div>
                            </div>
                        )}
                    </div>
                </div>

                {/* Footer Navigation */}
                <div className="flex justify-center gap-4 py-12">
                    <Link
                        href="/wiki"
                        className="px-6 py-3 bg-gradient-to-br from-[#1a1714] to-[#0f0e0c] border border-[#3a352c] rounded-lg text-gray-400 hover:text-[#D4AF37] hover:border-[#D4AF37]/50 transition-all"
                    >
                        Back to Wiki
                    </Link>
                    <Link
                        href="/builds"
                        className="px-6 py-3 bg-gradient-to-br from-[#D4AF37] to-[#8B6914] text-black font-medium rounded-lg hover:from-[#E5C767] hover:to-[#A68B1B] transition-all"
                    >
                        Go to Builds
                    </Link>
                </div>
            </main>
        </div>
    )
}
