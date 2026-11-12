export class UnAuthorizedError extends Error {
    constructor(message: string = 'Unauthorized access') {
        super(message);
        this.name = 'UnAuthorizedError';
        Object.setPrototypeOf(this, UnAuthorizedError.prototype);
    }
}
