"use strict";
document.addEventListener("DOMContentLoaded", init);

const activeProtections = [];

function init() {
    // Get HTML elements
    const form = document.querySelector("form");
    const closeOverlayButton = document.querySelector(".close");
    const linkList = document.querySelectorAll("#link-list a");

    // Add event listeners
    form.addEventListener("submit", updateProtections); // Listen for submitting to update protections
    closeOverlayButton.addEventListener("click", function () { // Listen for closing overlay
        toggleOverlay(false);
    });

    // Intercepting click events for all links in #link-list
    for (const link of linkList) {
        // Add click event
        link.addEventListener("click", function (event) {
            event.preventDefault(); // Prevents from going to the page in href attribute

            // If the 'all' protection is activated, prompt overlay with corresponding text
            if (activeProtections.includes("all")) {
                toggleOverlay(true, "All outgoing traffic blocked!", "You have blocked all outgoing traffic, so even though you can't browse the web,\n" +
                    " you'll still be kept safe from harm. Good, right?");
            } else if (activeProtections.includes(link.dataset.status)) {
                // Else if any other protections are on, prompt overlay with corresponding text
                toggleOverlay(true, "An attack was intercepted!", "One of your active shields intercepted the attack. You are safe!");
            } else {
                // Else send the user to the website
                location.assign(link.href);
            }
        });
    }
}

// Updates user protections
function updateProtections(event) {
    // Prevent submit button from sending form
    event.preventDefault();

    // Clear active protections array
    activeProtections.length = 0;

    // Check all checkboxes and add checked protections to array
    const protectionCheckboxes = document.querySelectorAll("form input");
    for (const checkbox of protectionCheckboxes) {
        if (checkbox.checked) {
            activeProtections.push(checkbox.value);
        }
    }

    // Update status message to show which protections are now active
    updateStatusMessage();
}

// Shows/hides the notification overlay with customisable title and text
function toggleOverlay(bool, title, bodyText) {
    // Get HTML overlay element
    const overlay = document.querySelector("#overlay");

    // If bool is true, show overlay and set text
    if (bool) {
        overlay.classList.add("active");
        overlay.querySelector("h2").textContent = title;
        overlay.querySelector("p").textContent = bodyText;
    } else {
        // Else hide the overlay
        overlay.classList.remove("active");
    }
}

// Updates the status message next to form submit button to show which protections are active
function updateStatusMessage() {
    // Get HTML status message element
    const statusMessage = document.querySelector(".status-message");

    // If no protections are activated, show message no shields were activated
    if (activeProtections.length === 0) {
        statusMessage.textContent = "No shields were activated";
    } else {
        // Set status message text to 'Shields active for' concatenated with active protections joined together with &
        statusMessage.textContent = "Shields active for ".concat(activeProtections.join(" & "));
    }
}