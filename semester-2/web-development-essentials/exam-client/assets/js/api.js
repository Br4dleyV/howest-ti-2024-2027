import { TOKEN, API_URL } from "./config.js";
import { renderErrorMessage } from "./util/helper.js";

function fetchData(requestMethod = "GET", route, body = null) {
    const url = `${API_URL}${route}`;
    const options = {
        method: requestMethod,
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${TOKEN}`,
        },
        body: body ? JSON.stringify(body) : null,
    }

    return fetch(url, options)
        .then(response => {
            if (response.ok) return response.json();
        }).then(data => {
            return data;
        }).catch(error => {
            renderErrorMessage(error.message);
            throw error;
        });
}

export { fetchData };