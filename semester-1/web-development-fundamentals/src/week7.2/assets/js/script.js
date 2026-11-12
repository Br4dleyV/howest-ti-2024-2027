"use strict";

const _drinks = ["Earl Grey Tea", "Coffee", "Beer", "Martini", "Milkshake", "Whiskey", "Hot Chocolate"];
let sortedAsc = false;

// Code that runs when document tree loads
document.addEventListener("DOMContentLoaded", () => {
    fillDrinksList(_drinks, "#dynamic ul");
    fillDrinksList(_drinks, "#sort-later ul");
    fillDrinksList(_drinks, "#find ul");

    document.querySelector("#sort button").addEventListener("click", sortList);
    document.querySelector("#sort-later button").addEventListener("click", sortListLater);
    document.querySelector("#find button").addEventListener("click", filterList);
});

// Fills the first drink list dynamically
function fillDrinksList(drinks, htmlTarget) {
    // Get all lists
    const list = document.querySelector(htmlTarget);
    drinks.forEach(drink => {
        // Add item to all lists
        const className = formatToClassName(drink);
        list.insertAdjacentHTML("beforeend", `<li class="${className}">${drink}</li>`);
    });
}

// Takes a drink name and returns it formatted as a classname to add to an item
function formatToClassName(drink) {
    // Replaces spaces with dashes => " " -> "-"
    return drink.toLowerCase().replace(/\s/g, "-");
}

function sortList() {
    clearList("#sort ul");
    _drinks.sort((a, b) => a.localeCompare(b));
    fillDrinksList(_drinks, "#sort ul");
}

function sortListLater() {
    // First clear list
    clearList("#sort-later ul");

    const button = document.querySelector("#sort-later button");

    // Sort list correctly and change button text
    if (!sortedAsc) {
        sortedAsc = true;
        _drinks.sort((a, b) => a.localeCompare(b));
        button.textContent = "Sort list descending";
    } else {
        sortedAsc = false;
        _drinks.reverse();
        button.textContent = "Sort list ascending";
    }

    fillDrinksList(_drinks, "#sort-later ul");
}

function clearList(htmlTarget) {
    document.querySelector(htmlTarget).innerHTML = "";
}

function filterList() {
    const input = document.querySelector("#find input").value;
    const filteredDrinks = [];

    clearList("#find ul");

    _drinks.forEach(drink => {
        if (containsQuery(drink, input) !== -1) {
            filteredDrinks.push(drink);
        }
    });

    fillDrinksList(filteredDrinks, "#find ul");
}

function containsQuery(drink, input) {
    return drink.toLowerCase().indexOf(input.toLowerCase());
}