"use client"

import { useState, useEffect } from 'react'
import Link from 'next/link'
import Image from 'next/image'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/contexts/auth-context'
import { Shield, LogOut } from 'lucide-react'
import {getImageUrl} from "@/lib/content-utils";

type BuildSet = {
    id: number
    name: string
    icon: string
}

type Build = {
    id: number
    game: string
    name: string
    level: number
    image: string
    stats: {
        vig: number
        end: number
        str: number
        dex: number
    }
    sets: BuildSet[]
}

export default function BuildsPage() {
    const router = useRouter()
    const { user, isLoading, logout } = useAuth()
    const [activeGame, setActiveGame] = useState('all')
    const [avatarCacheBuster, setAvatarCacheBuster] = useState(Date.now())
    const [selectedBuild, setSelectedBuild] = useState<Build | null>(null)

    useEffect(() => {
        if (!isLoading && !user) {
            router.push('/')
        }
    }, [user, isLoading, router])

    if (isLoading || !user) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-[#0a0a0a]">
                <div className="text-[#c4a456]">Loading...</div>
            </div>
        )
    }

    const handleLogout = () => {
        logout()
        router.push('/')
    }

    const games = [
        { id: 'all', name: 'All Games', icon: '/game-icons/all.jpg' },
        { id: 'ds1', name: 'Dark Souls 1', icon: '/game-icons/ds1.jpg' },
        { id: 'ds2', name: 'Dark Souls 2', icon: '/game-icons/ds2.jpg' },
        { id: 'ds3', name: 'Dark Souls 3', icon: '/game-icons/ds3.jpg' },
        { id: 'er', name: 'Elden Ring', icon: '/game-icons/er.png' },
    ]

    const buildsData: Build[] = [
        {
            id: 1, game: 'er', name: 'Giant Dad', level: 99,
            image: '/builds/giant.webp',
            stats: { vig: 48, end: 66, str: 16, dex: 10 },
            sets: [
                { id: 101, name: 'PVP Invasions', icon: '/sets/invasion.webp' },
                { id: 102, name: 'PVE Boss Rush', icon: '/sets/rush.jpg' },
                { id: 103, name: 'Farming Set', icon: '/sets/bull-goat.png' }
            ]
        },
        {
            id: 2, game: 'er', name: 'Moonveil Samurai', level: 150,
            image: '/builds/samurai.webp',
            stats: { vig: 60, end: 30, str: 12, dex: 30 },
            sets: [
                { id: 201, name: 'Glass Cannon', icon: '/sets/radahn-armor.jpg' },
                { id: 202, name: 'Balanced Poise', icon: '/sets/crucible-helm.webp' }
            ]
        },
        {
            id: 3, game: 'er', name: 'Hollow Bleed', level: 120,
            image: '/builds/malenia.webp',
            stats: { vig: 40, end: 40, str: 40, dex: 40 },
            sets: [
                { id: 301, name: 'Main Setup', icon: '/sets/blood.png' },
                { id: 302, name: 'Parry God', icon: '/sets/parry.jpg' },
                { id: 303, name: 'Bow Only', icon: '/sets/bow.png' }
            ]
        }
    ]

    const filteredBuilds = activeGame === 'all'
        ? buildsData
        : buildsData.filter(build => build.game === activeGame)

    return (
        <div className="flex h-screen bg-[#0a0a0a] text-gray-200 overflow-hidden">
            {/* Left Sidebar */}
            <aside className="w-72 bg-[#121212] border-r border-[#C89B64]/20 flex flex-col flex-shrink-0 z-10 shadow-[5px_0_15px_rgba(0,0,0,0.5)]">
                <div className="p-6 border-b border-[#C89B64]/20">
                    <Link
                        href="/profile"
                        className="flex items-center gap-4 hover:bg-white/5 transition-colors group rounded-lg p-2 -m-2"
                    >
                        <div className="w-14 h-14 rounded-full border-2 border-gray-600 group-hover:border-[#C89B64] transition-colors overflow-hidden bg-[#1a1a1a] flex items-center justify-center">
                            <img
                                // Використовуємо наш хелпер! Якщо avatarPath пустий, використовуємо ID юзера
                                src={getImageUrl(user?.avatarPath || `users/${user?.id}`, avatarCacheBuster)}
                                alt={user?.userName}
                                className={`w-full h-full object-cover transition-opacity`}
                                onError={(e) => {
                                    // Fallback, якщо зображення ще не існує
                                    (e.target as HTMLImageElement).src = "/placeholder-user.jpg"
                                }}
                            />
                        </div>
                        <div className="flex-1">
                            <p className="font-serif text-[#D4AF37] text-lg leading-tight group-hover:drop-shadow-[0_0_5px_rgba(212,175,55,0.8)] transition-all">
                                {user.firstName} {user.lastName}
                            </p>
                            <p className="text-xs text-gray-500 uppercase tracking-widest flex items-center gap-1">
                                {user.isAdmin && <Shield className="w-3 h-3 text-[#c4a456]" />}
                                {user.isAdmin ? 'Administrator' : 'Tarnished'}
                            </p>
                        </div>
                    </Link>
                    
                    {/* Logout button */}
                    <div className="mt-4">
                        <button
                            onClick={handleLogout}
                            className="w-full flex items-center justify-center gap-2 px-3 py-2 text-xs text-gray-400 border border-gray-700 hover:border-red-600/50 hover:text-red-400 rounded transition-colors uppercase tracking-wider"
                        >
                            <LogOut className="w-3 h-3" />
                            Logout
                        </button>
                    </div>
                </div>

                <div className="p-6 flex-1 overflow-y-auto">
                    <h3 className="text-xs text-gray-500 uppercase tracking-[0.2em] mb-4">Select Realm</h3>
                    <div className="flex flex-col gap-2">
                        {games.map(game => (
                            <button
                                key={game.id}
                                onClick={() => setActiveGame(game.id)}
                                className={`flex items-center gap-4 w-full text-left p-3 rounded-md transition-all ${
                                    activeGame === game.id
                                        ? 'bg-[#C89B64]/10 border border-[#C89B64]/50 text-[#D4AF37] shadow-[inset_0_0_10px_rgba(200,155,100,0.1)]'
                                        : 'border border-transparent text-gray-400 hover:bg-white/5 hover:text-gray-200'
                                }`}
                            >
                                <div className="w-8 h-8 rounded-sm bg-gray-800 border border-gray-700 flex items-center justify-center overflow-hidden">
                                    <span className="text-xs text-gray-500">{game.id.toUpperCase()}</span>
                                </div>
                                <span className="font-medium tracking-wide">{game.name}</span>
                            </button>
                        ))}
                    </div>
                </div>
            </aside>

            {/* Main Area */}
            <main
                className="flex-1 flex flex-col relative overflow-hidden bg-cover bg-center bg-no-repeat"
                style={{ backgroundImage: `url('${getImageUrl('builds/wallpaper')}')` }}
            >
                <header className="p-8 pb-4 flex justify-between items-end border-b-2 border-[#C89B64]/60 relative z-10">
                    <div>
                        <h1 className="text-4xl text-[#D4AF37] font-serif uppercase tracking-widest drop-shadow-[0_0_10px_rgba(212,175,55,0.2)]">
                            Armory
                        </h1>
                        <p className="text-gray-500 mt-2 text-sm">Showing {filteredBuilds.length} builds</p>
                    </div>
                    <Link
                        href="/editor/new"
                        className="px-6 py-3 bg-gradient-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.1em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all rounded-sm flex items-center gap-2"
                    >
                        <span className="text-xl leading-none">+</span> New Build
                    </Link>
                </header>

                <div className="flex-1 overflow-y-auto p-8 relative z-10">
                    <div className="grid grid-cols-1 xl:grid-cols-3 gap-8">
                        {filteredBuilds.map(build => (
                            <div
                                key={build.id}
                                onClick={() => setSelectedBuild(build)}
                                className="group relative bg-[#121212] border border-gray-800 hover:border-[#C89B64]/60 rounded-md overflow-hidden cursor-pointer transition-all duration-300 hover:shadow-[0_0_30px_rgba(200,155,100,0.15)] hover:-translate-y-1"
                            >
                                <div className="h-48 w-full relative overflow-hidden bg-gradient-to-br from-[#1a1a1a] to-[#0a0a0a]">
                                    <div className="absolute inset-0 bg-gradient-to-t from-[#121212] to-transparent z-10"></div>
                                    <div className="absolute inset-0 flex items-center justify-center">
                                        <span className="text-6xl text-gray-800 font-serif">{build.name[0]}</span>
                                    </div>
                                    <div className="absolute top-3 right-3 z-20 bg-black/80 border border-[#C89B64] px-2 py-1 rounded text-xs text-[#D4AF37] font-bold tracking-widest">
                                        LVL {build.level}
                                    </div>
                                </div>
                                <div className="p-5 relative z-20 -mt-8">
                                    <h3 className="font-serif text-2xl text-gray-200 group-hover:text-[#D4AF37] transition-colors mb-4 drop-shadow-md">
                                        {build.name}
                                    </h3>
                                    <div className="grid grid-cols-4 gap-2 border-t border-gray-800 pt-4">
                                        <div className="text-center">
                                            <p className="text-[10px] text-gray-500 uppercase tracking-wider mb-1">VIG</p>
                                            <p className="text-gray-300 font-medium">{build.stats.vig}</p>
                                        </div>
                                        <div className="text-center">
                                            <p className="text-[10px] text-gray-500 uppercase tracking-wider mb-1">END</p>
                                            <p className="text-gray-300 font-medium">{build.stats.end}</p>
                                        </div>
                                        <div className="text-center">
                                            <p className="text-[10px] text-gray-500 uppercase tracking-wider mb-1">STR</p>
                                            <p className="text-gray-300 font-medium">{build.stats.str}</p>
                                        </div>
                                        <div className="text-center">
                                            <p className="text-[10px] text-gray-500 uppercase tracking-wider mb-1">DEX</p>
                                            <p className="text-gray-300 font-medium">{build.stats.dex}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </main>

            {/* Modal for Set Selection */}
            {selectedBuild && (
                <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
                    <div
                        className="absolute inset-0 bg-black/80 backdrop-blur-sm cursor-pointer"
                        onClick={() => setSelectedBuild(null)}
                    ></div>

                    <div className="relative bg-[#1A1A1D] border border-[#C89B64]/50 shadow-[0_0_50px_rgba(0,0,0,1)] w-full max-w-3xl rounded-sm overflow-hidden z-10">
                        <div className="bg-[#121212] p-6 border-b border-gray-800 flex justify-between items-center">
                            <div>
                                <p className="text-xs text-[#C89B64] uppercase tracking-widest mb-1">Select Equipment Set for</p>
                                <h2 className="text-3xl text-gray-100 font-serif">{selectedBuild.name}</h2>
                            </div>
                            <button
                                onClick={() => setSelectedBuild(null)}
                                className="text-gray-500 hover:text-white text-3xl font-light transition-colors"
                            >
                                x
                            </button>
                        </div>

                        <div className="p-10">
                            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                                {selectedBuild.sets.map(set => (
                                    <Link
                                        href={`/editor/${set.id}`}
                                        key={set.id}
                                        className="group bg-black/40 border border-gray-700 hover:border-[#C89B64] p-8 flex flex-col items-center justify-center gap-4 transition-all hover:bg-black/60 hover:shadow-[0_0_20px_rgba(200,155,100,0.15)]"
                                    >
                                        <div className="w-16 h-16 rounded-md border border-gray-600 group-hover:border-[#D4AF37] group-hover:scale-110 transition-all shadow-lg bg-gray-800 flex items-center justify-center">
                                            <span className="text-2xl text-gray-600">{set.name[0]}</span>
                                        </div>
                                        <h3 className="text-gray-300 font-medium tracking-wide text-center group-hover:text-white mt-2">
                                            {set.name}
                                        </h3>
                                        <span className="text-xs text-[#C89B64] opacity-0 group-hover:opacity-100 transition-opacity uppercase tracking-widest mt-2 border-b border-[#C89B64]/30 pb-1">
                      Edit Loadout
                    </span>
                                    </Link>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}
