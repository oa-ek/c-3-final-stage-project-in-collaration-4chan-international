"use client"

import { Users, Sword, Shield, TrendingUp } from "lucide-react"
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
} from "recharts"

const classStats = [
  { name: "Knight", count: 300 },
  { name: "Sorcerer", count: 150 },
  { name: "Warrior", count: 180 },
  { name: "Thief", count: 45 },
  { name: "Cleric", count: 90 },
]

const weaponStats = [
  { name: "Claymore", count: 2500 },
  { name: "Moonlight GS", count: 800 },
  { name: "Uchigatana", count: 1500 },
  { name: "Rivers of Blood", count: 2200 },
  { name: "Colossal Sword", count: 950 },
]

const COLORS = ["#C89B64", "#D4AF37", "#9A7B4F", "#E5C07B", "#8B7355"]

const summaryStats = [
  { label: "Total Users", value: "1,245", icon: Users, trend: "+12%" },
  { label: "Total Builds", value: "3,892", icon: Shield, trend: "+8%" },
  { label: "Equipment Items", value: "2,156", icon: Sword, trend: "+5%" },
  { label: "Daily Active", value: "342", icon: TrendingUp, trend: "+23%" },
]

export function AdminDashboard() {
  return (
    <div className="space-y-6">
      {/* Summary Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {summaryStats.map((stat) => {
          const Icon = stat.icon
          return (
            <div
              key={stat.label}
              className="bg-[#1A1A1D] border border-[#C89B64]/30 rounded-lg p-4 hover:border-[#C89B64] transition"
            >
              <div className="flex items-center justify-between mb-3">
                <Icon className="w-6 h-6 text-[#C89B64]" />
                <span className="text-green-500 text-xs font-medium">{stat.trend}</span>
              </div>
              <p className="text-2xl font-serif font-bold text-gray-200 mb-1">{stat.value}</p>
              <p className="text-gray-400 text-xs">{stat.label}</p>
            </div>
          )
        })}
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Class Distribution Chart */}
        <div className="bg-[#1A1A1D] border border-[#C89B64]/30 rounded-lg p-4">
          <h3 className="text-lg font-serif text-gray-200 mb-4 border-b border-[#C89B64]/30 pb-2">
            Popular Classes
          </h3>
          <div className="h-48">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={classStats}>
                <CartesianGrid strokeDasharray="3 3" stroke="#333" />
                <XAxis dataKey="name" tick={{ fill: "#9CA3AF", fontSize: 11 }} />
                <YAxis tick={{ fill: "#9CA3AF", fontSize: 11 }} />
                <Tooltip
                  contentStyle={{
                    backgroundColor: "#1A1A1D",
                    border: "1px solid #C89B64",
                    borderRadius: "4px",
                  }}
                  labelStyle={{ color: "#D4AF37" }}
                />
                <Bar dataKey="count" fill="#C89B64" radius={[4, 4, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Weapon Popularity Chart */}
        <div className="bg-[#1A1A1D] border border-[#C89B64]/30 rounded-lg p-4">
          <h3 className="text-lg font-serif text-gray-200 mb-4 border-b border-[#C89B64]/30 pb-2">
            Most Used Weapons
          </h3>
          <div className="h-48 flex items-center">
            <ResponsiveContainer width="50%" height="100%">
              <PieChart>
                <Pie
                  data={weaponStats}
                  cx="50%"
                  cy="50%"
                  innerRadius={30}
                  outerRadius={60}
                  paddingAngle={2}
                  dataKey="count"
                >
                  {weaponStats.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip
                  contentStyle={{
                    backgroundColor: "#1A1A1D",
                    border: "1px solid #C89B64",
                    borderRadius: "4px",
                  }}
                />
              </PieChart>
            </ResponsiveContainer>
            <div className="flex-1 space-y-1">
              {weaponStats.map((weapon, index) => (
                <div key={weapon.name} className="flex items-center gap-2">
                  <div
                    className="w-2 h-2 rounded-full"
                    style={{ backgroundColor: COLORS[index % COLORS.length] }}
                  />
                  <span className="text-xs text-gray-300">{weapon.name}</span>
                  <span className="text-xs text-gray-500 ml-auto">{weapon.count}</span>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>

      {/* Recent Activity */}
      <div className="bg-[#1A1A1D] border border-[#C89B64]/30 rounded-lg p-4">
        <h3 className="text-lg font-serif text-gray-200 mb-4 border-b border-[#C89B64]/30 pb-2">
          Recent Activity
        </h3>
        <div className="space-y-3">
          {[
            { action: "New user registered", user: "john.doe@player.com", time: "2 min ago" },
            { action: "Build created", user: "warrior_main", time: "15 min ago" },
            { action: "Equipment added", user: "admin", time: "1 hour ago" },
            { action: "User updated profile", user: "sorcerer_fan", time: "2 hours ago" },
          ].map((activity, index) => (
            <div
              key={index}
              className="flex items-center justify-between py-2 border-b border-gray-800 last:border-0"
            >
              <div>
                <p className="text-gray-200 text-sm">{activity.action}</p>
                <p className="text-xs text-gray-500">{activity.user}</p>
              </div>
              <span className="text-xs text-gray-400">{activity.time}</span>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
