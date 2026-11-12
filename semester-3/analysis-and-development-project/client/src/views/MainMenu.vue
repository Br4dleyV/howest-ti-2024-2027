<template>
  <div v-if="isOpen" class="main-menu-overlay">
    <!-- Backdrop -->
    <div class="main-menu-backdrop" @click="onClose"></div>

    <!-- Menu Panel -->
    <div class="main-menu-panel" @click.stop>
      <!-- Header -->
      <div>
        <div>
          <h2>SKYNET</h2>
        </div>
      </div>

      <!-- Menu Items -->
      <div>
        <button
          v-for="item in menuItems"
          :key="item.id"
          class="items"
          @click="handleNavigate(item.id)"
        >
          {{ item.label }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { defineProps } from 'vue';

const props = defineProps({
  isOpen: Boolean,
  currentFlow: String,
  onClose: Function,
  onNavigate: Function,
});

const menuItems = [
  { id: 'passenger', label: 'Passenger' },
  { id: 'business', label: 'Business' },
  { id: 'employee', label: 'Employee' },
];

function handleNavigate(flow) {
  props.onNavigate(flow);
  props.onClose();
}
</script>
