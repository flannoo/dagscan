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
    totalTransactionCount: number;
};

type AggregatedData = {
    [snapshotDate: string]: AggregatedSnapshot;
};

const aggregateDataBySnapshotDate = (data: SnapshotMetric[]): AggregatedSnapshot[] => {
    const aggregatedData: AggregatedData = data.reduce((acc: AggregatedData, curr: SnapshotMetric) => {
        const snapshotDate = curr.snapshotDate;
        const existing = acc[snapshotDate];

        if (existing) {
            existing.totalTransactionCount += curr.totalTransactionCount;
        } else {
            acc[snapshotDate] = {
                snapshotDate: snapshotDate,
                totalTransactionCount: curr.totalTransactionCount,
            };
        }
        return acc;
    }, {});

    return Object.values(aggregatedData).sort((a, b) => new Date(a.snapshotDate).getTime() - new Date(b.snapshotDate).getTime());
};

interface ChartDagTransactionsProps {
    snapshotMetrics: SnapshotMetric[];
}

export function ChartDagTransactionCount({ snapshotMetrics }: ChartDagTransactionsProps) {
    const filteredData = snapshotMetrics.filter((metric) => new Date(metric.snapshotDate) > new Date('2024-07-08') && !metric.metagraphAddress);

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
                <CardTitle>DAG Transaction Count</CardTitle>
                <CardDescription>Total DAG Transactions Count</CardDescription>
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
                        <YAxis dataKey="totalTransactionCount" />
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
                            dataKey="totalTransactionCount"
                            type="linear"
                            dot={false}
                        />

                    </LineChart>
                </ChartContainer>
            </CardContent>
        </Card>
    )
}
