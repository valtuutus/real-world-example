import {Navigate, Outlet, useLocation} from "react-router";
import {useAuthState} from "@/core/state/auth-state-context.tsx";

export const AuthenticatedGuard = () => {
    const location = useLocation();
    const {token} = useAuthState();

    return (
        token
            ? <Outlet/>
            : <Navigate to={`/login?returnUrl=${encodeURIComponent(location.pathname + location.search)}`}/>
    )
}