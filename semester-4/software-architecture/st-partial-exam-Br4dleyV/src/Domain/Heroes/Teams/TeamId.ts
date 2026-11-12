import { UUIDEntityId } from '@/Domain/Shared/mod.ts';

export class TeamId extends UUIDEntityId {
    private constructor(value?: string) {
        super(value);
    }

    static create(value?: string): TeamId {
        return new TeamId(value);
    }
}
