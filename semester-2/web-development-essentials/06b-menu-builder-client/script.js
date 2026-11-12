function init() {
    fetch('https://06-menu-builder-server.test/api/menu/random').then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json();
    }).then(data => {
        console.log(data.menu);
        Object.entries(data.menu).forEach(([key, value]) => {
            const element = document.querySelector(`#${key}`);

            element.querySelector('img').src = `https://www.fredericvlummens.be/howest/dishes/${value}.jpg`;
            element.querySelector('p').innerText = value;

            console.log(key, value);
        });
    }).catch(error => {
        console.error('There was a problem with the fetch operation:', error);
    });
}

init();