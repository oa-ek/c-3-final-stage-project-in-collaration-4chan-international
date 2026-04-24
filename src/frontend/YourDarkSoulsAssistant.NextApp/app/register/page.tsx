"use client"

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { useForm } from 'react-hook-form'
import { useAuth } from '@/contexts/auth-context'
import Link from 'next/link'
import type { RegisterRequestDTO } from '@/types/dto'
import {getImageUrl} from "@/lib/content-utils";

export default function RegisterPage() {
    const [error, setError] = useState<string | null>(null)
    const { register, handleSubmit, watch, formState: { errors } } = useForm<RegisterRequestDTO>()

    const router = useRouter()
    const { register: registerUser, isAuthenticated, isLoading } = useAuth()

    useEffect(() => {
        if (isAuthenticated && !isLoading) {
            router.push('/builds')
        }
    }, [isAuthenticated, isLoading, router])

    const onSubmit = async (data: RegisterRequestDTO) => {
        setError(null)
        const result = await registerUser(data)
        if (!result.success) {
            setError(result.error || "Помилка реєстрації")
        }
    }

    return (
        <div
            className="min-h-screen flex items-center justify-center bg-[#0a0a0a] px-4 py-12 bg-cover bg-center"
            style={{ backgroundImage: `url('${getImageUrl('auth/wallpaper')}')` }}
        >
            <div className="w-full max-w-md p-10 bg-[#121212]/60 backdrop-blur-md border border-[#C89B64]/40 shadow-[0_0_40px_rgba(200,155,100,0.1)] rounded-sm z-10 relative">
                <div className="absolute top-0 left-0 w-full h-1 bg-gradient-to-r from-transparent via-[#C89B64] to-transparent opacity-50" />

                <h1 className="text-3xl font-serif text-center mb-8 text-[#C89B64] uppercase tracking-[0.3em]">
                    Реєстрація
                </h1>

                {error && (
                    <div className="mb-6 p-3 bg-red-900/30 border border-red-500/50 text-red-200 text-sm text-center">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div className="grid grid-cols-2 gap-4">
                        <input
                            {...register("firstName", { required: true })}
                            type="text"
                            placeholder="First Name"
                            className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
                        />
                        <input
                            {...register("lastName", { required: true })}
                            type="text"
                            placeholder="Last Name"
                            className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
                        />
                    </div>

                    <input
                        {...register("userName", { required: "Ім'я користувача обов'язкове" })}
                        type="text"
                        placeholder="Username"
                        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
                    />

                    <input
                        {...register("email", { required: "Email обов'язковий" })}
                        type="email"
                        placeholder="Email Address"
                        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
                    />

                    <input
                        {...register("password", { required: "Пароль обов'язковий", minLength: 6 })}
                        type="password"
                        placeholder="Password"
                        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
                    />

                    <input
                        {...register("confirmPassword", {
                            required: "Підтвердіть пароль",
                            validate: (value) => value === watch('password') || "Паролі не збігаються"
                        })}
                        type="password"
                        placeholder="CONFIRM PASSWORD"
                        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none placeholder:text-gray-600"
                    />
                    {errors.confirmPassword && <span className="text-xs text-red-500">{errors.confirmPassword.message}</span>}

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full py-3 mt-6 bg-gradient-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.2em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all disabled:opacity-50"
                    >
                        {isLoading ? "Loading..." : "Register"}
                    </button>
                </form>
                <div className="mt-8 text-center text-xs text-gray-500 uppercase tracking-widest">
                    <p>
                        Вже є акаунт?
                        <Link href="/login" className="ml-2 text-[#C89B64] hover:text-[#E5C07B] transition-colors underline decoration-[#C89B64]/30">
                            Увійти до гри
                        </Link>
                    </p>
                </div>
            </div>
        </div>
    )
}