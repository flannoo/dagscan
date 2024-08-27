"use client";

import React from "react"
import Link from "next/link"
import { Card, CardContent } from "@/components/ui/card"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table"
import { useQuery } from "@tanstack/react-query";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle } from "lucide-react";
import { getMetagraphs } from "@/lib/services/api-dagscan-request";

export function MetagraphList() {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });

    // we only show metagraphs that have a metagraphAddress
    const filteredData = data?.filter(metagraph => metagraph.metagraphAddress);

    return (
        <div>
            <Card>
                <CardContent>
                    {isLoading ? (
                        <SkeletonCard />
                    ) : isError ? (
                        <div className="flex justify-center items-center text-red-500">
                            <AlertCircle className="h-8 w-8 mr-2" />
                            <span>Failed to fetch data</span>
                        </div>
                    ) : (
                        <Table className="mt-4">
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Metagraph</TableHead>
                                    <TableHead>Id</TableHead>
                                    <TableHead>Website</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {filteredData?.map((metagraph) => (
                                    <TableRow key={metagraph.name}>
                                        <TableCell>
                                            <Link href={`/metagraphs/${metagraph.metagraphAddress}`} className="hover:underline" prefetch={false}>
                                                {metagraph.name}
                                            </Link>
                                        </TableCell>
                                        <TableCell>
                                            <Link href={`/metagraphs/${metagraph.metagraphAddress}`} className="hover:underline" prefetch={false}>
                                                {metagraph.metagraphAddress}
                                            </Link>
                                        </TableCell>
                                        <TableCell>
                                            <a href={`${metagraph.website}`} target="_blank" rel="noopener noreferrer" className="hover:underline">
                                                {metagraph.website}
                                            </a>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    )}
                </CardContent>
            </Card>
        </div>
    )
}
