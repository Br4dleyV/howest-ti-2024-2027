import config from '@/utils/config.js';

//helpers
// base64 (public key) => UInt8Array (PushManager)
function urlBase64ToUint8Array(base64String) {
  const padding = '='.repeat((4 - (base64String.length % 4)) % 4);
  const base64 = (base64String + padding).replace(/-/g, '+').replace(/_/g, '/');

  const rawData = atob(base64);
  const outputArray = new Uint8Array(rawData.length);

  for (let i = 0; i < rawData.length; i += 1) {
    outputArray[i] = rawData.charCodeAt(i);
  }

  return outputArray;
}

function browserSupportsNotifications() {
  if (!('Notification' in window)) {
    console.error('Deze browser ondersteunt geen notificaties.');
    return false;
  }
  return true;
}

async function requestNotificationPermission() {
  const permission = await Notification.requestPermission();

  if (permission !== 'granted') {
    console.error('Notificatie toestemming geweigerd door gebruiker.');
    return false;
  }

  return true;
}

function browserSupportsServiceWorkers() {
  if (!('serviceWorker' in navigator)) {
    console.error('Deze browser ondersteunt geen service workers.');
    return false;
  }
  return true;
}

async function getServiceWorkerRegistration() {
  return navigator.serviceWorker.ready;
}

//push subscription
async function getOrCreateSubscription(registration) {
  const subscription = await registration.pushManager.getSubscription();

  if (subscription) {
    return subscription;
  }

  if (!config.VAPID_PUBLIC_KEY) {
    console.error('VAPID public key ontbreekt (VITE_VAPID_PUBLIC_KEY).');
    return null;
  }

  return registration.pushManager.subscribe({
    userVisibleOnly: true,
    applicationServerKey: urlBase64ToUint8Array(config.VAPID_PUBLIC_KEY),
  });
}

function buildSubscriptionPayload(subscription) {
  const key = subscription.getKey('p256dh');
  const auth = subscription.getKey('auth');

  const p256dh = btoa(String.fromCharCode.apply(null, new Uint8Array(key)));

  const authKey = btoa(String.fromCharCode.apply(null, new Uint8Array(auth)));

  return {
    endpoint: subscription.endpoint,
    p256dh,
    auth: authKey,
  };
}

async function sendSubscriptionToBackend(payload) {
  const response = await fetch(`${config.API_URL}/weather-alerts/subscribe`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    const text = await response.text();
    console.error(`Failed to store subscription (${response.status}): ${text}`);
    return false;
  }

  return true;
}

// Public API: enable browser notifications + push subscription registration
export async function enableNotifications() {
  if (!browserSupportsNotifications()) return;
  if (!(await requestNotificationPermission())) return;
  if (!browserSupportsServiceWorkers()) return;

  try {
    const registration = await getServiceWorkerRegistration();
    const subscription = await getOrCreateSubscription(registration);

    if (!subscription) return;

    const payload = buildSubscriptionPayload(subscription);
    const success = await sendSubscriptionToBackend(payload);

    if (success) {
      console.error('Meldingen zijn ingeschakeld voor deze browser.');
    }
  } catch (err) {
    console.error('Fout bij verzenden van subscription naar backend:', err);
  }
}
