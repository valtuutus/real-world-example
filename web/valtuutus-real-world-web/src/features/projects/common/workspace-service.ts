import {useAuthState} from "@/core/state/auth-state-context.tsx";
import {WorkspaceInfo} from "@/features/projects/common/state/workspace-state-context.tsx";

export const API_URL = import.meta.env.VITE_API_URL;

export type WorkspaceProjectInfo = {
    id: string;
    name: string;
}

export const useWorkspaceService = () => {
    const {token} = useAuthState();

    async function getWorkspaceById(id: string) : Promise<WorkspaceInfo> {
        const res = await fetch(`${API_URL}/workspaces/${id}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            }
        })

        return await res.json();
    }

    async function getWorkspaceProjects(id: string) : Promise<WorkspaceProjectInfo[]> {
        const res = await fetch(`${API_URL}/workspaces/${id}/projects`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            }
        })

        return await res.json();
    }

    async function getWorkspaces() : Promise<WorkspaceInfo[]> {
        const res = await fetch(`${API_URL}/workspaces`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            }
        })

        return await res.json();
    }

    return {
        getWorkspaceById,
        getWorkspaces,
        getWorkspaceProjects
    }
}