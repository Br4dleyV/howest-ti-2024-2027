import config from '@/utils/config.js';

const HTTP_INTERNAL_SERVER_ERROR = 500;

async function getDestinations() {
  const response = await fetch(`${config.API_URL}/stations`);
  if (!response.ok) {
    throw new Error('Failed to fetch destinations');
  }
  return response.json();
}

async function getPods() {
  const response = await fetch(`${config.API_URL}/pods`);
  if (!response.ok) {
    throw new Error('Failed to fetch pods');
  }
  return response.json();
}

async function calculateReservations(startLocation, endStationId) {
  const response = await fetch(
    `${config.API_URL}/reservations/calculate?endStationId=${endStationId}&latitude=${startLocation.latitude}&longitude=${startLocation.longitude}`,
    {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    },
  );
  if (!response.ok) {
    throw new Error(`Failed to calculate reservations with status: ${response.status}`);
  }
  if (response.status === HTTP_INTERNAL_SERVER_ERROR) {
    throw new Error('Internal Server Error while calculating reservations');
  }
  return response.json();
}

export { getDestinations, getPods, calculateReservations };
