/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, OnInit, ViewChild, ElementRef} from '@angular/core';
import {ControlGroup, Control, FormBuilder} from '@angular/common';
import {GlobalContext, clone, gridConfigEquals} from '../../core/index';
import {AppNav} from '../application.nav/application.nav';
import {CommonMenu} from '../common.menu/common.menu';
import {Grid, GridSource, GridSourceView, GridConfiguration, SelectionStyle} from '@azure-iot/common-ux/grid';
import {Modal} from '@azure-iot/common-ux/modal';
import {SetEditor} from '@azure-iot/common-ux/set-editor';
import {DeviceGridSource} from './device.grid.source';
import {FilterEditor, FilterValue} from '../common.filter/common.filter';
import {Observable, BehaviorSubject, Subject} from 'rxjs/Rx';
import {DataService, ConfigurationService} from '../../services/index';
import {DROPDOWN_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {Http, Response, Headers} from '@angular/http';
import {JobSchedulerForm} from '../job.scheduler.form/job.scheduler.form';
import {JobConfiguration} from './device.grid.job.configuration';
import {GridNavConfigurationDevice} from './device.grid.nav.configuration';
import {GridNavConfigurationDeviceActions} from './device.grid.nav.configuration';
import {HalLinks, HalResponse, HalButton, DeviceGridAvailableFilters, IGridConfiguration, IGridColumn,
IGridFilter, Device, Alert, AlertType, DefaultAlertTimeout, DeviceGridAvailableColumns, Job} from '../../models/index';
import {ServicePropertiesForm} from '../device.service.properties.form/device.service.properties.form';
import {DevicePropertiesForm} from '../device.properties.form/device.properties.form';
import {RouteParams} from '@angular/router-deprecated';
import {Location} from '@angular/common';
import {NgClass} from '@angular/common';
import {Alert as AlertView} from '../common.alert/common.alert';

/**
 * Model Resolution Strategies
 * Note the memoized argument structure
 */
const modelHanders = {
    'default': key => datum => {
        let property = datum[key];
        return !!property ? property.toString() : '';
    },
    'system': key => datum => {
        let property = datum.deviceProperties[key];
        return !!property ? property.value : '';
    },
    'service': key => datum => {
        let property = datum.serviceProperties[key];
        return !!property ? property.toString() : '';
    },
    'custom': key => datum => {
        let property = datum.customProperties[key];
        return !!property ? property.value : '';
    }
};

@Component({
    selector: 'device-grid',
    templateUrl: 'dist/app/components/device.grid/device.grid.html',
    styleUrls: ['dist/app/components/device.grid/device.grid.css'],
    providers: [DeviceGridSource],
    directives: [NgClass, AppNav, SetEditor, CommonMenu, FilterEditor, Grid, Modal, AlertView, DROPDOWN_DIRECTIVES, JobSchedulerForm, ServicePropertiesForm, DevicePropertiesForm]
})
export class DeviceGrid extends GlobalContext implements OnInit {
    private gridNavConfigurationDevice: {};
    private gridNavConfigurationDeviceActions: {};

    public editorVisible: boolean = false;
    public editDeviceConfiguration: boolean = false;
    public editorCollapsed: boolean = true;
    public deviceConfigurationForm: ControlGroup;
    public deviceConfigurationControl: Control;

    public onConfirm: Function = null;
    public onCancel: Function = null;
    public isConfirming: boolean = false;
    public confirmMessage: string;
    public confirmTitle: string;

    public selectedDevices: Device[] = [];
    public disabledGridNavItems = [];
    public gridConfiguration: GridConfiguration<Device>;
    public selectedConfiguration: BehaviorSubject<IGridConfiguration>;
    public availableConfigurations: IGridConfiguration[];

    public jobSchedulerFormItems: Object[];
    public onJobSchedulerSuccess: (params: Object) => void = () => { };
    public schedulerVisible: boolean = false;

    public currentJob: string;
    public currentJobRel: string = '';

    public currentValidRels: HalLinks;
    public relsToRestore: HalLinks;
    public currentActiveNavButton: HalButton<any>;
    public editFormVisible: boolean = false;
    public editLoadingDevice: boolean = false;
    public editLoadingDeviceProperties: boolean = false;

    public deviceDetailsJSON: string;
    public searchDevice: string;
    public deviceConnectionString: string;

    public gridConfigurationUnsaved: boolean = false;

    public currentColumns: BehaviorSubject<IGridColumn[]> = new BehaviorSubject([]);
    public availableColumns: BehaviorSubject<IGridColumn[]> = new BehaviorSubject(DeviceGridAvailableColumns);
    
    public currentFilters: BehaviorSubject<FilterValue<IGridFilter>[]> = new BehaviorSubject([]);
    public availableFilters = DeviceGridAvailableFilters;

    @ViewChild(ServicePropertiesForm) servicePropertiesForm: ServicePropertiesForm;
    @ViewChild(DevicePropertiesForm) devicePropertiesForm: DevicePropertiesForm;

    public editableDevice: Device;

    public refreshCurrentDevice: Function;

    public jobSuccessAlert: Alert;

    constructor(
        public gridSource: DeviceGridSource,
        public gridState: ConfigurationService,
        public dataService: DataService,
        public routeParams: RouteParams,
        public formBuilder: FormBuilder,
        public location: Location
    ) {
        super();
        this.selectedConfiguration = new BehaviorSubject<IGridConfiguration>(this.gridState.CurrentDeviceGridView.value);
        this.gridNavConfigurationDevice = GridNavConfigurationDevice;
        this.gridNavConfigurationDeviceActions = GridNavConfigurationDeviceActions;
        this.deviceConfigurationControl = new Control('');
        this.deviceConfigurationForm = this.formBuilder.group({ 'configuration': this.deviceConfigurationControl });

        this.gridState.AvailableDeviceGridViews.subscribe(availableGridViews => {
            this.onAvailableConfigurationsLoaded(availableGridViews);
        });

        this.gridState.CurrentDeviceGridView.subscribe(currentGridView => {
            this.selectConfiguration(currentGridView);
        });
    }

    public ngOnInit() {
        // for changes between pages
        this.alert = null;
        this.jobSuccessAlert = null;

        let deviceId = this.routeParams.get('deviceId');

        this.gridSource.currentValidRels
            .first()
            .subscribe((gridRels: HalLinks) => {
                if (this.currentValidRels) {
                    // rels already set by edit routing 
                    this.relsToRestore = gridRels;
                } else {
                    this.currentValidRels = gridRels;

                    if (deviceId) {
                        this.findDevice(deviceId);
                    }
                }
            });
        this.currentColumns.skip(1).subscribe(columns => {
            this.selectedConfiguration.value.columns = columns;
            this.gridConfigurationUnsaved = true;
        });
        this.gridSource.subscribeTo(this.selectedConfiguration);    
    }

    private setColumnSelection() {
        this.currentColumns.next(this.selectedConfiguration.value.columns);
    }

    public getColumnLabel(column: IGridColumn | IGridFilter) {
        return column.name;
    }
    
    public getColumnId(column: IGridFilter) {
        return column.model + '.' + column.key;
    }

    private setFilters(): void {
        let filters: FilterValue<IGridFilter>[]
            = this.selectedConfiguration.value.filters.map((filter) => {
                // Note: we currently only support single values, not arrays
                let value = filter.in[0];
                let option = _.find(DeviceGridAvailableFilters, x => x.model === filter.model && x.key === filter.key);
                return { option: option, value: value };
            });
        this.currentFilters.next(filters);
    }

    private onAvailableConfigurationsLoaded(configurations: IGridConfiguration[]): void {
        this.availableConfigurations = configurations;
    }

    public startJob() {
        if (!this.currentJobRel) return;
        this.gridNavAction(new HalButton(
            null,
            null,
            this.currentJobRel,
            'POST'
        ));
    }

    public gridNavAction = (selectedNavAction: HalButton<Device>) => {
        this.alert = null;
        this.jobSuccessAlert = null;

        if (!this.editFormVisible && this.selectedDevices.length < 1) {
            this.alert = new Alert(AlertType.Danger, this.Resources.Common.mustSelectDevices);
            return;
        }

        this.currentActiveNavButton = selectedNavAction;

        this.isConfirming = true;
        switch (selectedNavAction.rel) {
            case 'devices:delete':
                this.relReduce(selectedNavAction.rel);
                this.confirmMessage = this.Resources.ModalPrompts.delete;
                this.confirmTitle = this.Resources.ConfirmDeleteTitle;
                this.onConfirm = this.deleteDevices;
                break;
            case 'jobs:jobFirmware':
                this.isConfirming = false;
                if (this.schedulerVisible) {
                    this.resetUI();
                } else {
                    this.relReduce(selectedNavAction.rel);
                    this.currentJob = this.Resources.Common.firmwareUpdate;
                    this.promptJobParams(this.Resources.ModalPrompts.firmwareUpdate, selectedNavAction, this.currentJob);
                }
                break;
            case 'jobs:jobReboot':
                this.relReduce(selectedNavAction.rel);
                this.confirmMessage = this.Resources.ModalPrompts.reboot;
                this.confirmTitle = this.Resources.ConfirmRebootDevices;
                this.onConfirm = this.scheduleJob;
                this.currentJob = this.Resources.Common.reboot;
                break;
            case 'jobs:jobReset':
                this.relReduce(selectedNavAction.rel);
                this.confirmMessage = this.Resources.ModalPrompts.factoryReset;
                this.confirmTitle = this.Resources.ConfirmFactoryResetDevices;
                this.onConfirm = this.scheduleJob;
                this.currentJob = this.Resources.Common.factoryReset;
                break;
            case 'devices:export':
                this.relReduce(selectedNavAction.rel);
                this.confirmMessage = this.Resources.ModalPrompts.export;
                this.confirmTitle = this.Resources.ConfirmExportDevices;
                this.onConfirm = this.exportDevices;
                break;
            case 'devices:edit':
                this.isConfirming = false;

                if (this.editFormVisible) {
                    this.alert = new Alert(AlertType.Info, this.Resources.DeviceGrid.titleAlertEditNoCanceled);

                    this.resetUI();
                } else {
                    if (this.selectedDevices.length > 1) {
                        this.alert = new Alert(AlertType.Danger, this.Resources.DeviceGrid.titleAlertTooManyDevicesError);
                        return;
                    }

                    this.relReduce(selectedNavAction.rel);
                    this.openEditForm(this.selectedDevices[0].deviceId);
                }
                break;
            default:
                // unhandled
                this.resetUI();
                break;
        }
    };

    public relReduce(rel: string) {
        // this method will reduce the rels for all but the current one
        // this allows the UI to disable actions bound to other rels.
        this.relsToRestore = this.currentValidRels;
        this.currentValidRels = <HalLinks>{};
        this.currentValidRels['self'] = this.relsToRestore['self'];
        this.currentValidRels[rel] = this.relsToRestore[rel];
    }

    public searchKeyPress($event: KeyboardEvent) {
        // enter key
        if ($event.keyCode === 13) {
            this.findDeviceClicked();
        }
    }

    public findDeviceClicked() {
        if (!this.searchDevice) {
            this.alert = new Alert(AlertType.Danger, this.Resources.DeviceGrid.titleAlertEmptySearchError);
            return;
        }

        this.findDevice(this.searchDevice);
    }

    public findDevice(deviceId: string) {
        this.jobSuccessAlert = null;
        this.relReduce('devices:edit');
        deviceId = deviceId.replace(new RegExp('\\s', 'g'), '');
        this.gridSource.select(deviceId);
        this.openEditForm(deviceId);
    }

    public openEditForm(deviceId: string) {

        this.currentActiveNavButton = this.gridNavConfigurationDevice['GridNavButtons'].find(button => button.rel === 'devices:edit');
        this.editableDevice = null;
        this.alert = null;
        this.searchDevice = null;

        this.refreshCurrentDevice = this.loadDevice(deviceId, (err) => {
            // first time we enter edit form for device we have 
            // set up the reduced rels. because we err'd out just reset.
            this.resetUI();
            this.alert = new Alert(AlertType.Danger, this.Resources.DeviceGrid.titleAlertGetDeviceError);
        });

        this.refreshCurrentDevice();
    }

    public loadDevice(deviceId: string, onError: (err) => void): Function {
        return () => {
            this.editLoadingDevice = true;

            return this.dataService.getDevice(deviceId, this.currentValidRels).subscribe(
                (res: HalResponse<Device>) => {
                    // returning rels from the response should also contain a rel to keep 
                    // the current UI state i.e. in this case devices:edit should be included.
                    this.currentValidRels = res.links;
                    this.editableDevice = res.data;

                    // check for null coming back from SDK
                    if (!this.editableDevice.serviceProperties) {
                        this.editableDevice.serviceProperties = <any>{};

                        this.editableDevice.serviceProperties.properties = <any>{};
                        this.editableDevice.serviceProperties.tags = [];
                    }

                    this.deviceConfigurationControl.updateValue(
                        this.editableDevice.deviceProperties
                        && this.editableDevice.deviceProperties.ConfigurationValue
                        && this.editableDevice.deviceProperties.ConfigurationValue.value
                        || '');

                    this.deviceConnectionString = this.editableDevice['_deviceConnectionString'];

                    // make sure we don't leak this later
                    delete this.editableDevice['_deviceConnectionString'];

                    // manually change displayed route
                    this.location.go(`/edit/${this.editableDevice.deviceId}`);

                    this.editLoadingDevice = false;
                    this.editFormVisible = true;
                    this.editorVisible = false;
                    document.body.scrollIntoView();
                }, onError);
        };
    }

    // this is only called by modals
    public confirm() {
        if (this.onConfirm) {
            this.onConfirm();
            this.onConfirm = null;
        }
        this.onCancel = null;
        this.isConfirming = false;
    }

    // this is only called by modals
    public cancel() {
        if (this.onCancel) {
            this.onCancel();
            this.onCancel = null;
        }
        this.onConfirm = null;
        this.isConfirming = false;

        // for all but firmware, resetting the UI is enough to reset the rels
        this.resetUI();
    }

    public promptJobParams(message: string, selectedNavAction: HalButton<Device>, jobName: string) {
        this.alert = null;
        this.editorVisible = false;

        this.jobSchedulerFormItems = JobConfiguration[selectedNavAction.rel];

        this.onJobSchedulerSuccess = (params) => {
            this.isConfirming = true;
            this.confirmMessage = message;
            this.confirmTitle = this.Resources.ConfirmFirmwareUpdateDevices;
            this.onConfirm = () => {
                // the scheduleJob() method will reset the ui as needed
                this.schedulerVisible = false;
                this.scheduleJob(params);
            };
            this.onCancel = () => {
                // here we have to reset the UI but we need to also reset the rels 
                // because we've done a local change. 
                this.onConfirm = null;
                this.isConfirming = false;
                this.resetUI();
            };
        };
        this.schedulerVisible = true;
    }

    public onJobSchedulerCancel() {
        this.resetUI();
    }

    public scheduleJob(params?: { [key: string]: any }) {
        this.dataService.scheduleJob(
            this.currentActiveNavButton.rel,
            this.currentActiveNavButton.method,
            params,
            this.selectedDevices.map(d => d.deviceId),
            this.currentValidRels
        ).subscribe(this.onJobSuccess, this.onJobFailure);
    };

    private onJobSuccess = (res: any) => {
        this.resetUI();
        this.jobSuccessAlert = new Alert(AlertType.Success, null, true, null);
    };

    private onJobFailure = (res: any) => {
        this.resetUI();

        let halResponse: HalResponse<any> = res.json();
        this.alert = new Alert(AlertType.Danger, halResponse._error.message);
    };

    public tagsChanged(updatedDevice: Device): boolean {
        let updatedTags = updatedDevice.serviceProperties.tags,
            editableTags = this.editableDevice.serviceProperties.tags;

        // check if tags are equivalent regardless of order
        return updatedTags.length !== editableTags.length || !updatedTags.every((val, i) => {
            return editableTags.indexOf(val) !== -1;
        });
    }

    public propertiesChanged(updatedDevice: Device): boolean {
        let updatedProperties = updatedDevice.serviceProperties.properties,
            editableProperties = this.editableDevice.serviceProperties.properties,
            updatedPropKeys = Object.keys(updatedProperties),
            editablePropKeys = Object.keys(editableProperties);

        return updatedPropKeys.length !== editablePropKeys.length || !updatedPropKeys.every((val) => {
            return updatedProperties[val] === editableProperties[val];
        });
    }

    public onBulkEditFormSubmit = () => {
        let updatedDevice = this.servicePropertiesForm.toDevice();
        let propertyUpdate = this.devicePropertiesForm.getChanges();
        let id = updatedDevice.deviceId;

        var requests = [];

        if (propertyUpdate.device) {
            requests.push(this.dataService.writeDeviceProperties(id, propertyUpdate.device, this.currentValidRels));
        }

        if (updatedDevice.status !== this.editableDevice.status) {
            requests.push(this.dataService.updateDevice(clone(updatedDevice), this.currentValidRels));
        }

        if (this.tagsChanged(updatedDevice) || this.propertiesChanged(updatedDevice)) {
            requests.push(this.dataService.setServiceProperties(id, updatedDevice.serviceProperties, this.currentValidRels));
        }

        if (requests.length === 0) {
            this.alert = new Alert(AlertType.Info, this.Resources.DeviceGrid.titleAlertEditNoChanges);
            return;
        }

        this.editLoadingDevice = true;

        this.deviceUpdate(requests);
    };

    public onConfigurationSubmit = () => {

        this.editLoadingDevice = true;

        this.deviceUpdate([this.dataService.updateCongifuration(this.editableDevice.deviceId, this.deviceConfigurationControl.value, this.currentValidRels)]);
    };

    public deviceUpdate(requests: Observable<any>[]) {
        Observable.forkJoin(requests)
            .subscribe((responses) => {
                this.resetUI();
                this.alert = new Alert(AlertType.Success, this.Resources.DeviceGrid.titleAlertEditSuccess);
            },
            (err) => {
                this.resetUI();
                this.alert = new Alert(AlertType.Danger, this.Resources.DeviceGrid.titleAlertEditError);
            });
    };

    public onBulkEditFormCancel = () => {
        this.alert = new Alert(AlertType.Danger, this.Resources.DeviceGrid.titleAlertEditNoCanceled);
        this.resetUI();
    };

    public resetUI = () => {

        this.schedulerVisible = false;
        this.currentActiveNavButton = null;

        this.editLoadingDevice = false;
        this.editFormVisible = false;
        this.editDeviceConfiguration = false;

        this.currentJob = null;
        this.searchDevice = null;

        this.currentValidRels = this.relsToRestore;

        // manually clear the location 
        this.location.go('');
    };

    public deleteDevices = () => {
        this.alert = null;
        this.dataService.deleteDevices(this.selectedDevices, this.currentValidRels)
            .subscribe(this.onDeleteDevicesSuccess, this.onDeleteDevicesError);
    };

    public onDeleteDevicesSuccess = (res: any) => {
        // check for partial failures here
        this.selectedDevices = [];
        this.gridSource.update();
        this.resetUI();
        
        let response = res.data;
        if (response._import.error > 0) {
            let msg = this.Resources.Common.partialFailureMessage
                .replace('{action}', this.Resources.Common.delete.toLowerCase())
                .replace('{total}', response._import.total)
                .replace('{error}', response._import.error)
                .replace('{success}', response._import.success);
            this.alert = new Alert(AlertType.Danger, msg);
        }
        else {
            this.alert = new Alert(AlertType.Success, this.Resources.DeviceGrid.titleAlertDeviceDeletedSuccess);
        }
    };

    public onDeleteDevicesError = (res: any) => {
        this.resetUI();
        let halResponse: HalResponse<any> = res.json();
        this.alert = new Alert(AlertType.Danger, halResponse._error.message);
    };

    public exportDevices = () => {
        this.dataService.exportDevices(this.selectedDevices, this.currentValidRels)
            .subscribe(this.onExportDevicesSuccess, this.onExportDevicesError);
    };

    public onExportDevicesSuccess = (res: Response) => {
        let fileName = res.headers.get('Content-Disposition');
        let fileType = res.headers.get('Content-Type');

        let blob = new Blob([res.text()], { type: fileType });
        let url = window.URL.createObjectURL(blob);

        // only way to specify the file name so it can be opened in excel
        let a = <any>document.createElement('a');
        document.body.appendChild(a);

        a.style = 'display: none';
        a.href = url;
        a.download = fileName;
        a.click();

        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);

        this.resetUI();
        this.alert = new Alert(AlertType.Success, this.Resources.DeviceGrid.titleAlertExportSuccess);
    };

    public onExportDevicesError = (res: any) => {
        this.resetUI();
        this.alert = new Alert(AlertType.Danger, this.Resources.DeviceGrid.titleAlertExportFailed);
    };

    private setGridConfiguration(): void {
        this.gridConfiguration = {
            columns: this.selectedConfiguration.value.columns.map(configuredColumn => {
                return {
                    header: () => configuredColumn.name,
                    value: modelHanders[configuredColumn.model](configuredColumn.key),
                    width: configuredColumn.width,
                    sortable: configuredColumn.indexed,
                    key: configuredColumn.key
                };
            }),
            selectionStyle: SelectionStyle.MultiSelect
        };
    }

    public toggleEditor(): void {
        this.editorVisible = !this.editorVisible;
    }

    public updateSelection = selected => {
        this.selectedDevices = selected;
    };

    public applyFilter(): void {
        let filters = this.currentFilters.value.map(filter => {
            if (!filter.option) return;
            filter.option.in = [filter.value];
            return clone(filter.option);
        }).filter(filter => !!filter);
        
        this.selectedConfiguration.value.filters = filters || null;

        if (!gridConfigEquals(this.gridState.CurrentDeviceGridView.value, this.selectedConfiguration.value)) {
            this.gridState.CurrentDeviceGridView.next(this.selectedConfiguration.value);
        }
    }

    public selectConfiguration(configuration: IGridConfiguration): void {
        this.selectedConfiguration.next(clone(configuration));

        this.setColumnSelection();
        this.setFilters();
        this.setGridConfiguration();
    }

    public onViewFilterChange() {
        this.gridConfigurationUnsaved = true;
    }

    public saveConfiguration(): void {
        var configuration = _.find(this.availableConfigurations, config => config.name === this.selectedConfiguration.value.name);
        if (configuration) {
            Object.assign(configuration, this.selectedConfiguration.value);
        }
        else {
            configuration = clone(this.selectedConfiguration.value);
            this.availableConfigurations.push(configuration);
        }

        this.applyFilter();

        this.gridState.AvailableDeviceGridViews.next(this.availableConfigurations);
        this.alert = new Alert(AlertType.Success, this.Resources.DeviceGrid.titleAlertViewSaved, true, 5000);
        this.gridConfigurationUnsaved = false;
    }
}
