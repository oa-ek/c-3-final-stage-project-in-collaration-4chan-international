"use client"

import Link from 'next/link'
import { BookOpen, LogOut } from 'lucide-react'

interface DashboardResourcesProps {
    onLogout: () => void
}

export function DashboardResources({ onLogout }: DashboardResourcesProps) {
    return (
        <div className="px-6 py-4 space-y-2">
            <Link
                href="/wiki"
                className="w-full flex items-center justify-center gap-2 px-3 py-2 text-xs text-[#D4AF37] border border-[#D4AF37]/50 hover:bg-[#D4AF37]/10 rounded transition-colors uppercase tracking-wider"
            >
                <BookOpen className="w-3 h-3" />
                Wiki
            </Link>
            <Link
                href="/archive"
                className="w-full flex items-center justify-center gap-2 px-3 py-2 text-xs text-[#6F42C1] border border-[#6F42C1]/50 hover:bg-[#6F42C1]/10 rounded transition-colors uppercase tracking-wider"
            >
                <BookOpen className="w-3 h-3" />
                Grand Archives
            </Link>
            <Link
                href="/guides"
                className="w-full flex items-center justify-center gap-2 px-3 py-2 text-xs text-[#DC3545] border border-[#DC3545]/50 hover:bg-[#DC3545]/10 rounded transition-colors uppercase tracking-wider"
            >
                <BookOpen className="w-3 h-3" />
                Video Guides
            </Link>
            <button
                onClick={onLogout}
                className="w-full flex items-center justify-center gap-2 px-3 py-2 text-xs text-gray-400 border border-gray-700 hover:border-red-600/50 hover:text-red-400 rounded transition-colors uppercase tracking-wider"
            >
                <LogOut className="w-3 h-3" />
                Logout
            </button>
        </div>
    )
}