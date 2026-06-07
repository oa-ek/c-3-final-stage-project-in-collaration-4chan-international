"use client"

import { AuthShell } from '@/components/auth/auth-shell'
import { RegisterForm } from '@/components/auth/register-form'
import { useRegisterPage } from '@/hooks/use-register-page'

export default function RegisterPage() {
  const {
    error,
    form,
    isAuthenticated,
    isLoading,
    onSubmit,
  } = useRegisterPage()

  if (isLoading || isAuthenticated) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-[#121212]">
        <div className="text-[#C89B64] text-xl font-serif">Loading...</div>
      </div>
    )
  }

  return (
    <AuthShell
      title="New Journey"
      error={error}
      footerText="Already have an account?"
      footerLinkText="Back to Login"
      footerLinkHref="/login"
    >
      <RegisterForm form={form} isLoading={isLoading} onSubmit={onSubmit} />
    </AuthShell>
  )
}
