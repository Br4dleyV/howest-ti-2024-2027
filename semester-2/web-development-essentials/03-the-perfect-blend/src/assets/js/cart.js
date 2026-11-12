import * as CartRenderer from './cart-component/renderer.js';
import * as CartHandler from './cart-component/handler.js';

function init() {
    CartRenderer.renderCart();
    document.querySelector('#summary tbody').addEventListener('click', CartHandler.handleClick);
}

init();