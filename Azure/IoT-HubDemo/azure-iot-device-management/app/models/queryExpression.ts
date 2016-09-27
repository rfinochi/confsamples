/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

class QueryExpression {
    project: ProjectionExpression;
    aggregate: AggregationExpression;
    sort: SortExpression[] = [];
    filter: FilterExpression;
}

class FilterExpression {
    type: string;
}

class LogicalExpression extends FilterExpression {
    type: string = FilterExpressionType.logical;
    constructor(public logicalOperator: string, public filters: FilterExpression[]) {
        super();
    }
}

class ComparisonExpression extends FilterExpression {
    type: string = FilterExpressionType.comparison;

    constructor(public property: QueryProperty,
        public value: Object,
        public comparisonOperator: string) {
        super();
    }
}

class SortExpression {
    constructor(public order: string, public property: QueryProperty) { }
}

class AggregationExpression {
    constructor(public keys: QueryProperty[], public properties: AggregationProperty[]) { }
}

class ProjectionExpression {
    constructor(public all: boolean, public properties: QueryProperty[]) { }
}

class AggregationProperty {
    constructor(public operator: string, public property: QueryProperty, public columnName: string) { }
}

class QueryProperty {
    constructor(public name: string, public type?: string) { }
}

let PropertyType = {
    default: 'default',
    system: 'system',
    custom: 'custom',
    service: 'service',
    aggregated: 'aggregated'
};

let AggregationOperatorType = {
    sum: 'sum',
    avg: 'avg',
    count: 'count',
    min: 'min',
    max: 'max'
};

let SortOrder = {
    asc: 'asc',
    desc: 'desc'
};

let FilterExpressionType = {
    comparison: 'comparison',
    logical: 'logical'
};

let ComparisonOperatorType = {
    Equals: 'eq',
    NotEquals: 'ne',
    GreaterThan: 'gt',
    GreaterThanEquals: 'gte',
    LessThan: 'lt',
    LessThanEquals: 'lte',
    In: 'in',
    NotIn: 'nin',
    All: 'all'
};

let LogicalOperatorType = {
    or: 'or',
    and: 'and',
    not: 'not'
};

export {LogicalOperatorType, LogicalExpression, AggregationOperatorType, AggregationProperty, AggregationExpression, ProjectionExpression, SortOrder, SortExpression, QueryExpression, FilterExpression, ComparisonExpression, ComparisonOperatorType, QueryProperty};