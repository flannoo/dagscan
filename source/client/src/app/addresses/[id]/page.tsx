"use client";

import GridRewards from "@/components/grid-rewards";
import { TransactionListAddress } from "@/components/transaction-list-wallet";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { Table, TableBody, TableCell, TableRow } from "@/components/ui/table";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { getBalance, getMetagraphs } from "@/lib/services/api-dagscan-request";
import { formatDagAmount, formatMetagraphAmount } from "@/lib/utils";
import { useQuery } from "@tanstack/react-query";
import { AlertCircle } from "lucide-react";

export default function AddressPage({ params }: { params: { id: string } }) {
    const id = Array.isArray(params.id) ? params.id[0] : params.id;

    const { data: metagraphs } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });

    const { data: walletbalance, isLoading, isError } = useQuery({
        queryKey: ['walletbalance' + id],
        queryFn: async () => getBalance(id),
        refetchOnWindowFocus: true,
    });

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <Card className="mb-4">
                <CardHeader>
                    <CardTitle>
                        Address Overview
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
                                        <span>Address</span>
                                    </TableCell>
                                    <TableCell>
                                        <span>{id}</span>
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        Balance
                                    </TableCell>
                                    <TableCell>
                                        {formatDagAmount(walletbalance?.balance)}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell>
                                        Metagraph Balances
                                    </TableCell>
                                    <TableCell>
                                        {walletbalance?.metagraphBalances.map((metagraph) => (
                                            <div key={metagraph.metagraphAddress}>
                                                {formatMetagraphAmount(metagraph.balance)} {metagraph.tokenSymbol}
                                            </div>
                                        ))}
                                    </TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    )
                    }
                </CardContent>
            </Card>

            <Tabs defaultValue="transactions" className="w-full">
                <TabsList className="w-full justify-start">
                    <TabsTrigger value="transactions">Transactions</TabsTrigger>
                    <TabsTrigger value="rewards">Rewards</TabsTrigger>
                    <TabsTrigger value="node">Node Details</TabsTrigger>
                </TabsList>
                <TabsContent value="transactions">
                    {metagraphs ? (
                        <>
                            <div className="mb-4">
                                <TransactionListAddress walletAddress={id} />
                            </div>
                            {metagraphs
                                .filter(metagraph => metagraph.metagraphAddress && metagraph.metagraphAddress.trim() !== "")
                                .map((metagraph) => (
                                    <div key={metagraph.metagraphAddress} className="mb-4">
                                        <TransactionListAddress
                                            walletAddress={id}
                                            metagraphId={metagraph.metagraphAddress}
                                            metagraphSymbol={metagraph.symbol}
                                        />
                                    </div>
                                ))}
                        </>
                    ) :
                        (
                            <SkeletonCard />
                        )
                    }
                </TabsContent>
                <TabsContent value="rewards">
                    <GridRewards addresses={id} />
                </TabsContent>
            </Tabs>
        </div>
    )
}
