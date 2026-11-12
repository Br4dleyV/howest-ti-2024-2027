"use strict";

document.addEventListener("DOMContentLoaded", function () {
    document.querySelector("form").addEventListener("submit", handleUserInput);
    document.querySelector("section button").addEventListener("click", sortList);
});

let _letters = {};
let sorted = true;

function handleUserInput(e) {
    e.preventDefault();

    _letters = {};

    // Get the word inputted by the user and split it in an array and remove spaces
    const input = document.querySelector("#word").value;
    const wordSplitUp = splitAndFilterSpaces(input);

    // Go through the split up word array and add them correctly to the letters dictionary
    wordSplitUp.forEach(character => {
        const formattedCharacter = character.toLowerCase();
        if (_letters[formattedCharacter]) {
            _letters[formattedCharacter] += 1; // Increment frequency if character exists
        } else {
            _letters[formattedCharacter] = 1; // Initialize frequency to 1 if character is new
        }
    });

    renderList(_letters);
}

function splitAndFilterSpaces(word) {
    const array = [];

    for (const char of word) {
        if (char !== " ") {
            array.push(char);
        }
    }

    return array;
}

function sortList() {
    // Convert the list to objects
    const entries = Object.entries(_letters);
    const sortButton = document.querySelector("section button");

    if (sorted) {
        sorted = false;
        sortButton.textContent = "sort descending";
        entries.sort((a, b) => a[1] - b[1]); // Sort list according to frequencies ascending
    } else {
        sorted = true;
        entries.sort((a, b) => b[1] - a[1]); // Sort but descending
        sortButton.textContent = "sort ascending";
    }

    renderList(Object.fromEntries(entries));
}

function renderList(list) {
    clearList();

    const lettersHTMLList = document.querySelector("#letters");
    const amountHTMLList = document.querySelector("#amount");

    // Go through characters dictionary and list them
    for (const character in list) {
        lettersHTMLList.insertAdjacentHTML("beforeend", `<li>${character}</li>`);
        amountHTMLList.insertAdjacentHTML("beforeend", `<li>${list[character]}</li>`);
    }

    // Show list
    document.querySelector("section").classList.remove("hidden");
}

function clearList() {
    document.querySelector("#letters").innerHTML = "";
    document.querySelector("#amount").innerHTML = "";
}