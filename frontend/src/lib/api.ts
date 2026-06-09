import axios from 'axios';
import { getSessionId } from './session';
import type { ApiResponse } from '../models/api';

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5045/api";

const client = axios.create({
    baseURL: API_URL
});

client.interceptors.request.use(async (config) => {
    const sessionId = await getSessionId();
    config.headers['X-Session-Id'] = sessionId;
    return config;
});

client.interceptors.response.use(
    response => response,
    error => {
        if (error.response?.data?.message) {
            throw new Error(error.response.data.message);
        }

        throw error;
    }
);

const api = {
    async get<T>(path: string, params?: object): Promise<T> {
        const { data } = await client.get<ApiResponse<T>>(path, { params });

        if (!data.success) {
            throw new Error(data.message ?? "Unknown error");
        }

        return data.data as T;
    },

    async post<T, TBody = unknown>(
        path: string,
        body: TBody
    ): Promise<T> {
        const { data } = await client.post<ApiResponse<T>>(path, body);

        if (!data.success) {
            throw new Error(data.message ?? "Unknown error");
        }

        return data.data as T;
    },

    async delete(path: string): Promise<void> {
        const { data } = await client.delete<ApiResponse<null>>(path);

        if (!data.success) {
            throw new Error(data.message ?? "Unknown error");
        }
    }
};

export default api;