"use client"

import Link from 'next/link'
import { Shield } from 'lucide-react'
import { getImageUrl } from '@/lib/content-utils'
import type { DashboardUserCardProps } from '@/types/dashboard'

export function DashboardUserCard({ user, isAdmin }: DashboardUserCardProps) {
    return (
        <div className="p-6">
            <Link
                href="/profile"
                className="flex items-center gap-4 hover:bg-white/5 transition-colors group rounded-lg p-2 -m-2"
            >
                <div className="w-14 h-14 rounded-full border-2 border-gray-600 group-hover:border-[#C89B64] transition-colors overflow-hidden bg-[#1a1a1a] flex items-center justify-center shrink-0">
                    {user.avatarPath ? (
                        <img
                            src={getImageUrl(user.avatarPath)}
                            alt={user.userName}
                            className="w-full h-full object-cover"
                            onError={(e) => { (e.target as HTMLImageElement).src = '/placeholder-user.jpg' }}
                        />
                    ) : (
                        <span className="text-2xl text-gray-500 uppercase">
                            {user.firstName?.[0] || ''}{user.lastName?.[0] || ''}
                        </span>
                    )}
                </div>
                <div className="flex-1 overflow-hidden">
                    <p className="font-serif text-[#D4AF37] text-lg leading-tight group-hover:drop-shadow-[0_0_5px_rgba(212,175,55,0.8)] transition-all truncate">
                        {user.firstName} {user.lastName}
                    </p>
                    <p className="text-xs text-gray-500 uppercase tracking-widest flex items-center gap-1 mt-1">
                        {isAdmin && <Shield className="w-3 h-3 text-[#c4a456]" />}
                        {isAdmin ? 'Administrator' : 'Tarnished'}
                    </p>
                </div>
            </Link>
        </div>
    )
}