import { NotFoundDomainException, DomainException } from '@/Domain/Shared/mod.ts';

export class InvalidTeamNameException extends DomainException {
    constructor(value: string) {
        super(`Team name cannot be empty. Received: '${value}'.`);
    }
}

export class InvalidTeamMemberNameException extends DomainException {
    constructor(value: string) {
        super(
            `Team member name must be between 5 and 20 characters (ignoring spaces). Received: '${value}'.`,
        );
    }
}

export class TeamMemberAlreadyExistsException extends DomainException {
    constructor(heroId: string) {
        super(`Team member with hero id '${heroId}' already exists.`);
    }
}

export class TeamMemberNotFoundException extends NotFoundDomainException {
    constructor(heroId: string) {
        super(`Team member with hero id '${heroId}' was not found.`);
    }
}