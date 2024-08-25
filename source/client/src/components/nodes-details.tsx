import { ValidatorNode } from "@/lib/shared/types";
import { ColumnDef } from "@tanstack/react-table";
import Link from "next/link";
import { DataTable } from "@/components/ui/data-table";
import { ArrowUpDown, CheckCircle, XCircle } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

export const columns: ColumnDef<ValidatorNode>[] = [
    {
        accessorKey: "ipAddress",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    Node
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        enableGlobalFilter: true,
    },
    {
        accessorKey: "nodeStatus",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    Status
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        enableGlobalFilter: true,
    },
    {
        accessorKey: "isInConsensus",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    In Consensus
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        cell: ({ row }) => {
            const isInConsensus: boolean = row.getValue("isInConsensus");
            return isInConsensus ? (
                <CheckCircle className="text-green-500" size={20} />
            ) : (
                <XCircle className="text-red-500" size={20} />
            );
        },
    },
    {
        accessorKey: "serviceProvider",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    Service Provider
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        enableGlobalFilter: true,
    },
    {
        accessorKey: "country",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    Country
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        enableGlobalFilter: true,
    },
    {
        accessorKey: "city",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    City
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        enableGlobalFilter: true,
    },
    {
        accessorKey: "walletAddress",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    Wallet Address
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        cell: ({ row }) => {
            const address: string = row.getValue("walletAddress");
            return <Link href={`/addresses/${address}`} className="hover:underline" prefetch={false}>
                {address.slice(0, 6)}...{address.slice(-6)}
            </Link>
        },
        enableGlobalFilter: true,
    },
    {
        accessorKey: "walletId",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    Wallet Id
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        cell: ({ row }) => {
            const walletId: string = row.getValue("walletId");
            return <span>{walletId.slice(0, 6)}...{walletId.slice(-6)}</span>
        },
        enableGlobalFilter: true,
    }
]

interface NodesDetailsProps {
    nodesData: ValidatorNode[];
}

export default function NodesDetails({ nodesData }: NodesDetailsProps) {
    return (
        <div>
            <DataTable columns={columns} data={nodesData ?? []} />
        </div>
    )
}