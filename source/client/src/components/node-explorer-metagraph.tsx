"use client";

import React, { useEffect, useMemo, useState } from "react"
import { AlertCircle, Loader } from "lucide-react"
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import { useQuery } from "@tanstack/react-query";
import { getMetagraphValidatorNodes } from "@/lib/services/api-dagscan-request";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import NodesMetagraphDetails from "@/components/nodes-metagraph-details";
import NodesMetagraphMap from "@/components/nodes-metagraph-map";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";

interface NodeExplorerMetagraphProps {
    metagraphAddress: string
}

export default function NodeExplorerMetagraph({ metagraphAddress }: NodeExplorerMetagraphProps) {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['metagraphnodes', metagraphAddress],
        queryFn: async () => getMetagraphValidatorNodes("mainnet", metagraphAddress),
        refetchOnWindowFocus: true,
    });

    const [selectedType, setSelectedType] = useState<string | null>(null);

    const distinctTypes = useMemo(() => {
        if (!data) return [];
        const types = data.map(node => node.metagraphType);
        const uniqueTypes = Array.from(new Set(types));
        return uniqueTypes.sort((a, b) => a.localeCompare(b));
    }, [data]);

    const filteredData = useMemo(() => {
        if (!data) return [];
        return data.filter(node =>
            node.nodeStatus !== "Offline" &&
            (!selectedType || node.metagraphType === selectedType)
        );
    }, [data, selectedType]);

    useEffect(() => {
        if (distinctTypes.length > 0) {
            setSelectedType(distinctTypes.includes("MetagraphL0") ? "MetagraphL0" : distinctTypes[0]);
        }
    }, [distinctTypes]);

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <div className="my-4">
                <Select onValueChange={(value) => setSelectedType(value)} defaultValue="MetagraphL0">
                    <SelectTrigger className="w-[200px]">
                        <SelectValue>MetagraphL0</SelectValue>
                    </SelectTrigger>
                    <SelectContent>
                        {distinctTypes.map((type) => (
                            <SelectItem key={type} value={type}>
                                {type}
                            </SelectItem>
                        ))}
                    </SelectContent>
                </Select>
            </div>

            <Tabs defaultValue="overview" className="w-full">
                <TabsList className="w-full justify-start">
                    <TabsTrigger value="overview">Overview</TabsTrigger>
                    <TabsTrigger value="detail">Detail</TabsTrigger>
                </TabsList>
                <TabsContent value="overview">
                    <div className="p-4">
                        <h2 className="text-xl font-semibold mb-2">Overview</h2>
                        <p>Here you can find an overview of all the connected nodes & their location.</p>
                    </div>
                    {isLoading ? (
                        <SkeletonCard />
                    ) : isError ? (
                        <div className="flex justify-center items-center text-red-500">
                            <AlertCircle className="h-8 w-8 mr-2" />
                            <span>Failed to fetch data</span>
                        </div>
                    ) : data ? (
                        <NodesMetagraphMap nodesData={filteredData} />
                    ) : null}
                </TabsContent>
                <TabsContent value="detail">
                    <div className="p-4">
                        <h2 className="text-xl font-semibold mb-2">Details</h2>
                        {isLoading ? (
                            <SkeletonCard />
                        ) : isError ? (
                            <div className="flex justify-center items-center text-red-500">
                                <AlertCircle className="h-8 w-8 mr-2" />
                                <span>Failed to fetch data</span>
                            </div>
                        ) : data ? (
                            <NodesMetagraphDetails nodesData={filteredData} />
                        ) : null}
                    </div>
                </TabsContent>
            </Tabs>
        </div>
    );
}
