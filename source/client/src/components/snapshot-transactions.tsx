"use client";

import React from "react";
import { getSnapshotDetailTransactions, Transaction } from "@/lib/services/api-blockexplorer-requests";
import { useQuery } from "@tanstack/react-query";
import { AlertCircle, ArrowUpDown } from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import Link from "next/link";
import { ColumnDef } from "@tanstack/react-table";
import { DataTable } from "@/components/ui/data-table"
import { formatDate, formatMetagraphAmount } from "@/lib/utils";

interface SnapshotTransactionsProps {
    snapshotId: string;
    metagraphId?: string;
    metagraphSymbol?: string;
}

export default function SnapshotTransactions({ snapshotId, metagraphId, metagraphSymbol }: SnapshotTransactionsProps) {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['snapshottrx-' + snapshotId + '-' + metagraphId],
        queryFn: async () => getSnapshotDetailTransactions(snapshotId, metagraphId),
    });

    const columns: ColumnDef<Transaction>[] = [
        {
            accessorKey: "timestamp",
            header: "Timestamp",
            cell: ({ row }) => {
                return formatDate(row.getValue("timestamp"))
            }
        },
        {
            accessorKey: "amount",
            header: "Amount",
            cell: ({ row }) => {
                const amount = formatMetagraphAmount(row.getValue("amount"));
                return `${amount} ${metagraphSymbol ?? 'DAG'}`;
            },
        },
        {
            accessorKey: "fee",
            header: "Fee",
            cell: ({ row }) => {
                const amount = formatMetagraphAmount(row.getValue("fee"));
                return `${amount} ${metagraphSymbol ?? 'DAG'}`;
            },
        },
        {
            accessorKey: "source",
            header: "From",
            cell: ({ row }) => {
                const address: string = row.getValue("source");
                return <Link href={`/addresses/${address}`} className="hover:underline" prefetch={false}>
                    {address.slice(0, 6)}...{address.slice(-6)}
                </Link>
            },
        },
        {
            accessorKey: "destination",
            header: "To",
            cell: ({ row }) => {
                const address: string = row.getValue("destination");
                return <Link href={`/addresses/${address}`} className="hover:underline" prefetch={false}>
                    {address.slice(0, 6)}...{address.slice(-6)}
                </Link>
            },
        },
        {
            accessorKey: "hash",
            header: "Txn Hash",
            cell: ({ row }) => {
                const transactionRef: string = row.getValue("hash");
                if (!transactionRef) {
                    return null;
                }

                if (metagraphId) {
                    return <Link href={`/metagraphs/${metagraphId}/transactions/${transactionRef}`} className="hover:underline" prefetch={false}>
                        {transactionRef.slice(0, 6)}...{transactionRef.slice(-6)}
                    </Link>
                }

                return <Link href={`/transactions/${transactionRef}`} className="hover:underline" prefetch={false}>
                    {transactionRef.slice(0, 6)}...{transactionRef.slice(-6)}
                </Link>
            },
        },
    ]

    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>
                        {metagraphSymbol} Transactions
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
                        <DataTable columns={columns} data={data ?? []} />
                    )
                    }
                </CardContent>
            </Card>
        </div>
    )
}