<script setup>
import { ref } from 'vue';
import { useRoute } from 'vue-router';
import Header from './components/Header.vue';
import MainMenu from './components/NavMenu.vue';
import { enableNotifications } from '@/services/pushService.js';

const mainMenu = ref(null);
const route = useRoute();

function toggleMenu() {
  mainMenu.value.toggleMenu();
}
</script>

<template>
  <Header :onHamburgerClick="toggleMenu"></Header>
  <MainMenu ref="mainMenu" />
  <main>
    <router-view />
  </main>
  <footer v-if="route.path === '/' || route.path === '/plan'">
    <div id="live-plan-nav">
      <router-link
        to="/"
        :class="['button', route.path === '/' ? 'button-primary' : 'button-secondary']"
        >LIVE</router-link
      >
      <router-link
        to="/plan"
        :class="['button', route.path === '/plan' ? 'button-primary' : 'button-secondary']"
        >PLAN</router-link
      >
    </div>
  </footer>

  <button @click="enableNotifications">Meldingen inschakelen</button>
</template>
