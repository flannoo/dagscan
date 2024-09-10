"use client";

import React from "react"
import Link from "next/link"
import { Card, CardContent } from "@/components/ui/card"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table"
import { Metagraph } from "@/lib/shared/types";

interface MetagraphListProps {
    metagraphs: Metagraph[];
}

export function MetagraphList({ metagraphs }: MetagraphListProps) {
    return (
        <div>
            <Card>
                <CardContent>
                    <Table className="mt-4">
                        <TableHeader>
                            <TableRow>
                                <TableHead>Metagraph</TableHead>
                                <TableHead>Id</TableHead>
                                <TableHead>Website</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {metagraphs?.map((metagraph) => (
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
                </CardContent>
            </Card>
        </div>
    )
}
