import { MetagraphNode } from "@/lib/shared/types";
import { ColumnDef } from "@tanstack/react-table";
import Link from "next/link";
import { DataTableWithSearch } from "@/components/ui/data-table-with-search";
import { ArrowUpDown, CheckCircle, XCircle } from "lucide-react";
import { Button } from "@/components/ui/button";

export const columns: ColumnDef<MetagraphNode>[] = [
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
        accessorKey: "metagraphType",
        header: ({ column }) => {
            return (
                <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
                    Type
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

interface NodesMetagraphDetailsProps {
    nodesData: MetagraphNode[];
}

export default function NodesMetagraphDetails({ nodesData }: NodesMetagraphDetailsProps) {
    return (
        <div>
            <DataTableWithSearch columns={columns} data={nodesData ?? []} />
        </div>
    )
}