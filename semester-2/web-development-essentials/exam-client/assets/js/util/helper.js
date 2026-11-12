function renderErrorMessage(message) {
    const errorElement = document.querySelector('#errors');
    errorElement.innerHTML = `<p>Error: ${message}</p>`;
}

function renderResults(report) {
    const feedbackElement = document.querySelector('#feedback');
    feedbackElement.textContent = report;
}

export { renderErrorMessage, renderResults };