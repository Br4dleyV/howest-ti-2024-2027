<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import * as DataFetcher from '../services/dataFetcher.js';
import FileSvg from '../components/svgs/FileSvg.vue';
import MarkerSvg from '../components/svgs/MarkerSvg.vue';
import WeightSvg from '../components/svgs/WeightSvg.vue';
import CircleSvg from '../components/svgs/CircleSvg.vue';
import CalenderSvg from '../components/svgs/CalenderSvg.vue';
import VolumeSvg from '../components/svgs/VolumeSvg.vue';
import ArrowSvg from '../components/svgs/ArrowSvg.vue';
import ClockSvg from '../components/svgs/ClockSvg.vue';

const props = defineProps({
  userName: String,
  requests: Array,
});

const emit = defineEmits(['back', 'new-request', 'submit-request']);

/* Demo-data (genegeerd als je via props requests doorgeeft d) */
const demo = [
  {
    id: '#TR-2847',
    date: '2025-10-18',
    destination: 'Launch Station',
    weight: '',
    status: 'pending',
    note: 'Je aanvraag wordt binnen 24 uur beoordeeld',
  },
  {
    id: '#TR-2801',
    date: '2025-10-15',
    destination: 'Central Hub',
    weight: '',
    status: 'approved',
  },
];

/* View–state: 'list' of 'form' */
const view = ref('form');

function goToForm() {
  view.value = 'form';
  emit('new-request');
}

/* --- Form state --- */
const form = ref({
  destination: '',
  weightKg: '',
  volumeM3: '',
  date: '',
  remarks: '',
  acceptInfo: true,
});

function handleSubmit(e) {
  e.preventDefault();
  if (!form.value.destination) {
    return;
  }
  emit('submit-request', { ...form.value });
  // Reset form
  form.value = {
    destination: '',
    weightKg: '',
    volumeM3: '',
    date: '',
    remarks: '',
    acceptInfo: true,
  };
  view.value = 'list';
}

// --- Autocomplete Logic ---

const showSuggestions = ref(false);
const searchQuery = ref('');

watch(searchQuery, (val) => {
  // Show list only when there is input and no selection just made
  showSuggestions.value = !!val;
});
const selectedDestination = ref(null);

const destinations = ref([]);
const ETA_MINUTES_MIN = 5;
const ETA_MINUTES_RANGE = 20;
const DISTANCE_KM_MIN = 1;
const DISTANCE_KM_RANGE = 10;
const POD_TYPES = ['SkyNet', 'SkyLane', 'CityHover'];

onMounted(async () => {
  try {
    const fetchedDestinations = await DataFetcher.getDestinations();
    // Add mock distance, ETA, and type to destinations
    destinations.value = fetchedDestinations.map((dest) => ({
      ...dest,
      distance: `${(Math.random() * DISTANCE_KM_RANGE + DISTANCE_KM_MIN).toFixed(1)} km`,
      eta: `${Math.floor(Math.random() * ETA_MINUTES_RANGE + ETA_MINUTES_MIN)} min`,
      type: POD_TYPES[Math.floor(Math.random() * POD_TYPES.length)],
    }));
  } catch (error) {
    console.error('Failed to fetch data:', error);
  }
});

const filteredDestinations = computed(() => {
  if (!searchQuery.value) {
    return [];
  }
  return destinations.value.filter((dest) =>
    dest.name.toLowerCase().includes(searchQuery.value.toLowerCase()),
  );
});

function onSelectDestination(destination) {
  selectedDestination.value = destination;
  searchQuery.value = destination.name;
  form.value.destination = destination.name;
  showSuggestions.value = false; // close dropdown
}
</script>

