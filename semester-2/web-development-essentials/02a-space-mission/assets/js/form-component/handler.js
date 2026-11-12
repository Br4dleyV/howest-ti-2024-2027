import { crewMembers, missions } from "../data.js";
import { calculateCrewMembersPower, findCrewMemberByCode, findMissionByCode } from "../utils/helper.js";
import { renderMissionReport } from "./renderer.js";

export function getCheckedCrewMembers() {
    const members = [];

    document.querySelectorAll("#crew-configuration input:checked").forEach(checkbox => {
        const member = findCrewMemberByCode(checkbox.value);
        members.push(member);
    });

    return members;
}

function executeMission(mission, crewMembers) {
    const crewPower = calculateCrewMembersPower(crewMembers);
    const success = crewPower >= mission.difficulty;
    const report = success ?
        `Mission with difficulty ${mission.difficulty} was a success with crew power ${crewPower}!` :
        `Mission with difficulty ${mission.difficulty} failed (crew power ${crewPower} wasn't enough).`;

    return {
        mission: mission,
        crewMembers: crewMembers,
        report: report,
        success: success
    };
}

export function submitMission(e) {
    e.preventDefault();

    const missionType = document.querySelector("#mission-type").value;
    const mission = findMissionByCode(missionType);
    const crewMembers = getCheckedCrewMembers();

    const report = executeMission(mission, crewMembers);

    renderMissionReport(report);
}
