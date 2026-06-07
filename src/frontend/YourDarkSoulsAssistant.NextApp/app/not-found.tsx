import Link from 'next/link'
import { Sword } from 'lucide-react'

export default function NotFound() {
    return (
        <div className="min-h-screen bg-[#0a0a0a] flex flex-col items-center justify-center text-center p-4">
            <Sword className="w-16 h-16 text-gray-800 mb-6" />

            <h1 className="text-6xl md:text-8xl font-serif text-[#C89B64] tracking-widest mb-4 opacity-80">
                404
            </h1>

            <p className="text-gray-400 text-sm md:text-base uppercase tracking-[0.3em] mb-8">
                This area does not exist in our world
            </p>

            <Link
                href="/"
                prefetch={false}
                className="px-8 py-3 bg-transparent border border-[#C89B64] text-[#C89B64] hover:bg-[#C89B64] hover:text-black transition-all font-bold uppercase tracking-widest text-xs"
            >
                Return to Bonfire
            </Link>
        </div>
    )
}
