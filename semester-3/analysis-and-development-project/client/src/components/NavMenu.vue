<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const isMenuOpen = ref(false);
const currentFlow = ref('passenger');
const router = useRouter();

function toggleMenu() {
  isMenuOpen.value = !isMenuOpen.value;
}

function closeMenu() {
  isMenuOpen.value = false;
}

defineExpose({
  toggleMenu,
});

const menuItems = [
  { id: 'passenger', label: 'Passenger', path: '/' },
  { id: 'business', label: 'Business', path: '/business' },
  { id: 'employee', label: 'Employee', path: '/employee' }, // Placeholder
];

function handleNavigate(flow, path) {
  currentFlow.value = flow;
  if (path !== '#') {
    router.push(path);
  }
  closeMenu();
}
</script>

<template>
  <div v-if="isMenuOpen" class="nav-menu-overlay">
    <!-- Backdrop covers everything except left menu -->
    <div class="nav-menu-backdrop" @click="closeMenu"></div>

    <!-- Left-side Menu Panel, prevents event bubbling -->
    <div class="nav-menu-panel" @click.stop>
      <!-- Header -->
      <div>
        <h2>SKYNET</h2>
      </div>
      <!-- Menu Items -->
      <div>
        <button
          v-for="item in menuItems"
          :key="item.id"
          class="items"
          @click="handleNavigate(item.id, item.path)"
        >
          {{ item.label }}
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
@import '../assets/css/navMenu.css';
</style>
