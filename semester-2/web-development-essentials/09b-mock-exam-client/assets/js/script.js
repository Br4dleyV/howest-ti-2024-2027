import * as FormHandler from "./form-component/handler.js"

function init() {
    FormHandler.loadDestinations();
    FormHandler.loadAstronauts();

    document.querySelector("form").addEventListener("submit", FormHandler.executeMission);
}

init();