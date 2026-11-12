import { minimumDestinationDistance } from "../config.js";

function renderDestinations(destinations) {
    const destinationsSelect = document.querySelector("#destination");

    destinations.forEach(destination => {
        if (destination.distance >= minimumDestinationDistance) {
            const option = document.createElement("option");
            option.value = destination.id;
            option.textContent = `${destination.name} (dist: ${destination.distance} AU)`;
            destinationsSelect.appendChild(option);
        }
    });
}

function renderAstronauts(astronauts) {
    const astronautsContainer = document.querySelector("#astronauts");

    astronauts.forEach(astronaut => {
        astronautsContainer.insertAdjacentHTML("beforeend", `
            <li>
                <label>
                    <input type='checkbox' id='${astronaut.id}'>${astronaut.name}</input>
                </label>
            </li>`);
    });
}

function renderResults(text) {
    const resultElement = document.querySelector("#results");
    resultElement.innerHTML = text;
}

function renderError(error) {
    const errorContainer = document.querySelector("#errors");
    errorContainer.innerHTML = ""; 
    errorContainer.insertAdjacentHTML("beforeend", `<li>${error.message}</li>`);
}

export { renderDestinations, renderAstronauts, renderResults, renderError };