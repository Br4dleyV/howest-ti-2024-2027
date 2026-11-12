import { updateAmountInCart, editLocalStorage } from '../utils/helper.js';

// Adds the item to the cart if add to cart button is clicked
function handleClick(e) {
    if (e.target.tagName === 'BUTTON') { // If click was on button
        // Get tea name
        const parent = e.target.parentElement;

        // Add item to localStorage
        editLocalStorage(parent.dataset.name, "add");

        // Update amount of items currently in the cart
        updateAmountInCart();
    }
}

export { handleClick };