const form = document.querySelector('#search-form');
const controls = document.querySelector('#controls');
const version = document.querySelector('#version');
const query = document.querySelector('#query');
const status = document.querySelector('#status');
const results = document.querySelector('#results');
const products = document.querySelector('#products');
const retry = document.querySelector('#retry');
const DELAY = 2500;

function showSkeletons() {
  for (let i = 0; i < 6; i++) {
    const card = document.createElement('div');
    card.className = 'card skeleton';
    card.setAttribute('aria-hidden', 'true');
    for (let line = 0; line < 5; line++) {
      card.append(document.createElement('span'));
    }
    products.append(card);
  }
}

function showProducts(items) {
  products.replaceChildren();
  for (const item of items) {
    const card = document.createElement('article');
    card.className = 'card';
    const category = document.createElement('p');
    category.className = 'category';
    category.textContent = item.category;
    const title = document.createElement('h2');
    title.textContent = item.title;
    const description = document.createElement('p');
    description.textContent = item.description;
    const price = document.createElement('p');
    price.className = 'price';
    price.textContent = new Intl.NumberFormat('nl-BE', {
      style: 'currency', currency: 'USD'
    }).format(item.price);
    card.append(category, title, description, price);
    products.append(card);
  }
}

async function loadProducts() {
  if (controls.disabled) return;
  const enhanced = version.value === 'enhanced';
  controls.disabled = true;
  retry.hidden = true;
  results.setAttribute('aria-busy', 'true');
  products.classList.toggle('enhanced', enhanced);
  products.replaceChildren();
  status.textContent = enhanced
    ? 'Producten laden. De voorbeeldkaarten worden zo ingevuld…'
    : 'Laden…';
  if (enhanced) showSkeletons();

  const controller = new AbortController();
  let timeout;
  try {
    // Both versions wait exactly the same amount before making the same request.
    await new Promise(resolve => setTimeout(resolve, DELAY));
    timeout = setTimeout(() => controller.abort(), 10000);
    const url = new URL('https://dummyjson.com/products/search');
    url.search = new URLSearchParams({
      q: query.value.trim(), limit: '6', select: 'title,description,category,price'
    });
    const response = await fetch(url, { signal: controller.signal });
    if (!response.ok) throw new Error('Request failed');
    const data = await response.json();
    if (!Array.isArray(data.products)) throw new Error('Invalid response');
    showProducts(data.products);
    status.textContent = data.products.length
      ? `${data.products.length} ${data.products.length === 1 ? 'product geladen' : 'producten geladen'} (maximaal 6).`
      : 'Geen producten gevonden. Probeer een andere Engelse zoekterm of maak het zoekveld leeg.';
  } catch (error) {
    products.replaceChildren();
    status.textContent = error.name === 'AbortError'
      ? 'Het laden duurt te lang. Probeer het opnieuw.'
      : 'Producten konden niet worden geladen. Controleer je verbinding en probeer opnieuw.';
    retry.hidden = false;
  } finally {
    clearTimeout(timeout);
    results.setAttribute('aria-busy', 'false');
    controls.disabled = false;
  }
}

form.addEventListener('submit', event => {
  event.preventDefault();
  loadProducts();
});
version.addEventListener('change', loadProducts);
retry.addEventListener('click', () => {
  // Move focus before hiding the retry button during the next load.
  query.focus();
  loadProducts();
});
loadProducts();
