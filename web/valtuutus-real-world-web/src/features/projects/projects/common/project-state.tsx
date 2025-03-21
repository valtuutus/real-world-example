import {createContext, useContext, useEffect, useState} from "react";
import {Outlet, useParams} from "react-router";
import {useProjectService} from "@/features/projects/projects/common/projects-service.ts";

export enum ProjectStatusType {
    Waiting,
    Active,
    Done,
    Archived
}

export type ProjectStatusInfo = {
    id: string;
    name: string;
    order: number;
    type: ProjectStatusType;
}

export type ProjectInfo = {
    id: string;
    name: string;
    statuses: ProjectStatusInfo[]
}

export type ProjectTaskInfo = {
    id: string;
    name: string;
    order: number;
    statusId: string;
    assignees: string[];
}


export type ProjectStateContextType = {
    project: ProjectInfo;
    tasks: ProjectTaskInfo[];
}

export const ProjectStateContext = createContext<ProjectStateContextType>({} as ProjectStateContextType);


export const ProjectStateContextProvider = () => {
    const [loading, setLoading] = useState<boolean>(true);
    const [project, setProject] = useState<ProjectInfo>();
    const [tasks, setTasks] = useState<ProjectTaskInfo[]>([]);

    const {getProjectById, getProjectTasks} = useProjectService();

    const {projectId} = useParams();

    useEffect(() => {
        getProjectById(projectId!)
            .then((w) => {
                setProject(w);
            })
            .then(() => getProjectTasks(projectId!))
            .then((t) => {
                setTasks(t);
            })
            .then(() => setLoading(false));

    }, []);

    return !loading
        ? (
            <ProjectStateContext.Provider value={{project: project!, tasks}}>
                <Outlet/>
            </ProjectStateContext.Provider>
        )
        : <></>
}

export const useProjectState = () => {
    return useContext(ProjectStateContext);
}