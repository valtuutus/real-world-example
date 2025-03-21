import {useAuthState} from "@/core/state/auth-state-context.tsx";

export interface LoginData {
    email: string
    password: string
}

export type LoginResponse = {
    accessToken: string
}

export const API_URL = import.meta.env.VITE_API_URL;

export const useAuthService = () => {
    const authState = useAuthState();

    async function login(data: LoginData): Promise<void> {
        const res = await fetch(`${API_URL}/users/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (res.ok) {
            const resJson = await res.json();

            await authState.onLogin(resJson.accessToken);
        }
    }

    return {
        login,
    }
}