"use client"

import Link from 'next/link'
import { useHomePage } from '@/hooks/use-home-page'
import { DashboardUserCard } from '@/components/dashboard/user-card'
import { DashboardResources } from '@/components/dashboard/resources-block'
import { DashboardGames } from '@/components/dashboard/games-section'
import { DashboardBuilds } from '@/components/dashboard/builds-section'
import { getImageUrl } from "@/lib/content-utils";

export default function BuildsPage() {
    const {
        user,
        isLoading,
        isAdmin,
        games,
        activeGame,
        selectedBuild,
        filteredBuilds,
        setActiveGame,
        setSelectedBuild,
        handleLogout,
    } = useHomePage()

    if (isLoading || !user) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-[#0a0a0a]">
                <div className="text-[#c4a456]">Loading...</div>
            </div>
        )
    }

    return (
        <div className="flex flex-col md:flex-row min-h-screen bg-[#0a0a0a] text-gray-200 overflow-hidden">
            <aside className="w-full md:w-80 bg-[#121212] border-r border-[#C89B64]/20 flex flex-col h-screen shrink-0 z-20 relative shadow-[5px_0_15px_rgba(0,0,0,0.5)]">
                <div className="border-b border-[#C89B64]/20">
                    <DashboardUserCard user={user} isAdmin={isAdmin} />
                    <DashboardResources onLogout={handleLogout} />
                </div>

                <DashboardGames
                    games={games}
                    activeGame={activeGame}
                    setActiveGame={setActiveGame}
                />
            </aside>

            <main
                className="flex-1 h-screen flex flex-col relative bg-cover bg-center bg-no-repeat"
                style={{ backgroundImage: `url('${getImageUrl('home/wallpaper')}')` }}
            >
                <div className="absolute inset-0 bg-[#0a0a0a]/45 z-0" />
                <DashboardBuilds builds={filteredBuilds} onSelectBuild={setSelectedBuild} />
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
