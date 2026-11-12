import { knights } from '../data/knights.js';

const knight1select = document.querySelector('#knight1');
const knight2select = document.querySelector('#knight2');

export function renderKnights() {
    knights.forEach(knight => {
        const option = `<option value="${knight.id}">${knight.name}</option>`;

        knight1select.insertAdjacentHTML("beforeend", option);
        knight2select.insertAdjacentHTML("beforeend", option);
    });
}