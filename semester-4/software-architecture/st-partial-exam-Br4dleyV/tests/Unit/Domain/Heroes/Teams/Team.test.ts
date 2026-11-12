import { assertEquals, assertThrows } from '@std/assert';
import { HeroId } from '@/Domain/Heroes/Heroes/mod.ts';
import {
    HeroAlignement,
    IntelligenceSkill,
    Skills,
    SpeedSkill,
    StrengthSkill,
} from '@/Domain/Heroes/ValueObjects/mod.ts';
import {
    MemberAddedToTeamDomainEvent,
    Team,
    TeamId,
    TeamMember,
    TeamMemberAlreadyExistsException,
} from '@/Domain/Heroes/Teams/mod.ts';

function createTeamMember(
    heroId: string,
    memberName: string,
    alignment: 'hero' | 'villain' | 'neutral',
    strength: number,
): TeamMember {
    return TeamMember.create(
        HeroId.create(heroId),
        memberName,
        HeroAlignement.create(alignment),
        Skills.create(
            StrengthSkill.create(strength),
            SpeedSkill.create(20),
            IntelligenceSkill.create(10),
        ),
    );
}

Deno.test('Team - addMember - valid member - member is added', () => {
    const team = Team.create(
        TeamId.create('11111111-1111-4111-8111-111111111111'),
        'Justice League',
    );

    const member = createTeamMember(
        'aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa',
        'Clark Kent',
        'hero',
        75,
    );

    team.addMember(member);

    assertEquals(team.memberCount, 1);
    assertEquals(team.members[0]?.heroId.toString(), member.heroId.toString());
    assertEquals(team.domainEvents.length, 1);

    const event = team.domainEvents[0] as MemberAddedToTeamDomainEvent;
    assertEquals(event.FQDN.toString(), 'heroes.teams.member_added_to_team');
    assertEquals(event.teamId, team.id.toString());
    assertEquals(event.teamName, team.name);
    assertEquals(event.heroId, member.heroId.toString());
    assertEquals(event.teamMemberName, member.name);
});

Deno.test('Team - addMember - duplicate hero id - throws TeamMemberAlreadyExistsException', () => {
    const team = Team.create(
        TeamId.create('22222222-2222-4222-8222-222222222222'),
        'Night Watch',
    );

    const firstMember = createTeamMember(
        'bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb',
        'Bruce Wayne',
        'hero',
        80,
    );
    const duplicateMember = createTeamMember(
        'bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb',
        'Batman Hero',
        'hero',
        70,
    );

    team.addMember(firstMember);

    assertThrows(
        () => {
            team.addMember(duplicateMember);
        },
        TeamMemberAlreadyExistsException,
    );
});

Deno.test('Team - isReady - valid team rules satisfied - returns true', () => {
    const team = Team.create(
        TeamId.create('33333333-3333-4333-8333-333333333333'),
        'Starlight Unit',
    );

    team.addMember(createTeamMember('11111111-aaaa-4aaa-8aaa-aaaaaaaaaaaa', 'Alpha Hero', 'hero', 75));
    team.addMember(createTeamMember('22222222-bbbb-4bbb-8bbb-bbbbbbbbbbbb', 'Bravo Hero', 'hero', 75));
    team.addMember(createTeamMember('33333333-cccc-4ccc-8ccc-cccccccccccc', 'Charlie Hero', 'hero', 75));
    team.addMember(createTeamMember('44444444-dddd-4ddd-8ddd-dddddddddddd', 'Delta Ally', 'neutral', 75));

    assertEquals(team.memberCount, 4);
    assertEquals(team.power, 600);
    assertEquals(team.isReady, true);
});

Deno.test('Team - removeMember - existing member - member removed and readiness recalculated', () => {
    const team = Team.create(
        TeamId.create('44444444-4444-4444-8444-444444444444'),
        'Guardians Core',
    );

    const alphaHeroId = HeroId.create('55555555-aaaa-4aaa-8aaa-aaaaaaaaaaaa');
    const bravoHeroId = HeroId.create('66666666-bbbb-4bbb-8bbb-bbbbbbbbbbbb');
    const charlieHeroId = HeroId.create('77777777-cccc-4ccc-8ccc-cccccccccccc');
    const deltaHeroId = HeroId.create('88888888-dddd-4ddd-8ddd-dddddddddddd');

    team.addMember(createTeamMember(alphaHeroId.toString(), 'Alpha Hero', 'hero', 75));
    team.addMember(createTeamMember(bravoHeroId.toString(), 'Bravo Hero', 'hero', 75));
    team.addMember(createTeamMember(charlieHeroId.toString(), 'Charlie Hero', 'hero', 75));
    team.addMember(createTeamMember(deltaHeroId.toString(), 'Delta Ally', 'neutral', 75));

    assertEquals(team.isReady, true);

    team.removeMember(bravoHeroId);

    assertEquals(team.memberCount, 3);
    assertEquals(team.members.some((member) => member.heroId.equals(bravoHeroId)), false);
    assertEquals(team.isReady, false);
});
