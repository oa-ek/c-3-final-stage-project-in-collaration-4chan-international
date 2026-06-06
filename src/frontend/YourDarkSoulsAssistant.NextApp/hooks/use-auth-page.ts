import { useCallback, useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useForm } from 'react-hook-form'
import { useAuth } from '@/contexts/auth-context'
import { LoginRequestDTO } from '@/types/dto/auth'
import { CreateUserRequestDTO } from '@/types/dto/users'

export function useAuthPage() {
  const [isLogin, setIsLogin] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const loginForm = useForm<LoginRequestDTO>()
  const registerForm = useForm<CreateUserRequestDTO>()

  const router = useRouter()
  const { login, register: registerUser, isAuthenticated, isLoading } = useAuth()

  useEffect(() => {
    if (isAuthenticated && !isLoading) {
      router.push('/home')
    }
  }, [isAuthenticated, isLoading, router])

  const onLoginSubmit = useCallback(async (data: LoginRequestDTO) => {
    setError(null)
    const result = await login({ login: data.login, password: data.password })
    if (!result.success) {
      setError(result.error || 'Login failed')
    }
  }, [login])

  const onRegisterSubmit = useCallback(async (data: CreateUserRequestDTO) => {
    setError(null)

    if (data.password !== data.confirmPassword) {
      setError('Passwords do not match')
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
      setError(result.error || 'Registration failed')
    }
  }, [registerUser])

  const toggleMode = useCallback(() => {
    setIsLogin((prev) => !prev)
    setError(null)
    loginForm.reset()
    registerForm.reset()
  }, [loginForm, registerForm])

  return {
    isLogin,
    error,
    loginForm,
    registerForm,
    isAuthenticated,
    isLoading,
    onLoginSubmit,
    onRegisterSubmit,
    toggleMode,
  }
}
