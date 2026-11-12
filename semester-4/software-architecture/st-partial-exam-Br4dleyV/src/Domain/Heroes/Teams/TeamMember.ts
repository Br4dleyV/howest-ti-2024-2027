import { HeroId } from '@/Domain/Heroes/Heroes/mod.ts';
import { HeroAlignement, Skills } from '@/Domain/Heroes/ValueObjects/mod.ts';
import { InvalidTeamMemberNameException } from './TeamExceptions.ts';

export class TeamMember {
    private readonly _heroId: HeroId;
    private readonly _name: string;
    private readonly _alignment: HeroAlignement;
    private readonly _skills: Skills;

    private constructor(
        heroId: HeroId,
        name: string,
        alignment: HeroAlignement,
        skills: Skills,
    ) {
        this._heroId = heroId;
        this._name = name;
        this._alignment = alignment;
        this._skills = skills;
    }

    static create(
        heroId: HeroId,
        name: string,
        alignment: HeroAlignement,
        skills: Skills,
    ): TeamMember {
        TeamMember.validateName(name);

        return new TeamMember(heroId, name.trim(), alignment, skills);
    }

    private static validateName(value: string): void {
        const normalizedLength = value.replace(/\s/g, '').length;
        if (normalizedLength < 5 || normalizedLength > 20) {
            throw new InvalidTeamMemberNameException(value);
        }
    }

    get heroId(): HeroId {
        return this._heroId;
    }

    get name(): string {
        return this._name;
    }

    get alignment(): HeroAlignement {
        return this._alignment;
    }

    get skills(): Skills {
        return this._skills;
    }

    get power(): number {
        return this._skills.power;
    }

    equals(other: TeamMember): boolean {
        return this._heroId.equals(other._heroId) &&
            this._name === other._name &&
            this._alignment.equals(other._alignment) &&
            this._skills.equals(other._skills);
    }
}
