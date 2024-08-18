"use client"

import React from "react"
import Link from "next/link";
import { navigationMenuTriggerStyle } from "@/components/ui/navigation-menu";
import {
    NavigationMenuLink,
} from "@/components/ui/navigation-menu";

export function MainNavLinks() {
    return (
        <>
            <Link href="/metagraphs" legacyBehavior passHref>
                <NavigationMenuLink className={navigationMenuTriggerStyle()}>
                    Metagraphs
                </NavigationMenuLink>
            </Link>
            <Link href="/rewards" legacyBehavior passHref>
                <NavigationMenuLink className={navigationMenuTriggerStyle()}>
                    Reward Explorer
                </NavigationMenuLink>
            </Link>
            <Link href="/node-explorer" legacyBehavior passHref>
                <NavigationMenuLink className={navigationMenuTriggerStyle()}>
                    Node Explorer
                </NavigationMenuLink>
            </Link>
        </>
    )
}
