import { StringValueObject } from '../Shared/StringValueObject.ts';

export class ToolId extends StringValueObject {
    constructor(value: string) {
        super(value);
        if (!value || value.length === 0) {
            throw new Error('ToolId cannot be empty');
        }
    }
}
