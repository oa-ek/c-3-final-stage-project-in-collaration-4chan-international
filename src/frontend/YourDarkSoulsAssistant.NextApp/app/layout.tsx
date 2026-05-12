import type { Metadata } from 'next'
import { EB_Garamond, Geist_Mono } from 'next/font/google'
import { Analytics } from '@vercel/analytics/next'
import { AuthProvider } from '@/contexts/auth-context'
import './globals.css'

const ebGaramond = EB_Garamond({
    subsets: ["latin"],
    weight: ["400", "500", "600", "700", "800"],
    variable: '--font-eb-garamond'
});

const geistMono = Geist_Mono({
    subsets: ["latin"],
    variable: '--font-geist-mono'
});

export const metadata: Metadata = {
  title: 'Equipment - Elden Ring UI',
  description: 'Elden Ring inspired equipment and inventory system',
  icons: {
    icon: [
      {
        url: '/icon-light-32x32.png',
        media: '(prefers-color-scheme: light)',
      },
      {
        url: '/icon-dark-32x32.png',
        media: '(prefers-color-scheme: dark)',
      },
      {
        url: '/icon.svg',
        type: 'image/svg+xml',
      },
    ],
    apple: '/apple-icon.png',
  },
}

export default function RootLayout({
                                     children,
                                   }: Readonly<{
  children: React.ReactNode
}>) {
  return (
      <html lang="en">
      <body className={`${ebGaramond.variable} ${geistMono.variable} font-sans antialiased`}>
      <AuthProvider>
        {children}
      </AuthProvider>
      {process.env.NODE_ENV === 'production' && <Analytics />}
      </body>
      </html>
  )
}
