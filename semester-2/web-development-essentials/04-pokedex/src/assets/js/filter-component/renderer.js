import { filters } from "../data/filters.js";
import { getAllTypes } from "../utils/helper.js";

const filterBySize = document.querySelector("#poke-size");
const filterByType = document.querySelector("#poke-type");

export function init() {
    // Add 'isAny, isSmall, isMedium, isLarge' to the filterBySize
    filters.size.forEach((size) => {
        filterBySize.innerHTML += `<option value="${size.value}">${size.label}</option>`;
    });
    // Add all types to the filterByType
    filterByType.innerHTML += `<option value="isAny">isAny</option>`;
    getAllTypes().forEach((type) => {
        filterByType.innerHTML += `<option value="${type}">${type}</option>`;
    });
}