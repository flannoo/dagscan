"use client";

import React from "react";
import { getSnapshotDetail } from "@/lib/services/api-blockexplorer-requests";
import { useQuery } from "@tanstack/react-query";
import { AlertCircle } from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { TableRow, TableBody, TableCell, Table } from "@/components/ui/table";
import { formatDate } from "@/lib/utils";

interface SnapshotDetailProps {
    snapshotId: string;
    metagraphId?: string;
    metagraphSymbol?: string;
}

export default function SnapshotDetail({ snapshotId, metagraphId, metagraphSymbol }: SnapshotDetailProps) {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['snapshotdetail-' + snapshotId + '-' + metagraphId],
        queryFn: async () => getSnapshotDetail(snapshotId, metagraphId),
    });

    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>
                        {metagraphSymbol} Snapshot Details
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
                                        <span>Ordinal</span>
                                    </TableCell>
                                    <TableCell>
                                        <span>{data?.ordinal}</span>
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        Hash
                                    </TableCell>
                                    <TableCell>
                                        {data?.hash}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        Last Snapshot Hash
                                    </TableCell>
                                    <TableCell>
                                        {data?.lastSnapshotHash}
                                    </TableCell>
                                </TableRow>
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
                                        Blocks
                                    </TableCell>
                                    <TableCell>
                                        {data?.blocks.length}
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