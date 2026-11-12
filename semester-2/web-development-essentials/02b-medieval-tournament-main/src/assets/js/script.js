import * as FormRenderer from "./form-component/renderer.js";
import * as FormHandler from "./form-component/handler.js";

function init() {
    FormRenderer.renderKnights();
    hookupEventHandlers();
}

function hookupEventHandlers() {
    document.querySelector("#container form").addEventListener("submit", FormHandler.startBattle);
}

init();
