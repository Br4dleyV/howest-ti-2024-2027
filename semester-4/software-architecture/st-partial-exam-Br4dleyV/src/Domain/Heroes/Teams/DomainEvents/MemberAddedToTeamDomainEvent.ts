import { EventFQDN, type DomainEvent } from '@/Domain/Shared/mod.ts';

export class MemberAddedToTeamDomainEvent implements DomainEvent {
    static readonly EVENT_FQDN = EventFQDN.create('heroes.teams.member_added_to_team');

    private readonly _occurredOn: Date;

    constructor(
        readonly teamId: string,
        readonly teamName: string,
        readonly heroId: string,
        readonly teamMemberName: string,
    ) {
        this._occurredOn = new Date();
    }

    get FQDN(): EventFQDN {
        return MemberAddedToTeamDomainEvent.EVENT_FQDN;
    }

    get occurredOn(): Date {
        return this._occurredOn;
    }
}
