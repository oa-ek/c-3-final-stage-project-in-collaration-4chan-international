"use client"

import {getImageUrl} from "@/lib/content-utils";
import type { DashboardGamesProps } from '@/types/dashboard'

export function DashboardGames({ games, activeGame, setActiveGame }: DashboardGamesProps) {
    return (
        <div className="p-6 flex-1 overflow-y-auto">
            <h3 className="text-xs text-gray-500 uppercase tracking-[0.2em] mb-4">Select Realm</h3>
            <div className="flex flex-col gap-2">
                {games.map((game) => (
                    <button
                        key={game.id}
                        onClick={() => setActiveGame(game.id)}
                        className={`flex items-center gap-4 w-full text-left p-3 rounded-md transition-all ${
                            activeGame === game.id
                                ? 'bg-[#C89B64]/10 border border-[#C89B64]/50 text-[#D4AF37] shadow-[inset_0_0_10px_rgba(200,155,100,0.1)]'
                                : 'border border-transparent text-gray-400 hover:bg-white/5 hover:text-gray-200'
                        }`}
                    >
                        <div className="w-8 h-8 rounded-sm bg-gray-800 border border-gray-700 flex items-center justify-center overflow-hidden shrink-0">
                            <img
                                src={getImageUrl(game.icon)}
                                alt={game.name}
                                className="w-full h-full object-cover"
                            />
                        </div>
                        <span className="font-medium tracking-wide">{game.name}</span>
                    </button>
                ))}
            </div>
        </div>
    )
}