"use client";

import { getMetagraphWallets, Wallet } from "@/lib/services/api-nebula-requests";
import { ColumnDef } from "@tanstack/react-table";
import Link from "next/link";
import { useQuery } from "@tanstack/react-query";
import { DataTableWithSearch } from "@/components/ui/data-table-with-search";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { formatAmount } from "@/lib/utils";

export const columns: ColumnDef<Wallet>[] = [
    {
        accessorKey: "Rank",
        header: "Rank",
    },
    {
        accessorKey: "Address",
        header: "Address",
        cell: ({ row }) => {
            const address: string = row.getValue("Address");
            return <Link href={`/addresses/${address}`} className="hover:underline" prefetch={false}>
                {address.slice(0, 6)}...{address.slice(-6)}
            </Link>
        },
    },
    {
        accessorKey: "Tag",
        header: "Tag",
    },
    {
        accessorKey: "Balance",
        header: "Balance",
        cell: ({ row }) => {
            return formatAmount(row.getValue("Balance"));
        },
    },
    {
        accessorKey: "BalanceUSD",
        header: "USD Value",
        cell: ({ row }) => {
            const parsedAmount = parseFloat(row.getValue("BalanceUSD"));
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
        accessorKey: "SupplyPercentage",
        header: "% Supply",
        cell: ({ row }) => {
            const parsedAmount = parseFloat(row.getValue("SupplyPercentage"));
            if (isNaN(parsedAmount)) {
                return "";
            }
            const formattedAmount = new Intl.NumberFormat('en-US', {
                minimumFractionDigits: 0,
                maximumFractionDigits: 8,
            }).format(parsedAmount)

            return `${formattedAmount}%`;
        },
    },
    {
        accessorKey: "Address",
        header: "Rewards",
        cell: ({ row }) => {
            const address: string = row.getValue("Address");
            return <Link href={`/rewards/${address}`} className="hover:underline" prefetch={false}>
                View Rewards
            </Link>
        },
    },
]

export default function GridWallets({ metagraphSymbol }: { metagraphSymbol: string }) {
    const { data: wallets, isLoading, isError } = useQuery({
        queryKey: ['metagraphWallets-' + metagraphSymbol],
        queryFn: async () => getMetagraphWallets(metagraphSymbol),
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
