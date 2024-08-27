"use client";

import React, { useEffect, useState } from "react"
import Link from "next/link"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table"
import { useQuery } from "@tanstack/react-query";
import { getLatestTransactions } from "@/lib/services/api-blockexplorer-requests";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle } from "lucide-react";
import { formatDagAmount, formatDate } from "@/lib/utils";

export function LatestTransactions({ metagraphId, metagraphSymbol }: { metagraphId?: string, metagraphSymbol?: string }) {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['latestTransactions-' + metagraphId],
        queryFn: async () => getLatestTransactions(metagraphId),
        staleTime: 10 * 1000,
        refetchInterval: 10 * 1000,
        refetchOnWindowFocus: true,
    });

    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>{metagraphSymbol ? `${metagraphSymbol} Transactions` : "DAG Transactions"}</CardTitle>
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
                            <TableHeader>
                                <TableRow>
                                    <TableHead>TXN Hash</TableHead>
                                    <TableHead>Timestamp</TableHead>
                                    <TableHead>Amount</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {data?.map((transaction) => (
                                    <TableRow key={transaction.hash}>
                                        <TableCell>
                                            {metagraphId ? (
                                                <Link href={`/metagraphs/${metagraphId}/transactions/${transaction.hash}`} className="hover:underline" prefetch={false}>
                                                    {transaction.hash.slice(0, 6)}...{transaction.hash.slice(-6)}
                                                </Link>
                                            ) : (
                                                <Link href={`/transactions/${transaction.hash}`} className="hover:underline" prefetch={false}>
                                                    {transaction.hash.slice(0, 6)}...{transaction.hash.slice(-6)}
                                                </Link>
                                            )}
                                        </TableCell>
                                        <TableCell>{formatDate(transaction.timestamp)}</TableCell>
                                        <TableCell>{formatDagAmount(transaction.amount)}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    )
                    }
                </CardContent>
            </Card>
        </div>
    )
}
