//install
self.addEventListener("install", () => {
  self.skipWaiting();
});

//activate
self.addEventListener("activate", () => {
  return self.clients.claim();
});

//push event
self.addEventListener("push", e => {
  let data = {};

  try {
    data = e.data ? e.data.json() : {};
  } catch (err) {
    console.error("Push event bevatte geen geldige JSON:", err);
  }

  const title = data.title || "Skynet Alert";
  const body = data.body || "Je hebt een nieuwe melding.";

  const options = {
    body,
    badge: "/icons/badge.png",
    icon: "/icons/icon-192.png",
    vibrate: [100, 50, 100],
    data: {
      url: data.url || "/",
      ...data
    }
  };

  e.waitUntil(self.registration.showNotification(title, options));
});

//notification click
self.addEventListener("notificationclick", e => {
  e.notification.close();

  const url = e.notification.data?.url || "/";

  e.waitUntil(
    self.clients.matchAll({ type: "window", includeUncontrolled: true })
      .then(clientList => {
        for (const client of clientList) {
          if (client.url.includes(url) && client.focus) {
            return client.focus();
          }
        }
        if (self.clients.openWindow) {
          return self.clients.openWindow(url);
        }

        return null;
      })
      .catch(err => {
        console.error("Fout bij notificationclick:", err);
      })
  );
});
