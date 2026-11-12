"use strict";

// Don't worry if you don't understand this code yet, all will be explained tomorrow.
document.addEventListener("DOMContentLoaded", init);

function init() {
    // Place all DOM manipulation code here
    const name = document.querySelector("#user-dashboard .name").textContent;
    const age = document.querySelector("#user-dashboard .age").textContent;
    const address = document.querySelector("#user-dashboard .address").textContent;

    document.querySelector("#hacker-dashboard .name").textContent = name;
    document.querySelector("#hacker-dashboard .age").textContent = age;
    document.querySelector("#hacker-dashboard .address").textContent = address;

    //Don't change this line
    document.querySelector("#password").addEventListener("keyup", showPasswordBeingTyped);
}

function showPasswordBeingTyped(e) {

    const passwordHackerElement = document.querySelector("#password-hacker");
    const keyPressed = e.key;

    if (keyPressed === "Backspace") {
        passwordHackerElement.textContent = passwordHackerElement.textContent.slice(0, -1);
    } else {
        passwordHackerElement.textContent += keyPressed;
    }
}