<template>
  <div class="transport-status" id="page-transport-status">
    <!-- Hoofdinhoud -->
    <main class="content" aria-labelledby="page-title">
      <!-- TITEL LINKS • USER RECHTS -->
      <section v-if="view === 'list'" class="requests" aria-labelledby="requests-title">
        <ul class="request-list" role="list">
          <li v-for="req in props.requests || demo" :key="req.id">
            <article class="request-card" :aria-labelledby="`h-${req.id}`">
              <header class="request-card__header">
                <div class="card-left">
                  <div class="iconbox" aria-hidden="true">
                    <FileSvg />
                  </div>
                  <div class="id-and-date">
                    <div class="id-line">
                      <h3 :id="`h-${req.id}`" class="req-id">{{ req.id }}</h3>
                      <span
                        class="status-badge"
                        :data-status="req.status"
                        role="status"
                        :aria-label="req.status === 'pending' ? 'In afwachting' : 'Goedgekeurd'"
                      >
                        <span class="status-dot" aria-hidden="true"></span>
                        {{ req.status === 'pending' ? 'In afwachting' : 'Goedgekeurd' }}
                      </span>
                    </div>
                    <div class="req-date">{{ req.date }}</div>
                  </div>
                </div>
              </header>

              <section class="request-card__body">
                <dl class="details">
                  <div class="detail">
                    <dt>
                      <span class="detail-icon" aria-hidden="true">
                        <MarkerSvg />
                      </span>
                      Pod bestemming:
                    </dt>
                    <dd>{{ req.destination || '—' }}</dd>
                  </div>

                  <div class="detail">
                    <dt>
                      <span class="detail-icon" aria-hidden="true">
                        <WeightSvg />
                      </span>
                      Gewicht:
                    </dt>
                    <dd>{{ req.weight || '' }}</dd>
                  </div>
                </dl>
              </section>

              <p v-if="req.status === 'pending' && req.note" class="request-card__note">
                <CircleSvg />
                {{ req.note }}
              </p>
            </article>
          </li>
        </ul>

        <!-- CTA -->
        <div class="page-cta" aria-label="Nieuwe aanvraag">
          <button class="button button-primary cta-btn" type="button" @click="goToForm">
            Nieuwe aanvraag
          </button>
        </div>
      </section>

      <!-- ===================== FORM VIEW ===================== -->
      <section v-else class="transport-form" aria-labelledby="form-title">
        <h2 id="form-title" class="visually-hidden">Transportaanvraag</h2>

        <!-- Visuele container (grote kaart) -->
        <div class="form-shell" role="group" aria-labelledby="form-title">
          <form class="form" @submit="handleSubmit" novalidate>
            <!-- Pod bestemming (autocomplete) -->
            <div class="form-group full">
              <label for="dest">Pod bestemming</label>

              <div class="input with-icon autocomplete">
                <span class="input-icon" aria-hidden="true">
                  <MarkerSvg />
                </span>

                <!-- zoekveld -->
                <input
                  id="dest"
                  type="search"
                  v-model="searchQuery"
                  autocomplete="off"
                  placeholder="Bijv. Launch Station"
                  required
                />

                <!-- suggesties -->
                <ul
                  v-if="showSuggestions && filteredDestinations.length"
                  class="suggestions-dropdown"
                  role="listbox"
                  aria-label="Bestemmingssuggesties"
                >
                  <li v-for="dest in filteredDestinations" :key="dest.id" role="option">
                    <button
                      type="button"
                      class="suggestion-row"
                      @mousedown.prevent="onSelectDestination(dest)"
                      :aria-label="`Kies ${dest.name}`"
                    >
                      <span class="icon-wrap" aria-hidden="true">
                        <MarkerSvg />
                      </span>

                      <span class="s-main">
                        <span class="s-name">{{ dest.name }}</span>
                        <span class="s-meta">
                          <span class="s-distance">
                            <ArrowSvg />
                            {{ dest.distance }}
                          </span>
                          <span class="s-eta">
                            <ClockSvg />
                            {{ dest.eta }}
                          </span>
                        </span>
                      </span>

                      <span class="badge" :data-type="dest.type">{{ dest.type }}</span>
                    </button>
                  </li>
                </ul>
              </div>
            </div>

            <!-- Gewicht + Volume -->
            <div class="form-row two" role="group" aria-label="Lading">
              <div class="form-group">
                <label for="weight">Gewicht (kg)</label>
                <div class="input with-icon">
                  <span class="input-icon" aria-hidden="true">
                    <WeightSvg />
                  </span>
                  <input
                    id="weight"
                    type="number"
                    min="0"
                    step="0.1"
                    v-model="form.weightKg"
                    placeholder="0"
                  />
                </div>
              </div>

              <div class="form-group">
                <label for="volume">Volume (m³)</label>
                <div class="input with-icon">
                  <span class="input-icon" aria-hidden="true">
                    <VolumeSvg />
                  </span>
                  <input
                    id="volume"
                    type="number"
                    min="0"
                    step="0.1"
                    v-model="form.volumeM3"
                    placeholder="0.0"
                  />
                </div>
              </div>
            </div>

            <!-- Datum -->
            <div class="form-group full">
              <label for="date">Gewenste datum</label>
              <div class="input with-icon">
                <span class="input-icon" aria-hidden="true">
                  <CalenderSvg />
                </span>
                <input id="date" type="date" v-model="form.date" placeholder="dd/mm/jjjj" />
              </div>
            </div>

            <!-- Opmerkingen -->
            <div class="form-group full">
              <label for="remarks">Opmerkingen (optioneel)</label>
              <div class="input">
                <textarea
                  id="remarks"
                  rows="4"
                  v-model="form.remarks"
                  placeholder="Extra instructies of speciale vereisten..."
                ></textarea>
              </div>
            </div>

            <!-- Info note -->
            <div class="info-note" id="form-info-note" role="note" aria-live="polite">
              <span class="note-icon" aria-hidden="true">
                <CircleSvg />
              </span>
              <span
                >Je aanvraag wordt binnen 24 uur beoordeeld. Je ontvangt een bevestiging via
                e-mail.</span
              >
            </div>

            <!-- Submit -->
            <div class="form-actions">
              <button
                type="submit"
                class="button button-primary submit-btn"
                aria-describedby="form-info-note"
              >
                Verzend aanvraag
              </button>
            </div>
          </form>
        </div>
      </section>
    </main>
  </div>
</template>

<style scoped>
@import '../assets/css/businessPage.css';
</style>
