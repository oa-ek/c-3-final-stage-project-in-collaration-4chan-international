import Link from 'next/link'
import { getImageUrl } from '@/lib/content-utils'

interface AuthShellProps {
  title: string
  error: string | null
  footerText: string
  footerLinkText: string
  footerLinkHref: string
  children: React.ReactNode
}

export function AuthShell({
  title,
  error,
  footerText,
  footerLinkText,
  footerLinkHref,
  children,
}: AuthShellProps) {
  return (
    <div
      className="min-h-screen flex items-center justify-center bg-cover bg-center bg-no-repeat relative"
      style={{ backgroundImage: `url('${getImageUrl('auth/wallpaper')}')` }}
    >
      <div className="w-full max-w-md p-10 bg-[#121212]/60 backdrop-blur-md border border-[#C89B64]/40 shadow-[0_0_40px_rgba(200,155,100,0.1)] rounded-sm z-10 relative">
        <h2 className="text-2xl font-serif text-[#D4AF37] text-center mb-8 uppercase tracking-widest">
          {title}
        </h2>

        {error && (
          <div className="mb-4 p-3 bg-red-900/30 border border-red-800 text-red-400 text-sm text-center">
            {error}
          </div>
        )}

        {children}

        <div className="mt-6 text-center text-sm text-gray-400">
          <p>
            {footerText}
            <Link href={footerLinkHref} className="ml-2 text-[#C89B64] hover:underline focus:outline-none">
              {footerLinkText}
            </Link>
          </p>
        </div>
      </div>

      <div className="absolute bottom-0 left-0 right-0 h-64 bg-gradient-to-t from-orange-900/10 to-transparent pointer-events-none"></div>
    </div>
  )
}
