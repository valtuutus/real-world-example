import {createContext, useContext, useEffect, useState} from "react";
import {Outlet, useNavigate, useParams} from "react-router";
import {useWorkspaceService} from "@/features/projects/common/workspace-service.ts";

export type WorkspaceInfo = {
    id: string,
    name: string,
}

export type WorkspaceStateContextType = {
    workspace: WorkspaceInfo
    changeWorkspace: (workspaceInfo: WorkspaceInfo) => void
}

export const WorkspaceStateContext = createContext<WorkspaceStateContextType>({} as WorkspaceStateContextType);

const LAST_WORKSPACE_KEY = "VALTUUTUS_LAST_WORKSPACE"

export const WorkspaceStateContextProvider = () => {
    const navigate = useNavigate();

    const [workspace, setWorkspace] = useState<WorkspaceInfo>();
    const {getWorkspaceById} = useWorkspaceService();

    const {workspaceId} = useParams();

    useEffect(() => {
        if (!workspaceId) {
            const lastWorkspaceStr = localStorage.getItem(LAST_WORKSPACE_KEY);

            if (!lastWorkspaceStr) {
                // TODO: redirect to creat workspace page
                return;
            }

            const lastWorkspace: WorkspaceInfo = JSON.parse(lastWorkspaceStr);

            setWorkspace(lastWorkspace);
            navigate(`/w/${lastWorkspace.id}`);
            return;
        }

        getWorkspaceById(workspaceId)
            .then((w) => {
                setWorkspace(w);
                navigate(`/w/${workspaceId}`);
            });
    }, []);

    const changeWorkspace = (workspaceInfo: WorkspaceInfo) => {
        setWorkspace(workspaceInfo);
        localStorage.setItem(LAST_WORKSPACE_KEY, JSON.stringify(workspaceInfo));
    }

    return workspace != null
        ? (
            <WorkspaceStateContext.Provider value={{workspace, changeWorkspace}}>
                <Outlet/>
            </WorkspaceStateContext.Provider>
        )
        : <></>
}

export const useWorkspaceState = () => {
    return useContext(WorkspaceStateContext);
}