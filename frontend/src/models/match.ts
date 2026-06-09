import type { ItemResponse } from './item';

export interface MatchResponse {
    id: string;
    tierListId: string;
    winner: ItemResponse;
    loser: ItemResponse;
    winnerDelta: number;
    loserDelta: number;
    playedAt: string;
}

export interface CreateMatchRequest {
    tierListId: string;
    winnerId: string;
    loserId: string;
}

export interface NextMatchResponse {
    match: MatchPair | null;
    progress: Progress;
}

export interface MatchPair {
    itemA: ItemResponse;
    itemB: ItemResponse;
}

export interface Progress {
    played: number;
    total: number;
}

export interface TierListResultsResponse {
    tiers: TierGroupResponse[];
}

export interface TierGroupResponse {
    tierLabel: string;
    items: TierResultItemResponse[];
}

export interface TierResultItemResponse {
    id: string;
    name: string;
    imageUrl: string;
    externalId: string | null;
    externalSource: string | null;
    eloRating: number;
    matchCount: number;
    tierLabel: string;
}
