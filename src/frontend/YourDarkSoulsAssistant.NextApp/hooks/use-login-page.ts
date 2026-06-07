import { useCallback, useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useForm } from 'react-hook-form'
import { useAuth } from '@/contexts/auth-context'
import { LoginRequestDTO } from '@/types/dto/auth'

export function useLoginPage() {
  const [error, setError] = useState<string | null>(null)
  const form = useForm<LoginRequestDTO>()

  const router = useRouter()
  const { login, isAuthenticated, isLoading } = useAuth()

  useEffect(() => {
    if (isAuthenticated && !isLoading) {
      router.push('/home')
    }
  }, [isAuthenticated, isLoading, router])

  const onSubmit = useCallback(async (data: LoginRequestDTO) => {
    setError(null)

    const result = await login({ login: data.login, password: data.password })
    if (!result.success) {
      setError(result.error || 'Login failed')
    }
  }, [login])

  return {
    error,
    form,
    isAuthenticated,
    isLoading,
    onSubmit,
  }
}
