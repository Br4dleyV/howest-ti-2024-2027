import { HeroId } from '@/Domain/Heroes/Heroes/mod.ts';
import { AggregateRoot } from '@/Domain/Shared/mod.ts';
import { TeamName } from './ValueObjects/mod.ts';
import { TeamId } from './TeamId.ts';
import {
    TeamMemberAlreadyExistsException,
    TeamMemberNotFoundException,
} from './TeamExceptions.ts';
import { TeamMember } from './TeamMember.ts';
import { MemberAddedToTeamDomainEvent } from './DomainEvents/mod.ts';

export class Team extends AggregateRoot<TeamId> {
    static readonly MIN_READY_MEMBERS = 4;
    static readonly MAX_MEMBERS = 7;
    static readonly MIN_READY_POWER = 500;
    static readonly MAX_POWER = 700;

    private readonly _name: TeamName;
    private _members: TeamMember[];

    private constructor(id: TeamId, name: TeamName, members: TeamMember[]) {
        super(id);
        this._name = name;
        this._members = members;
    }

    static create(id: TeamId, name: string | TeamName, members: TeamMember[] = []): Team {
        const teamName = typeof name === 'string' ? TeamName.create(name) : name;
        return new Team(id, teamName, [...members]);
    }

    get name(): string {
        return this._name.value;
    }

    get members(): ReadonlyArray<TeamMember> {
        return this._members;
    }

    get memberCount(): number {
        return this._members.length;
    }

    get power(): number {
        return this._members.reduce((sum, member) => sum + member.power, 0);
    }

    get isReady(): boolean {
        if (this.memberCount < Team.MIN_READY_MEMBERS || this.memberCount > Team.MAX_MEMBERS) {
            return false;
        }

        if (this.power < Team.MIN_READY_POWER || this.power > Team.MAX_POWER) {
            return false;
        }

        return true;
    }

    addMember(member: TeamMember): void {
        const alreadyExists = this._members.some((m) => m.heroId.equals(member.heroId));
        if (alreadyExists) {
            throw new TeamMemberAlreadyExistsException(member.heroId.toString());
        }

        this._members.push(member);
        this.raise(
            new MemberAddedToTeamDomainEvent(
                this.id.toString(),
                this.name,
                member.heroId.toString(),
                member.name,
            ),
        );
    }

    removeMember(heroId: HeroId): void {
        const memberIndex = this._members.findIndex((member) => member.heroId.equals(heroId));
        if (memberIndex < 0) {
            throw new TeamMemberNotFoundException(heroId.toString());
        }

        this._members.splice(memberIndex, 1);
    }
}
