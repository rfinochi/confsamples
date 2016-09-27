/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

export interface IGridColumn {
    name: string;
    model: 'system' | 'service' | 'default' | 'custom' | 'calculated';
    key: string;
    indexed: boolean;
    width?: number;
}

/**
 * This is the structure of a single 'filter'
 */
export interface IGridFilter {
    name: string;
    model: 'system' | 'service' | 'default' | 'custom';
    key: string;
    in: string[];
    isArray: boolean;
}

/**
 * This is the structure of a single view in the grid.
 * This isn't dynamically updated when the user changes sort order or filtering, but instead represents a 'saved view'.
 */
export interface IGridConfiguration {
    name: string;
    columns: IGridColumn[];
    filters: IGridFilter[];
}