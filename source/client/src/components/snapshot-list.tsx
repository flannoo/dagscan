"use client";

import React, { useEffect, useState } from "react"
import Link from "next/link"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table"
import { useQuery } from "@tanstack/react-query";
import { getSnapshots } from "@/lib/services/api-blockexplorer-requests";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle, Loader } from "lucide-react";
import { formatDate } from "@/lib/utils";
import { Button } from "@/components/ui/button";

export function SnapshotList({ metagraphId, metagraphSymbol }: { metagraphId?: string, metagraphSymbol?: string }) {
    const [nextTokens, setNextTokens] = useState<string[]>([]);
    const [currentNext, setCurrentNext] = useState<string | undefined>(undefined);

    // Keep previous data so we can show it while fetching the next page
    const [previousData, setPreviousData] = useState<any>(null);

    // Track the button being clicked ("next" or "previous")
    const [loadingButton, setLoadingButton] = useState<string | null>(null);

    const { data, isError, isFetching } = useQuery({
        queryKey: ['snapshots', metagraphId, currentNext],
        queryFn: async () => {
            const result = await getSnapshots(metagraphId, currentNext);
            setLoadingButton(null);  // Reset the loading button state after fetch
            return result;
        },
        refetchOnWindowFocus: true,
        placeholderData: previousData,
    });

    const handleNextPage = () => {
        if (data?.meta?.next) {
            setLoadingButton("next");
            setPreviousData(data); // Store the current data to show it while fetching the next page
            setNextTokens([...nextTokens, currentNext ?? ""]); // Add the current next token to the history
            setCurrentNext(data.meta.next); // Set the new next token
        }
    };

    const handlePreviousPage = () => {
        if (nextTokens.length > 0) {
            setLoadingButton("previous");
            setPreviousData(data); // Store the current data to show it while fetching the next page
            const prevNextToken = nextTokens[nextTokens.length - 1]; // Get the last token from history
            setNextTokens(nextTokens.slice(0, -1)); // Remove the last token from history
            setCurrentNext(prevNextToken); // Move back to the previous page
        }
    };

    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>{metagraphSymbol ? `${metagraphSymbol} Snapshots` : "Global L0 Snapshots"}</CardTitle>
                </CardHeader>
                <CardContent>
                    {!data && !previousData ? (
                        <SkeletonCard />
                    ) : isError ? (
                        <div className="flex justify-center items-center text-red-500">
                            <AlertCircle className="h-8 w-8 mr-2" />
                            <span>Failed to fetch data</span>
                        </div>
                    ) : (
                        <div>
                            <Table>
                                <TableHeader>
                                    <TableRow>
                                        <TableHead className="px-2 py-1 leading-loose">Ordinal</TableHead>
                                        <TableHead className="px-2 py-1 leading-loose">Timestamp</TableHead>
                                        <TableHead className="px-2 py-1 leading-loose">Blocks</TableHead>
                                        <TableHead className="px-2 py-1 leading-loose">Rewards</TableHead>
                                    </TableRow>
                                </TableHeader>
                                <TableBody>
                                    {data?.data?.map((snapshot) => (
                                        <TableRow key={snapshot.ordinal}>
                                            <TableCell className="px-2 py-1 leading-loose">
                                                {metagraphId ? (
                                                    <Link href={`/metagraphs/${metagraphId}/snapshots/${snapshot.ordinal}`} className="hover:underline" prefetch={false}>
                                                        {snapshot.ordinal}
                                                    </Link>
                                                ) : (
                                                    <Link href={`/snapshots/${snapshot.ordinal}`} className="hover:underline" prefetch={false}>
                                                        {snapshot.ordinal}
                                                    </Link>
                                                )}
                                            </TableCell>
                                            <TableCell className="px-2 py-1 leading-loose">{formatDate(snapshot.timestamp)}</TableCell>
                                            <TableCell className="px-2 py-1 leading-loose">{snapshot.blocks.length}</TableCell>
                                            <TableCell className="px-2 py-1 leading-loose">{snapshot.rewards.length} recipients</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>

                            <div className="flex items-center justify-end space-x-2 py-4">
                                <Button
                                    variant="outline" size="sm" onClick={handlePreviousPage} disabled={nextTokens.length === 0}>
                                    {loadingButton === "previous" && isFetching ? (
                                        <Loader className="animate-spin h-4 w-4 mr-2" />
                                    ) : (
                                        "Previous"
                                    )}
                                </Button>
                                <Button variant="outline" size="sm" onClick={handleNextPage} disabled={!data?.meta?.next}>
                                    {loadingButton === "next" && isFetching ? (
                                        <Loader className="animate-spin h-4 w-4 mr-2" />
                                    ) : (
                                        "Next"
                                    )}
                                </Button>
                            </div>
                        </div>
                    )
                    }
                </CardContent>
            </Card>
        </div>
    )
}
