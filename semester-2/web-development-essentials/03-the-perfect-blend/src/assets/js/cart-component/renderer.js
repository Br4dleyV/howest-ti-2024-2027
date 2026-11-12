import { teas } from '../data/tea.js';
import { updateCartTotalPrice, updateAmountInCart } from '../utils/helper.js';

// Renders the cart with the items stored in the local storage
function renderCart() {
    // Get the cart from the local storage
    const cart = JSON.parse(localStorage.getItem('cart'));

    // Get the template and the target element
    const template = document.querySelector('#cart-template');
    const target = document.querySelector('#summary tbody');

    // If the is not empty, render the cart
    if (cart) {
        Object.keys(cart).forEach(element => { // Go over all entries in the cart
            // Find tea data
            const teaData = teas.find(tea => tea.name === element);

            // Clone template and populate it with data and insert it into the target element
            const cartEntry = template.content.firstElementChild.cloneNode(true);
            populateCartEntry(teaData, cartEntry, cart);
            target.insertAdjacentHTML('beforeend', cartEntry.outerHTML);
        });

        // Update the total price and the amount of items in the cart
        updateCartTotalPrice(cart);
        updateAmountInCart();
    } 
}

// Populates the cart entry with the data
function populateCartEntry(teaData, cartElement, cart) {
    const cells = cartElement.querySelectorAll('td');
    cells[0].textContent = teaData.name;
    cells[1].textContent = `$${teaData.price}`;
    cells[2].textContent = cart[teaData.name];
    cells[3].textContent = `$${teaData.price * cart[teaData.name]}`;

    cartElement.dataset.name = teaData.name;
}

export { renderCart };