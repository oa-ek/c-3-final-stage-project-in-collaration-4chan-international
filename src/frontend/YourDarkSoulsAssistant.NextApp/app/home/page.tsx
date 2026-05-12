"use client"

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/contexts/auth-context'
import { getImageUrl } from '@/lib/content-utils'

import { DashboardUserCard } from '@/components/dashboard/user-card'
import { DashboardResources } from '@/components/dashboard/resources-block'
import { DashboardGames } from '@/components/dashboard/games-section'
import { DashboardBuilds } from '@/components/dashboard/builds-section'

export default function HubPage() {
    const { user, isLoading, isAuthenticated } = useAuth()
    const router = useRouter()

    const [activeGame, setActiveGame] = useState('all')

    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            const timer = setTimeout(() => {
                router.push('/login')
            }, 0);
            return () => clearTimeout(timer);
        }
    }, [user, isLoading, isAuthenticated, router])

    if (isLoading) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-[#0a0a0a]">
                <div className="text-[#D4AF37] font-serif tracking-widest animate-pulse uppercase">
                    Завантаження...
                </div>
            </div>
        )
    }

    if (!user) return null

    return (
        <div className="flex flex-col md:flex-row min-h-screen bg-[#0a0a0a] text-gray-200 overflow-hidden">
            <aside className="w-full md:w-80 bg-[#121212] border-r border-[#C89B64]/20 flex flex-col h-screen shrink-0 z-20 relative shadow-[5px_0_15px_rgba(0,0,0,0.5)]">
                <div className="border-b border-[#C89B64]/20">
                    <DashboardUserCard user={user} />

                    <DashboardResources />
                </div>

                <DashboardGames activeGame={activeGame} setActiveGame={setActiveGame} />

            </aside>

            <main
                className="flex-1 h-screen flex flex-col relative bg-cover bg-center"
                style={{ backgroundImage: `url('${getImageUrl('dashboard/wallpaper')}')` }}
            >
                <div className="absolute inset-0 bg-[#0a0a0a]/85 backdrop-blur-[2px] z-0" />

                <DashboardBuilds activeGame={activeGame} />
            </main>

        </div>
    )
}
