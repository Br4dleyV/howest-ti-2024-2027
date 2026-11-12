<script setup>
import { ref, computed, onMounted, nextTick } from 'vue';
import { useRoute } from 'vue-router';
import * as DataFetcher from '../services/dataFetcher.js';

import MagnifyingGlassSvg from '@/components/svgs/MagnifyingGlassSvg.vue';
import MarkerSvg from '@/components/svgs/MarkerSvg.vue';
import ArrowSvg from '@/components/svgs/ArrowSvg.vue';
import ClockSvg from '@/components/svgs/ClockSvg.vue';
import SmilingFaceSvg from '@/components/svgs/SmilingFaceSvg.vue';
import GreaterThanSvg from '@/components/svgs/GreaterThanSvg.vue';

const route = useRoute();

const destinations = ref([]);
const pods = ref([]);

const startLocation = ref(null);
const endLocation = ref(null);
const selectedPod = ref(null);

const step = ref('destination'); // destination | loading | pods | confirm
const searchQuery = ref('');
const selectedFrom = ref('Huidige locatie');
const destinationId = ref('');

let map = null;

const filteredDestinations = computed(() =>
  destinations.value.filter((destination) =>
    destination.name.toLowerCase().includes(searchQuery.value.toLowerCase()),
  ),
);

onMounted(async () => {
  await loadDestinations();
  applyDestinationFromRoute();
});

async function loadDestinations() {
  try {
    destinations.value = await DataFetcher.getDestinations();
    addMockDistanceAndEta(destinations.value);
  } catch (error) {
    console.error('PlanPage: Failed to fetch destinations:', error);
  }
}

function applyDestinationFromRoute() {
  if (!route.query.destinationId) return;

  const destination = destinations.value.find((d) => d.id === route.query.destinationId);

  if (destination) {
    onSelectDestination(destination);
  }
}

function addMockDistanceAndEta(dests) {
  for (const destination of dests) {
    destination.distance = `${(Math.random() * 10 + 1).toFixed(1)} km`; // NOSONAR
    const eta = Math.floor(Math.random() * 20 + 5); // NOSONAR
    destination.eta = `${eta} min`;
  }
}

function onSelectDestination(destination) {
  prepareDestinationSelection(destination);
  requestUserLocation(destination.id);
}

function prepareDestinationSelection(destination) {
  searchQuery.value = destination.name;
  destinationId.value = destination.id;
  step.value = 'loading';
}

function requestUserLocation(destId) {
  navigator.geolocation.getCurrentPosition(
    (position) => handleGeolocationSuccess(position, destId),
    handleGeolocationError,
  );
}

async function handleGeolocationSuccess(position, destId) {
  setStartLocation(position);
  setEndLocation(destId);
  await loadPods(destId);
  step.value = 'pods';
}

function handleGeolocationError() {
  step.value = 'destination';
}

function setStartLocation(position) {
  startLocation.value = {
    latitude: position.coords.latitude,
    longitude: position.coords.longitude,
  };
}

function setEndLocation(destId) {
  const destination = destinations.value.find((d) => d.id === destId);
  if (destination?.coordinates) {
    endLocation.value = {
      latitude: destination.coordinates.latitude,
      longitude: destination.coordinates.longitude,
    };
  }
}

async function loadPods(destId) {
  const result = await DataFetcher.calculateReservations(startLocation.value, destId);
  pods.value = result.pods;
}

function selectPod(pod) {
  selectedPod.value = pod;
  step.value = 'confirm';
  renderMap();
}

function renderMap() {
  nextTick(() => {
    resetMap();
    if (startLocation.value && endLocation.value) {
      initializeMap();
    }
  });
}

function resetMap() {
  if (map) {
    map.remove();
    map = null;
  }
}

function initializeMap() {
  const zoom = 13;
  map = L.map('map').setView(getMapCenter(), zoom);
  addTileLayer();
  addMarkersAndRoute();
}

function getMapCenter() {
  return [
    (startLocation.value.latitude + endLocation.value.latitude) / 2,
    (startLocation.value.longitude + endLocation.value.longitude) / 2,
  ];
}

function addTileLayer() {
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution:
      '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
  }).addTo(map);
}

function addMarkersAndRoute() {
  const latlngs = [
    [startLocation.value.latitude, startLocation.value.longitude],
    [endLocation.value.latitude, endLocation.value.longitude],
  ];

  L.marker(latlngs[0]).addTo(map);
  L.marker(latlngs[1]).addTo(map);
  L.polyline(latlngs, { color: 'blue' }).addTo(map);
  map.fitBounds(latlngs);
}

