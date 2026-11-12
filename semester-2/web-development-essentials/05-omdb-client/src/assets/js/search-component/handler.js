import { apiUrl } from "../config.js";
import { renderMovies } from "./renderer.js";

const searchForm = document.querySelector("form");

export function init() {
    searchForm.addEventListener("submit", fetchMovies);
}

function fetchMovies(e) {
    const searchValue = e.target.querySelector("input").value;
    const url = `${apiUrl}s=${searchValue}`;

    fetch(url)
        .then(response => response.json())
        .then(data => {
            renderMovies(data.Search);
        });
}