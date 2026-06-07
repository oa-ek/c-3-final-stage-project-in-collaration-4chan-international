import Link from "next/link"
import { Sword } from "lucide-react"
import { getImageUrl } from "@/lib/content-utils"

export default function HomePage() {
    return (
        <div
            className="min-h-screen bg-[#0a0a0a] flex flex-col items-center justify-center relative overflow-hidden bg-cover bg-center"
            style={{ backgroundImage: `url('${getImageUrl('root/wallpaper')}')` }}
        >
            <div className="absolute inset-0 bg-linear-to-b from-transparent via-[#0a0a0a]/80 to-[#0a0a0a]" />

            <div className="z-10 text-center space-y-6 max-w-3xl px-4 flex flex-col items-center">
                <Sword className="w-16 h-16 text-[#C89B64] mb-4 opacity-80" />
                <h1 className="text-4xl md:text-6xl font-serif text-[#C89B64] uppercase tracking-[0.2em] drop-shadow-[0_0_15px_rgba(200,155,100,0.5)]">
                    Soul Forge
                </h1>
                <p className="text-gray-400 text-lg md:text-xl font-serif italic max-w-xl mx-auto">
                    "Your journey begins here. Craft perfect builds, manage your equipment, and prepare for new trials."
                </p>
                <div className="flex flex-col sm:flex-row gap-4 justify-center mt-12 w-full sm:w-auto">
                    <Link
                        href="/login"
                        className="px-8 py-3 bg-transparent border border-[#C89B64] text-[#C89B64] hover:bg-[#C89B64] hover:text-black transition-all font-bold uppercase tracking-widest text-sm text-center"
                    >
                        Enter the Game
                    </Link>
                    <Link
                        href="/register"
                        className="px-8 py-3 bg-[#C89B64] text-black hover:bg-[#E5C07B] transition-all font-bold uppercase tracking-widest text-sm shadow-[0_0_15px_rgba(200,155,100,0.3)] text-center"
                    >
                        Create a Hero
                    </Link>
                </div>
                <div className="mt-12 text-xs text-gray-600 uppercase tracking-widest">
                    <p>Ultimate Build Planner & Database</p>
                </div>
            </div>
        </div>
    )
}
