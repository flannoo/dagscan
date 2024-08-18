"use client";

import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import "leaflet-defaulticon-compatibility/dist/leaflet-defaulticon-compatibility.css";
import "leaflet-defaulticon-compatibility"; // This ensures icons load correctly
import { NodeVpsData } from "@/lib/shared/types";
import L from "leaflet";

// Props interface definition
interface NodesMapProps {
  vpsData: NodeVpsData[];
}

// Group the data by latitude and longitude
const groupDataByCoordinates = (vpsData: NodeVpsData[]) => {
  const grouped: { [key: string]: NodeVpsData[] } = {};

  vpsData.forEach(vps => {
    const key = `${vps.latitude},${vps.longitude}`;
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

export default function NodesMap({ vpsData }: NodesMapProps) {
  const groupedVpsData = groupDataByCoordinates(vpsData); // Group VPS data by coordinates

  return (
    <MapContainer center={[50, 10]} zoom={2} minZoom={2} maxZoom={18} scrollWheelZoom={true} style={{ height: "70vh", width: "100%" }}>
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />

      {/* Render markers for each group of coordinates */}
      {Object.entries(groupedVpsData).map(([key, vpsList], index) => {
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
              <strong>{firstVps.isp}</strong><br />
              City: {firstVps.city}<br />
              Country: {firstVps.country}<br />
              <strong>{count} nodes in this location</strong> {/* Display count */}
            </Popup>
          </Marker>
        );
      })}
    </MapContainer>
  );
}
