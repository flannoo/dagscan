"use client";

import React, { useMemo } from "react"
import { NodeVpsData } from "@/lib/shared/types";
import dynamic from "next/dynamic";
import { Loader } from "lucide-react"
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";

// Mock data
const vpsNodesData = [
    { latitude: 37.7749, longitude: -122.4194, isp: "ISP A", ipAddress: "", country: "Belgium", city: "", organization: "" },
    { latitude: 37.7749, longitude: -122.4194, isp: "ISP A", ipAddress: "", country: "Belgium", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "Belgium", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "Belgium", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "US", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "US", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "US", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "US", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "Mexico", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "Mexico", city: "", organization: "" },
    { latitude: 34.0522, longitude: -118.2437, isp: "ISP C", ipAddress: "", country: "Mexico", city: "", organization: "" }
] as NodeVpsData[];

export default function Nodes() {
    const NodesMap = useMemo(() => dynamic(() => import('@/components/nodes-map'), {
        loading: () => (
            <div className="flex items-center justify-center h-[70vh]">
                <Loader className="animate-spin text-gray-500" size={48} /> {/* Spinner */}
            </div>
        ),
        ssr: false, // Ensure this is rendered on the client-side
    }) as unknown as React.ComponentType<{ vpsData: NodeVpsData[] }>, []);

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">Node Explorer</h1>

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
                    <NodesMap vpsData={vpsNodesData} />
                </TabsContent>
                <TabsContent value="detail">
                    <div className="p-4">
                        <h2 className="text-xl font-semibold mb-2">Details</h2>
                        <p>TODO</p>
                    </div>
                </TabsContent>
            </Tabs>
        </div>
    );
}
