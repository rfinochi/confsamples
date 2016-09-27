/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

declare module "azure-iothub" {
    export class Registry {
        static fromConnectionString(connStr: string): Registry;
        get(deviceId: string, done: Function): Device;
        create(device: Device, done: Function): Device;
        delete(deviceId: string, done: Function): any;
        update(device: Device, done: Function): Device;
        queryDevices(query: any, done: Function): Device[];
        setServiceProperties(deviceId: string, serviceProperties: ServiceProperties, done: Function);
        getRegistryStatistics(done: Function);
    }

    export class JobClient {
        static fromConnectionString(connStr: string): JobClient;
        queryJobHistory(jobQuery: any, done: Function): JobResponse[];
        getJob(jobId: string, done: Function): JobResponse;
        scheduleFirmwareUpdate(jobId: string, deviceIds: string[], packageUri: string, timeout: number, done: Function): JobResponse;
        scheduleReboot(jobId: string, deviceIds: string[], done: Function): JobResponse;
        scheduleFactoryReset(jobId: string, deviceIds: string[], done: Function): JobResponse;
        scheduleDevicePropertyRead(jobId: string, deviceIds: string[], propertyName: string, done: Function): JobResponse;
        scheduleDevicePropertyWrite(jobId: string, deviceIds: string[], properties: any[], done: Function): JobResponse;
        scheduleDeviceConfigurationUpdate(jobId: string, deviceIds: string[], value: string, done: Function): JobResponse;
    }

    export class JobResponse {
        jobId: string;
        startTimeUtc: number | Date;
        endTimeUtc: number | Date;
        type: string;
        status: string;
        failureReason: string;
        statusMessage: string;
        deviceId: string;
        parentJobId: string;
    }


    export class Device {
        deviceProperties: DeviceProperties;
        serviceProperties: ServiceProperties;
        authentication: Authentication;
        deviceId: string;
        generationId: string;
        etag: string;
        connectionState: string;
        status: string;
        statusReason: string;
        connectionStateUpdatedTime: string;
        statusUpdatedTime: string;
        lastActivityTime: string;
        cloudToDeviceMessageCount: string;
        isManaged: string;
    }

    interface DeviceProperties {
        batteryLevel: string;
        batteryStatus: string;
        currentTime: string;
        defaultMaxPeriod: string;
        defaultMinPeriod: string;
        deviceDescription: string;
        firmwarePackage: string;
        firmwarePackageName: string;
        firmwarePackageUri: string;
        firmwarePackageVersion: string;
        firmwareUpdateResult: string;
        firmwareUpdateState: string;
        firmwareVersion: string;
        hardwareVersion: string;
        manufacturer: string;
        memoryFree: string;
        memoryTotal: string;
        modelNumber: string;
        registrationLifetime: string;
        serialNumber: string;
        timezone: string;
        utcOffset: string;
    }

    interface ServiceProperties {
        etag: string;
        properties: Object;
        tags: string[];
    }
    interface Authentication {
        symmetricKey: SymmetricKey;
    }

    interface SymmetricKey {
        primaryKey: string;
        secondaryKey: string;
    }
}