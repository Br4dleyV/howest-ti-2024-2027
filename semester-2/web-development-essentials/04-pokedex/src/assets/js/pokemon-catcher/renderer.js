import { getPokemonByName } from "../utils/helper.js";

const caughtPokemon = document.querySelector("#poke-catch");
const pokemonTemplate = document.querySelector("#poke-template");

export function renderCaughtPokemon(pokemon) {
    // Get the pokemon data
    const pokemonData = getPokemonByName(pokemon);

    // Render the pokemon
    const template = pokemonTemplate.content.firstElementChild.cloneNode(true);
    template.querySelector("img").src = pokemonData.img;
    template.querySelector("figcaption").textContent = pokemonData.name;

    // Append the pokemon to the list
    caughtPokemon.appendChild(template);
}