import { create } from 'zustand';
import type { NextMatchResponse, MatchResponse, CreateMatchRequest, TierListResultsResponse, Progress, MatchPair } from '../models/match';
import api from '../lib/api';

interface MatchState {
    currentMatch: MatchPair | null;
    history: MatchResponse[];
    results: TierListResultsResponse | null;
    progress: Progress | null;
    loading: boolean;
    resultsLoading: boolean;
    historyLoading: boolean;
    error: string | null;

    fetchNextMatch: (tierListId: string) => Promise<void>;
    submitMatch: (req: CreateMatchRequest) => Promise<MatchResponse>;
    fetchHistory: (tierListId: string) => Promise<void>;
    fetchResults: (tierListId: string) => Promise<void>;
}

export const useMatchStore = create<MatchState>((set) => ({
    currentMatch: null,
    history: [],
    results: null,
    progress: null,
    loading: false,
    resultsLoading: false,
    historyLoading: false,
    error: null,

    fetchNextMatch: async (tierListId: string) => {
        set({ loading: true, error: null });
        try {
            const res = await api.get<NextMatchResponse>(`/lists/${tierListId}/next-match`);
            set({ currentMatch: res.match, progress: res.progress, loading: false });
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
        }
    },

    submitMatch: async (req: CreateMatchRequest) => {
        set({ loading: true, error: null });
        try {
            const match = await api.post<MatchResponse>("/matches", req);
            set({ loading: false });
            return match;
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
            throw err;
        }
    },

    fetchHistory: async (tierListId: string) => {
        set({ historyLoading: true, error: null });
        try {
            const history = await api.get<MatchResponse[]>(`/lists/${tierListId}/history`);
            set({ history, historyLoading: false });
        } catch (err) {
            set({
                historyLoading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
        }
    },

    fetchResults: async (tierListId: string) => {
        set({ resultsLoading: true, error: null });
        try {
            const results = await api.get<TierListResultsResponse>(`/lists/${tierListId}/results`);
            set({ results, resultsLoading: false });
        } catch (err) {
            set({
                resultsLoading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
        }
    },
}));
