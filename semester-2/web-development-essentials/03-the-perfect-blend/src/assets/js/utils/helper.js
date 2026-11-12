import { teas } from '../data/tea.js';

// Updates the total price of all items in the cart
function updateCartTotalPrice(cart) {
    // Get total price element
    const totalElement = document.querySelector('#carttotal');
    let totalPrice = 0;

    // Count total price
    Object.keys(cart).forEach(element => {
        const teaData = teas.find(tea => tea.name === element);
        totalPrice += teaData.price * cart[teaData.name];
    });

    // Update total price element label
    totalElement.textContent = `$${totalPrice}`;
}

// Updates the amount of items currently in the cart top right on the page
function updateAmountInCart() {
    // Get viewcart element
    const cartElement = document.querySelector('#viewcart span');

    // Get current cart from localStorage
    const cart = JSON.parse(localStorage.getItem('cart'));
    let amount = 0;

    // Count total amount of items in cart
    if (cart) {
        Object.keys(cart).forEach(element => {
            amount += cart[element];
        });
    }

    // Update total amount of items in cart element label
    cartElement.textContent = amount;
}

// Adds or removes items from the cart in localStorage
function editLocalStorage(teaName, type) {
    // Get cart from localStorage
    const cart = JSON.parse(localStorage.getItem('cart')) || {};

    if (type === "add") { // If wanting to add item to cart
        // Check if item is already in cart and increment, otherwise set to 1
        if (cart[teaName]) {
            cart[teaName]++;
        } else {
            cart[teaName] = 1;
        }
    } else if (type === "remove") { // If wanting to remove item from cart
        delete cart[teaName]; // Remove item from cart
    } else {
        console.log("No type specified");
    }

    // Update localStorage with new cart
    localStorage.setItem('cart', JSON.stringify(cart));
    // Return updated cart
    return cart;
}

export { updateCartTotalPrice, updateAmountInCart, editLocalStorage };