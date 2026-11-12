import { DomainException, ValueObject } from '@/Domain/Shared/mod.ts';

export class InvalidSpeedSkillException extends DomainException {
    constructor(value: number) {
        super(`Speed skill must be an integer between 0 and 20. Received: ${value}.`);
    }
}

export class SpeedSkill extends ValueObject {
    static readonly MIN = 0;
    static readonly MAX = 20;

    private readonly _value: number;

    private constructor(value: number) {
        super();
        this._value = value;
    }

    static create(value: number): SpeedSkill {
        const speed = new SpeedSkill(value);
        speed.validate();

        return speed;
    }

    private validate(): void {
        const isInRange = this._value >= SpeedSkill.MIN && this._value <= SpeedSkill.MAX;
        if (!Number.isInteger(this._value) || !isInRange) {
            throw new InvalidSpeedSkillException(this._value);
        }
    }

    get value(): number {
        return this._value;
    }

    equals(other: SpeedSkill): boolean {
        return this._value === other._value;
    }
}
