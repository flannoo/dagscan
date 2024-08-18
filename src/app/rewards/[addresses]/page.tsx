"use client";

import React, { useState } from "react"
import { useQuery } from "@tanstack/react-query";
import { getRewards } from "@/lib/services/api-nebula-requests";
import { columns } from "./columns"
import { DataTable } from "@/components/ui/data-table"
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { Textarea } from "@/components/ui/textarea";
import Link from "next/link";
import { Button } from "@/components/ui/button";

export default function Reward({ params }: { params: { addresses: string } }) {
    const [walletAddresses, setWalletAddresses] = useState(params.addresses);

    const { data: rewards, isLoading, isError } = useQuery({
        queryKey: ['rewards-' + params.addresses],
        queryFn: async () => getRewards(params.addresses),
    });

    return <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
        <h1 className="text-2xl font-bold mb-4">Reward Explorer</h1>
        <div className="mb-4">
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

        {isLoading ? (
            <SkeletonCard />
        ) : isError ? (
            <p className="text-red-500">Error loading rewards data.</p>
        ) : (
            <DataTable columns={columns} data={rewards ?? []} />
        )}
    </div>
}
