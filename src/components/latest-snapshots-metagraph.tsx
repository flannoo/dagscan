"use client";

import React, { useEffect, useState } from "react"
import Link from "next/link"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table"
import { useQuery } from "@tanstack/react-query";
import { Snapshot, getLatestSnapshotsMetagraph } from "@/lib/services/blockexplorer-requests";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle } from "lucide-react";

export function LatestSnapshotsMetagraph({ metagraphId }: { metagraphId: string }) {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['latestSnapshots-' + metagraphId],
        queryFn: async () => getLatestSnapshotsMetagraph(metagraphId),
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
                                    <TableHead>Rewards</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {data?.map((snapshot) => (
                                    <TableRow key={snapshot.ordinal}>
                                        <TableCell>
                                            <Link href={`/snapshots/${metagraphId}/${snapshot.ordinal}`} className="hover:underline" prefetch={false}>
                                                {snapshot.ordinal}
                                            </Link>
                                        </TableCell>
                                        <TableCell>{new Date(snapshot.timestamp).toLocaleString()}</TableCell>
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
