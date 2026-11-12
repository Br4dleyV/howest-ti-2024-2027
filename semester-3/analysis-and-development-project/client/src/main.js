import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import './assets/css/main.css';

createApp(App).use(router).mount('#app');

if ('serviceWorker' in navigator) {
  window.addEventListener('DOMContentLoaded', () => {
    navigator.serviceWorker.register('/service-worker.js').catch((err) => {
      console.error('Registratie van service worker mislukt:', err);
    });
  });
}
