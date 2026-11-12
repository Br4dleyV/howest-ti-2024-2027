const resultsDiv = document.querySelector('#results');
const movieTemplate = document.querySelector('#movie-template');

export function renderMovies(movies) {
    console.log(movies);

    // Render the movies
    resultsDiv.innerHTML = '';
    movies.forEach(renderMovie);
}

function renderMovie(movie) {
    const template = movieTemplate.content.firstElementChild.cloneNode(true);
    const img = template.querySelector('img');
    img.src = movie.Poster;
    img.alt = movie.Title;
    img.title = movie.Title;
    template.querySelector('figcaption').textContent = movie.Title;

    // Append the movie to the list
    resultsDiv.appendChild(template);
}