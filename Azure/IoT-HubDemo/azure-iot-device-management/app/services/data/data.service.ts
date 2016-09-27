/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {LoggerService} from '../logging/logger.service';
import {Trace} from '@azure-iot/logging/client';
import {Observable, BehaviorSubject, Subject} from 'rxjs/Rx';
import {Job, Device, IGridColumn, IGridFilter, QueryExpression, DeviceProperties, ServiceProperties, HalLinks, HalResponse} from '../../models/index';

const fakeDataCount = 40;

var id = 0;
var jobid = 0;

type DeviceInfo = { deviceId: string };

@Injectable()
export class DataService {

    private _discovery: Observable<HalLinks>;

    private _requestsInFlight: BehaviorSubject<number> = new BehaviorSubject(0);
    public requestsInFlight: Observable<number>;

    constructor(private http: Http, private loggerService?: LoggerService) {
        // Top level RELs for the WebAPI are loaded here. They should never need to 
        // be loaded again unless the WebAPI creates dynamic top level RELs

        this.requestsInFlight = this._requestsInFlight.debounce(function(x) { return Observable.timer(50); });

        this._discovery = this.http.get('/api/discovery')
            .map<HalLinks>(res => {
                var data = res.json();
                return data._links;
            })
            .publishReplay(1)
            .refCount();
    }

    markRequestStart() {
        this._requestsInFlight.next(this._requestsInFlight.value + 1);
    }

    markRequestEnd() {
        this._requestsInFlight.next(this._requestsInFlight.value - 1);
    }

    discovery(): Observable<HalLinks> {
        return this._discovery;
    }

    hateoasApi<T>(rel: string, method: string, currentWorkspaceRels: HalLinks, postBody?: string, templatedPath?: Object): Observable<HalResponse<T>> {
        this.markRequestStart();
        return this.callApi(rel, method, currentWorkspaceRels, postBody, templatedPath)
            .finally(() => {
                this.markRequestEnd();
            })
            .map((res: Response) => {
                var halResponse: any = res.json();
                return { data: halResponse._results ? halResponse._results : halResponse, links: halResponse._links, error: halResponse };
            });
    }

    callApi(rel: string, method: string, currentWorkspaceRels: HalLinks, body?: string, templatedPath?: Object): Observable<Response> {

        return <any>this._discovery.map(discovery => {
            // Here we combine the discovery set of rels with the current workspace rels. This ensures
            // that when we look up a rel it doesn't matter if it's a top level rel or if we are calling
            // a rel that become known from a state change
            let rels = Object.assign({}, currentWorkspaceRels, discovery);
            return templatedPath ? this.constructUrl(rels[rel]['href'], templatedPath) : rels[rel]['href'];
        }).flatMap(url => {

            let opts = new RequestOptions({ method: method });

            if (body) {
                let headers = new Headers();
                headers.append('Content-Type', 'application/json');

                opts.headers = headers;
                opts.body = body;
            }

            return this.http.request(url, opts);
        });
    }

    private constructUrl(url: string, queryString: { [key: string]: any }) {
        var url: string;
        var key: string;
        for (key in queryString) {
            url = url.replace(key, queryString[key]);
        }
        return url;
    }

    addDevice(device: Device, currentWorkspaceRels?: HalLinks): Observable<HalResponse<Device>> {
        let payload = JSON.stringify(device);
        return this.hateoasApi<Device>('devices:add', 'POST', currentWorkspaceRels, payload)
            .map((d: HalResponse<any>) => {
                // Here we massage the original response so that the JSON is created as an object
                return { data: d.data, links: d.links };
            });
    }

