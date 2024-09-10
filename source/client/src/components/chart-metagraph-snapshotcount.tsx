"use client";

import React from "react"
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

    return Object.values(aggregatedData).sort((a, b) => new Date(a.snapshotDate).getTime() - new Date(b.snapshotDate).getTime());
};

interface ChartMetagraphSnapshotCountProps {
    snapshotMetrics: SnapshotMetric[];
    metagraphAddress: string
}

export function ChartMetagraphSnapshotCount({ snapshotMetrics, metagraphAddress }: ChartMetagraphSnapshotCountProps) {
    const filteredData = snapshotMetrics.filter((metric) => metric.metagraphAddress?.value === metagraphAddress);
    const processedData = filteredData ? aggregateDataBySnapshotDate(filteredData) : [];

    const chartConfig = {
        Count: {
            label: "Count",
            color: "hsl(var(--chart-1))",
        },
    } satisfies ChartConfig

    return (
        <Card>
            <CardHeader>
                <CardTitle>Global L0 Snapshot Count</CardTitle>
                <CardDescription>Total event-triggered global L0 snapshots</CardDescription>
            </CardHeader>
            <CardContent>
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
                            dataKey="totalSnapshotCount"
                            type="linear"
                            dot={false}
                        />

                    </LineChart>
                </ChartContainer>
            </CardContent>
        </Card>
    )
}
