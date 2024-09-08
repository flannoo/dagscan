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
    totalSnapshotFeeAmount: number;
};

type AggregatedData = {
    [snapshotDate: string]: AggregatedSnapshot;
};

const aggregateDataBySnapshotDate = (data: SnapshotMetric[]): AggregatedSnapshot[] => {
    const filteredData = data.filter((metric) => new Date(metric.snapshotDate) > new Date('2024-08-07')); //.filter((metric) => metric.isTimeTriggered);

    const aggregatedData: AggregatedData = filteredData.reduce((acc: AggregatedData, curr: SnapshotMetric) => {
        const snapshotDate = curr.snapshotDate;
        const existing = acc[snapshotDate];

        if (existing) {
            existing.totalSnapshotFeeAmount += curr.totalSnapshotFeeAmount / 100000000;
        } else {
            acc[snapshotDate] = {
                snapshotDate: snapshotDate,
                totalSnapshotFeeAmount: curr.totalSnapshotFeeAmount / 100000000,
            };
        }
        return acc;
    }, {});

    return Object.values(aggregatedData).sort((a, b) => new Date(a.snapshotDate).getTime() - new Date(b.snapshotDate).getTime());
};

interface ChartMetagraphSnapshotFeesProps {
    snapshotMetrics: SnapshotMetric[];
    metagraphAddress: string
}

export function ChartMetagraphSnapshotFees({ snapshotMetrics, metagraphAddress }: ChartMetagraphSnapshotFeesProps) {
    const filteredData = snapshotMetrics.filter((metric) => metric.metagraphAddress === metagraphAddress);
    
    const processedData = filteredData ? aggregateDataBySnapshotDate(filteredData) : [];
    const totalFees = processedData.reduce((acc, curr) => acc + curr.totalSnapshotFeeAmount, 0);

    const chartConfig = {
        FeeAmount: {
            label: "Fee Amount",
            color: "hsl(var(--chart-1))",
        },
    } satisfies ChartConfig

    return (
        <Card>
            <CardHeader>
                <CardTitle>Snapshot Fees</CardTitle>
                <CardDescription>Total snapshot fees consumed: {totalFees.toFixed(0)} $DAG</CardDescription>
            </CardHeader>
            <CardContent>
                <ChartContainer config={chartConfig}>
                    <LineChart
                        accessibilityLayer
                        data={processedData}
                        margin={{
                            left: 12,
                            right: 12,
                            bottom: 20,
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
                        <YAxis dataKey="totalSnapshotFeeAmount" />
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
                            dataKey="totalSnapshotFeeAmount"
                            type="linear"
                            dot={false}
                        />

                    </LineChart>
                </ChartContainer>
            </CardContent>
        </Card>
    )
}
