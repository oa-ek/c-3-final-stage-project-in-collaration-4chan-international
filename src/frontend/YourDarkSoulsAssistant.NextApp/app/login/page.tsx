"use client"

import {useState, useEffect} from 'react'
import {useRouter} from 'next/navigation'
import {useForm} from 'react-hook-form'
import {useAuth} from '@/contexts/auth-context'
import Link from 'next/link'
import type {LoginRequestDTO} from '@/types/dto'
import {getImageUrl} from '@/lib/content-utils'

export default function LoginPage() {
    const [error, setError] = useState<string | null>(null)
    const {register, handleSubmit, formState: {errors}} = useForm<LoginRequestDTO>()

    const router = useRouter()
    const {login, isAuthenticated, isLoading} = useAuth()

    // Редирект, якщо вже залогінений
    useEffect(() => {
        if (isAuthenticated && !isLoading) {
            router.push('/profile')
        }
    }, [isAuthenticated, isLoading, router])

    const onSubmit = async (data: LoginRequestDTO) => {
        setError(null)
        const result = await login(data)
        if (!result.success) {
            setError(result.error || "Помилка входу")
        }
    }

    return (
        <div
            className="min-h-screen flex items-center justify-center bg-[#0a0a0a] px-4 bg-cover bg-center"
            style={{backgroundImage: `url('${getImageUrl('auth/wallpaper')}')`}}
        >
            <div
                className="w-full max-w-md p-10 bg-[#121212]/60 backdrop-blur-md border border-[#C89B64]/40 shadow-[0_0_40px_rgba(200,155,100,0.1)] rounded-sm z-10 relative">
                {/* Декоративний елемент у стилі DS */}
                <div
                    className="absolute top-0 left-0 w-full h-1 bg-gradient-to-r from-transparent via-[#C89B64] to-transparent opacity-50"/>

                <h1 className="text-3xl font-serif text-center mb-8 text-[#C89B64] uppercase tracking-[0.3em]">
                    Вхід
                </h1>

                {error && (
                    <div className="mb-6 p-3 bg-red-900/30 border border-red-500/50 text-red-200 text-sm text-center">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div>
                        <input
                            {...register("email", {required: "Email обов'язковий"})}
                            type="email"
                            placeholder="Email Address"
                            className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
                        />
                        {errors.email && <span className="text-xs text-red-500 mt-1">{errors.email.message}</span>}
                    </div>

                    <div>
                        <input
                            {...register("password", {required: "Пароль обов'язковий"})}
                            type="password"
                            placeholder="Password"
                            className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
                        />
                        {errors.password && <span className="text-xs text-red-500 mt-1">{errors.password.message}</span>}
                    </div>

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full py-3 mt-6 bg-gradient-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.2em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all disabled:opacity-50"
                    >
                        {isLoading ? "Loading..." : "Log In"}
                    </button>
                </form>

            <div className="mt-8 text-center text-xs text-gray-500 uppercase tracking-widest">
                <p>
                    Вперше тут?
                    <Link href="/register"
                          className="ml-2 text-[#C89B64] hover:text-[#E5C07B] transition-colors underline decoration-[#C89B64]/30">
                        Створити обліковий запис
                    </Link>
                </p>
            </div>
        </div>
</div>
)
}