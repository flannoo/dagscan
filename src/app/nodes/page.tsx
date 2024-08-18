"use client";

import React, { useMemo } from "react"
import { NodeVpsData } from "@/lib/shared/types";
import dynamic from "next/dynamic";
import { Loader } from "lucide-react"

// Mock data
const vpsNodesData = [
    { latitude: 37.7749, longitude: -122.4194, isp: "ISP A", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 37.7749, longitude: -122.4194, isp: "ISP A", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 48.8566, longitude: 2.3522, isp: "ISP B", ipAddress: "", country: "", city: "", organization: "" },
    { latitude: 34.0522, longitude: -118.2437, isp: "ISP C", ipAddress: "", country: "", city: "", organization: "" }
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

    return <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
        <h1 className="text-2xl font-bold mb-4">Node Explorer</h1>
        <NodesMap vpsData={vpsNodesData} />
    </div>
}
