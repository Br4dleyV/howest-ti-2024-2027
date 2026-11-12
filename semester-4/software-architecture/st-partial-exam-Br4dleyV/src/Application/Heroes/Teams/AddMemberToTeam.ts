import { HeroId } from '../../../Domain/Heroes/Heroes/mod.ts';
import { TeamId } from '../../../Domain/Heroes/Teams/TeamId.ts';
import { TeamMember } from '../../../Domain/Heroes/Teams/TeamMember.ts';
import { TeamRepository } from '../../../Domain/Heroes/Teams/TeamRepository.ts';
import { HeroAlignement, IntelligenceSkill, Skills, SpeedSkill, StrengthSkill } from '../../../Domain/Heroes/ValueObjects/mod.ts';
import { UnitOfWork, UseCase } from '../../Ports/mod.ts';

export interface AddMemberToTeamInput {
    teamId: string;
    heroId: string;
    memberName: string;
    alignment: 'hero' | 'villain' | 'neutral';
    strength: number;
    speed: number;
    intelligence: number;
}

export class AddMemberToTeamUseCase implements UseCase<AddMemberToTeamInput> {
    constructor(private readonly uow: UnitOfWork) {}

    async execute(input: AddMemberToTeamInput): Promise<void> {
        await this.uow.do(async () => {
            const teamRepository = this.uow.getRepository<TeamRepository>('Team');

            const teamId = TeamId.create(input.teamId);
            const team = (await teamRepository.byId(teamId)).orElseThrow(
                () => new Error("Team not found"),
            );

            const member = TeamMember.create(
                // heroId: HeroId, name: string, alignment: HeroAlignement, skills: Skills
                HeroId.create(input.heroId),
                input.memberName,
                HeroAlignement.create(input.alignment),
                Skills.create(
                    StrengthSkill.create(input.strength),
                    SpeedSkill.create(input.speed),
                    IntelligenceSkill.create(input.intelligence),
                ),
            );

            team.addMember(member);
            await this.uow.save(team);
        });
    }
}