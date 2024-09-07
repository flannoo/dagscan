import { getValidatorNodeUptime } from "@/lib/services/api-dagscan-request";
import { useQuery } from "@tanstack/react-query";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle } from "lucide-react";
import React from "react";
import { ChartConfig, ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart";
import { LineChart, CartesianGrid, XAxis, YAxis, Line } from "recharts";
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";

interface NodeUptimeProps {
    walletAddress: string;
}

export default function ChartNodeUptime({ walletAddress }: NodeUptimeProps) {
    const { data, isError, isFetching } = useQuery({
        queryKey: ['nodeuptime', walletAddress],
        queryFn: async () => {
            const result = await getValidatorNodeUptime(walletAddress);
            return result;
        },
        refetchOnWindowFocus: true
    });

    const chartConfig = {
        UptimePercentage: {
            label: "uptimePercentage",
            color: "hsl(var(--chart-1))",
        },
    } satisfies ChartConfig


    if (isFetching) {
        return <SkeletonCard />;
    }

    if (isError) {
        return (
            <div className="flex justify-center items-center text-red-500">
                <AlertCircle className="h-8 w-8 mr-2" />
                <span>Failed to fetch data</span>
            </div>
        );
    }

    if (!data || data.length === 0) {
        return null;
    }

    return (
        <div>
            <Card>
                <CardHeader className="flex flex-col items-stretch space-y-0 border-b p-0 sm:flex-row">
                    <div className="flex flex-1 flex-col justify-center gap-1 px-6 py-5 sm:py-6">
                        <CardTitle>Hypergraph Uptime</CardTitle>
                    </div>
                </CardHeader>
                <CardContent className="px-2 sm:p-6">
                    <ChartContainer config={chartConfig} className="aspect-auto h-[250px] w-full">
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
                            <YAxis dataKey="uptimePercentage" />
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
                                dataKey="uptimePercentage"
                                type="linear"
                                dot={false}
                            />

                        </LineChart>
                    </ChartContainer>
                </CardContent>
            </Card>
        </div>
    );
}
