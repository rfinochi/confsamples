/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

export interface HalLinks {
    self: HalLink;
    curies: Array<HalLink>;
}

export interface HalLink {
    href: string;
    templated: boolean;
    name: string;
}

export interface HalResponse<T> {
    data: T;
    links: HalLinks;
    _error?: any;
}