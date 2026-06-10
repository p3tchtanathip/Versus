import { create } from 'zustand';
import type { TierListResponse, TierListQueryRequest, PaginatedList } from '../models/tierList';
import type { CreateTierListRequest } from '../models/tierList';
import type { ItemResponse, CreateTierListItemRequest } from '../models/item';
import api from '../lib/api';

interface TierListState {
    lists: PaginatedList<TierListResponse> | null;
    currentList: TierListResponse | null;
    query: TierListQueryRequest;
    loading: boolean;
    error: string | null;

    fetchLists: () => Promise<void>;
    fetchMyLists: () => Promise<void>;
    fetchById: (id: string) => Promise<void>;
    fetchByIdIfNeeded: (id: string) => Promise<void>;
    create: (req: CreateTierListRequest) => Promise<TierListResponse>;
    deleteList: (id: string) => Promise<void>;
    setQuery: (partial: Partial<TierListQueryRequest>) => void;

    addItem: (tierListId: string, req: CreateTierListItemRequest) => Promise<ItemResponse>;
    deleteItem: (tierListId: string, itemId: string) => Promise<void>;
}

export const useTierListStore = create<TierListState>((set, get) => ({
    lists: null,
    currentList: null,
    query: {},
    loading: false,
    error: null,

    fetchLists: async () => {
        set({ loading: true, error: null });
        try {
            const lists = await api.get<PaginatedList<TierListResponse>>("/lists", get().query);
            set({ lists, loading: false });
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
        }
    },

    fetchMyLists: async () => {
        set({ loading: true, error: null });
        try {
            const lists = await api.get<PaginatedList<TierListResponse>>("/lists/me", get().query);
            set({ lists, loading: false });
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
        }
    },

    fetchById: async (id: string) => {
        set({ loading: true, error: null });
        try {
            const currentList = await api.get<TierListResponse>(`/lists/${id}`);
            set({ currentList, loading: false });
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
        }
    },

    fetchByIdIfNeeded: async (id: string) => {
        const { currentList } = get();
        if (currentList?.id === id) return;
        return get().fetchById(id);
    },

    create: async (req: CreateTierListRequest) => {
        set({ loading: true, error: null });
        try {
            const list = await api.post<TierListResponse>("/lists", req);
            set({ loading: false });
            return list;
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
            throw err;
        }
    },

    deleteList: async (id: string) => {
        set({ loading: true, error: null });
        try {
            await api.delete(`/lists/${id}`);
            const { lists } = get();
            if (lists) {
                const items = lists.items.filter((l) => l.id !== id);
                set({ lists: { ...lists, items }, loading: false });
            } else {
                set({ loading: false });
            }
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
        }
    },

    setQuery: (partial: Partial<TierListQueryRequest>) => {
        set((state) => ({ query: { ...state.query, ...partial } }));
    },

    addItem: async (tierListId: string, req: CreateTierListItemRequest) => {
        set({ loading: true, error: null });
        try {
            const item = await api.post<ItemResponse>(`/lists/${tierListId}/items`, req);
            set({ loading: false });
            return item;
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
            throw err;
        }
    },

    deleteItem: async (tierListId: string, itemId: string) => {
        set({ loading: true, error: null });
        try {
            await api.delete(`/lists/${tierListId}/items/${itemId}`);
            set({ loading: false });
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error",
            });
        }
    },
}));
