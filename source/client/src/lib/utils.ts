import { type ClassValue, clsx } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export const formatDate = (value: string | number | undefined): string => {
  if (!value) {
    return "";
  }

  const date = new Date(value);

  // Check if the date is valid
  if (isNaN(date.getTime())) {
    return ""; // Invalid date
  }

  return date.toLocaleString();
};

export const formatAmount = (amount: number | undefined) => {
  if (amount === undefined) {
    return "";
  }

  const parsedAmount = parseFloat(String(amount));
  if (isNaN(parsedAmount)) {
    return "";
  }

  const formattedAmount = new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 8,
  }).format(parsedAmount);

  return `${formattedAmount}`;
};

export const formatMetagraphAmount = (amount: number | undefined) => {
  if (amount === undefined) {
    return "";
  }

  const parsedAmount = parseFloat(String(amount));
  if (isNaN(parsedAmount)) {
    return "";
  }

  // Format the amount (divided by 100000000) and add 'DAG'
  const formattedAmount = new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 8,
  }).format(parsedAmount / 100000000);

  return `${formattedAmount}`;
};

export const formatDagAmount = (amount: number | undefined) => {
  if (amount === undefined) {
    return "";
  }

  const parsedAmount = parseFloat(String(amount));
  if (isNaN(parsedAmount)) {
    return "";
  }

  // Format the amount (divided by 100000000) and add 'DAG'
  const formattedAmount = new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 8,
  }).format(parsedAmount / 100000000);

  return `${formattedAmount} DAG`;
};

export const formatDdagAmount = (amount: number | undefined) => {
  if (amount === undefined) {
    return "";
  }

  const parsedAmount = parseFloat(String(amount));
  if (isNaN(parsedAmount)) {
    return "";
  }

  const formattedAmount = new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 8,
  }).format(parsedAmount);

  return `${formattedAmount} dDAG`;
};

export const getRawStringFromByteArray = (byteArray: string[]) => {
  return "[" + byteArray.join(',') + "]";
};

export const getConvertedStringFromByteArray = (byteArray: string[]) => {
  const jsonString = String.fromCharCode(...byteArray.map(byte => parseInt(byte, 10)));
  return jsonString;
};
