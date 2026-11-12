import { teas } from '../data/tea.js';
import { updateAmountInCart } from '../utils/helper.js';

// Renders the teas in the store on page load
function renderTeas() {
    // Get the tea template and target where it needs to go
    const template = document.querySelector('#tea-template');
    const target = document.querySelector('#products .wrapper');

    // Add them all and populate the data
    teas.forEach(element => {
        const tea = template.content.firstElementChild.cloneNode(true);
        populateTea(element, tea);
        target.insertAdjacentHTML('beforeend', tea.outerHTML);
    });

    // Update the amount of items currently in the cart
    updateAmountInCart();
}

// Populates the tea template with the data
function populateTea(teaData, teaElement) {
    teaElement.querySelector('img').src = `./${teaData.imageurl}`;
    teaElement.querySelector('h3').textContent = teaData.name;
    teaElement.querySelector('p').textContent = teaData.description;
    teaElement.querySelector('.price').textContent = `$${teaData.price}`;
    teaElement.querySelector('.category').textContent = teaData.category;
    teaElement.dataset.name = teaData.name;
}

export { renderTeas };