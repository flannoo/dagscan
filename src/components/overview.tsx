import React from "react"
import Link from "next/link"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table"

export function Overview() {
    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>Latest Transactions</CardTitle>
                </CardHeader>
                <CardContent>
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>Timestamp</TableHead>
                                <TableHead>Amount</TableHead>
                                <TableHead>From</TableHead>
                                <TableHead>To</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            <TableRow>
                                <TableCell>2023-04-15 12:34:56</TableCell>
                                <TableCell>0.5 BTC</TableCell>
                                <TableCell>
                                    <Link href="#" className="hover:underline" prefetch={false}>
                                        1234567890abcdef
                                    </Link>
                                </TableCell>
                                <TableCell>
                                    <Link href="#" className="hover:underline" prefetch={false}>
                                        fedcba0987654321
                                    </Link>
                                </TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>2023-04-14 09:12:34</TableCell>
                                <TableCell>2.1 ETH</TableCell>
                                <TableCell>
                                    <Link href="#" className="hover:underline" prefetch={false}>
                                        abcd1234efgh5678
                                    </Link>
                                </TableCell>
                                <TableCell>
                                    <Link href="#" className="hover:underline" prefetch={false}>
                                        8765hgfe4321dcba
                                    </Link>
                                </TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>2023-04-13 15:45:00</TableCell>
                                <TableCell>0.01 LTC</TableCell>
                                <TableCell>
                                    <Link href="#" className="hover:underline" prefetch={false}>
                                        xyz987pqr654wvu
                                    </Link>
                                </TableCell>
                                <TableCell>
                                    <Link href="#" className="hover:underline" prefetch={false}>
                                        uvw321tsr456xyz
                                    </Link>
                                </TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </CardContent>
            </Card>
        </div>
    )
}
