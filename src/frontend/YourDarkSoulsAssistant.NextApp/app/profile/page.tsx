"use client"

import {useState, useEffect, useRef} from 'react'
import { useRouter } from 'next/navigation'
import Link from 'next/link'
import { getImageUrl } from '@/lib/content-utils'
import { useAuth } from '@/contexts/auth-context'
import {
    Shield, X, LayoutDashboard, Users, Sword,
    LogOut, Edit2, Save, Calendar, Star, Trophy, UploadCloud
} from 'lucide-react'
import { AdminDashboard } from '@/components/admin/admin-dashboard'
import { AdminUsers } from '@/components/admin/admin-users'
import { AdminEquipment } from '@/components/admin/admin-equipment'
import type { UpdateUserDTO } from '@/types/dto'
import {tokenStorage} from "@/lib/api-client";

type AdminTab = 'dashboard' | 'users' | 'equipment'

// Тимчасовий мок для активності (поки немає API)
const MOCK_ACTIVITIES = [
    { id: 1, icon: 'sword', title: 'Створено новий білд', detail: 'Лицар Лотріка (STR/FTH)', time: '2 години тому', color: 'text-orange-500' },
    { id: 2, icon: 'shield', title: 'Оновлено спорядження', detail: 'Додано Травʼяний щит +5', time: '5 годин тому', color: 'text-blue-500' },
    { id: 3, icon: 'users', title: 'Зміна ковенанту', detail: 'Приєднано до Воїнів Сонця', time: '1 день тому', color: 'text-yellow-500' },
]

