import { renderCaughtPokemon } from "./renderer.js";

const pokeForm = document.querySelector("#poke-form");
const matchingPokemon = document.querySelector("#poke-match");

export function init() {
    pokeForm.addEventListener("submit", handleCatchPokemon);
}

function handleCatchPokemon(e) {
    e.preventDefault();

    const selectedPokemon = matchingPokemon.value;
    renderCaughtPokemon(selectedPokemon);
}