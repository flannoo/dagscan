import { TransactionList } from "@/components/transaction-list";

export default function TransactionsPage() {
    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">Transactions</h1>
            <div className="mb-4">
                <TransactionList />
            </div>
        </div>
    )
}
