import { pokedex } from "../data/data.js";

export function getAllTypes() {
    // Get all types from the pokedex
    const types = pokedex.map((pokemon) => pokemon.type).flat(); 
    // Remove duplicate types
    return [...new Set(types)];
}

export function getPokemonByType(type) {
    // Filter the pokedex based on the type
    if (!type) return pokedex;
    return pokedex.filter((pokemon) => pokemon.type.includes(type));
}

export function getPokemonByName(name) {
    // Find a pokemon by name
    return pokedex.find((pokemon) => pokemon.name === name);
}