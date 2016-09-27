/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {ServerError} from './serverError';

describe('Server Error Tests', () => {

    let error: ServerError;

    beforeEach(() => {
        error = new ServerError();
    });

    it('Should Construct', () => {
        expect(error).toBeDefined();


    });

    it('Should Construct - toBeTruthy', () => {
        expect(error).toBeTruthy();

    });
});
