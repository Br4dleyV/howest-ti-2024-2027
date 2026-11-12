<script setup>
import { onMounted } from 'vue';
import * as DataFetcher from '@/services/dataFetcher.js';
import { resolveImage } from '@/utils/resolveAsset.js';

const DEFAULT_MAP_COORDS = [51.211288, 3.227009]; // NOSONAR
const DEFAULT_MAP_ZOOM = 5;
const USER_LOCATION_ZOOM = 15;
const DEFAULT_BOUNDS = [
  [51.1, 3.1], // NOSONAR
  [51.3, 3.4], // NOSONAR
];
const MARKER_ICON_SIZE = 24;
const MARKER_ICON_ANCHOR = MARKER_ICON_SIZE / 2;

let map;

onMounted(async () => {
  map = L.map('live-map').setView(DEFAULT_MAP_COORDS, DEFAULT_MAP_ZOOM);
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 19 }).addTo(map);
  map.fitBounds(DEFAULT_BOUNDS);

  // Get user location
  if ('geolocation' in navigator) {
    navigator.geolocation.getCurrentPosition(
      async (position) => {
        const userGeo = { lat: position.coords.latitude, lng: position.coords.longitude };

        // Add user location marker
        const userMarkerIcon = L.icon({
          iconUrl: resolveImage('user_location.png'),
          iconSize: [16, 16],
          iconAnchor: [8, 8],
        });
        L.marker(userGeo, { icon: userMarkerIcon }).addTo(map);
        map.setView(userGeo, USER_LOCATION_ZOOM);

        addDestinationMarkers(await DataFetcher.getDestinations());
      },
      () => {
        map.fitBounds(DEFAULT_BOUNDS); // Fallback if user denies location access
      },
    );
  } else {
    map.fitBounds(DEFAULT_BOUNDS); // Fallback if geolocation is not supported
  }
});

function addDestinationMarkers(destinations) {
  destinations.forEach((destination) => {
    const destinationMarkerIcon = L.icon({
      iconUrl: resolveImage('destination_marker.png'),
      iconSize: [MARKER_ICON_SIZE, MARKER_ICON_SIZE],
      iconAnchor: [MARKER_ICON_ANCHOR, MARKER_ICON_ANCHOR],
    });
    const marker = L.marker([destination.coordinates.latitude, destination.coordinates.longitude], {
      icon: destinationMarkerIcon,
    }).addTo(map);
    marker.on('click', () => {
      // Go to /plan and pass destination ID as query parameter
      window.location.href = `/plan?destinationId=${destination.id}`;
    });
  });
}
</script>

<template>
  <div id="live-map"></div>
</template>
