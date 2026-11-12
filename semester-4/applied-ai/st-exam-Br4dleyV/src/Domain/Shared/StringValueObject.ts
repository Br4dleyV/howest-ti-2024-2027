import { ValueObject } from './ValueObject.ts';

export abstract class StringValueObject extends ValueObject {
    constructor(public readonly value: string) {
        super();
    }

    equals(other: ValueObject): boolean {
        if (!(other instanceof StringValueObject)) {
            return false;
        }
        return this.value === other.value;
    }
}
