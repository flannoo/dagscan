"use client";

import React, { useEffect, useState } from "react"
import Link from "next/link"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table"
import { useQuery } from "@tanstack/react-query";
import { getLatestSnapshots } from "@/lib/services/api-blockexplorer-requests";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle } from "lucide-react";
import { formatDate } from "@/lib/utils";

export function LatestSnapshots() {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['latestSnapshots'],
        queryFn: async () => getLatestSnapshots(),
        staleTime: 10 * 1000,
        refetchInterval: 10 * 1000,
        refetchOnWindowFocus: true,
    });

    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>Latest Snapshots</CardTitle>
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
                                    <TableHead>Ordinal</TableHead>
                                    <TableHead>Timestamp</TableHead>
                                    <TableHead>Blocks</TableHead>
                                    <TableHead>Protocol Rewards</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {data?.map((snapshot) => (
                                    <TableRow key={snapshot.ordinal}>
                                        <TableCell>
                                            <Link href={`/snapshots/${snapshot.ordinal}`} className="hover:underline" prefetch={false}>
                                                {snapshot.ordinal}
                                            </Link>
                                        </TableCell>
                                        <TableCell>{formatDate(snapshot.timestamp)}</TableCell>
                                        <TableCell>{snapshot.blocks.length}</TableCell>
                                        <TableCell>{snapshot.rewards.length} recipients</TableCell>
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
