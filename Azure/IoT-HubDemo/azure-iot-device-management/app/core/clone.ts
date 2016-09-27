/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/**
 * This is a simple deep clone operation
 */
export function clone<T>(sourceObject: T): T {
    return JSON.parse(JSON.stringify(sourceObject));
}