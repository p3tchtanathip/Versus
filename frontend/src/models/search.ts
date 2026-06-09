import type { TierListCategory } from "../constants/category";
import type { SportsSearchType } from "../constants/searchType";

export interface SearchRequest {
    query: string;
    category: TierListCategory;
    type?: SportsSearchType;
}

export interface SearchResponse {
    externalId: string | null;
    externalSource: string | null;
    name: string | null;
    imageUrl: string | null;
    metadata: string | null;
}

