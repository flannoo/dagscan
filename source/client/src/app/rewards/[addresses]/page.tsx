"use client";

import React, { useState } from "react"
import { Textarea } from "@/components/ui/textarea";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import GridRewards from "@/components/grid-rewards";

export default function RewardPage({ params }: { params: { addresses: string } }) {
    const [walletAddresses, setWalletAddresses] = useState(params.addresses);

    return <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
        <h1 className="text-2xl font-bold mb-4">Reward Explorer</h1>
        <div className="bg-yellow-100 border-l-4 border-yellow-500 text-yellow-700 p-4 mb-6" role="alert">
                <p className="font-bold">Maintenance Notice</p>
                <p>
                    The Reward Explorer is currently not available anymore and is scheduled for some rework.<br />
                    Please use the alternative explorer at&nbsp;
                    <a
                        href="https://constellation.nebula-tech.io/reward-explorer"
                        target="_blank"
                        rel="noopener noreferrer"
                        className="underline text-blue-600"
                    >
                        https://constellation.nebula-tech.io/reward-explorer
                    </a>
                    &nbsp;which is still available.
                </p>
            </div>
    </div>
}
