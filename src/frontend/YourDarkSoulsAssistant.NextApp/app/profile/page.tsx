"use client"

import Link from 'next/link'
import {Shield, X, LayoutDashboard, Users, Sword, LogOut, Edit2, Save, UploadCloud} from 'lucide-react'
import { AdminDashboard } from '@/components/admin/admin-dashboard'
import { AdminUsers } from '@/components/admin/admin-users'
import { AdminEquipment } from '@/components/admin/admin-equipment'
import {getImageUrl} from "@/lib/content-utils";
import { useProfilePage } from '@/hooks/use-profile-page'

type AdminTab = 'dashboard' | 'users' | 'equipment'

type Activity = {
    id: number
    icon: string
    title: string
    detail: string
    time: string
    color: string
}

export default function ProfilePage() {
    const {
        adminTab,
        avatarCacheBuster,
        closeAdminPanel,
        editError,
        editForm,
        fileInputRef,
        handleAvatarUpload,
        handleCancelEdit,
        handleEditProfile,
        handleLogout,
        handleSaveProfile,
        isAdmin,
        isEditing,
        isLoading,
        isSaving,
        isUploadingAvatar,
        setAdminTab,
        showAdminPanel,
        toggleAdminPanel,
        updateEditFormField,
        user,
    } = useProfilePage()

    if (isLoading || !user) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-[#0a0a0a]">
                <div className="text-[#c4a456]">Loading...</div>
            </div>
        )
    }

    const stats = {
        favoriteGame: "Dark Souls 3",
        mostUsedWeapon: "Black Knight Greatsword",
        mostUsedWeaponIcon: "Sword",
        favoriteClass: "Knight",
        winRate: "68%"
    }

    const activities: Activity[] = [
        { id: 1, icon: "Sword", title: "Created new build", detail: "\"Giant Dad\"", time: "2 hours ago", color: "text-blue-400" },
        { id: 2, icon: "Fire", title: "Maxed out weapon", detail: "Zweihander +15", time: "5 hours ago", color: "text-orange-400" },
        { id: 3, icon: "Shield", title: "Updated stats", detail: "\"Sorcerer\" (Elden Ring)", time: "1 day ago", color: "text-blue-400" },
        { id: 4, icon: "Crown", title: "Attained Role", detail: "Lord of Cinder", time: "3 days ago", color: "text-yellow-400" },
    ]

    const getActivityIcon = (iconName: string) => {
        switch (iconName) {
            case 'Sword': return '\u2694'
            case 'Fire': return '\uD83D\uDD25'
            case 'Shield': return '\uD83D\uDEE1'
            case 'Crown': return '\uD83D\uDC51'
            default: return '\u25CF'
        }
    }

    const adminTabs = [
        { id: 'dashboard' as AdminTab, label: 'Dashboard', icon: LayoutDashboard },
        { id: 'users' as AdminTab, label: 'Users', icon: Users },
        { id: 'equipment' as AdminTab, label: 'Equipment', icon: Sword },
    ]

    return (
        <div className="min-h-screen bg-fixed bg-cover bg-center bg-no-repeat from-[#1A1A1D] via-[#0a0a0a] text-gray-200 font-sans p-12"
             style={{ backgroundImage: `url('${getImageUrl('profile/wallpaper')}')` }}
        >
            <input
                type="file"
                accept="image/*"
                className="hidden"
                ref={fileInputRef}
                onChange={handleAvatarUpload}
            />
            <div className="max-w-7xl mx-auto">

                {/* Header */}
                <div className="bg-[#121212]/40 backdrop-blur-md flex justify-between items-end rounded-lg mb-8 p-6 border border-[#C89B64]/20 pb-4">
                    <div>
                        <h1 className="text-4xl md:text-5xl text-[#D4AF37] font-serif uppercase tracking-[0.15em] drop-shadow-[0_0_15px_rgba(212,175,55,0.3)]">
                            Player Sanctum
                        </h1>
                        <p className="text-gray-500 mt-2 font-serif italic">Review your legacy, Unkindled one.</p>
                    </div>
                    <div className="flex items-center gap-3">
                        {isAdmin && (
                            <button
                                onClick={toggleAdminPanel}
                                className={`hidden md:flex items-center gap-2 px-6 py-2 border transition-all rounded-sm uppercase tracking-widest text-sm font-bold ${
                                    showAdminPanel 
                                        ? 'bg-[#C89B64] text-black border-[#C89B64]' 
                                        : 'bg-[#C89B64]/10 border-[#C89B64]/50 text-[#C89B64] hover:bg-[#C89B64] hover:text-black'
                                }`}
                            >
                                <Shield className="w-4 h-4" />
                                Admin Info
                            </button>
                        )}
                        <Link
                            href="/home"
                            className="hidden md:flex items-center gap-2 px-6 py-2 bg-[#C89B64]/10 border border-[#C89B64]/50 text-[#C89B64] hover:bg-[#C89B64] hover:text-black transition-all rounded-sm uppercase tracking-widest text-sm font-bold"
                        >
                            Home
                        </Link>
                        <button
                            onClick={handleLogout}
                            className="hidden md:flex items-center gap-2 px-4 py-2 border border-gray-700 text-gray-400 hover:border-red-600/50 hover:text-red-400 transition-all rounded-sm uppercase tracking-widest text-sm"
                        >
                            <LogOut className="w-4 h-4" />
                        </button>
                    </div>
                </div>

                {/* Admin Panel (shown when admin clicks button) */}
                {showAdminPanel && isAdmin && (
                    <div className="mb-8 bg-[#121212]/40 backdrop-blur-md border border-[#C89B64]/50 shadow-[0_0_30px_rgba(200,155,100,0.2)] rounded-lg overflow-hidden">
                        {/* Admin Panel Header */}
                        <div className="flex items-center justify-between px-6 py-4 border-b border-[#C89B64]/30 bg-[#1A1A1D]">
                            <div className="flex items-center gap-3">
                                <Shield className="w-5 h-5 text-[#D4AF37]" />
                                <h2 className="text-xl font-serif text-[#D4AF37]">Admin Panel</h2>
                            </div>
                            <button
                                onClick={closeAdminPanel}
                                className="p-2 hover:bg-[#C89B64]/20 rounded-lg transition-colors"
                            >
                                <X className="w-5 h-5 text-gray-400 hover:text-white" />
                            </button>
                        </div>

                        {/* Admin Tabs */}
                        <div className="flex border-b border-[#3a352c] bg-[#1A1A1D]/50">
                            {adminTabs.map(tab => {
                                const Icon = tab.icon
                                return (
                                    <button
                                        key={tab.id}
                                        onClick={() => setAdminTab(tab.id)}
                                        className={`flex items-center gap-2 px-6 py-3 text-sm uppercase tracking-wider transition-colors border-b-2 -mb-px ${
                                            adminTab === tab.id
                                                ? 'text-[#c4a456] border-[#c4a456]'
                                                : 'text-gray-500 border-transparent hover:text-gray-300'
                                        }`}
                                    >
                                        <Icon className="w-4 h-4" />
                                        {tab.label}
                                    </button>
                                )
                            })}
                        </div>

                        {/* Admin Content */}
                        <div className="p-6">
                            {adminTab === 'dashboard' && <AdminDashboard />}
                            {adminTab === 'users' && <AdminUsers />}
                            {adminTab === 'equipment' && <AdminEquipment />}
                        </div>
                    </div>
                )}

                {/* Mobile Admin Button */}
                {isAdmin && (
                    <button
                        onClick={toggleAdminPanel}
                        className={`md:hidden w-full mb-6 flex items-center justify-center gap-2 px-6 py-3 border transition-all rounded-sm uppercase tracking-widest text-sm font-bold ${
                            showAdminPanel 
                                ? 'bg-[#C89B64] text-black border-[#C89B64]' 
                                : 'bg-[#C89B64]/10 border-[#C89B64]/50 text-[#C89B64]'
                        }`}
                    >
                        <Shield className="w-4 h-4" />
                        {showAdminPanel ? 'Hide Admin Panel' : 'Show Admin Info'}
                    </button>
                )}

                {/* Grid Layout */}
                <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">

                    {/* Column 1: Profile (3/12) */}
                    <div className="lg:col-span-3 flex flex-col gap-6">
                        <div className="bg-[#121212]/40 backdrop-blur-md rounded-lg border border-[#C89B64]/30 shadow-[0_0_30px_rgba(0,0,0,0.8)] p-6 relative overflow-hidden group">
                            <div className="absolute top-0 left-1/2 -translate-x-1/2 w-full h-32 bg-[#C89B64]/5 blur-3xl pointer-events-none"></div>

                            <div className="flex flex-col items-center text-center relative z-10">
                                <div
                                    className={`relative group shrink-0 w-32 h-32 md:w-36 md:h-36 rounded-full border-4 border-[#C89B64] overflow-hidden bg-black shadow-[0_0_20px_rgba(200,155,100,0.3)] ${isEditing && !isUploadingAvatar ? 'cursor-pointer' : ''}`}
                                    onClick={() => isEditing && !isUploadingAvatar && fileInputRef.current?.click()}
                                >
                                    <img
                                        src={getImageUrl(user?.avatarPath || `users/${user?.id}`, avatarCacheBuster)}
                                        alt={user?.userName}
                                        className={`w-full h-full object-cover transition-opacity ${isUploadingAvatar ? 'opacity-50 grayscale' : ''}`}
                                        onError={(e) => {
                                            (e.target as HTMLImageElement).src = "/placeholder-user.jpg"
                                        }}
                                    />

                                    {isEditing && (
                                        <div className="absolute inset-0 flex items-center justify-center bg-black/60 opacity-0 group-hover:opacity-100 transition-opacity">
                                            {isUploadingAvatar ? (
                                                <span className="text-xs text-[#C89B64] font-bold animate-pulse">ЗАВАНТАЖЕННЯ...</span>
                                            ) : (
                                                <UploadCloud className="w-8 h-8 text-[#C89B64]" />
                                            )}
                                        </div>
                                    )}
                                </div>

                                {isEditing ? (
                                    // Edit Form
                                    <div className="w-full space-y-3 mt-4">
                                        {editError && (
                                            <div className="p-2 bg-red-900/30 border border-red-800 text-red-400 text-xs text-center rounded">
                                                {editError}
                                            </div>
                                        )}
                                        <div className="grid grid-cols-2 gap-2">
                                            <input
                                                type="text"
                                                value={editForm.firstName || ""}
                                                onChange={(e) => updateEditFormField('firstName', e.target.value)}
                                                placeholder="First Name"
                                                className="p-2 bg-black/40 border border-gray-700 text-gray-200 text-sm focus:border-[#C89B64] outline-none"
                                            />
                                            <input
                                                type="text"
                                                value={editForm.lastName || ""}
                                                onChange={(e) => updateEditFormField('lastName', e.target.value)}
                                                placeholder="Last Name"
                                                className="p-2 bg-black/40 border border-gray-700 text-gray-200 text-sm focus:border-[#C89B64] outline-none"
                                            />
                                        </div>
                                        <input
                                            type="text"
                                            value={editForm.userName || ""}
                                            onChange={(e) => updateEditFormField('userName', e.target.value)}
                                            placeholder="Username"
                                            className="w-full p-2 bg-black/40 border border-gray-700 text-gray-200 text-sm focus:border-[#C89B64] outline-none"
                                        />
                                        <input
                                            type="email"
                                            value={editForm.email || ""}
                                            onChange={(e) => updateEditFormField('email', e.target.value)}
                                            placeholder="Email"
                                            className="w-full p-2 bg-black/40 border border-gray-700 text-gray-200 text-sm focus:border-[#C89B64] outline-none"
                                        />
                                        <input
                                            type="text"
                                            value={editForm.covenant || ""}
                                            onChange={(e) => updateEditFormField('covenant', e.target.value)}
                                            placeholder="Covenant"
                                            className="w-full p-2 bg-black/40 border border-gray-700 text-gray-200 text-sm focus:border-[#C89B64] outline-none"
                                        />
                                        <div className="flex gap-2">
                                            <button
                                                onClick={handleSaveProfile}
                                                disabled={isSaving}
                                                className="flex-1 py-2 bg-[#C89B64] text-black uppercase text-xs tracking-[0.2em] hover:bg-[#D4AF37] transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
                                            >
                                                <Save className="w-3 h-3" />
                                                {isSaving ? 'Saving...' : 'Save'}
                                            </button>
                                            <button
                                                onClick={handleCancelEdit}
                                                disabled={isSaving}
                                                className="flex-1 py-2 bg-transparent border border-gray-600 text-gray-400 uppercase text-xs tracking-[0.2em] hover:border-red-600 hover:text-red-400 transition-colors disabled:opacity-50"
                                            >
                                                Cancel
                                            </button>
                                        </div>
                                    </div>
                                ) : (
                                    // Display Mode
                                    <>
                                        <p className="font-serif text-3xl text-[#D4AF37] mt-2">{user.firstName} {user.lastName}</p>
                                        <p className="text-gray-400 text-sm">@{user.userName}</p>
                                        <p className="text-gray-500 text-xs mb-4">{user.email}</p>

                                        <div className="w-full bg-black/40 rounded-md p-3 border border-gray-800 mb-6">
                                            <p className="text-xs text-gray-500 uppercase tracking-widest mb-1">Covenant</p>
                                            <p className="text-yellow-500 font-serif text-lg">{user.covenant || 'None'}</p>
                                        </div>

                                        <button 
                                            onClick={handleEditProfile}
                                            className="w-full py-2 bg-transparent border border-gray-600 hover:border-[#C89B64] hover:text-[#C89B64] text-gray-400 transition-colors uppercase text-xs tracking-[0.2em] mb-3 flex items-center justify-center gap-2"
                                        >
                                            <Edit2 className="w-3 h-3" />
                                            Edit Profile
                                        </button>
                                    </>
                                )}
                                <Link
                                    href="/home"
                                    className="lg:hidden w-full py-3 text-center bg-[#C89B64]/10 border border-[#C89B64] text-[#C89B64] transition-all uppercase tracking-widest text-sm font-bold"
                                >
                                    Home
                                </Link>
                            </div>
                        </div>

                        {/* Mini Stats */}
                        <div className="grid grid-cols-2 gap-4">
                            <div className="rounded-lg bg-[#121212]/40 border border-gray-800 p-4 text-center">
                                <p className="text-3xl font-serif text-gray-200">{user.buildCounts || 0}</p>
                                <p className="text-xs text-gray-500 uppercase tracking-widest mt-1">Total Builds</p>
                            </div>
                            <div className="rounded-lg bg-[#121212]/40 border border-gray-800 p-4 text-center">
                                <p className="text-3xl font-serif text-gray-200">{stats.winRate}</p>
                                <p className="text-xs text-gray-500 uppercase tracking-widest mt-1">PVP Win Rate</p>
                            </div>
                        </div>
                    </div>

                    {/* Column 2: Analytics (5/12) */}
                    <div className="lg:col-span-5 rounded-lg bg-[#121212]/40 backdrop-blur-md border border-gray-800 shadow-[0_0_30px_rgba(0,0,0,0.8)] p-6 flex flex-col">
                        <h2 className="font-serif text-2xl text-gray-300 border-b border-gray-800 pb-2 mb-6 flex items-center justify-between">
                            Combat Analytics
                            <span className="text-xs text-[#C89B64] uppercase tracking-widest border border-[#C89B64]/30 px-2 py-1 rounded">Live Data</span>
                        </h2>

                        {/* Main Weapon Card */}
                        <div className="bg-linear-to-br from-[#1A1A1D] to-black border border-[#C89B64]/20 p-5 rounded-sm relative overflow-hidden mb-6 group hover:border-[#C89B64]/50 transition-colors">
                            <div className="absolute -right-4 -bottom-4 text-8xl opacity-5 grayscale group-hover:opacity-10 transition-opacity">
                                {'\u2694'}
                            </div>
                            <p className="text-xs text-[#C89B64] uppercase tracking-widest mb-1">Most Used Weapon</p>
                            <p className="font-serif text-2xl text-gray-100 mb-2">{stats.mostUsedWeapon}</p>
                            <div className="flex gap-4 text-sm text-gray-400">
                                <span className="bg-black/40 px-2 py-1 rounded border border-gray-800">Equipped in 8 builds</span>
                                <span className="bg-black/40 px-2 py-1 rounded border border-gray-800">Scaling: S / D / - / -</span>
                            </div>
                        </div>

                        {/* Analytics Grid */}
                        <div className="grid grid-cols-2 gap-4 flex-1">
                            <div className="bg-black/40 border border-gray-800 p-4 rounded-sm flex flex-col justify-center">
                                <p className="text-xs text-gray-500 uppercase tracking-widest mb-1">Favorite Era</p>
                                <p className="font-serif text-xl text-gray-200">{stats.favoriteGame}</p>
                                <p className="text-xs text-gray-600 mt-2">Accounts for 60% of builds</p>
                            </div>
                            <div className="bg-black/40 border border-gray-800 p-4 rounded-sm flex flex-col justify-center">
                                <p className="text-xs text-gray-500 uppercase tracking-widest mb-1">Preferred Style</p>
                                <p className="font-serif text-xl text-gray-200">{stats.favoriteClass}</p>
                                <p className="text-xs text-gray-600 mt-2">Strength / Poise focus</p>
                            </div>
                        </div>
                    </div>

                    {/* Column 3: History (4/12) */}
                    <div className="lg:col-span-4 rounded-lg bg-[#121212]/40 backdrop-blur-md border border-gray-800 shadow-[0_0_30px_rgba(0,0,0,0.8)] p-6">
                        <h2 className="font-serif text-2xl text-gray-300 border-b border-gray-800 pb-2 mb-6">
                            Recent Chronicle
                        </h2>

                        <div className="flex flex-col divide-y divide-gray-800/50">
                            {activities.map((activity) => (
                                <div key={activity.id} className="py-4 flex items-start gap-4 hover:bg-white/[0.02] transition-colors -mx-2 px-2 rounded-md">
                                    <div className="mt-1 w-10 h-10 text-center bg-black/50 p-2 rounded-full border border-gray-800 shadow-inner">
                                        {getActivityIcon(activity.icon)}
                                    </div>
                                    <div className="flex-1">
                                        <div className="flex justify-between items-baseline mb-1">
                                            <p className={`font-medium ${activity.color}`}>{activity.title}</p>
                                            <p className="text-sm text-gray-300">{activity.time}</p>
                                        </div>
                                        <p className="text-sm text-gray-300 font-serif italic">{activity.detail}</p>
                                    </div>
                                </div>
                            ))}
                        </div>

                        <button className="w-full mt-6 py-2 text-xs text-gray-300 hover:text-gray-100 uppercase tracking-widest border-t border-gray-800 pt-4 transition-colors">
                            View Full History
                        </button>
                    </div>

                </div>
            </div>
        </div>
    )
}
