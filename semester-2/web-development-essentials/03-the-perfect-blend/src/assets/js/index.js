import * as StoreRenderer from './store-component/renderer.js';
import * as StoreHandler from './store-component/handler.js';

function init() {
    // Render the teas in the store
    StoreRenderer.renderTeas();
    // Listen for clicks on the products
    document.querySelector('#products').addEventListener('click', StoreHandler.handleClick);
}

init();