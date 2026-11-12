import { DomainException, ValueObject } from '@/Domain/Shared/mod.ts';

export class InvalidIntelligenceSkillException extends DomainException {
    constructor(value: number) {
        super(`Intelligence skill must be an integer between 0 and 10. Received: ${value}.`);
    }
}

export class IntelligenceSkill extends ValueObject {
    static readonly MIN = 0;
    static readonly MAX = 10;

    private readonly _value: number;

    private constructor(value: number) {
        super();
        this._value = value;
    }

    static create(value: number): IntelligenceSkill {
        const intelligence = new IntelligenceSkill(value);
        intelligence.validate();

        return intelligence;
    }

    private validate(): void {
        const isInRange = this._value >= IntelligenceSkill.MIN &&
            this._value <= IntelligenceSkill.MAX;
        if (!Number.isInteger(this._value) || !isInRange) {
            throw new InvalidIntelligenceSkillException(this._value);
        }
    }

    get value(): number {
        return this._value;
    }

    equals(other: IntelligenceSkill): boolean {
        return this._value === other._value;
    }
}
