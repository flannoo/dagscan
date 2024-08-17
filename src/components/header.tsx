"use client"

import Link from "next/link";
import { MainNav } from "@/components/main-nav";

export function Header() {
    return (
        <header className="bg-background border-b px-4 md:px-6 py-3">
            <div className="flex h-16 items-center justify-between w-full">
                {/* Logo or branding */}
                <Link href="/" className="text-xl font-bold hidden lg:block">
                    DagScan
                </Link>

                {/* Main navigation with Search and Theme Toggle */}
                <MainNav className="ml-auto" />
            </div>
        </header>
    );
}