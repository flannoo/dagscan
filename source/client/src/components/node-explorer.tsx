"use client";

import React, { useMemo } from "react"
import { ValidatorNode } from "@/lib/shared/types";
import dynamic from "next/dynamic";
import { AlertCircle, Loader } from "lucide-react"
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import { useQuery } from "@tanstack/react-query";
import { getHypergraphValidatorNodes } from "@/lib/services/api-dagscan-request";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import NodesDetails from "@/components/nodes-details";

export default function NodeExplorer() {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['hypergraphnodes'],
        queryFn: async () => getHypergraphValidatorNodes(),
        refetchOnWindowFocus: true,
    });

    const NodesMap = useMemo(() => dynamic(() => import('@/components/nodes-map'), {
        loading: () => (
            <div className="flex items-center justify-center h-[70vh]">
                <Loader className="animate-spin text-gray-500" size={48} /> {/* Spinner */}
            </div>
        ),
        ssr: false, // Ensure this is rendered on the client-side
    }) as unknown as React.ComponentType<{ nodesData: ValidatorNode[] }>, []);

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <Tabs defaultValue="overview" className="w-full">
                <TabsList className="w-full justify-start">
                    <TabsTrigger value="overview">Overview</TabsTrigger>
                    <TabsTrigger value="detail">Detail</TabsTrigger>
                </TabsList>
                <TabsContent value="overview">
                    <div className="p-4">
                        <h2 className="text-xl font-semibold mb-2">Overview</h2>
                        <p>Here you can find an overview of all the connected global L0 nodes & their location.</p>
                    </div>
                    {isLoading ? (
                        <SkeletonCard />
                    ) : isError ? (
                        <div className="flex justify-center items-center text-red-500">
                            <AlertCircle className="h-8 w-8 mr-2" />
                            <span>Failed to fetch data</span>
                        </div>
                    ) : data ? (
                        <NodesMap nodesData={data.filter(node => node.nodeStatus !== "Offline")} />
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
                            <NodesDetails nodesData={data.filter(node => node.nodeStatus !== "Offline")} />
                        ) : null}
                    </div>
                </TabsContent>
            </Tabs>
        </div>
    );
}
