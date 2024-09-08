"use client"

import React from "react"
import Link from "next/link";
import { usePathname } from "next/navigation";

interface MainNavLinksProps {
    setIsOpen?: (isOpen: boolean) => void; // Optional prop to control sheet state
}

export function MainNavLinks({ setIsOpen }: MainNavLinksProps) {
    const pathname = usePathname();

    const handleClick = () => {
        if (setIsOpen) {
            setIsOpen(false);
        }
    };

    return (
        <>
            <Link href="/" passHref>
                <span
                    onClick={handleClick}
                    className={`ml-1 w-max rounded-md px-4 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground ${pathname === "/" || pathname.startsWith("/snapshots/") || pathname.startsWith("/transactions/")
                            ? "bg-accent text-accent-foreground active"
                            : ""
                        }`}
                >
                    Hypergraph
                </span>
            </Link>
            <Link href="/metagraphs" passHref>
                <span
                    onClick={handleClick}
                    className={`w-max rounded-md px-4 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground ${pathname.startsWith("/metagraphs") ? "bg-accent text-accent-foreground active" : ""
                        }`}
                >
                    Metagraphs
                </span>
            </Link>
            <Link href="/rewards" passHref>
                <span
                    onClick={handleClick}
                    className={`w-max rounded-md px-4 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground ${pathname.startsWith("/rewards") ? "bg-accent text-accent-foreground" : ""
                        }`}
                >
                    Reward Explorer
                </span>
            </Link>
        </>
    )
}
