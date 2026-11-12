import { crewMembers, missions } from "../data.js";
import { findMissionByCode } from "../utils/helper.js";

export function renderMissionTypes() {
    const $missionTypes = document.querySelector("#mission-type");

    missions.forEach(mission => {
        const option = `<option value="${mission.code}" data-difficulty="${mission.difficulty}">${mission.code} ${mission.title}</option>`;
        $missionTypes.insertAdjacentHTML("beforeend", option);
    });
}

export function renderMissionTypeDetails() {
    const missionType = document.querySelector("#mission-type").value;
    
    const mission = findMissionByCode(missionType);

    const $missionDescription = document.querySelector("#mission-description");
    $missionDescription.innerHTML = `
        <strong>${mission.code}</strong> ${mission.description}<br>
        <em>Difficulty level: ${mission.difficulty} stars</em>
    `;

    $missionDescription.classList.remove("hidden");
}

export function renderAvailableCrewMembers() {
    const $crewConfiguration = document.querySelector("#crew-configuration");

    crewMembers.forEach(crewMember => renderCrewMember($crewConfiguration, crewMember));
}

function renderCrewMember($crewConfiguration, crewMember) {
    const li = `<li>
                    <input type="checkbox" id="${crewMember.code}" value="${crewMember.code}" data-strength="${crewMember.strength}" data-endurance="${crewMember.endurance}">
                    <label for="${crewMember.code}" title="Strength: ${crewMember.strength} - endurance: ${crewMember.endurance}">${crewMember.name}</label>
                </li>`;

    $crewConfiguration.insertAdjacentHTML("beforeend", li);
}

export function renderMissionReport(report) {
    const $report = document.querySelector("#mission-report");
    $report.querySelector("#mission-report-text").textContent = report.report;
    $report.classList.remove("hidden");
}