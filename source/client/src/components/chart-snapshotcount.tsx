"use client";

import React from "react"
import { useQuery } from "@tanstack/react-query";
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
import { getSnapshotMetrics } from "@/lib/services/api-dagscan-request";
import { SnapshotMetric } from "@/lib/shared/types";

type AggregatedSnapshot = {
    snapshotDate: string;
    totalSnapshotCount: number;
};

type AggregatedData = {
    [snapshotDate: string]: AggregatedSnapshot;
};

const aggregateDataBySnapshotDate = (data: SnapshotMetric[]): AggregatedSnapshot[] => {
    const aggregatedData: AggregatedData = data.reduce((acc: AggregatedData, curr: SnapshotMetric) => {
        const snapshotDate = curr.snapshotDate;
        const existing = acc[snapshotDate];

        if (existing) {
            existing.totalSnapshotCount += curr.totalSnapshotCount;
        } else {
            acc[snapshotDate] = {
                snapshotDate: snapshotDate,
                totalSnapshotCount: curr.totalSnapshotCount,
            };
        }
        return acc;
    }, {});

    // Return the data as an array of objects
    return Object.values(aggregatedData);
};

export function ChartSnapshotCount() {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['snapshotcount'],
        queryFn: async () => getSnapshotMetrics(),
        refetchOnWindowFocus: true,
    });
    
    const processedData = data ? aggregateDataBySnapshotDate(data) : [];

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
                            data={processedData}
                            margin={{
                                left: 12,
                                right: 12,
                                bottom: 16,
                            }}
                        >
                            <CartesianGrid vertical={false} />
                            <XAxis
                                dataKey="snapshotDate"
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
                            <YAxis dataKey="totalSnapshotCount" />
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
