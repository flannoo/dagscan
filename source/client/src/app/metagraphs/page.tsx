import { MetagraphList } from "@/components/metagraph-list"
import React from "react"

export default function Metagraphs() {
    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">Metagraphs</h1>
            <MetagraphList />
        </div>
    )
}
