"use client";

import React, { useState } from "react";
import { NavigationMenu, NavigationMenuList } from "@/components/ui/navigation-menu";
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet";
import { ThemeToggle } from "@/components/ui/theme-toggle";
import { Search } from "@/components/search";
import { MainNavLinks } from "./main-nav-links";

export function MainNav({ className, ...props }: React.HTMLAttributes<HTMLElement>) {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div className={`flex items-center justify-between w-full ${className}`} {...props}>
            <div className="flex items-center space-x-4">
                {/* Hamburger Menu for Mobile View */}
                <div className="lg:hidden">
                    <Sheet open={isOpen} onOpenChange={setIsOpen}>
                        <SheetTrigger asChild>
                            <button className="p-2 border rounded-md" aria-label="Toggle navigation">
                                {/* Hamburger Icon */}
                                <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16m-7 6h7" />
                                </svg>
                            </button>
                        </SheetTrigger>
                        <SheetContent side="left" className="p-4">
                            <div className="mb-4 text-xl font-bold">Dag Scan</div>
                            <NavigationMenu>
                                <NavigationMenuList className="flex flex-col items-start space-y-4">
                                    <MainNavLinks setIsOpen={setIsOpen} />
                                </NavigationMenuList>
                            </NavigationMenu>
                        </SheetContent>
                    </Sheet>
                </div>

                {/* Normal Navigation Menu for Desktop */}
                <div className="hidden lg:flex items-center space-x-4">
                    <NavigationMenu>
                        <NavigationMenuList className="flex space-x-4">
                            <MainNavLinks setIsOpen={setIsOpen} />
                        </NavigationMenuList>
                    </NavigationMenu>
                </div>
            </div>

            {/* Right Side: Search and Theme Toggle */}
            <div className="flex items-center space-x-4 ml-auto w-auto">
                {/* Set a responsive width for the Search Box */}
                <div className="flex-grow lg:flex-grow-0 md:w-96">
                    <Search />
                </div>
                <ThemeToggle />
            </div>
        </div>
    );
}
