import { Skeleton } from "@/components/ui/skeleton"

export function SkeletonCard() {
  return (
    <div className="flex flex-col space-y-3">
        <div className="space-y-2">
        <Skeleton className="h-6 w-full" />
      </div>
      <Skeleton className="h-[125px] w-full rounded-xl" />
    </div>
  )
}
