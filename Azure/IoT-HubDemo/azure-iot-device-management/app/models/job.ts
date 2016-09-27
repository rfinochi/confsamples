/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

// This class can be deprecated if JobResponse class from node SDK comes with typings  
export class Job {
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
  
  