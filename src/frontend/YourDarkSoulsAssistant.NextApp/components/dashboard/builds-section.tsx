"use client"

import { useState, useEffect } from 'react'
import Link from 'next/link'

export type BuildData = {
    id: string | number;
    game: string;
    name: string;
    level: number;
    stats: { vig: number; end: number; str: number; dex: number };
}

// ФЕЙКОВИЙ API-КЛІЄНТ
async function fetchBuildsFromApi(gameId: string): Promise<BuildData[]> {
    return new Promise((resolve) => {
        setTimeout(() => {
            const mockData: BuildData[] = [
                { id: "1", game: 'er', name: 'Giant Dad', level: 99, stats: { vig: 48, end: 66, str: 16, dex: 10 } },
                { id: "2", game: 'er', name: 'Moonveil Samurai', level: 150, stats: { vig: 60, end: 30, str: 12, dex: 30 } },
                { id: "3", game: 'er', name: 'Hollow Bleed', level: 120, stats: { vig: 40, end: 40, str: 40, dex: 40 } }
            ]

            if (gameId === 'all') {
                resolve(mockData)
            } else {
                resolve(mockData.filter(b => b.game === gameId))
            }
        }, 600)
    })
}

export function DashboardBuilds({ activeGame }: { activeGame: string }) {
    const [builds, setBuilds] = useState<BuildData[]>([])
    const [isLoading, setIsLoading] = useState(true)

    useEffect(() => {
        let isMounted = true
        setIsLoading(true)

        fetchBuildsFromApi(activeGame)
            .then(data => {
                if (isMounted) {
                    setBuilds(data)
                    setIsLoading(false)
                }
            })
            .catch(error => {
                console.error("Failed to fetch builds:", error)
                if (isMounted) setIsLoading(false)
            })

        return () => { isMounted = false }
    }, [activeGame])

    return (
        <>
            <header className="p-8 pb-4 flex justify-between items-end border-b-2 border-[#C89B64]/60 relative z-10">
                <div>
                    <h1 className="text-4xl text-[#D4AF37] font-serif uppercase tracking-widest drop-shadow-[0_0_10px_rgba(212,175,55,0.2)]">
                        Armory
                    </h1>
                    <p className="text-gray-500 mt-2 text-sm">Showing {builds.length} builds</p>
                </div>
                <Link
                    href="/editor/new"
                    className="px-6 py-3 bg-gradient-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.1em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all rounded-sm flex items-center gap-2"
                >
                    <span className="text-xl leading-none">+</span> New Build
                </Link>
            </header>

            <div className="flex-1 overflow-y-auto p-8 relative z-10">
                {isLoading ? (
                    <div className="flex items-center justify-center h-full min-h-[50vh]">
                        <div className="text-[#c4a456]">Loading...</div>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 xl:grid-cols-3 gap-8">
                        {builds.map(build => (
                            <Link
                                key={build.id}
                                href={`/editor/${build.id}`}
                                className="group relative bg-[#121212] border border-gray-800 hover:border-[#C89B64]/60 rounded-md overflow-hidden transition-all duration-300 hover:shadow-[0_0_30px_rgba(200,155,100,0.15)] hover:-translate-y-1 block"
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
                            </Link>
                        ))}
                    </div>
                )}
            </div>
        </>
    )
}
