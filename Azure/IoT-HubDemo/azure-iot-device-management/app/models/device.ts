/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

export class Device {
    deviceProperties: DeviceProperties;
    serviceProperties: ServiceProperties;
    authentication: Authentication;
    configuration: string;
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

    constructor() {
        this.deviceProperties = new DeviceProperties();
        this.serviceProperties = new ServiceProperties();
        this.authentication = new Authentication();
    }
}

export class DeviceProperties {
    BatteryLevel: { value: string };
    BatteryStatus: { value: string };
    CurrentTime: { value: string };
    DefaultMaxPeriod: { value: string };
    DefaultMinPeriod: { value: string };
    DeviceDescription: { value: string };
    FirmwarePackage: { value: string };
    FirmwarePackageName: { value: string };
    FirmwarePackageUri: { value: string };
    FirmwarePackageVersion: { value: string };
    FirmwareUpdateResult: { value: string };
    FirmwareUpdateState: { value: string };
    FirmwareVersion: { value: string };
    HardwareVersion: { value: string };
    Manufacturer: { value: string };
    MemoryFree: { value: string };
    MemoryTotal: { value: string };
    ModelNumber: { value: string };
    RegistrationLifetime: { value: string };
    SerialNumber: { value: string };
    Timezone: { value: string };
    UtcOffset: { value: string };
    ConfigurationValue: { value: string };
}

export class ServiceProperties {
    etag: string;
    properties: Object;
    tags: string[];

    constructor() {
        this.properties = {};
        this.tags = [];
    }
}

export class Authentication {
    symmetricKey: SymmetricKey;
    constructor() {
        this.symmetricKey = new SymmetricKey();
    }
}

export class SymmetricKey {
    primaryKey: string;
    secondaryKey: string;
}