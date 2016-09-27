import * as server from './server';
import {ServerError} from './core/serverError';
import {Config} from './config';

describe('Server Error Handlers', () => {
    it('handles a 404 by passing onto the main error handler', () => {
        let req = null;
        let res = null;
        let next = function (error: ServerError) {
            expect(error.status).toEqual(404);
            expect(error.message).toEqual('Not Found');
        };
        
        server.error404Handler(req, res, next);
    });
    
    it('handles an uncaught error by deleting the response and sending back 500', () => {
        let err: any = new ServerError('Message', 500);
        let req = null;
        let res: any = {
            status: jasmine.createSpy('status'),
            send: jasmine.createSpy('send')
        };
        let next = null;
        
        server.error500Handler(err, req, res, next);
        
        expect(res.status).toHaveBeenCalledWith(500);
        expect(res.send).toHaveBeenCalledWith({ _error: err });
    });
    
    it('handles an uncaught error by deleting the response', () => {
        let err: any = new ServerError('Message', 500);
        err['response'] = 'test';
        let req = null;
        let res: any = {
            status: jasmine.createSpy('status'),
            send: jasmine.createSpy('send')
        };
        let next = null;
        
        server.error500Handler(err, req, res, next);
        expect(err['response']).toBeUndefined();
    });
    
    it('on initialize, creates an express application', (done) => {
        Config.initialize()
            .then(() => server.initialize())
            .then(app => {
                expect(app).toBeDefined();
                done();
            })
            .catch(err => {
                done.fail(err);
            });
    });
});