import * as FormRenderer from "./form-component/renderer.js";
import * as FormHandler from "./form-component/handler.js";

function init() {
    hookupEventHandlers();
    FormRenderer.renderMissionTypes();
    FormRenderer.renderAvailableCrewMembers();
}

function hookupEventHandlers() {
    document.querySelector("form").addEventListener("submit", FormHandler.submitMission);
    document.querySelector("#mission-type").addEventListener("change", FormRenderer.renderMissionTypeDetails);
}

init();