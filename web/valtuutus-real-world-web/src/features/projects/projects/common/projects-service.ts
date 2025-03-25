import {useAuthState} from "@/core/state/auth-state-context.tsx";
import {ProjectInfo, ProjectTaskInfo} from "@/features/projects/projects/common/project-state.tsx";

const API_URL = import.meta.env.VITE_API_URL;

export const useProjectService = () => {
    const {token} = useAuthState();

    async function getProjectById(id: string): Promise<ProjectInfo> {
        const res = await fetch(`${API_URL}/projects/${id}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            }
        })

        return await res.json();
    }

    async function getProjectTasks(id: string): Promise<ProjectTaskInfo[]> {
        const res = await fetch(`${API_URL}/projects/${id}/tasks`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            }
        })

        return await res.json();
    }

    async function updateProjectStatusOrder(projectId: string, statusId: string, order: number) {
        await fetch(`${API_URL}/projects/${projectId}/statuses/${statusId}`, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            },
            body: JSON.stringify(order),
        })
    }


    return {
        getProjectById,
        getProjectTasks,
        updateProjectStatusOrder
    }
}