function backToPods() {
  step.value = 'pods';
  selectedPod.value = null;
}

function confirmPod() {
  const notification = document.querySelector('#notification');
  notification.style.display = 'block';
  notification.querySelector('p').textContent = `Pod ${selectedPod.value?.name} bevestigd!`;

  backToDestination();
}

function closeNotification() {
  const notification = document.querySelector('#notification');
  notification.style.display = 'none';
}

function backToDestination() {
  step.value = 'destination';
  selectedPod.value = null;
  searchQuery.value = '';
}
</script>

<template>
  <div id="notification">
    <p>TEMPLATE NOTIFICATION</p>
    <!-- Close button -->
    <button @click="closeNotification">Close</button>
  </div>
  <main class="destination-search">
    <!-- FROM -->
    <section aria-labelledby="from-label">
      <div class="header-wrap">
        <div class="dot from pulse"></div>
        <span id="from-label">From</span>
      </div>
      <address>{{ selectedFrom }}</address>
    </section>

    <!-- TO -->
    <section aria-labelledby="to-label">
      <div class="header-wrap">
        <div class="dot to"></div>
        <span id="to-label">To</span>
      </div>
      <form @submit.prevent>
        <MagnifyingGlassSvg />
        <label for="destination-search-input" class="visually-hidden"> Bestemming zoeken </label>
        <input
          id="destination-search-input"
          v-model="searchQuery"
          type="search"
          autocomplete="off"
          placeholder="Voer je pod bestemming in..."
        />
      </form>
    </section>

    <!-- DESTINATIONS -->
    <section v-if="step === 'destination'">
      <h2 class="suggestions-title">Suggestions</h2>
      <ul class="suggestions-list" role="listbox">
        <li v-for="destination in filteredDestinations" :key="destination.id">
          <button @click="onSelectDestination(destination)" class="suggestion" role="option">
            <span class="icon-wrap">
              <MarkerSvg />
            </span>
            <span class="suggestion-main">
              <span class="suggestion-name">{{ destination.name }}</span>
              <span class="suggestion-meta">
                <span class="suggestion-distance"> <ArrowSvg /> {{ destination.distance }} </span>
                <span class="suggestion-eta"> <ClockSvg /> {{ destination.eta }} </span>
              </span>
            </span>
            <GreaterThanSvg />
          </button>
        </li>
      </ul>
    </section>

    <!-- LOADING -->
    <section v-if="step === 'loading'">
      <h2 class="suggestions-title">Calculating route...</h2>
    </section>

    <!-- PODS -->
    <section v-if="step === 'pods'">
      <h2 class="suggestions-title">Pods</h2>
      <ul class="suggestions-list">
        <li v-for="pod in pods" :key="pod.id">
          <button @click="selectPod(pod)" class="suggestion">
            <span class="icon-wrap">
              <SmilingFaceSvg />
            </span>
            <span class="suggestion-main">
              <span class="suggestion-name">{{ pod.name }}</span>
              <span class="suggestion-meta">
                {{ pod.capacity }}/{{ pod.maxCapacity }} |
                <ClockSvg />
                {{
                  new Date(pod.eta).toLocaleTimeString([], {
                    hour: '2-digit',
                    minute: '2-digit',
                  })
                }}
              </span>
            </span>
            <GreaterThanSvg />
          </button>
        </li>
      </ul>
      <div class="select-best-wrap">
        <button @click="backToDestination" class="select-best-btn">Back To Destinations</button>
      </div>
    </section>

    <!-- CONFIRM -->
    <section v-if="step === 'confirm'">
      <div class="confirmation-view">
        <div class="card">
          <div class="suggestion-name">Gekozen pod: {{ selectedPod?.name }}</div>
        </div>
        <div id="map" class="map-container"></div>
      </div>
      <div class="select-best-wrap">
        <button @click="backToPods" class="select-best-btn">Terug</button>
        <button @click="confirmPod" class="select-best-btn">Bevestig</button>
      </div>
    </section>
  </main>
</template>

<style scoped>
@import '../assets/css/planPage.css';

#notification {
  position: fixed;
  top: 1.25rem;
  right: 1.25rem;
  background: #4caf50;
  color: white;
  padding: 1rem 1.5rem;
  border-radius: 0.5rem;
  display: none;
  z-index: 1000;
}

#notification p {
  margin: 0 0 0.75rem 0;
}

#notification button {
  background: rgba(255, 255, 255, 0.2);
  color: white;
  border: none;
  padding: 0.375rem 0.75rem;
  border-radius: 0.25rem;
  cursor: pointer;
}
</style>
