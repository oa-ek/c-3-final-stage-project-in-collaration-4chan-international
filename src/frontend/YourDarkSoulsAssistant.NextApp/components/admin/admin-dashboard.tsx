"use client"

import { useState, useEffect } from "react"
import { apiClient } from "@/lib/api-client"
import { checkIsAdmin } from "@/lib/utils"
import { Users, Shield, UserCheck, Loader2, BarChart3 } from "lucide-react"
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts"
import type { SmallUserResponseDTO } from "@/types/dto/users"

export function AdminDashboard() {
  const [users, setUsers] = useState<SmallUserResponseDTO[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const fetchUsers = async () => {
      setIsLoading(true)
      setError(null)
      try {
        const response = await apiClient("/Admin/all-users")
        if (response.ok) {
          const data: SmallUserResponseDTO[] = await response.json()
          setUsers(data)
        } else {
          setError("Failed to load dashboard data")
        }
      } catch {
        setError("Network error")
      } finally {
        setIsLoading(false)
      }
    }
    fetchUsers()
  }, [])

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64 text-[#C89B64]">
        <Loader2 className="w-8 h-8 animate-spin" />
      </div>
    )
  }

  const adminCount = users.filter((u) => checkIsAdmin(u.roles)).length
  const usersWithRoles = users.filter((u) => u.roles.length > 0).length

  // Build a real role-distribution dataset from the live user list
  const roleCounts: Record<string, number> = {}
  for (const u of users) {
    if (u.roles.length === 0) {
      roleCounts["No role"] = (roleCounts["No role"] || 0) + 1
    } else {
      for (const r of u.roles) {
        roleCounts[r] = (roleCounts[r] || 0) + 1
      }
    }
  }
  const roleStats = Object.entries(roleCounts).map(([name, count]) => ({ name, count }))

  const summaryStats = [
    { label: "Total Users", value: users.length, icon: Users },
    { label: "Administrators", value: adminCount, icon: Shield },
    { label: "Users with Roles", value: usersWithRoles, icon: UserCheck },
  ]

  return (
    <div className="space-y-6">
      {error && (
        <div className="text-sm text-red-400 bg-red-900/20 border border-red-900/50 rounded px-3 py-2">
          {error}
        </div>
      )}

      {/* Summary Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {summaryStats.map((stat) => {
          const Icon = stat.icon
          return (
            <div
              key={stat.label}
              className="bg-[#1A1A1D] border border-[#C89B64]/30 rounded-lg p-4 hover:border-[#C89B64] transition"
            >
              <div className="flex items-center justify-between mb-3">
                <Icon className="w-6 h-6 text-[#C89B64]" />
              </div>
              <p className="text-2xl font-serif font-bold text-gray-200 mb-1">{stat.value}</p>
              <p className="text-gray-400 text-xs">{stat.label}</p>
            </div>
          )
        })}
      </div>

      {/* Role Distribution Chart */}
      <div className="bg-[#1A1A1D] border border-[#C89B64]/30 rounded-lg p-4">
        <h3 className="text-lg font-serif text-gray-200 mb-4 border-b border-[#C89B64]/30 pb-2">
          Role Distribution
        </h3>
        {roleStats.length === 0 ? (
          <div className="flex flex-col items-center justify-center h-48 text-center">
            <BarChart3 className="w-10 h-10 text-gray-700 mb-3" />
            <p className="text-gray-500 text-sm">No user data available yet</p>
          </div>
        ) : (
          <div className="h-48">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={roleStats}>
                <CartesianGrid strokeDasharray="3 3" stroke="#333" />
                <XAxis dataKey="name" tick={{ fill: "#9CA3AF", fontSize: 11 }} />
                <YAxis allowDecimals={false} tick={{ fill: "#9CA3AF", fontSize: 11 }} />
                <Tooltip
                  contentStyle={{
                    backgroundColor: "#1A1A1D",
                    border: "1px solid #C89B64",
                    borderRadius: "4px",
                  }}
                  labelStyle={{ color: "#D4AF37" }}
                  cursor={{ fill: "rgba(200,155,100,0.1)" }}
                />
                <Bar dataKey="count" fill="#C89B64" radius={[4, 4, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        )}
      </div>

      {/* Recent Users */}
      <div className="bg-[#1A1A1D] border border-[#C89B64]/30 rounded-lg p-4">
        <h3 className="text-lg font-serif text-gray-200 mb-4 border-b border-[#C89B64]/30 pb-2">
          Users
        </h3>
        {users.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-10 text-center">
            <Users className="w-10 h-10 text-gray-700 mb-3" />
            <p className="text-gray-500 text-sm">No users registered yet</p>
          </div>
        ) : (
          <div className="space-y-3">
            {users.slice(0, 6).map((u) => (
              <div
                key={u.id}
                className="flex items-center justify-between py-2 border-b border-gray-800 last:border-0"
              >
                <div>
                  <p className="text-gray-200 text-sm">{u.firstName} {u.lastName}</p>
                  <p className="text-xs text-gray-500">@{u.userName}</p>
                </div>
                <span className="text-xs text-[#C89B64] uppercase tracking-widest">
                  {checkIsAdmin(u.roles) ? "Admin" : (u.roles[0] ?? "Member")}
                </span>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
