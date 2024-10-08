"use client"

import {
  ColumnDef,
  ColumnFiltersState,
  FilterFn,
  SortingFn,
  SortingState,
  flexRender,
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  sortingFns,
  useReactTable,
} from "@tanstack/react-table"

import {
  RankingInfo,
  rankItem,
  compareItems,
} from '@tanstack/match-sorter-utils'

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import React from "react"

declare module '@tanstack/react-table' {
  //add fuzzy filter to the filterFns
  interface FilterFns {
    fuzzy: FilterFn<unknown>
    transactionRef: FilterFn<unknown>;
  }
  interface FilterMeta {
    itemRank: RankingInfo
  }
}

// Define a custom fuzzy filter function that will apply ranking info to rows (using match-sorter utils)
const fuzzyFilter: FilterFn<any> = (row, columnId, value, addMeta) => {
  // Rank the item
  const itemRank = rankItem(row.getValue(columnId), value)

  // Store the itemRank info
  if (addMeta) {
    addMeta({
      itemRank,
    })
  }

  // Return if the item should be filtered in/out
  return itemRank.passed
}

// Custom filter function for transactionRef column
const transactionRefFilter: FilterFn<any> = (row, columnId, value, addMeta) => {
  const transactionRef = row.original.transactionHash;
  const snapshotRef = row.original.ordinal;

  const itemRank = rankItem(transactionRef, value);
  if (typeof addMeta === 'function') {
    addMeta({ itemRank });
  }

  return (
    itemRank.passed ||
    snapshotRef?.toString().includes(value) ||
    transactionRef?.toLowerCase().includes(value.toLowerCase())
  );
};

const combinedGlobalFilter: FilterFn<any> = (row, columnId, value, addMeta) => {
  const fuzzyResult = fuzzyFilter(row, columnId, value, addMeta);
  const transactionRefResult = transactionRefFilter(row, columnId, value, addMeta);

  return fuzzyResult || transactionRefResult;
};

interface DataTableWithSearchProps<TData, TValue> {
  columns: ColumnDef<TData, TValue>[]
  data: TData[]
}

// Define a custom fuzzy sort function that will sort by rank if the row has ranking information
const fuzzySort: SortingFn<any> = (rowA, rowB, columnId) => {
  let dir = 0

  // Only sort by rank if the column has ranking information
  if (rowA.columnFiltersMeta[columnId]) {
    dir = compareItems(
      rowA.columnFiltersMeta[columnId]?.itemRank!,
      rowB.columnFiltersMeta[columnId]?.itemRank!
    )
  }

  // Provide an alphanumeric fallback for when the item ranks are equal
  return dir === 0 ? sortingFns.alphanumeric(rowA, rowB, columnId) : dir
}

export function DataTableWithSearch<TData, TValue>({
  columns,
  data,
}: DataTableWithSearchProps<TData, TValue>) {
  const [sorting, setSorting] = React.useState<SortingState>([])
  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>([])
  const [globalFilter, setGlobalFilter] = React.useState('')

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    onColumnFiltersChange: setColumnFilters,
    getFilteredRowModel: getFilteredRowModel(),
    onSortingChange: setSorting,
    getSortedRowModel: getSortedRowModel(),
    globalFilterFn: combinedGlobalFilter,
    state: {
      sorting,
      columnFilters,
      globalFilter
    },
    filterFns: {
      fuzzy: fuzzyFilter,
      transactionRef: transactionRefFilter,
    },
  })

  return (
    <div>
      <div className="flex items-center py-4">
        <Input
          placeholder="Search..."
          value={globalFilter ?? ''}
          onChange={e => setGlobalFilter(String(e.target.value))}
          className="max-w-sm"
        />
      </div>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead key={header.id}>
                      {header.isPlaceholder
                        ? null
                        : flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                    </TableHead>
                  )
                })}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {table.getRowModel().rows?.length ? (
              table.getRowModel().rows.map((row) => (
                <TableRow
                  key={row.id}
                  data-state={row.getIsSelected() && "selected"}
                >
                  {row.getVisibleCells().map((cell) => (
                    <TableCell key={cell.id}>
                      {flexRender(cell.column.columnDef.cell, cell.getContext())}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={columns.length} className="h-24 text-center">
                  No results.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
      <div className="flex items-center justify-end space-x-2 py-4">
        <Button
          variant="outline" size="sm" onClick={() => table.setPageIndex(0)} disabled={!table.getCanPreviousPage()}>
          First
        </Button>
        <Button
          variant="outline" size="sm" onClick={() => table.previousPage()} disabled={!table.getCanPreviousPage()}>
          Previous
        </Button>
        <Button variant="outline" size="sm" onClick={() => table.nextPage()} disabled={!table.getCanNextPage()}>
          Next
        </Button>
        <Button variant="outline" size="sm" onClick={() => table.setPageIndex(table.getPageCount() - 1)} disabled={!table.getCanNextPage()}>
          Last
        </Button>
      </div>
    </div>
  )
}
