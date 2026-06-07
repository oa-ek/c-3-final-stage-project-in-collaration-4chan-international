import { useCallback, useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useForm } from 'react-hook-form'
import { useAuth } from '@/contexts/auth-context'
import { CreateUserRequestDTO } from '@/types/dto/users'

export function useRegisterPage() {
  const [error, setError] = useState<string | null>(null)
  const form = useForm<CreateUserRequestDTO>()

  const router = useRouter()
  const { register: registerUser, isAuthenticated, isLoading } = useAuth()

  useEffect(() => {
    if (isAuthenticated && !isLoading) {
      router.push('/home')
    }
  }, [isAuthenticated, isLoading, router])

  const onSubmit = useCallback(async (data: CreateUserRequestDTO) => {
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

  return {
    error,
    form,
    isAuthenticated,
    isLoading,
    onSubmit,
  }
}
