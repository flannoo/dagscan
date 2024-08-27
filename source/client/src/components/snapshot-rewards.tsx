"use client";

import React from "react";
import { getSnapshotDetailRewards, Reward } from "@/lib/services/api-blockexplorer-requests";
import { useQuery } from "@tanstack/react-query";
import { AlertCircle, ArrowUpDown } from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import Link from "next/link";
import { ColumnDef } from "@tanstack/react-table";
import { DataTableWithSearch } from "@/components/ui/data-table-with-search"
import { Button } from "@/components/ui/button";
import { formatMetagraphAmount } from "@/lib/utils";

interface SnapshotRewardsProps {
    snapshotId: string;
    metagraphId?: string;
    metagraphSymbol?: string;
}

export default function SnapshotRewards({ snapshotId, metagraphId, metagraphSymbol }: SnapshotRewardsProps) {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['snapshotrewards-' + snapshotId + '-' + metagraphId],
        queryFn: async () => getSnapshotDetailRewards(snapshotId, metagraphId),
    });

    const columns: ColumnDef<Reward>[] = [
        {
            accessorKey: "destination",
            header: ({ column }) => {
                return (
                    <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                        Destination
                        <ArrowUpDown className="ml-2 h-4 w-4" />
                    </Button>
                )
            },
            cell: ({ row }) => {
                const address: string = row.getValue("destination");
                return <Link href={`/addresses/${address}`} className="hover:underline" prefetch={false}>
                    {address}
                </Link>
            },
        },
        {
            accessorKey: "amount",
            header: ({ column }) => {
                return (
                    <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                        Amount
                        <ArrowUpDown className="ml-2 h-4 w-4" />
                    </Button>
                )
            },
            cell: ({ row }) => {
                const amount = formatMetagraphAmount(row.getValue("amount"));
                return `${amount} ${metagraphSymbol ?? 'DAG'}`;
            },
        },
    ]

    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>
                        {metagraphSymbol} Rewards
                    </CardTitle>
                </CardHeader>
                <CardContent>
                    {isLoading ? (
                        <SkeletonCard />
                    ) : isError ? (
                        <div className="flex justify-center items-center text-red-500">
                            <AlertCircle className="h-8 w-8 mr-2" />
                            <span>Failed to fetch data</span>
                        </div>
                    ) : (
                        <DataTableWithSearch columns={columns} data={data ?? []} />
                    )
                    }
                </CardContent>
            </Card>
        </div>
    )
}