export default function ProfilePage() {
    const { user, isLoading, logout, updateProfile, isAdmin } = useAuth()
    const router = useRouter()

    const [showAdminPanel, setShowAdminPanel] = useState(false)
    const [adminTab, setAdminTab] = useState<AdminTab>('dashboard')
    const [isEditing, setIsEditing] = useState(false)
    const [isUploadingAvatar, setIsUploadingAvatar] = useState(false)
    const [avatarCacheBuster, setAvatarCacheBuster] = useState(Date.now())
    const fileInputRef = useRef<HTMLInputElement>(null)
    const [isSaving, setIsSaving] = useState(false)
    const [editError, setEditError] = useState<string | null>(null)

    // Стан форми редагування
    const [editForm, setEditForm] = useState<UpdateUserDTO>({
        id: '',
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        avatarPath: '',
        covenant: ''
    })

    // Синхронізація форми з даними юзера
    useEffect(() => {
        if (user) {
            setEditForm({
                id: user.id,
                firstName: user.firstName,
                lastName: user.lastName,
                userName: user.userName,
                email: user.email,
                avatarPath: user.avatarPath,
                covenant: user.covenant
            })
        }
    }, [user])

    if (isLoading) return (
        <div className="min-h-screen bg-[#0a0a0a] flex items-center justify-center">
            <div className="text-[#C89B64] font-serif animate-pulse tracking-widest">ЗАВАНТАЖЕННЯ ДУШІ...</div>
        </div>
    )

    if (!user) {
        router.push('/login')
        return null
    }

    const handleAvatarUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (!file || !user) return;

        setIsUploadingAvatar(true);
        setEditError(null);

        // Створюємо FormData для нашого Content Service API
        const formData = new FormData();
        formData.append("file", file);
        formData.append("Name", `${user.userName} Avatar`);
        formData.append("Route", `users/${user.id}`);

        try {
            const tokens = tokenStorage.getTokens();
            const response = await fetch("/api/content/ContentItem/upload", {
                method: "POST",
                headers: {
                    "Authorization": `Bearer ${tokens?.accessToken}`
                },
                body: formData,
            });

            if (response.ok) {
                setAvatarCacheBuster(Date.now());
                await updateProfile({ ...editForm, avatarPath: `users/${user.id}` });
            } else {
                setEditError("Не вдалося завантажити зображення");
            }
        } catch (error) {
            setEditError("Помилка мережі при завантаженні зображення");
        } finally {
            setIsUploadingAvatar(false);
        }
    };

    const handleSave = async () => {
        setIsSaving(true)
        setEditError(null)

        const result = await updateProfile(editForm)

        if (result.success) {
            setIsEditing(false)
        } else {
            setEditError(result.error || "Не вдалося зберегти зміни")
        }
        setIsSaving(false)
    }

    return (
        <div className="min-h-screen bg-cover bg-center bg-no-repeat from-[#1A1A1D] via-[#0a0a0a] text-gray-200 font-sans pb-12"
             style={{ backgroundImage: `url('${getImageUrl('profile/wallpaper')}')` }}
        >
            <input
                type="file"
                accept="image/*"
                className="hidden"
                ref={fileInputRef}
                onChange={handleAvatarUpload}
            />

            {/* Header / Profile Island */}
            <div className="max-w-6xl mx-auto px-6 pt-8">
                <div className="bg-black/40 border border-gray-800 p-6 md:p-8 backdrop-blur-sm rounded-lg flex flex-col md:flex-row items-center gap-6 md:gap-8 shadow-lg">

                    {/* БЛОК АВАТАРКИ */}
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
                                    <span className="text-xs text-[#C89B64] font-bold animate-pulse">ЗАВАНТ...</span>
                                ) : (
                                    <UploadCloud className="w-8 h-8 text-[#C89B64]" />
                                )}
                            </div>
                        )}
                    </div>

                    {/* ІНФО */}
                    <div className="flex-1 text-center md:text-left flex flex-col justify-center">
                        <div className="flex flex-wrap items-center justify-center md:justify-start gap-3 mb-2">
                            <h1 className="text-3xl md:text-4xl font-serif text-white tracking-tight uppercase break-all">
                                {user.userName}
                            </h1>
                            {user.isAdmin && (
                                <span className="px-2 py-0.5 bg-red-900/30 border border-red-500/50 text-red-400 text-[10px] uppercase tracking-tighter rounded shrink-0">
                                    ADMIN
                                </span>
                            )}
                        </div>
                        <p className="text-gray-400 font-serif italic break-all">{user.email}</p>
                    </div>

                    {/* КНОПКИ */}
                    <div className="flex flex-wrap justify-center md:justify-end gap-3 w-full md:w-auto">
                        {isEditing ? (
                            <>
                                <button
                                    onClick={handleSave}
                                    disabled={isSaving}
                                    className="flex-1 md:flex-none flex items-center justify-center gap-2 px-6 py-2 bg-[#C89B64] text-black font-bold rounded shadow-lg hover:bg-[#E5C07B] transition-all disabled:opacity-50"
                                >
                                    <Save className="w-4 h-4" /> {isSaving ? "ЗБЕРЕЖЕННЯ..." : "ЗБЕРЕГТИ"}
                                </button>
                                <button
                                    onClick={() => setIsEditing(false)}
                                    className="p-2 border border-gray-700 hover:bg-white/5 rounded transition-colors flex items-center justify-center"
                                >
                                    <X className="w-5 h-5 text-gray-400" />
                                </button>
                            </>
                        ) : (
                            <>
                                <Link
                                    href="/builds"
                                    className="flex-1 md:flex-none flex items-center justify-center gap-2 px-6 py-2 bg-[#C89B64] text-black font-bold uppercase tracking-widest text-sm hover:bg-[#E5C07B] transition-all rounded shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)]"
                                >
                                    <Sword className="w-4 h-4" /> Мої Білди
                                </Link>
                                <button
                                    onClick={() => setIsEditing(true)}
                                    className="flex-1 md:flex-none flex items-center justify-center gap-2 px-6 py-2 bg-gray-700/50 border border-gray-700 hover:border-[#C89B64] text-white transition-all rounded"
                                >
                                    <Edit2 className="w-4 h-4" /> РЕДАГУВАТИ
                                </button>
                                <button
                                    onClick={logout}
                                    className="p-2 border border-red-900/30 hover:bg-red-900/20 rounded transition-colors flex items-center justify-center shrink-0"
                                    title="Вийти"
                                >
                                    <LogOut className="w-5 h-5 text-red-500" />
                                </button>
                            </>
                        )}
                    </div>
                </div>
            </div>

            <div className="max-w-6xl mx-auto px-6 mt-8 grid grid-cols-1 lg:grid-cols-3 gap-8">
                {/* Left Column: Info & Stats */}
                <div className="lg:col-span-1 space-y-6">
                    <div className="bg-black/40 border border-gray-800 p-6 backdrop-blur-sm rounded-lg">
                        <h3 className="text-[#C89B64] font-serif uppercase tracking-widest text-sm mb-6 border-b border-gray-800 pb-2">Статус персонажа</h3>

                        <div className="space-y-4">
                            <StatItem icon={<Star className="w-4 h-4" />} label="Рівень" value={user.level} />
                            <StatItem icon={<Sword className="w-4 h-4" />} label="Білдів" value={user.buildCounts} />
                            <StatItem icon={<Trophy className="w-4 h-4" />} label="Ковенант" value={user.covenant || "Немає"} />
                            <StatItem icon={<Calendar className="w-4 h-4" />} label="У грі з" value={new Date(user.joinDate).toLocaleDateString()} />
                        </div>
                    </div>

                    {isAdmin && (
                        <button
                            onClick={() => setShowAdminPanel(!showAdminPanel)}
                            className="w-full py-3 flex items-center justify-center gap-2 bg-red-900/10 border border-red-900/40 text-red-400 hover:bg-red-900/20 transition-all rounded font-serif uppercase text-xs tracking-widest"
                        >
                            <Shield className="w-4 h-4" /> {showAdminPanel ? "ЗАКРИТИ АДМІН-ПАНЕЛЬ" : "ПАНЕЛЬ УПРАВЛІННЯ"}
                        </button>
                    )}
                </div>

                {/* Right Column: Main Content */}
                <div className="lg:col-span-2">
                    {isEditing ? (
                        <div className="bg-black/40 border border-gray-800 p-8 space-y-6 rounded-lg">
                            <h2 className="text-xl font-serif text-white uppercase tracking-wider mb-4">Налаштування профілю</h2>

                            {editError && <div className="p-3 bg-red-900/20 border border-red-500/50 text-red-200 text-sm">{editError}</div>}

                            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                <InputGroup label="Ім'я" value={editForm.firstName || ''} onChange={(v) => setEditForm({...editForm, firstName: v})} />
                                <InputGroup label="Прізвище" value={editForm.lastName || ''} onChange={(v) => setEditForm({...editForm, lastName: v})} />
                                <InputGroup label="Username" value={editForm.userName || ''} onChange={(v) => setEditForm({...editForm, userName: v})} />
                                <InputGroup label="Ковенант" value={editForm.covenant || ''} onChange={(v) => setEditForm({...editForm, covenant: v})} />
                            </div>
                        </div>
                    ) : showAdminPanel ? (
                        <div className="bg-black/40 border border-gray-800 min-h-[500px] rounded-lg overflow-hidden flex flex-col">
                            <div className="flex border-b border-gray-800 flex-wrap">
                                <AdminTabButton active={adminTab === 'dashboard'} onClick={() => setAdminTab('dashboard')} icon={<LayoutDashboard className="w-4 h-4" />} label="Огляд" />
                                <AdminTabButton active={adminTab === 'users'} onClick={() => setAdminTab('users')} icon={<Users className="w-4 h-4" />} label="Юзери" />
                                <AdminTabButton active={adminTab === 'equipment'} onClick={() => setAdminTab('equipment')} icon={<Sword className="w-4 h-4" />} label="Еквіп" />
                            </div>
                            <div className="p-6 flex-1 overflow-auto">
                                {adminTab === 'dashboard' && <AdminDashboard />}
                                {adminTab === 'users' && <AdminUsers />}
                                {adminTab === 'equipment' && <AdminEquipment />}
                            </div>
                        </div>
                    ) : (
                        <div className="space-y-8">
                            <div className="bg-black/40 border border-gray-800 p-8 rounded-lg">
                                <h3 className="text-white font-serif uppercase tracking-wider mb-6 flex items-center gap-2">
                                    <LayoutDashboard className="w-5 h-5 text-[#C89B64]" /> Останні мандри
                                </h3>
                                <div className="space-y-4">
                                    {MOCK_ACTIVITIES.map((activity) => (
                                        <div key={activity.id} className="flex gap-4 p-4 hover:bg-white/[0.02] transition-colors border border-transparent hover:border-gray-800 rounded">
                                            <div className={`mt-1 ${activity.color}`}>
                                                <Sword className="w-5 h-5" />
                                            </div>
                                            <div className="flex-1">
                                                <div className="flex justify-between">
                                                    <p className="font-medium text-gray-200">{activity.title}</p>
                                                    <span className="text-[10px] uppercase text-gray-600 tracking-tighter">{activity.time}</span>
                                                </div>
                                                <p className="text-sm text-gray-500 italic font-serif mt-1">{activity.detail}</p>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </div>
    )
}

// Допоміжні компоненти для чистоти коду
function StatItem({ icon, label, value }: { icon: any, label: string, value: string | number }) {
    return (
        <div className="flex items-center justify-between text-sm py-1">
            <div className="flex items-center gap-2 text-gray-500 font-serif uppercase text-[11px] tracking-widest">
                {icon} <span>{label}</span>
            </div>
            <span className="text-gray-200 font-medium">{value}</span>
        </div>
    )
}

function InputGroup({ label, value, onChange }: { label: string, value: string, onChange: (v: string) => void }) {
    return (
        <div className="space-y-1">
            <label className="text-[10px] uppercase text-[#C89B64] tracking-[0.2em] ml-1">{label}</label>
            <input
                value={value}
                onChange={(e) => onChange(e.target.value)}
                className="w-full bg-black/60 border border-gray-800 p-3 text-gray-200 focus:border-[#C89B64] outline-none transition-all text-sm rounded"
            />
        </div>
    )
}

function AdminTabButton({ active, onClick, icon, label }: { active: boolean, onClick: () => void, icon: any, label: string }) {
    return (
        <button
            onClick={onClick}
            className={`flex items-center justify-center flex-1 md:flex-none gap-2 px-6 py-4 text-xs uppercase tracking-widest transition-all ${
                active ? 'bg-red-900/10 text-red-400 border-b-2 border-red-500' : 'text-gray-500 hover:bg-white/5'
            }`}
        >
            {icon} {label}
        </button>
    )
}