import { API_URL, token } from "../config.js";
import { renderError } from "../form-component/renderer.js";

function get(url) {
    return fetch(API_URL + url).then(response => {
        if (!response.ok) {
            if (response.status === 401) {
                renderError(new Error("Unauthorized access"));
            } else {
                renderError(new Error("Network response was not ok"));
            }
        }
        return response.json();
    }).then(data => {
        if (!data) {
            renderError(new Error("No data found"));
        }
        return data.data;
    }).catch(error => {
        renderError(new Error("There was a problem with the fetch operation:", error));
        throw error;
    });
}

function post(url, payload) {
    return fetch(API_URL + url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
        },
        body: JSON.stringify(payload),
    }).then(response => {
        if (!response.ok) {
            // If error code is 401,
            if (response.status === 401) {
                renderError(new Error("Unauthorized access"));
            } else {
                renderError(new Error("Network response was not ok"));
            }
        }
        return response.json();
    }).then(data => {
        if (!data) {
            renderError(new Error("No data found"));
        }
        return data;
    }).catch(error => {
        renderError(new Error("There was a problem with the fetch operation:", error));
        throw error;
    });
}

export { get, post };