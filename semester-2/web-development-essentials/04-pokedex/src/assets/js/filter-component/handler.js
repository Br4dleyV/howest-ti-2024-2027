import { getPokemonByType } from "../utils/helper.js";

const filterByType = document.querySelector("#poke-type");
const filterByMatch = document.querySelector("#poke-match");

export function init() {
    filterByType.addEventListener("change", handleTypeFilterChange);
    handleTypeFilterChange({ target: { value: "isAny" } });
}

export function handleTypeFilterChange(e) {
    const type = e.target.value;
    filterByMatch.innerHTML = "";
    let filteredPokemon;
    if (type === "isAny") {
        filteredPokemon = getPokemonByType();
    } else {
        filteredPokemon = getPokemonByType(type);
    }
    renderPokemon(filteredPokemon);
}

function renderPokemon(filteredPokemon) {
    filterByMatch.innerHTML = "";
    filteredPokemon.forEach((pokemon) => {
        filterByMatch.innerHTML += `$<option value="${pokemon.name}">${pokemon.name}</option>`;
    });
}