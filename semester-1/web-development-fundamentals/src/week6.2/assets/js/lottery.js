"use strict";
document.addEventListener("DOMContentLoaded", init);

function init() {
    document.querySelector("#generate").addEventListener("click", rollLottery);
}

// Keep track of lottery numbers
const currentLotteryNumbers = [];
let previousLotteryNumbers = [];

const amountToRoll = 6;
const minimumNumber = 1;
const maximumNumber = 40;

function getNewLotteryNumbers(min, max) {
    return Math.floor(Math.random() * max) + min;
}

// Rolls 6 new lottery numbers and saves the previous roll
function rollLottery() {
    // Copy current lottery numbers to previous lottery numbers
    previousLotteryNumbers = [...currentLotteryNumbers];
    // Clear current lottery numbers array
    currentLotteryNumbers.length = 0;

    // Generate 6 new numbers and add them to currentLotteryNumbers table
    while (currentLotteryNumbers.length < amountToRoll) {
        const randomNum = getNewLotteryNumbers(minimumNumber, maximumNumber);

        // Check if random number is already in currentLotteryNumbers
        if (!currentLotteryNumbers.includes(randomNum)) {
            currentLotteryNumbers[currentLotteryNumbers.length] = randomNum;
        }
    }

    addNumbersToList();
}

function addNumbersToList() {
    const generatedList = document.querySelector("#generated");
    const previousList = document.querySelector("#previous");

    // Clear numbers in HTML
    generatedList.innerHTML = "";
    previousList.innerHTML = "";

    // Add numbers to HTML
    currentLotteryNumbers.forEach(function (number) {
        generatedList.innerHTML += `<li>${number}</li>`;
    });
    previousLotteryNumbers.forEach(function (number) {
        previousList.innerHTML += `<li>${number}</li>`;
    });
}