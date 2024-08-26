"use client"

import { Button } from "@/components/ui/button";
import { getRewards, Reward } from "@/lib/services/api-nebula-requests"
import { useQuery } from "@tanstack/react-query";
import { ColumnDef } from "@tanstack/react-table"
import { ArrowUpDown } from "lucide-react"
import Link from "next/link";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { DataTableWithSearch } from "@/components/ui/data-table-with-search"
import { formatDate, formatMetagraphAmount } from "@/lib/utils";

export const columns: ColumnDef<Reward>[] = [
    {
        accessorKey: "Timestamp",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    Transaction Date
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        cell: ({ row }) => {
            return formatDate(row.getValue("Timestamp"))
        }
    },
    {
        accessorKey: "Currency",
        header: "Currency",
        cell: ({ row }) => {
            const currency = row.getValue("Currency");
            return <span className="flex items-center">
                <img
                    src={`/icons/${currency}-icon.svg`} className="w-5 h-5 mr-2"
                    onError={(e) => (e.currentTarget.style.display = "none")} />
                {row.getValue("Currency")}
            </span>;
        },
        enableColumnFilter: true,
        filterFn: 'includesString',
    },
    {
        accessorKey: "Amount",
        header: "Amount",
        cell: ({ row }) => {
            return formatMetagraphAmount(row.getValue("Amount"));
        },
    },
    {
        accessorKey: "RewardType",
        header: "Type",
    },
    {
        accessorKey: "Address",
        header: "Address",
        cell: ({ row }) => {
            const address: string = row.getValue("Address");
            return <Link href={`/addresses/${address}`} className="hover:underline" prefetch={false}>
                {address}
            </Link>
        },
    },
    {
        accessorKey: "TransactionHash",
        header: "TransactionRef",
        cell: ({ row }) => {
            const transactionRef: string = row.getValue("TransactionHash");
            if (!transactionRef) {
                return null;
            }

            return <Link href={`/transactions/${transactionRef}`} className="hover:underline" prefetch={false}>
                {transactionRef.slice(0, 6)}...{transactionRef.slice(-6)}
            </Link>
        },
    },
]

export default function GridRewards({ addresses }: { addresses: string }) {
    const { data: rewards, isLoading, isError } = useQuery({
        queryKey: ['rewards-' + addresses],
        queryFn: async () => getRewards(addresses),
    });
    return (
        <div className="mb-4">
            {isLoading ? (
                <SkeletonCard />
            ) : isError ? (
                <p className="text-red-500">Error loading reward data.</p>
            ) : (
                <DataTableWithSearch columns={columns} data={rewards ?? []} />
            )}
        </div>
    )
}
