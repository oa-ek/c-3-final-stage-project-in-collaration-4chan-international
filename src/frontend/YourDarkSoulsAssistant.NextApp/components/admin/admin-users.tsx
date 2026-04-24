"use client"

import { useState, useEffect, useCallback } from "react"
import { apiClient } from "@/lib/api-client"
import {
  Users, Search, MoreVertical, Shield, User as UserIcon,
  Trash2, Loader2, RefreshCw, Plus, X
} from "lucide-react"
import {
  DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import type { SmallUserDTO, RoleDTO } from "@/types/dto"

// Налаштування кешу
const CACHE_KEY = "admin_users_cache"
const CACHE_TTL_MS = 5 * 60 * 1000 // 5 хвилин

export function AdminUsers() {
  const [users, setUsers] = useState<SmallUserDTO[]>([])
  const [allRoles, setAllRoles] = useState<RoleDTO[]>([])

  const [isLoading, setIsLoading] = useState(true)
  const [isRefreshing, setIsRefreshing] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [searchQuery, setSearchQuery] = useState("")

  // Стан для модалки ролей
  const [managingUser, setManagingUser] = useState<SmallUserDTO | null>(null)
  const [selectedRoles, setSelectedRoles] = useState<string[]>([])
  const [isSavingRoles, setIsSavingRoles] = useState(false)

  // --- ЛОГІКА КЕШУВАННЯ ТА ЗАВАНТАЖЕННЯ ---
  const fetchUsers = useCallback(async (forceRefresh = false) => {
    try {
      if (forceRefresh) setIsRefreshing(true)
      else setIsLoading(true)

      // Перевіряємо кеш, якщо це не примусове оновлення
      if (!forceRefresh) {
        const cachedStr = sessionStorage.getItem(CACHE_KEY)
        if (cachedStr) {
          const cached = JSON.parse(cachedStr)
          if (Date.now() - cached.timestamp < CACHE_TTL_MS) {
            setUsers(cached.data)
            setIsLoading(false)
            return
          }
        }
      }

      const response = await apiClient("/Admin/all-users")
      if (response.ok) {
        const data: SmallUserDTO[] = await response.json()
        setUsers(data)
        // Зберігаємо в кеш
        sessionStorage.setItem(CACHE_KEY, JSON.stringify({ data, timestamp: Date.now() }))
      } else {
        setError("Не вдалося завантажити список користувачів")
      }
    } catch (err) {
      setError("Помилка мережі")
    } finally {
      setIsLoading(false)
      setIsRefreshing(false)
    }
  }, [])

  useEffect(() => {
    fetchUsers()
  }, [fetchUsers])

  // --- ЛОГІКА УПРАВЛІННЯ РОЛЯМИ ---
  const handleManageRoles = async (user: SmallUserDTO) => {
    setManagingUser(user)

    // ФІКС 1: Видаляємо всі null, undefined або пусті строки з масиву
    const cleanRoles = user.roles.filter(role => role != null && role !== "");
    setSelectedRoles(cleanRoles);

    if (allRoles.length === 0) {
      try {
        const res = await apiClient("/Admin/all-roles")
        if (res.ok) {
          const rolesData: RoleDTO[] = await res.json()
          setAllRoles(rolesData)
        }
      } catch (e) {
        console.error("Failed to fetch roles", e)
      }
    }
  }

  const addRole = (roleName: string) => {
    if (!selectedRoles.includes(roleName)) {
      setSelectedRoles([...selectedRoles, roleName])
    }
  }

  const removeRole = (roleName: string) => {
    setSelectedRoles(selectedRoles.filter(r => r !== roleName))
  }

  const saveRoles = async () => {
    if (!managingUser) return
    setIsSavingRoles(true)

    try {
      // ФІКС 2: Ще раз гарантуємо, що відправляємо чистий масив
      const cleanRoles = selectedRoles.filter(role => role != null && role !== "");

      // ЗАПИТ (Обери той, який відповідає твоєму бекенду.
      // Нижче варіант для випадку, якщо ти додав ChangeRoleDTO у C#)
      const response = await apiClient("/Admin/change-role", {
        method: "POST",
        body: JSON.stringify({ id: managingUser.id, roles: cleanRoles })
      })

      if (response.ok) {
        const updatedUsers = users.map(u =>
            u.id === managingUser.id ? { ...u, roles: cleanRoles, isAdmin: cleanRoles.includes("Admin") } : u
        )
        setUsers(updatedUsers)
        sessionStorage.setItem(CACHE_KEY, JSON.stringify({ data: updatedUsers, timestamp: Date.now() }))
        setManagingUser(null)
      } else {
        alert("Не вдалося оновити ролі. Перевір консоль сервера.")
      }
    } catch (e) {
      alert("Помилка мережі при збереженні")
    } finally {
      setIsSavingRoles(false)
    }
  }

  const handleDeleteUser = async (userId: string) => {
    if (confirm("Ти впевнений, що хочеш видалити цього користувача?")) {
      // Тут має бути: await apiClient(`/Admin/users/${userId}`, { method: "DELETE" })
      const updatedUsers = users.filter(u => u.id !== userId)
      setUsers(updatedUsers)
      sessionStorage.setItem(CACHE_KEY, JSON.stringify({ data: updatedUsers, timestamp: Date.now() }))
    }
  }

  const filteredUsers = users.filter(u =>
      `${u.firstName} ${u.lastName}`.toLowerCase().includes(searchQuery.toLowerCase()) ||
      u.email.toLowerCase().includes(searchQuery.toLowerCase()) ||
      u.userName.toLowerCase().includes(searchQuery.toLowerCase())
  )

  if (isLoading) return (
      <div className="flex items-center justify-center h-64 text-[#C89B64]">
        <Loader2 className="w-8 h-8 animate-spin" />
      </div>
  )

  return (
      <div className="space-y-4">
        {/* Header */}
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <div className="flex items-center gap-2">
              <Users className="w-5 h-5 text-[#C89B64]" />
              <h2 className="text-lg font-serif text-[#C89B64] uppercase tracking-wider">Юзери</h2>
            </div>
            <button
                onClick={() => fetchUsers(true)}
                disabled={isRefreshing}
                className="flex items-center gap-1 text-[10px] uppercase tracking-widest text-gray-500 hover:text-[#C89B64] transition-colors"
            >
              <RefreshCw className={`w-3 h-3 ${isRefreshing ? "animate-spin" : ""}`} /> Оновити дані
            </button>
          </div>
          <span className="text-xs text-gray-500 uppercase tracking-widest">
          Всього: <span className="text-[#C89B64]">{users.length}</span>
        </span>
        </div>

        {/* Search */}
        <div className="relative">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500" />
          <Input
              placeholder="Пошук юзерів..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="pl-10 bg-black/40 border-gray-800 text-gray-300 placeholder:text-gray-600 h-10 text-sm focus:border-[#C89B64]"
          />
        </div>

        {/* Users List */}
        <div className="border border-gray-800 rounded-lg overflow-hidden max-h-[500px] overflow-y-auto">
          <table className="w-full">
            <thead className="bg-[#1a1a1a] sticky top-0 z-10 border-b border-gray-800">
            <tr>
              <th className="px-4 py-3 text-left text-[10px] font-medium text-gray-500 uppercase tracking-widest">Користувач</th>
              <th className="px-4 py-3 text-left text-[10px] font-medium text-gray-500 uppercase tracking-widest hidden md:table-cell">Email</th>
              <th className="px-4 py-3 text-left text-[10px] font-medium text-gray-500 uppercase tracking-widest">Ролі</th>
              <th className="px-4 py-3 text-right text-[10px] font-medium text-gray-500 uppercase tracking-widest">Дії</th>
            </tr>
            </thead>
            <tbody className="divide-y divide-gray-800/50 bg-black/20">
            {filteredUsers.map((u) => (
                <tr key={u.id} className="hover:bg-white/[0.02] transition-colors group">
                  <td className="px-4 py-3">
                    <div className="flex items-center gap-3">
                      <div className="w-8 h-8 rounded-full bg-[#1a1a1a] border border-gray-800 flex items-center justify-center shrink-0">
                        {u.isAdmin ? <Shield className="w-4 h-4 text-[#C89B64]" /> : <UserIcon className="w-4 h-4 text-gray-500" />}
                      </div>
                      <div className="flex flex-col">
                        <span className="text-gray-200 text-sm font-medium">{u.firstName} {u.lastName}</span>
                        <span className="text-[#C89B64] text-[11px] font-serif tracking-wide">@{u.userName}</span>
                      </div>
                    </div>
                  </td>
                  <td className="px-4 py-3 text-gray-400 text-xs hidden md:table-cell">{u.email}</td>
                  <td className="px-4 py-3">
                    <div className="flex flex-wrap gap-1">
                      {u.roles.length > 0 ? (
                          u.roles.map(r => (
                              <span key={r} className={`px-2 py-0.5 rounded text-[9px] font-bold tracking-widest uppercase border ${
                                  r === "Admin" ? "bg-red-900/20 text-red-400 border-red-900/50" : "bg-gray-800/50 text-gray-400 border-gray-700/50"
                              }`}>
                          {r}
                        </span>
                          ))
                      ) : (
                          <span className="text-gray-600 text-xs italic">Немає</span>
                      )}
                    </div>
                  </td>
                  <td className="px-4 py-3 text-right">
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <button className="p-2 hover:bg-white/5 rounded transition-colors border border-transparent hover:border-gray-700">
                          <MoreVertical className="w-4 h-4 text-gray-500" />
                        </button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end" className="bg-[#1a1a1a] border-gray-800">
                        <DropdownMenuItem
                            onClick={() => handleManageRoles(u)}
                            className="text-gray-300 hover:text-[#C89B64] hover:bg-white/5 cursor-pointer text-xs uppercase tracking-wider"
                        >
                          <Shield className="w-3 h-3 mr-2" /> Управління ролями
                        </DropdownMenuItem>
                        <DropdownMenuItem
                            onClick={() => handleDeleteUser(u.id)}
                            className="text-red-400 hover:text-red-300 hover:bg-red-900/20 cursor-pointer text-xs uppercase tracking-wider"
                        >
                          <Trash2 className="w-3 h-3 mr-2" /> Видалити
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </td>
                </tr>
            ))}
            </tbody>
          </table>
        </div>

        {/* Діалог управління ролями */}
        <Dialog open={!!managingUser} onOpenChange={(open) => !open && setManagingUser(null)}>
          <DialogContent className="bg-[#0a0a0a] border-[#3c3c3c] max-w-md p-0 overflow-hidden shadow-[0_0_50px_rgba(0,0,0,0.8)]">
            <DialogHeader className="p-6 pb-4 border-b border-gray-800 bg-[#1a1a1a]">
              <DialogTitle className="text-[#C89B64] font-serif uppercase tracking-widest flex items-center gap-2">
                <Shield className="w-5 h-5" /> Управління доступом
              </DialogTitle>
              <p className="text-xs text-gray-400 mt-1">
                Користувач: <span className="text-gray-200">{managingUser?.userName}</span>
              </p>
            </DialogHeader>

            <div className="p-6 space-y-6">
              {/* Доступні ролі */}
              <div>
                <label className="text-[10px] uppercase text-gray-500 tracking-[0.2em] mb-3 block">Доступні ролі (Натисніть щоб додати)</label>
                <div className="flex flex-wrap gap-2">
                  {allRoles.length === 0 ? (
                      <span className="text-xs text-gray-600 flex items-center gap-2"><Loader2 className="w-3 h-3 animate-spin"/> Завантаження...</span>
                  ) : (
                      allRoles.map(role => {
                        const isSelected = selectedRoles.includes(role.name)
                        return (
                            <button
                                key={role.id}
                                onClick={() => addRole(role.name)}
                                disabled={isSelected}
                                className={`flex items-center gap-1 px-3 py-1.5 rounded-full text-xs font-medium transition-all ${
                                    isSelected
                                        ? 'bg-gray-800/30 text-gray-600 border border-gray-800/50 cursor-not-allowed'
                                        : 'bg-black border border-gray-700 text-gray-300 hover:border-[#C89B64] hover:text-[#C89B64]'
                                }`}
                            >
                              <Plus className="w-3 h-3" /> {role.name}
                            </button>
                        )
                      })
                  )}
                </div>
              </div>

              {/* Призначені ролі */}
              <div className="bg-black/40 p-4 border border-gray-800 rounded">
                <label className="text-[10px] uppercase text-[#C89B64] tracking-[0.2em] mb-3 block">Активні ролі користувача</label>
                <div className="flex flex-wrap gap-2 min-h-[32px]">
                  {selectedRoles.length === 0 ? (
                      <span className="text-xs text-gray-600 italic">Користувач не має призначених ролей</span>
                  ) : (
                      selectedRoles.map(roleName => (
                          <div
                              key={roleName}
                              className="flex items-center gap-1 px-3 py-1 bg-red-900/10 border border-red-900/50 text-red-400 rounded text-xs font-bold uppercase tracking-wider group"
                          >
                            {roleName}
                            <button
                                onClick={() => removeRole(roleName)}
                                className="ml-1 p-0.5 rounded-full hover:bg-red-900/50 text-red-400 hover:text-red-200 transition-colors"
                            >
                              <X className="w-3 h-3" />
                            </button>
                          </div>
                      ))
                  )}
                </div>
              </div>
            </div>

            <DialogFooter className="p-4 border-t border-gray-800 bg-[#1a1a1a] flex gap-2">
              <Button
                  variant="outline"
                  onClick={() => setManagingUser(null)}
                  className="border-gray-700 text-gray-400 hover:text-white bg-transparent h-10 text-xs uppercase tracking-widest"
              >
                Скасувати
              </Button>
              <Button
                  onClick={saveRoles}
                  disabled={isSavingRoles}
                  className="bg-[#C89B64] text-black hover:bg-[#E5C07B] h-10 text-xs font-bold uppercase tracking-widest shadow-[0_0_15px_rgba(200,155,100,0.2)] disabled:opacity-50"
              >
                {isSavingRoles ? "Збереження..." : "Зберегти зміни"}
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>
  )
}