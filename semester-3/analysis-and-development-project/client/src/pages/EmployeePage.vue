<script setup>
import { ref, computed, onMounted } from 'vue';
import { getPods } from '../services/dataFetcher.js';

const pods = ref([]);

const activeCount = computed(() => pods.value.filter((p) => p.isActive).length);
const inactiveCount = computed(() => pods.value.filter((p) => !p.isActive).length);

onMounted(async () => {
  try {
    pods.value = await getPods();
  } catch (error) {
    console.error('PodManagement: Failed to fetch pods:', error);
  }
});
</script>

<template>
  <h1>Pod Management</h1>
  <div class="pod-page-layout">
    <div class="pod-management">
      <section aria-labelledby="pod-label">
        <h2 id="pod-label" class="pod-title">Pods</h2>
        <div class="pod-list-scroll">
          <ul class="pod-list" role="listbox">
            <li v-for="pod in pods" :key="pod.id" role="option">
              <div class="pod">
                <span class="pod-main">
                  <span class="pod-name">{{ pod.name }}</span>
                  <span class="pod-meta">
                    ID: {{ pod.id }}
                    <span class="pod-distance">
                      {{ pod.capacity }}/{{ pod.maxCapacity }} Occupied
                    </span>
                    <span class="pod-status"> ACTIVE: {{ pod.isActive ? 'Yes' : 'No' }} </span>
                  </span>
                </span>
              </div>
            </li>
          </ul>
        </div>
      </section>
    </div>
    <aside class="pod-dashboard">
      <h2 class="dashboard-title">Pod Status Dashboard</h2>
      <div class="dashboard-card active">
        <span class="dashboard-count">{{ activeCount }}</span>
        <span class="dashboard-label">Active Pods</span>
      </div>
      <div class="dashboard-card inactive">
        <span class="dashboard-count">{{ inactiveCount }}</span>
        <span class="dashboard-label">Inactive Pods</span>
      </div>
    </aside>
  </div>
</template>

<style scoped>
@import '../assets/css/employeePage.css';
</style>
