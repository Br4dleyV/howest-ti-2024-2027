import { DomainException, ValueObject } from '@/Domain/Shared/mod.ts';

export type HeroAlignementValue = 'hero' | 'villain' | 'neutral';

export class InvalidHeroAlignementException extends DomainException {
    constructor(value: string) {
        super(
            `Hero alignement must be one of hero, villain, neutral. Received: '${value}'.`,
        );
    }
}

export class HeroAlignement extends ValueObject {
    private static readonly ALLOWED: readonly HeroAlignementValue[] = [
        'hero',
        'villain',
        'neutral',
    ];

    private readonly _value: HeroAlignementValue;

    private constructor(value: HeroAlignementValue) {
        super();
        this._value = value;
    }

    static create(value: string): HeroAlignement {
        const normalized = value.trim().toLowerCase();
        if (!HeroAlignement.ALLOWED.includes(normalized as HeroAlignementValue)) {
            throw new InvalidHeroAlignementException(value);
        }

        return new HeroAlignement(normalized as HeroAlignementValue);
    }

    static hero(): HeroAlignement {
        return new HeroAlignement('hero');
    }

    static villain(): HeroAlignement {
        return new HeroAlignement('villain');
    }

    static neutral(): HeroAlignement {
        return new HeroAlignement('neutral');
    }

    get value(): HeroAlignementValue {
        return this._value;
    }

    override toString(): string {
        return this._value;
    }

    equals(other: HeroAlignement): boolean {
        return this._value === other._value;
    }
}
