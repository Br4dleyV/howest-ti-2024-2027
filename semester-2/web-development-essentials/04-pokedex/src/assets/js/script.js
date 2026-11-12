import * as FilterRenderer from './filter-component/renderer.js';
import * as FilterHandler from './filter-component/handler.js';
import * as PokemonCatcherHandler from './pokemon-catcher/handler.js';

function init() {
    FilterRenderer.init(); // Renders the filter choices
    FilterHandler.init(); // Handles the filter choices
    PokemonCatcherHandler.init(); // Handles the pokemon catching
}

init();