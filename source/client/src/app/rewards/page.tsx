"use client";

import React, { useState } from "react";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";

export default function RewardsPage() {
    const [walletAddresses, setWalletAddresses] = useState("");

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">Reward Explorer</h1>
            <div className="mb-4">
                <Textarea
                    placeholder="Enter comma-separated wallet addresses"
                    value={walletAddresses}
                    onChange={(e) => setWalletAddresses(e.target.value)}
                    rows={3}
                    className="w-full"
                />
            </div>
            <Link href={`/rewards/${walletAddresses}`} passHref>
                <Button disabled={!walletAddresses}>
                    Search Rewards
                </Button>
            </Link>
        </div>
    );
}
