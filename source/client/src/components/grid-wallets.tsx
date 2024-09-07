"use client";

import { ColumnDef } from "@tanstack/react-table";
import Link from "next/link";
import { useQuery } from "@tanstack/react-query";
import { DataTableWithSearch } from "@/components/ui/data-table-with-search";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { formatMetagraphAmount } from "@/lib/utils";
import { WalletRichlistInfo } from "@/lib/shared/types";
import { getMetagraphWallets } from "@/lib/services/api-dagscan-request";

export const columns: ColumnDef<WalletRichlistInfo>[] = [
    {
        accessorKey: "rank",
        header: "Rank",
    },
    {
        accessorKey: "address",
        header: "Address",
        cell: ({ row }) => {
            const address: string = row.getValue("address");
            return <Link href={`/addresses/${address}`} className="hover:underline" prefetch={false}>
                {address.slice(0, 6)}...{address.slice(-6)}
            </Link>
        },
    },
    {
        accessorKey: "tag",
        header: "Tag",
    },
    {
        accessorKey: "balance",
        header: "Balance",
        cell: ({ row }) => {
            return formatMetagraphAmount(row.getValue("balance"));
        },
    },
    {
        accessorKey: "usdValue",
        header: "USD Value",
        cell: ({ row }) => {
            const parsedAmount = parseFloat(row.getValue("usdValue"));
            if (isNaN(parsedAmount)) {
                return "";
            }

            const formattedAmount = new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD',
                minimumFractionDigits: 0,
                maximumFractionDigits: 0,
            }).format(parsedAmount);

            return formattedAmount;
        },
    },
    {
        accessorKey: "supplyPercent",
        header: "% Supply",
        cell: ({ row }) => {
            const parsedAmount = parseFloat(row.getValue("supplyPercent"));
            if (isNaN(parsedAmount)) {
                return "";
            }
            const formattedAmount = new Intl.NumberFormat('en-US', {
                minimumFractionDigits: 0,
                maximumFractionDigits: 8,
            }).format(parsedAmount);

            return `${formattedAmount} %`;
        },
    },
    {
        accessorKey: "address",
        header: "Rewards",
        cell: ({ row }) => {
            const address: string = row.getValue("address");
            return <Link href={`/rewards/${address}`} className="hover:underline" prefetch={false}>
                View Rewards
            </Link>
        },
    },
]

export default function GridWallets({ metagraphAddress }: { metagraphAddress?: string }) {
    const { data: wallets, isLoading, isError } = useQuery({
        queryKey: ['metagraphWallets-' + metagraphAddress],
        queryFn: async () => getMetagraphWallets("mainnet", metagraphAddress),
    });

    return (
        <div className="mb-4">
            {isLoading ? (
                <SkeletonCard />
            ) : isError ? (
                <p className="text-red-500">Error loading wallet data.</p>
            ) : (
                <DataTableWithSearch columns={columns} data={wallets ?? []} />
            )}
        </div>
    )
}
