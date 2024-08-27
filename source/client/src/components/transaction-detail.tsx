"use client";

import { getTransactionDetail } from "@/lib/services/api-blockexplorer-requests";
import { useQuery } from "@tanstack/react-query";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle } from "lucide-react";
import { Table, TableBody, TableCell, TableRow } from "@/components/ui/table";
import { formatDate, formatMetagraphAmount } from "@/lib/utils";
import Link from "next/link";

interface TransactionDetailProps {
    transactionId: string;
    metagraphId?: string;
    metagraphSymbol?: string;
}

export default function TransactionDetail({ transactionId, metagraphId, metagraphSymbol }: TransactionDetailProps) {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['transactiondetail-' + transactionId + '-' + metagraphId],
        queryFn: async () => getTransactionDetail(transactionId, metagraphId),
    });

    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>
                        Transaction Details
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
                        <Table>
                            <TableBody>
                                <TableRow>
                                    <TableCell>
                                        Timestamp
                                    </TableCell>
                                    <TableCell>
                                        {formatDate(data?.timestamp)}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        Amount
                                    </TableCell>
                                    <TableCell>
                                        {formatMetagraphAmount(data?.amount)} {metagraphSymbol ? metagraphSymbol : "DAG"}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        Fee
                                    </TableCell>
                                    <TableCell>
                                        {formatMetagraphAmount(data?.fee)} {metagraphSymbol ? metagraphSymbol : "DAG"}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        From
                                    </TableCell>
                                    <TableCell>
                                        <Link href={`/addresses/${data?.source}`} className="hover:underline" prefetch={false}>
                                            {data?.source}
                                        </Link>
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        To
                                    </TableCell>
                                    <TableCell>
                                        <Link href={`/addresses/${data?.destination}`} className="hover:underline" prefetch={false}>
                                            {data?.destination}
                                        </Link>
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        <span>Snapshot Ordinal</span>
                                    </TableCell>
                                    <TableCell>
                                        {(!metagraphId || metagraphId === "DAG") ? (
                                            <Link href={`/snapshots/${data?.snapshotOrdinal}`} className="hover:underline" prefetch={false}>
                                                {data?.snapshotOrdinal}
                                            </Link>
                                        ) : (
                                            <Link href={`/metagraphs/${metagraphId}/snapshots/${data?.snapshotOrdinal}`} className="hover:underline" prefetch={false}>
                                                {data?.snapshotOrdinal}
                                            </Link>
                                        )}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        Transaction Hash
                                    </TableCell>
                                    <TableCell>
                                        {data?.hash}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        Block
                                    </TableCell>
                                    <TableCell>
                                        {data?.blockHash}
                                    </TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    )
                    }
                </CardContent>
            </Card>
        </div>
    )
}