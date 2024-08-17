"use client";

import React, { useEffect, useState } from "react"
import Link from "next/link"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table"
import { useQuery } from "@tanstack/react-query";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle } from "lucide-react";
import { getMetagraphs } from "@/lib/services/api-nebula-requests";

export function MetagraphList() {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });

    return (
        <div>
            <Card>
                <CardHeader>Metagraphs</CardHeader>
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
                                    <TableHead>Metagraph</TableHead>
                                    <TableHead>Id</TableHead>
                                    <TableHead>Website</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {data?.map((metagraph) => (
                                    <TableRow key={metagraph.MetagraphName}>
                                        <TableCell>
                                            <Link href={`/metagraphs/${metagraph.MetagraphAddress}`} className="hover:underline" prefetch={false}>
                                                {metagraph.MetagraphName}
                                            </Link>
                                        </TableCell>
                                        <TableCell>
                                            <Link href={`/metagraphs/${metagraph.MetagraphAddress}`} className="hover:underline" prefetch={false}>
                                                {metagraph.MetagraphAddress.slice(0, 6)}...{metagraph.MetagraphAddress.slice(-6)}
                                            </Link>
                                        </TableCell>
                                        <TableCell>
                                            <a href={`${metagraph.Website}`} target="_blank" rel="noopener noreferrer" className="hover:underline">
                                                {metagraph.Website}
                                            </a>
                                        </TableCell>
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
