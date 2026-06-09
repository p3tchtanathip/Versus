export interface ItemResponse {
    id: string;
    name: string;
    imageUrl: string;
    externalId: string | null;
    externalSource: string | null;
    eloRating: number;
    matchCount: number;
}

export interface CreateTierListItemRequest {
    name: string;
    imageUrl?: string;
    externalId?: string | null;
    externalSource?: string | null;
}

export interface ItemEloHistoryResponse {
    itemId: string;
    itemName: string;
    initialRating: number;
    points: EloHistoryGraphPointResponse[];
}

export interface EloHistoryGraphPointResponse {
    playedAt: string;
    ratingBefore: number;
    ratingAfter: number;
    delta: number;
    matchId: string;
}
