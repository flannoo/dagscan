"use client";

import React, { useState } from "react";
import { useRouter } from "next/navigation";
import { Input } from "@/components/ui/input";
import { useToast } from "@/components/ui/use-toast";

export const isValidHash = new RegExp('^[a-fA-F0-9]{64}$');
export const isValidAddress = new RegExp('^DAG[0-9][a-zA-Z0-9]{36}$');
export const isValidHeight = new RegExp('^[0-9]{1,64}$');
export const isValidNode = new RegExp('^[a-fA-F0-9]{128}$');

export enum SearchableItem {
  Hash = "Hash",
  Address = "Address",
  Snapshot = "Snapshot",
  NodeAddress = "NodeAddress"
}

export const getSearchInputType = (input: string): SearchableItem | undefined => {
  if (isValidHash.test(input)) {
    return SearchableItem.Hash;
  }

  if (isValidAddress.test(input)) {
    return SearchableItem.Address;
  }

  if (isValidHeight.test(input)) {
    return SearchableItem.Snapshot;
  }

  if (isValidNode.test(input)) {
    return SearchableItem.NodeAddress;
  }

  return undefined;
};

export function Search() {
  const [searchInput, setSearchInput] = useState("");
  const [toastVisible, setToastVisible] = useState(false);
  const router = useRouter();
  const { toast } = useToast();

  // Handle the search form submission
  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // Hide any currently active toast by changing visibility
    setToastVisible(false);

    const searchType = getSearchInputType(searchInput.trim());

    if (!searchType) {
      console.log("No Results Found");
      toast({
        title: "No Results Found",
        description: "Your search did not match any transactions, addresses, or snapshots.",
        variant: "destructive",
      });
      setToastVisible(true);
      return;
    }

    // Redirect based on search input type
    switch (searchType) {
      case SearchableItem.Hash:
        router.push(`/transactions/${searchInput}`);
        break;
      case SearchableItem.Address:
        router.push(`/addresses/${searchInput}`);
        break;
      case SearchableItem.Snapshot:
        router.push(`/snapshots/${searchInput}`);
        break;
      case SearchableItem.NodeAddress:
          router.push(`/nodes/${searchInput}`);
          break;
      default:
        break;
    }
  };

  return (
    <form onSubmit={handleSearchSubmit}>
      <Input
        type="search"
        placeholder="Search for transactions, addresses or snapshots..."
        className="w-full"
        value={searchInput}
        onChange={(e) => setSearchInput(e.target.value)}
      />
    </form>
  );
}
