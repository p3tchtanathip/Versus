export interface TierListResponse {
    id: string;
    name: string;
    categoryName: string;
    itemCount: number;
    matchCount: number;
    createdAt: string;
}

import type { CreateTierListItemRequest } from './item';

export interface CreateTierListRequest {
    name: string;
    categoryId: number;
    items: CreateTierListItemRequest[];
}

export interface TierListQueryRequest {
    pageNumber?: number;
    pageSize?: number;
    categoryId?: number;
    search?: string;
}

export interface PaginatedList<T> {
    items: T[];
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}