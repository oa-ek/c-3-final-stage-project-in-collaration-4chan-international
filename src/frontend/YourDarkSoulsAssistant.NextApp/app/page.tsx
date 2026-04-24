"use client"

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { useForm } from 'react-hook-form'
import { useAuth } from '@/contexts/auth-context'

interface LoginFormData {
  email: string
  password: string
}

interface RegisterFormData {
  firstName: string
  lastName: string
  userName: string
  email: string
  password: string
  confirmPassword: string
}

export default function AuthPage() {
  const [isLogin, setIsLogin] = useState(true)
  const [error, setError] = useState<string | null>(null)
  
  const loginForm = useForm<LoginFormData>()
  const registerForm = useForm<RegisterFormData>()
  
  const router = useRouter()
  const { login, register: registerUser, isAuthenticated, isLoading } = useAuth()

  // Redirect if already authenticated
  useEffect(() => {
    if (isAuthenticated && !isLoading) {
      router.push('/builds')
    }
  }, [isAuthenticated, isLoading, router])

  const onLoginSubmit = async (data: LoginFormData) => {
    setError(null)
    const result = await login({ email: data.email, password: data.password })
    if (!result.success) {
      setError(result.error || "Login failed")
    }
  }

  const onRegisterSubmit = async (data: RegisterFormData) => {
    setError(null)
    
    // Validate passwords match
    if (data.password !== data.confirmPassword) {
      setError("Passwords do not match")
      return
    }
    
    const result = await registerUser({
      firstName: data.firstName,
      lastName: data.lastName,
      userName: data.userName,
      email: data.email,
      password: data.password,
      confirmPassword: data.confirmPassword,
    })
    
    if (!result.success) {
      setError(result.error || "Registration failed")
    }
  }

  const toggleMode = () => {
    setIsLogin(!isLogin)
    setError(null)
    loginForm.reset()
    registerForm.reset()
  }

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-[#121212]">
        <div className="text-[#C89B64] text-xl font-serif">Loading...</div>
      </div>
    )
  }

  return (
    <div
      className="min-h-screen flex items-center justify-center bg-cover bg-center bg-no-repeat relative"
      style={{ backgroundImage: `url('/wallpaper.jpg')` }}
    >
      <div className="absolute inset-0 bg-black/75"></div>

      <div className="w-full max-w-md p-10 bg-[#121212]/60 backdrop-blur-md border border-[#C89B64]/40 shadow-[0_0_40px_rgba(200,155,100,0.1)] rounded-sm z-10 relative">
        <h2 className="text-2xl font-serif text-[#D4AF37] text-center mb-8 uppercase tracking-widest">
          {isLogin ? "Welcome Unkindled/Tarnished" : "New Journey"}
        </h2>

        {error && (
          <div className="mb-4 p-3 bg-red-900/30 border border-red-800 text-red-400 text-sm text-center">
            {error}
          </div>
        )}

        {isLogin ? (
          // Login Form
          <form onSubmit={loginForm.handleSubmit(onLoginSubmit)} className="space-y-4">
            <input
              {...loginForm.register("email", { required: true })}
              type="email"
              placeholder="Email Address"
              className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
            />

            <input
              {...loginForm.register("password", { required: true })}
              type="password"
              placeholder="Password"
              className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
            />

            <button
              type="submit"
              disabled={isLoading}
              className="w-full py-3 mt-6 bg-gradient-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.2em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all disabled:opacity-50"
            >
              {isLoading ? "Loading..." : "Log In"}
            </button>
          </form>
        ) : (
          // Register Form
          <form onSubmit={registerForm.handleSubmit(onRegisterSubmit)} className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <input
                {...registerForm.register("firstName", { required: true })}
                type="text"
                placeholder="First Name"
                className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
              />
              <input
                {...registerForm.register("lastName", { required: true })}
                type="text"
                placeholder="Last Name"
                className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
              />
            </div>

            <input
              {...registerForm.register("userName", { required: true })}
              type="text"
              placeholder="Username"
              className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
            />

            <input
              {...registerForm.register("email", { required: true })}
              type="email"
              placeholder="Email Address"
              className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
            />

            <input
              {...registerForm.register("password", { required: true })}
              type="password"
              placeholder="Password"
              className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
            />

            <input
              {...registerForm.register("confirmPassword", { required: true })}
              type="password"
              placeholder="Confirm Password"
              className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
            />

            <button
              type="submit"
              disabled={isLoading}
              className="w-full py-3 mt-6 bg-gradient-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.2em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all disabled:opacity-50"
            >
              {isLoading ? "Loading..." : "Register"}
            </button>
          </form>
        )}

        <div className="mt-6 text-center text-sm text-gray-400">
          <p>
            {isLogin ? "First time here?" : "Already have an account?"}
            <button
              onClick={toggleMode}
              className="ml-2 text-[#C89B64] hover:underline focus:outline-none"
            >
              {isLogin ? "Create an account" : "Back to Login"}
            </button>
          </p>
        </div>
      </div>

      <div className="absolute bottom-0 left-0 right-0 h-64 bg-gradient-to-t from-orange-900/10 to-transparent pointer-events-none"></div>
    </div>
  )
}
