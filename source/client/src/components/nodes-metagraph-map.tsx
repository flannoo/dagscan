"use client";

import { useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import "leaflet-defaulticon-compatibility/dist/leaflet-defaulticon-compatibility.css";
import "leaflet-defaulticon-compatibility"; // This ensures icons load correctly
import { MetagraphNode } from "@/lib/shared/types";
import L from "leaflet";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

// Props interface definition
interface NodesMapProps {
  nodesData: MetagraphNode[];
}

// Group the data by latitude and longitude
const groupDataByCoordinates = (nodesData: MetagraphNode[]) => {
  const grouped: { [key: string]: MetagraphNode[] } = {};

  nodesData.forEach(vps => {
    const key = `${vps.latitude},${vps.longitude}`;
    if (!grouped[key]) {
      grouped[key] = [];
    }
    grouped[key].push(vps);
  });

  return grouped;
};

const groupDataByProperty = (nodesData: MetagraphNode[], property: keyof MetagraphNode) => {
  const grouped: { [key: string]: MetagraphNode[] } = {};

  nodesData.forEach(vps => {
    const key = String(vps[property]) || "Unknown"; // Group by property (e.g., country or isp)
    if (!grouped[key]) {
      grouped[key] = [];
    }
    grouped[key].push(vps);
  });

  return grouped;
};

const createCustomIcon = (size: number) => {
  return L.icon({
    iconUrl: "/icons/pinmap-icon.svg", // Path to your marker icon
    iconSize: [size, size],   // Dynamic size
    iconAnchor: [size / 2, size], // Anchor point
  });
};

export default function NodesMap({ nodesData }: NodesMapProps) {
  const nodesByCountry = groupDataByProperty(nodesData, "country");
  const sortedCountries = Object.keys(nodesByCountry)
    .sort((a, b) => nodesByCountry[b].length - nodesByCountry[a].length);
  const nodesByIsp = groupDataByProperty(nodesData, "serviceProvider");
  const sortedIsps = Object.keys(nodesByIsp)
    .sort((a, b) => nodesByIsp[b].length - nodesByIsp[a].length);

  // State for currently selected filter (either country or ISP)
  const [selectedFilter, setSelectedFilter] = useState<{ type: keyof MetagraphNode | null, value: string | null }>({
    type: null,
    value: null,
  });

  const handleFilter = (type: keyof MetagraphNode, value: string) => {
    if (selectedFilter.type === type && selectedFilter.value === value) {
      // If clicking the same row, deselect it (reset the filter)
      setSelectedFilter({ type: null, value: null });
    } else {
      // Otherwise, set the new filter
      setSelectedFilter({ type, value });
    }
  };

  const filteredVpsData =
    selectedFilter.type !== null && selectedFilter.value !== null
      ? nodesData.filter(node => node[selectedFilter.type!] === selectedFilter.value)
      : nodesData;

  return (
    <div className="flex flex-col md:flex-row">
      <div className="w-full md:w-1/4 p-4">
        <h2 className="font-bold mb-2">Map Filter</h2>
        <Table className="table-auto border-collapse mb-4">
          <TableHeader>
            <TableRow>
              <TableHead className="px-2 py-1 text-sm">Country</TableHead>
              <TableHead className="px-2 py-1 text-sm">Count</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {sortedCountries.map(country => (
              <TableRow
                key={country}
                onClick={() => handleFilter("country", country)}
                className={`cursor-pointer ${selectedFilter.type === "country" && selectedFilter.value === country
                  ? "bg-gray-300 hover:bg-gray-200 dark:bg-gray-700 dark:hover:bg-gray-600" // Selected row
                  : "hover:bg-gray-300 dark:hover:bg-gray-700" // Non-selected rows
                  }`}
              >
                <TableCell className="px-2 py-1 text-sm leading-tight">{country}</TableCell>
                <TableCell className="px-2 py-1 text-sm leading-tight">{nodesByCountry[country].length}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>

        {/* Table for Nodes by ISP */}
        <Table className="table-auto border-collapse">
          <TableHeader>
            <TableRow>
              <TableHead className="px-2 py-1 text-sm">ISP</TableHead>
              <TableHead className="px-2 py-1 text-sm">Count</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {sortedIsps.map(isp => (
              <TableRow
                key={isp}
                onClick={() => handleFilter("serviceProvider", isp)}
                className={`cursor-pointer ${selectedFilter.type === "serviceProvider" && selectedFilter.value === isp
                  ? "bg-gray-300 hover:bg-gray-200 dark:bg-gray-700 dark:hover:bg-gray-600" // Selected row
                  : "hover:bg-gray-300 dark:hover:bg-gray-700" // Non-selected rows
                  }`}
              >
                <TableCell className="px-2 py-1 text-sm leading-tight">{isp}</TableCell>
                <TableCell className="px-2 py-1 text-sm leading-tight">{nodesByIsp[isp].length}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>

      <div className="w-full md:w-3/4">
        <MapContainer center={[50, 10]} zoom={2} minZoom={1} maxZoom={18} scrollWheelZoom={true} style={{ height: "70vh", width: "100%" }} className="relative z-0">
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />

          {/* Render markers for each group of coordinates */}
          {Object.entries(groupDataByCoordinates(filteredVpsData)).map(([key, vpsList], index) => {
            const firstVps = vpsList[0]; // Take the first item for marker position
            const count = vpsList.length; // Number of VPS nodes in the same location

            // Define marker size: The more items, the larger the marker
            const markerSize = Math.min(50, 20 + count * 5); // Cap the size to prevent overly large markers

            return (
              <Marker
                key={index}
                position={[firstVps.latitude, firstVps.longitude]}
                icon={createCustomIcon(markerSize)} // Custom icon with dynamic size
              >
                <Popup>
                  <strong>{firstVps.serviceProvider}</strong><br />
                  City: {firstVps.city}<br />
                  Country: {firstVps.country}<br />
                  <strong>{count} nodes in this location</strong> {/* Display count */}
                </Popup>
              </Marker>
            );
          })}
        </MapContainer>
      </div>
    </div>
  );
}
