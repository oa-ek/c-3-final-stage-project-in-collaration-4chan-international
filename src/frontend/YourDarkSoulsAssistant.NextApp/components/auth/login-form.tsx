import { UseFormReturn } from 'react-hook-form'
import { LoginRequestDTO } from '@/types/dto/auth'

interface LoginFormProps {
  form: UseFormReturn<LoginRequestDTO>
  isLoading: boolean
  onSubmit: (data: LoginRequestDTO) => Promise<void>
}

export function LoginForm({ form, isLoading, onSubmit }: LoginFormProps) {
  return (
    <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
      <input
        {...form.register('login', { required: true })}
        type="text"
        placeholder="Email Address or Username"
        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
      />

      <input
        {...form.register('password', { required: true })}
        type="password"
        placeholder="Password"
        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
      />

      <button
        type="submit"
        disabled={isLoading}
        className="w-full py-3 mt-6 bg-gradient-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.2em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all disabled:opacity-50"
      >
        {isLoading ? 'Loading...' : 'Log In'}
      </button>
    </form>
  )
}
