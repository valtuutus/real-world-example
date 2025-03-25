import {useAuthState} from "@/core/state/auth-state-context.tsx";
import {ProjectInfo} from "@/features/projects/projects/common/project-state.tsx";

const API_URL = import.meta.env.VITE_API_URL;

export type MoveTaskData = {
    newOrder: number;
    newStatusId: string;
}

export const useTaskService = () => {
    const {token} = useAuthState();

    async function moveTask(projectId: string, id: string, data: MoveTaskData): Promise<ProjectInfo> {
        const res = await fetch(`${API_URL}/projects/${projectId}/tasks/${id}/move`, {
            method: "POST",
            body: JSON.stringify(data),
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            }
        })

        return await res.json();
    }

    return {moveTask};
}