import { DomainException, ValueObject } from '@/Domain/Shared/mod.ts';
import { IntelligenceSkill } from './IntelligenceSkill.ts';
import { SpeedSkill } from './SpeedSkill.ts';
import { StrengthSkill } from './StrengthSkill.ts';

export class InvalidSkillsException extends DomainException {
    constructor(message: string) {
        super(message);
    }
}

export class Skills extends ValueObject {
    private readonly _strength: StrengthSkill;
    private readonly _speed: SpeedSkill;
    private readonly _intelligence: IntelligenceSkill;

    private constructor(
        strength: StrengthSkill,
        speed: SpeedSkill,
        intelligence: IntelligenceSkill,
    ) {
        super();
        this._strength = strength;
        this._speed = speed;
        this._intelligence = intelligence;
    }

    static create(
        strength: StrengthSkill,
        speed: SpeedSkill,
        intelligence: IntelligenceSkill,
    ): Skills {
        if (!strength || !speed || !intelligence) {
            throw new InvalidSkillsException(
                'Skills require strength, speed and intelligence value objects.',
            );
        }

        return new Skills(strength, speed, intelligence);
    }

    get strength(): StrengthSkill {
        return this._strength;
    }

    get speed(): SpeedSkill {
        return this._speed;
    }

    get intelligence(): IntelligenceSkill {
        return this._intelligence;
    }

    get power(): number {
        const normalizedSpeed = (this._speed.value - SpeedSkill.MIN) /
            (SpeedSkill.MAX - SpeedSkill.MIN);

        const normalizedIntelligence = (this._intelligence.value - IntelligenceSkill.MIN) /
            (IntelligenceSkill.MAX - IntelligenceSkill.MIN);

        return (this._strength.value * normalizedSpeed) +
            (this._strength.value * normalizedIntelligence);
    }

    equals(other: Skills): boolean {
        return this._strength.equals(other._strength) &&
            this._speed.equals(other._speed) &&
            this._intelligence.equals(other._intelligence);
    }
}
