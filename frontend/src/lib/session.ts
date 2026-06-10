const SESSION_KEY = 'sessionId';

let sessionPromise: Promise<string> | null = null;

async function createSession(): Promise<string> {
    const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5045/api";
    const response = await fetch(`${API_URL}/sessions`, { method: 'POST' });

    if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${await response.text()}`);
    }

    const data = await response.json();
    const sessionId: string = data.data;
    localStorage.setItem(SESSION_KEY, sessionId);
    return sessionId;
}

export async function getSessionId(): Promise<string> {
    const stored = localStorage.getItem(SESSION_KEY);
    if (stored) return stored;

    if (!sessionPromise) {
        sessionPromise = createSession();
    }
    return sessionPromise;
}

export function clearSession(): void {
    localStorage.removeItem(SESSION_KEY);
    sessionPromise = null;
}
