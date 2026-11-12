import { ValueObject } from '@/Domain/Shared/mod.ts';
import { InvalidTeamNameException } from '../TeamExceptions.ts';

export class TeamName extends ValueObject {
    private readonly _value: string;

    private constructor(value: string) {
        super();
        this._value = value;
    }

    static create(value: string): TeamName {
        const trimmed = value.trim();
        if (!trimmed) {
            throw new InvalidTeamNameException(value);
        }

        return new TeamName(trimmed);
    }

    get value(): string {
        return this._value;
    }

    override toString(): string {
        return this._value;
    }

    equals(other: TeamName): boolean {
        return this._value === other._value;
    }
}
