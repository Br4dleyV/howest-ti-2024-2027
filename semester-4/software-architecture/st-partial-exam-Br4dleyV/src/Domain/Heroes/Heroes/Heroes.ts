import { UUIDEntityId } from '@/Domain/Shared/mod.ts';

export class HeroId extends UUIDEntityId {
    private constructor(value?: string) {
        super(value);
    }

    static create(value?: string): HeroId {
        return new HeroId(value);
    }
}
