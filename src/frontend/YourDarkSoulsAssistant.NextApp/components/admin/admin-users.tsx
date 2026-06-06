"use client"

import { useState, useEffect, useCallback } from "react"
import { apiClient } from "@/lib/api-client"
import { checkIsAdmin } from "@/lib/utils"
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
import type { RoleDTO } from "@/types/dto/roles"
import type { SmallUserResponseDTO } from "@/types/dto/users"

// Session cache settings
const CACHE_KEY = "admin_users_cache"
const CACHE_TTL_MS = 5 * 60 * 1000 // 5 minutes

export function AdminUsers() {
  const [users, setUsers] = useState<SmallUserResponseDTO[]>([])
  const [allRoles, setAllRoles] = useState<RoleDTO[]>([])

  const [isLoading, setIsLoading] = useState(true)
  const [isRefreshing, setIsRefreshing] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [searchQuery, setSearchQuery] = useState("")

  // Role management modal state
  const [managingUser, setManagingUser] = useState<SmallUserResponseDTO | null>(null)
  const [selectedRoles, setSelectedRoles] = useState<string[]>([])
  const [isSavingRoles, setIsSavingRoles] = useState(false)

  // --- CACHING + LOADING LOGIC ---
  const fetchUsers = useCallback(async (forceRefresh = false) => {
    try {
      if (forceRefresh) setIsRefreshing(true)
      else setIsLoading(true)
      setError(null)

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
        const data: SmallUserResponseDTO[] = await response.json()
        setUsers(data)
        sessionStorage.setItem(CACHE_KEY, JSON.stringify({ data, timestamp: Date.now() }))
      } else {
        setError("Failed to load the list of users")
      }
    } catch (err) {
      setError("Network error")
    } finally {
      setIsLoading(false)
      setIsRefreshing(false)
    }
  }, [])

  useEffect(() => {
    fetchUsers()
  }, [fetchUsers])

  // --- ROLE MANAGEMENT LOGIC ---
  const handleManageRoles = async (user: SmallUserResponseDTO) => {
    setManagingUser(user)

    const cleanRoles = user.roles.filter(role => role != null && role !== "")
    setSelectedRoles(cleanRoles)

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
      const cleanRoles = selectedRoles.filter(role => role != null && role !== "")

      const response = await apiClient("/Admin/change-role", {
        method: "POST",
        body: JSON.stringify({ id: managingUser.id, roles: cleanRoles })
      })

      if (response.ok) {
        const updatedUsers = users.map(u =>
          u.id === managingUser.id ? { ...u, roles: cleanRoles } : u
        )
        setUsers(updatedUsers)
        sessionStorage.setItem(CACHE_KEY, JSON.stringify({ data: updatedUsers, timestamp: Date.now() }))
        setManagingUser(null)
      } else {
        alert("Failed to update roles. Check the server console.")
      }
    } catch (e) {
      alert("Network error while saving")
    } finally {
      setIsSavingRoles(false)
    }
  }

  const handleDeleteUser = async (userId: string) => {
    if (confirm("Are you sure you want to delete this user?")) {
      const response = await apiClient(`/Admin/users/${userId}`, { method: "DELETE" })
      if (response.ok) {
        const updatedUsers = users.filter(u => u.id !== userId)
        setUsers(updatedUsers)
        sessionStorage.setItem(CACHE_KEY, JSON.stringify({ data: updatedUsers, timestamp: Date.now() }))
      } else {
        alert("Error while deleting")
      }
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
            <h2 className="text-lg font-serif text-[#C89B64] uppercase tracking-wider">Users</h2>
          </div>
          <button
            onClick={() => fetchUsers(true)}
            disabled={isRefreshing}
            className="flex items-center gap-1 text-[10px] uppercase tracking-widest text-gray-500 hover:text-[#C89B64] transition-colors"
          >
            <RefreshCw className={`w-3 h-3 ${isRefreshing ? "animate-spin" : ""}`} /> Refresh data
          </button>
        </div>
        <span className="text-xs text-gray-500 uppercase tracking-widest">
          Total: <span className="text-[#C89B64]">{users.length}</span>
        </span>
      </div>

      {error && (
        <div className="text-sm text-red-400 bg-red-900/20 border border-red-900/50 rounded px-3 py-2">
          {error}
        </div>
      )}

      {/* Search */}
      <div className="relative">
        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500" />
        <Input
          placeholder="Search users..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          className="pl-10 bg-black/40 border-gray-800 text-gray-300 placeholder:text-gray-600 h-10 text-sm focus:border-[#C89B64]"
        />
      </div>

      {/* Users List */}
      <div className="border border-gray-800 rounded-lg overflow-hidden max-h-[500px] overflow-y-auto">
        {filteredUsers.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-16 text-center">
            <Users className="w-10 h-10 text-gray-700 mb-3" />
            <p className="text-gray-500 text-sm">No users found</p>
          </div>
        ) : (
          <table className="w-full">
            <thead className="bg-[#1a1a1a] sticky top-0 z-10 border-b border-gray-800">
              <tr>
                <th className="px-4 py-3 text-left text-[10px] font-medium text-gray-500 uppercase tracking-widest">User</th>
                <th className="px-4 py-3 text-left text-[10px] font-medium text-gray-500 uppercase tracking-widest hidden md:table-cell">Email</th>
                <th className="px-4 py-3 text-left text-[10px] font-medium text-gray-500 uppercase tracking-widest">Roles</th>
                <th className="px-4 py-3 text-right text-[10px] font-medium text-gray-500 uppercase tracking-widest">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-800/50 bg-black/20">
              {filteredUsers.map((u) => (
                <tr key={u.id} className="hover:bg-white/[0.02] transition-colors group">
                  <td className="px-4 py-3">
                    <div className="flex items-center gap-3">
                      <div className="w-8 h-8 rounded-full bg-[#1a1a1a] border border-gray-800 flex items-center justify-center shrink-0">
                        {checkIsAdmin(u.roles) ? <Shield className="w-4 h-4 text-[#C89B64]" /> : <UserIcon className="w-4 h-4 text-gray-500" />}
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
                        <span className="text-gray-600 text-xs italic">None</span>
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
                          <Shield className="w-3 h-3 mr-2" /> Manage roles
                        </DropdownMenuItem>
                        <DropdownMenuItem
                          onClick={() => handleDeleteUser(u.id)}
                          className="text-red-400 hover:text-red-300 hover:bg-red-900/20 cursor-pointer text-xs uppercase tracking-wider"
                        >
                          <Trash2 className="w-3 h-3 mr-2" /> Delete
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {/* Role management dialog */}
      <Dialog open={!!managingUser} onOpenChange={(open) => !open && setManagingUser(null)}>
        <DialogContent className="bg-[#0a0a0a] border-[#3c3c3c] max-w-md p-0 overflow-hidden shadow-[0_0_50px_rgba(0,0,0,0.8)]">
          <DialogHeader className="p-6 pb-4 border-b border-gray-800 bg-[#1a1a1a]">
            <DialogTitle className="text-[#C89B64] font-serif uppercase tracking-widest flex items-center gap-2">
              <Shield className="w-5 h-5" /> Access management
            </DialogTitle>
            <p className="text-xs text-gray-400 mt-1">
              User: <span className="text-gray-200">{managingUser?.userName}</span>
            </p>
          </DialogHeader>

          <div className="p-6 space-y-6">
            {/* Available roles */}
            <div>
              <label className="text-[10px] uppercase text-gray-500 tracking-[0.2em] mb-3 block">Available roles (click to add)</label>
              <div className="flex flex-wrap gap-2">
                {allRoles.length === 0 ? (
                  <span className="text-xs text-gray-600 flex items-center gap-2"><Loader2 className="w-3 h-3 animate-spin" /> Loading...</span>
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

            {/* Assigned roles */}
            <div className="bg-black/40 p-4 border border-gray-800 rounded">
              <label className="text-[10px] uppercase text-[#C89B64] tracking-[0.2em] mb-3 block">Active user roles</label>
              <div className="flex flex-wrap gap-2 min-h-[32px]">
                {selectedRoles.length === 0 ? (
                  <span className="text-xs text-gray-600 italic">This user has no assigned roles</span>
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
              Cancel
            </Button>
            <Button
              onClick={saveRoles}
              disabled={isSavingRoles}
              className="bg-[#C89B64] text-black hover:bg-[#E5C07B] h-10 text-xs font-bold uppercase tracking-widest shadow-[0_0_15px_rgba(200,155,100,0.2)] disabled:opacity-50"
            >
              {isSavingRoles ? "Saving..." : "Save changes"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}
