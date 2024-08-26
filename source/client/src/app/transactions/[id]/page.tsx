import TransactionDetail from "@/components/transaction-detail";

export default function TransactionDetailPage({ params }: { params: { id: string } }) {
    const id = Array.isArray(params.id) ? params.id[0] : params.id;

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <div className="mb-4">
                <TransactionDetail transactionId={id} metagraphId='' />
            </div>
        </div>
    )
}
