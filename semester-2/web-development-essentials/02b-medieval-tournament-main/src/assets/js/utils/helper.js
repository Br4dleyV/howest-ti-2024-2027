import { knights } from '../data/knights.js';

export function calculateKnightPower(knight) { // Knight is an id
    const knightData = getKnightData(knight);
    return knightData.strength + (0.75 * knightData.agility);
}

export function getKnightData(knight) {
    return knights.find(k => k.id === Number(knight));
}

export function showResult(result) {
    const resultParagraph = document.querySelector('#result');
    resultParagraph.textContent = result;
}