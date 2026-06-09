import { create } from 'zustand';
import type { CategoryResponse } from '../models/category';
import api from '../lib/api';

interface CategoryState {
    categories: CategoryResponse[];
    loading: boolean;
    error: string | null;

    fetchCategories: () => Promise<void>;
}

export const useCategoryStore = create<CategoryState>((set) => ({
    categories: [],
    loading: false,
    error: null,

    fetchCategories: async () => {
        set({ loading: true, error: null });
    
        try {
            const categories = await api.get<CategoryResponse[]>("/categories");
            set({ categories, loading: false });
        } catch (err) {
            set({
                loading: false,
                error: err instanceof Error ? err.message : "Unknown error"
            });
    
            console.error(err);
        }
    },
}));
