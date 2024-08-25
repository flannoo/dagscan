import { ValidatorNode } from "@/lib/shared/types";
import { ColumnDef } from "@tanstack/react-table";
import Link from "next/link";
import { DataTable } from "@/components/ui/data-table";
import { CheckCircle, XCircle } from "lucide-react";

export const columns: ColumnDef<ValidatorNode>[] = [
    {
        accessorKey: "ipAddress",
        header: "Node",
    },
    {
        accessorKey: "nodeStatus",
        header: "Status",
    },
    {
        accessorKey: "isInConsensus",
        header: "In Consensus",
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
        header: "Service Provider",
    },
    {
        accessorKey: "country",
        header: "Country",
    },
    {
        accessorKey: "city",
        header: "City",
    },
    {
        accessorKey: "walletAddress",
        header: "Wallet Address",
        cell: ({ row }) => {
            const address: string = row.getValue("walletAddress");
            return <Link href={`/addresses/${address}`} className="hover:underline" prefetch={false}>
                {address.slice(0, 6)}...{address.slice(-6)}
            </Link>
        },
    },
    {
        accessorKey: "walletId",
        header: "Wallet ID",
        cell: ({ row }) => {
            const walletId: string = row.getValue("walletId");
            return <span>{walletId.slice(0, 6)}...{walletId.slice(-6)}</span>
        },
    }
]

interface NodesDetailsProps {
    nodesData: ValidatorNode[];
}

export default function NodesDetails({ nodesData }: NodesDetailsProps) {
    return <DataTable columns={columns} data={nodesData ?? []} />
}