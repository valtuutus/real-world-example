import './App.css'
import {Route, Routes} from "react-router";
import {LoginPage} from "@/features/auth/login/page.tsx";
import {HomePage} from "@/features/projects/home/home-page.tsx";
import {AuthStateProvider} from "@/core/state/auth-state-context.tsx";
import {AuthenticatedGuard} from "@/core/guards/authenticated-guard.tsx";
import {BaseLayout} from "@/features/projects/common/base-layout/base-layout.tsx";
import {WorkspaceStateContextProvider} from "@/features/projects/common/state/workspace-state-context.tsx";
import {ProjectPage} from "@/features/projects/projects/project-page.tsx";
import {ProjectStateContextProvider} from "@/features/projects/projects/common/project-state.tsx";

function App() {
    return (
        <AuthStateProvider>
            <Routes>
                <Route path="login" element={<LoginPage/>}/>
                <Route path="" element={<AuthenticatedGuard/>}>
                    <Route path="" element={<WorkspaceStateContextProvider/>}>
                        <Route path="w/:workspaceId">
                            <Route path="" element={<BaseLayout/>}>
                                <Route path="" element={<HomePage/>}/>
                                <Route path="p/:projectId" element={<ProjectStateContextProvider/>}>
                                    <Route path="" element={<ProjectPage/>}/>
                                </Route>
                            </Route>
                        </Route>
                    </Route>
                </Route>
            </Routes>
        </AuthStateProvider>
    )
}

export default App
