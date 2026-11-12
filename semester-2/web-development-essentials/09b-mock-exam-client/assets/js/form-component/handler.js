import { get, post } from "../data/data-connector.js";
import { renderDestinations, renderAstronauts, renderResults, renderError } from "./renderer.js";

function loadDestinations() {
    get("/destinations").then((data) => {
        if (!data) return;
        renderDestinations(data);
    }).catch(error => renderError(error));
}

function loadAstronauts() {
    get("/astronauts").then((data) => {
        if (!data) return;
        renderAstronauts(data);
    }).catch(error => renderError(error));
}

function executeMission(e) {
    e.preventDefault();

    // Get all selected astronauts
    const astronauts = [];
    const selectedCheckboxes = document.querySelectorAll("input[type='checkbox']:checked");
    selectedCheckboxes.forEach((checkbox) => {
        astronauts.push(checkbox.id);
    });

    // Get selected destination
    const destination = document.querySelector("#destination").value;
    
    // Get captain's email
    const captainEmail = document.querySelector("#captain").value;

    // Create payload 
    const payload = {
        astronauts: astronauts,
        destination: destination,
        emailCaptain: captainEmail,
    };

    post("/execute-mission", payload)
        .then((data) => {
            if (!data) return;
            renderResults(data.report);
        })
        .catch((error) => {
            renderError(error);
        });
}

export { loadDestinations, loadAstronauts, executeMission };