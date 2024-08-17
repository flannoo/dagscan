import React from "react";
import { Input } from "@/components/ui/input";

export function Search() {
  return (
    <Input
      type="search"
      placeholder="Search for transactions, addresses or snapshots..."
      className="w-full"
    />
  );
}
