<script setup>
import { ref } from 'vue';

// Current time in HH:MM format
const time = ref(new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }));

// Update time every second
setInterval(() => {
  time.value = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
}, 1000);
</script>

<template>
  {{ time }}
</template>
