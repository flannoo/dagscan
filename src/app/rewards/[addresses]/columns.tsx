"use client"

import { Button } from "@/components/ui/button";
import { Reward } from "@/lib/services/api-nebula-requests"
import { ColumnDef } from "@tanstack/react-table"
import { ArrowUpDown } from "lucide-react"
import Link from "next/link";

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
            const timestamp = new Date(row.getValue("Timestamp"));

            return new Date(timestamp).toLocaleString()
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
            const parsedAmount = parseFloat(row.getValue("Amount"));
            const formattedAmount = new Intl.NumberFormat('en-US', {
                minimumFractionDigits: 0,
                maximumFractionDigits: 8,
            }).format(parsedAmount)

            return formattedAmount;
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
            return <Link href={`/transactions/${transactionRef}`} className="hover:underline" prefetch={false}>
                {transactionRef.slice(0, 6)}...{transactionRef.slice(-6)}
            </Link>
        },
    },
]
