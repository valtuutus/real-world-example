import {useSortable} from "@dnd-kit/sortable";
import {CSS} from "@dnd-kit/utilities";
import {Card, CardContent, CardHeader, CardTitle} from "@/components/ui/card";
import {cva} from "class-variance-authority";
import {ProjectTaskInfo} from "@/features/projects/projects/common/project-state.tsx";
import {cn} from "@/lib/utils.ts";


interface TaskCardProps {
    task: ProjectTaskInfo;
    isOverlay?: boolean;
}

export type TaskType = "Task";

export interface TaskDragData {
    type: TaskType;
    task: ProjectTaskInfo;
}

export function TaskCard({task, isOverlay}: TaskCardProps) {
    const {
        setNodeRef,
        attributes,
        listeners,
        transform,
        transition,
        isDragging,
    } = useSortable({
        id: task.id,
        data: {
            type: "Task",
            task,
        } satisfies TaskDragData,
        attributes: {
            roleDescription: "Task",
        },
    });

    const style = {
        transition,
        transform: CSS.Translate.toString(transform),
    };

    const variants = cva("", {
        variants: {
            dragging: {
                over: "ring-2 opacity-30",
                overlay: "ring-2 ring-primary",
            },
        },
    });

    return (
        <Card
            ref={setNodeRef}
            {...attributes}
            {...listeners}
            style={style}
            className={cn("cursor-grab active:cursor-grabbing shadow-sm hover:shadow-md transition-all", variants({
                dragging: isOverlay ? "overlay" : isDragging ? "over" : undefined,
            }))}
        >
            <CardHeader className="p-3 pb-0">
                <CardTitle className="text-base">{task.name}</CardTitle>
            </CardHeader>
            <CardContent className="p-3 pt-2">
                {/*<CardDescription className="text-xs">{task.description}</CardDescription>*/}
            </CardContent>
        </Card>
    );
}
