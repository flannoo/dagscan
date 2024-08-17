import React from "react"
import { LatestTransactionsMetagraph } from "@/components/latest-transactions-metagraph"
import { LatestSnapshotsMetagraph } from "@/components/latest-snapshots-metagraph"

export default function Metagraphs({ params }: { params: { id: string } }) {
    const id = Array.isArray(params.id) ? params.id[0] : params.id;
    
    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <div className="flex flex-col lg:flex-row lg:space-x-4 space-y-4 lg:space-y-0">
                <div className="lg:w-1/2">
                    <LatestSnapshotsMetagraph metagraphId={id} />
                </div>
                <div className="lg:w-1/2">
                    <LatestTransactionsMetagraph metagraphId={id} />
                </div>
            </div>
        </div>
    )
}
