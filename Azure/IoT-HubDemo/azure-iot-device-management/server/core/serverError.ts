/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

export class ServerError extends Error {
    status: number;
    innerException: any;
    message: string;

    constructor(message?: string, status?: number, innerException?: any) {
        super(message);
        this.message = message;
        this.innerException = innerException;
        this.status = status;
    }
}
