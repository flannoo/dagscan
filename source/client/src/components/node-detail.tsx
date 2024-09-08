import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableRow, TableHead, TableHeader } from "@/components/ui/table";
import { getValidatorNodes } from "@/lib/services/api-dagscan-request";
import { useQuery } from "@tanstack/react-query";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle, CheckCircle, XCircle } from "lucide-react";
import React from "react";

interface NodeDetailProps {
    walletAddress: string;
}

export default function NodeDetail({ walletAddress }: NodeDetailProps) {
    const { data, isError, isFetching } = useQuery({
        queryKey: ['node', walletAddress],
        queryFn: async () => {
            const result = await getValidatorNodes(walletAddress);
            return result;
        },
        refetchOnWindowFocus: true
    });

    return (
        <div>
            {isFetching ? (
                <SkeletonCard />
            ) : isError ? (
                <div className="flex justify-center items-center text-red-500">
                    <AlertCircle className="h-8 w-8 mr-2" />
                    <span>Failed to fetch data</span>
                </div>
            ) : (
                <Card className="mb-4">
                    <CardHeader>
                        <CardTitle>
                            Node Overview
                        </CardTitle>
                    </CardHeader>
                    <CardContent>
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>IP Address</TableHead>
                                    <TableHead>Node Type</TableHead>
                                    <TableHead>Metagraph Address</TableHead>
                                    <TableHead>Node Status</TableHead>
                                    <TableHead>In Consensus</TableHead>
                                    <TableHead>Country</TableHead>
                                    <TableHead>City</TableHead>
                                    <TableHead>Service Provider</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {data?.hypergraphValidatorNodeDto && (
                                    <TableRow>
                                        <TableCell>{data?.hypergraphValidatorNodeDto.ipAddress || ''}</TableCell>
                                        <TableCell>{'Hypergraph'}</TableCell>
                                        <TableCell></TableCell>
                                        <TableCell>{data?.hypergraphValidatorNodeDto.nodeStatus || ''}</TableCell>
                                        <TableCell>{
                                            data?.hypergraphValidatorNodeDto.isInConsensus ? (
                                                <CheckCircle className="text-green-500" size={20} />
                                            ) : (
                                                <XCircle className="text-red-500" size={20} />
                                            )
                                        }
                                        </TableCell>
                                        <TableCell>{data?.hypergraphValidatorNodeDto.country || ''}</TableCell>
                                        <TableCell>{data?.hypergraphValidatorNodeDto.city || ''}</TableCell>
                                        <TableCell>{data?.hypergraphValidatorNodeDto.serviceProvider || ''}</TableCell>
                                    </TableRow>
                                )}
                                {data?.metagraphNodes && data.metagraphNodes.length > 0 && (
                                    data?.metagraphNodes.map((node, index) => (
                                        <TableRow key={index}>
                                            <TableCell>{node.ipAddress || ''}</TableCell>
                                            <TableCell>{node.metagraphType || ''}</TableCell>
                                            <TableCell>{node.metagraphAddress || ''}</TableCell>
                                            <TableCell>{node.nodeStatus || ''}</TableCell>
                                            <TableCell>N/A</TableCell>
                                            <TableCell>{node.country || ''}</TableCell>
                                            <TableCell>{node.city || ''}</TableCell>
                                            <TableCell>{node.serviceProvider || ''}</TableCell>
                                        </TableRow>
                                    ))
                                )}
                            </TableBody>
                        </Table>
                    </CardContent>
                </Card>
            )}
        </div>
    );
}
