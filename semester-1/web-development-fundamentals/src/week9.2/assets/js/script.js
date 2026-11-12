"use strict";
document.addEventListener("DOMContentLoaded", init)

const _PEOPLE = ["Jane Doe", "Johnny Fever", "Jack Brick", "James McQueen", "Randall Wise", "Lydia Flynn", "Rebecca Heart", "Nick Balthar", "Joan Sloan", "Lili Creed", "Walter White"]
const _SEATS_CONFIG = {
    vip: {seats: [], amountOfSeats: 5},
    regular: {seats: [], amountOfSeats: 10},
    budget: {seats: [], amountOfSeats: 15},
};
let _selectedPerson;

function init() {
    fillPeople(_PEOPLE);
    generateSeats(_SEATS_CONFIG)

    document.querySelector('.people').addEventListener('click', selectPerson)
    document.querySelector('aside').addEventListener('click', allocateSeat)
}

function fillPeople(people) {
    const peopleHtmlElement = document.querySelector(".people");

    people.forEach(person => {
        peopleHtmlElement.insertAdjacentHTML('beforeend', `<li>${person}</li>`);
    })
}

function generateSeats(seatsConfig) {
    Object.entries(seatsConfig).forEach(([key, value]) => {
        const ulElement = document.querySelector(`.${key}`);

        for (let i = 1; i <= value.amountOfSeats; i++) {
            ulElement.insertAdjacentHTML('beforeend', `<li>seat available</li>`)
        }
    })
}

function selectPerson(e) {
    const peopleHtmlElement = document.querySelector(".people");

    peopleHtmlElement.childNodes.forEach(li => {
        if (li.nodeName === "LI" && li !== e.target) {
            li.classList.remove("selected");
        }
    });
    e.target.classList.add("selected");

    _selectedPerson = e.target;
}

function allocateSeat(e) {
    if (e.target.nodeName === "LI") {
        // Get the selected seat class
        const type = e.target.parentElement.className
        const seats = _SEATS_CONFIG[type].seats;

        // If no person is selected, cancel the function
        if (!_selectedPerson) {
            return;
        }

        // Check if the seat is free
        if (seats.includes(e.target)) {
            console.log("Already assigned");
        } else {
            // Add the seat to the seat list
            seats.push(e.target);
            e.target.textContent = _selectedPerson.textContent;
        }

        // And if the same person is selected and you click on the same thing again, it goes back to available
    }
}