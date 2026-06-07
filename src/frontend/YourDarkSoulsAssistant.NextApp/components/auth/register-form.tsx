import { UseFormReturn } from 'react-hook-form'
import { CreateUserRequestDTO } from '@/types/dto/users'

interface RegisterFormProps {
  form: UseFormReturn<CreateUserRequestDTO>
  isLoading: boolean
  onSubmit: (data: CreateUserRequestDTO) => Promise<void>
}

export function RegisterForm({ form, isLoading, onSubmit }: RegisterFormProps) {
  return (
    <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
      <div className="grid grid-cols-2 gap-4">
        <input
          {...form.register('firstName', { required: true })}
          type="text"
          placeholder="First Name"
          className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
        />
        <input
          {...form.register('lastName', { required: true })}
          type="text"
          placeholder="Last Name"
          className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
        />
      </div>

      <input
        {...form.register('userName', { required: true })}
        type="text"
        placeholder="Username"
        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
      />

      <input
        {...form.register('email', { required: true })}
        type="email"
        placeholder="Email Address"
        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
      />

      <input
        {...form.register('password', { required: true })}
        type="password"
        placeholder="Password"
        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
      />

      <input
        {...form.register('confirmPassword', { required: true })}
        type="password"
        placeholder="Confirm Password"
        className="w-full p-3 bg-black/40 border border-gray-700 text-gray-200 focus:border-[#C89B64] outline-none transition-all placeholder:text-gray-500"
      />

      <button
        type="submit"
        disabled={isLoading}
        className="w-full py-3 mt-6 bg-gradient-to-r from-[#9A7B4F] to-[#C89B64] hover:from-[#C89B64] hover:to-[#E5C07B] text-black font-bold uppercase tracking-[0.2em] shadow-[0_0_15px_rgba(200,155,100,0.3)] hover:shadow-[0_0_25px_rgba(200,155,100,0.5)] transition-all disabled:opacity-50"
      >
        {isLoading ? 'Loading...' : 'Register'}
      </button>
    </form>
  )
}
