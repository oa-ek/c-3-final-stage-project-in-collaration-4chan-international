"use client"

import { AuthShell } from '@/components/auth/auth-shell'
import { LoginForm } from '@/components/auth/login-form'
import { useLoginPage } from '@/hooks/use-login-page'

export default function LoginPage() {
  const {
    error,
    form,
    isAuthenticated,
    isLoading,
    onSubmit,
  } = useLoginPage()

  if (isLoading || isAuthenticated) {
    return (
        <div className="min-h-screen flex items-center justify-center bg-[#121212]">
          <div className="text-[#C89B64] text-xl font-serif">Loading...</div>
        </div>
    )
  }

  return (
    <AuthShell
      title="Welcome Unkindled/Tarnished"
      error={error}
      footerText="First time here?"
      footerLinkText="Create an account"
      footerLinkHref="/register"
    >
      <LoginForm form={form} isLoading={isLoading} onSubmit={onSubmit} />
    </AuthShell>
  )
}
