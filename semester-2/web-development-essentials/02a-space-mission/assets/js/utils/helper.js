import { crewMembers, missions } from "../data.js";

const _enduranceMultiplier = 1.5;

export function findMissionByCode(code) {
    for (const mission of missions) {
        if (mission.code === code) {
            return mission;
        }
    }
    return null;
}

export function findCrewMemberByCode(code) {
    for (const crewMember of crewMembers) {
        if (crewMember.code === code) {
            return crewMember;
        }
    }
    return null;
}

export function calculateCrewMembersPower(members) {
    let power = 0;

    members.forEach(member => {
        power += calculateCrewMemberPower(member);
    });

    return power;
}

function calculateCrewMemberPower(member) {
    return Math.floor(member.strength + _enduranceMultiplier * member.endurance);
}