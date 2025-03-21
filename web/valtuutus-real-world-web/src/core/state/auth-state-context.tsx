import {createContext, ReactNode, useContext, useEffect, useState} from "react";

export type AuthStateContextType = {
    token?: string;
    onLogin: (accessToken: string) => Promise<void>;
    onLogout: () => Promise<void>;
}

export const AuthStateContext = createContext<AuthStateContextType>({} as AuthStateContextType);

const ACCESS_TOKEN_KEY = "VALTUUTUS_ACCESS_TOKEN_KEY";

export const AuthStateProvider = ({children}: { children: ReactNode | ReactNode[] }) => {
    const [token, setToken] = useState<string>();
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const token = localStorage.getItem(ACCESS_TOKEN_KEY) ?? undefined;
        setToken(token);
        setLoading(false);
    }, [])

    const onLogin = (accessToken: string): Promise<void> => {
        setToken(accessToken);
        localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
        return Promise.resolve()
    }

    const onLogout = (): Promise<void> => {
        return Promise.resolve()
    }

    return (
        loading
            ? <></>
            : <AuthStateContext.Provider value={{token, onLogin, onLogout}}>
                {children}
            </AuthStateContext.Provider>
    )
}

export const useAuthState = () => {
    return useContext(AuthStateContext)
}