    /**
     * Retrieve the list of devices from the discover api, then create a device from them
     */
    getDevicesFromQuery = (skip?: number, count?: number, query?: QueryExpression, currentWorkspaceRels?: HalLinks): Observable<HalResponse<Device[]>> => {
        let payload = query ? JSON.stringify(query) : null;
        return this.hateoasApi<Device[]>('devices:queryGets', 'POST', currentWorkspaceRels, payload)
            .map((d: HalResponse<Device[]>) => {
                // Here we massage the original response so that the JSON is created as objects
                let devices = d.data.map(raw => raw);
                return { data: devices, links: d.links };
            });
    };

    getRegistryStatistics(currentWorkspaceRels?: HalLinks): Observable<HalResponse<any>> {
        return this.hateoasApi<any>('devices:registryStatistics', 'GET', currentWorkspaceRels);
    }

    getDevice(deviceId: string, currentWorkspaceRels?: HalLinks): Observable<HalResponse<Device>> {
        var queryString = { '{Id}': deviceId };
        return this.hateoasApi<Device>('devices:get', 'GET', currentWorkspaceRels, null, queryString);
    }

    deleteDevices(devices: Device[], currentWorkspaceRels?: HalLinks): Observable<HalResponse<DeviceInfo[]>> {
        let payload = JSON.stringify(devices.map(device => device.deviceId));
        return this.hateoasApi<DeviceInfo[]>('devices:delete', 'DELETE', currentWorkspaceRels, payload);
    }

    exportDevices(devices: Device[], currentWorkspaceRels?: HalLinks): Observable<Response> {
        let payload = JSON.stringify(devices.map(device => device));
        return this.callApi('devices:export', 'POST', currentWorkspaceRels, payload);
    }

    updateDevice(device: Device, currentWorkspaceRels?: HalLinks): Observable<HalResponse<DeviceInfo>> {
        return this.hateoasApi<DeviceInfo>('devices:edit', 'PUT', currentWorkspaceRels, JSON.stringify(device));
    }

    writeDeviceProperties(deviceId: string, props: { [key: string]: string }, currentWorkspaceRels?: HalLinks): Observable<HalResponse<Job>> {
        let payload = JSON.stringify({
            deviceIds: [deviceId],
            properties: props
        });

        return this.hateoasApi<Job>('jobs:devicePropWrite', 'POST', currentWorkspaceRels, payload);
    }

    setServiceProperties(deviceId: string, props: ServiceProperties, currentWorkspaceRels?: HalLinks): Observable<HalResponse<ServiceProperties>> {
        let payload = JSON.stringify({
            deviceId: deviceId,
            serviceProperties: props
        });

        return this.hateoasApi<ServiceProperties>('devices:servicePropWrite', 'POST', currentWorkspaceRels, payload);
    }

    updateCongifuration(deviceId: string, config: string, currentWorkspaceRels?: HalLinks): Observable<HalResponse<Job>> {
        let payload = JSON.stringify({
            deviceIds: [deviceId],
            value: config
        });

        return this.hateoasApi<Job>('jobs:jobConfigurationUpdate', 'POST', currentWorkspaceRels, payload);
    }

    getJobs = (skip?: number, count?: number, query?: any, currentWorkspaceRels?: HalLinks): Observable<HalResponse<Job[]>> => {
        let payload = query ? JSON.stringify(query) : null;
        return this.hateoasApi<Job[]>('jobs:history', 'POST', currentWorkspaceRels, payload);
    };

    public scheduleJob(rel: string, method: string, params: { [key: string]: any }, ids: string[], currentWorkspaceRels?: HalLinks): Observable<HalResponse<Job[]>> {
        params = params || {};
        params['deviceIds'] = ids;

        return this.hateoasApi<Job[]>(rel, method, currentWorkspaceRels, JSON.stringify(params), currentWorkspaceRels);
    }

    getJob(jobId: string, currentWorkspaceRels?: HalLinks): Observable<HalResponse<Job>> {
        // refactor: replace templated path additions
        return this.hateoasApi<Job>('jobs:get', 'GET', currentWorkspaceRels, null, { '{Id}': jobId });
    }
}
