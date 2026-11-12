import { updateCartTotalPrice, updateAmountInCart, editLocalStorage } from '../utils/helper.js';

// Removes the item from the cart if remove button is clicked
function handleClick(e) {
    if (e.target.tagName === 'BUTTON') { // If click was on button
        // Get tea name
        const parent = e.target.parentElement.parentElement;

        // Remove item from localStorage and remove cart element
        const cart = editLocalStorage(parent.dataset.name, "remove");
        parent.remove();

        // Update amount of items currently in cart and total price
        updateCartTotalPrice(cart);
        updateAmountInCart();
    }
}

export { handleClick };