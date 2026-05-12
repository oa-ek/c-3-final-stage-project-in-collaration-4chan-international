"use client"

import {useState, useEffect} from 'react'
import {useRouter} from 'next/navigation'
import {useForm} from 'react-hook-form'
import {useAuth} from '@/contexts/auth-context'
import Link from 'next/link'
import type {LoginRequestDTO} from '@/types/dto/auth'
import {getImageUrl} from '@/lib/content-utils'

export default function LoginPage() {
    const [error, setError] = useState<string | null>(null)
    const {register, handleSubmit, formState: {errors}} = useForm<LoginRequestDTO>()

    const router = useRouter()
    const {login, isAuthenticated, isLoading} = useAuth()

    useEffect(() => {
        if (isAuthenticated && !isLoading) {
            const timer = setTimeout(() => {
                router.push('/profile')
            }, 0);
            return () => clearTimeout(timer);
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
                <div
                    className="absolute top-0 left-0 w-full h-1 bg-linear-to-r from-transparent via-[#C89B64] to-transparent opacity-50"/>

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
                            {...register("login", {required: "Login обов'язковий"})}
                            type="login"
                            placeholder="Email Address or Username"
                            className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
                        />
                        {errors.login && <span className="text-xs text-red-500 mt-1">{errors.login.message}</span>}
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

                    <div className="flex items-center gap-3 mb-4 px-1">
                        <div className="relative flex items-center">
                            <input
                                {...register("rememberMe")}
                                type="checkbox"
                                id="rememberMe"
                                className="w-4 h-4 rounded border-gray-700 bg-black/40 text-[#C89B64] focus:ring-[#C89B64] accent-[#C89B64] cursor-pointer"
                            />
                        </div>
                        <label
                            htmlFor="rememberMe"
                            className="text-[10px] text-gray-400 uppercase tracking-[0.2em] cursor-pointer hover:text-[#C89B64] transition-colors"
                        >
                            Записати мою душу в Вальгалу, чернь!
                        </label>
                    </div>

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full py-3 mt-6 bg-linear-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.2em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all disabled:opacity-50"
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