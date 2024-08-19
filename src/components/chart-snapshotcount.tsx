"use client";

import React, { useEffect, useState } from "react"
import { useQuery } from "@tanstack/react-query";
import { getSnapshotCount } from "@/lib/services/api-nebula-requests";
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import {
    ChartConfig,
    ChartContainer,
    ChartTooltip,
    ChartTooltipContent,
} from "@/components/ui/chart";
import { LineChart, CartesianGrid, XAxis, YAxis, Line } from "recharts";
import { SkeletonCard } from "./ui/skeleton-card";
import { AlertCircle } from "lucide-react";

export function ChartSnapshotCount() {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['snapshotcount'],
        queryFn: async () => getSnapshotCount(),
        refetchOnWindowFocus: true,
    });

    const chartConfig = {
        Count: {
            label: "Count",
            color: "hsl(var(--chart-1))",
        },
    } satisfies ChartConfig

    return (
        <Card>
            <CardHeader>
                <CardTitle>Snapshot Count</CardTitle>
                <CardDescription>Global L0 snapshots</CardDescription>
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
                    <ChartContainer config={chartConfig}>
                        <LineChart
                            accessibilityLayer
                            data={data}
                            margin={{
                                left: 12,
                                right: 12,
                                bottom: 16,
                            }}
                        >
                            <CartesianGrid vertical={false} />
                            <XAxis
                                dataKey="Timestamp"
                                tickLine={false}
                                axisLine={false}
                                tickMargin={8}
                                tickFormatter={(value: string) => new Date(value).toLocaleDateString("en-US", {
                                    month: "short",
                                    day: "numeric",
                                })}
                                angle={-45}
                                textAnchor="end"
                            />
                            <YAxis dataKey="Count" />
                            <ChartTooltip
                                cursor={false}
                                content={<ChartTooltipContent indicator="line" />}
                                labelFormatter={(label: string) =>
                                    new Date(label).toLocaleDateString("en-US", {
                                        month: "short",
                                        day: "numeric",
                                        year: "numeric",
                                    })
                                }
                            />
                            <Line
                                dataKey="Count"
                                type="linear"
                                dot={false}
                            />

                        </LineChart>
                    </ChartContainer>
                )}
            </CardContent>
        </Card>
    )
}
