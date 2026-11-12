const runtimeConfig = window.__APP_CONFIG__ ?? {};

const config = {
  API_URL: runtimeConfig.API_URL || import.meta.env.VITE_API_URL,
  VAPID_PUBLIC_KEY: runtimeConfig.VAPID_PUBLIC_KEY || import.meta.env.VITE_VAPID_PUBLIC_KEY,
};

export default config;
