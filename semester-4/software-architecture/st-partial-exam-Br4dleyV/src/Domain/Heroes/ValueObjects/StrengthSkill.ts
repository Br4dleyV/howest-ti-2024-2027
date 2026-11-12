import { DomainException, ValueObject } from '@/Domain/Shared/mod.ts';

export class InvalidStrengthSkillException extends DomainException {
    constructor(value: number) {
        super(`Strength skill must be an integer between 0 and 100. Received: ${value}.`);
    }
}

export class StrengthSkill extends ValueObject {
    static readonly MIN = 0;
    static readonly MAX = 100;

    private readonly _value: number;

    private constructor(value: number) {
        super();
        this._value = value;
    }

    static create(value: number): StrengthSkill {
        const strength = new StrengthSkill(value);
        strength.validate();

        return strength;
    }

    private validate(): void {
        const isInRange = this._value >= StrengthSkill.MIN &&
            this._value <= StrengthSkill.MAX;
        if (!Number.isInteger(this._value) || !isInRange) {
            throw new InvalidStrengthSkillException(this._value);
        }
    }

    get value(): number {
        return this._value;
    }

    equals(other: StrengthSkill): boolean {
        return this._value === other._value;
    }
}
