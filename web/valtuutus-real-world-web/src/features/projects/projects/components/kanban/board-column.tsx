import {SortableContext, useSortable} from "@dnd-kit/sortable";
import {useDndContext} from "@dnd-kit/core";
import {CSS} from "@dnd-kit/utilities";
import {useMemo} from "react";

import {cva} from "class-variance-authority";
import {ScrollArea, ScrollBar} from "@/components/ui/scroll-area.tsx";
import {TaskCard} from "@/features/projects/projects/components/kanban/task-card.tsx";
import {ProjectStatusInfo, ProjectTaskInfo} from "@/features/projects/projects/common/project-state.tsx";
import {cn} from "@/lib/utils.ts";


export type ColumnType = "Column";

export interface ColumnDragData {
    type: ColumnType;
    column: ProjectStatusInfo;
}

interface BoardColumnProps {
    column: ProjectStatusInfo;
    tasks: ProjectTaskInfo[];
    isOverlay?: boolean;
}

export function BoardColumn({column, tasks, isOverlay}: BoardColumnProps) {
    const tasksIds = useMemo(() => {
        return tasks.map((task) => task.id);
    }, [tasks]);

    const {
        setNodeRef,
        attributes,
        listeners,
        transform,
        transition,
        isDragging,
    } = useSortable({
        id: column.id,
        data: {
            type: "Column",
            column,
        } satisfies ColumnDragData,
        attributes: {
            roleDescription: `Column: ${column.name}`,
        },
    });

    const style = {
        transition,
        transform: CSS.Translate.toString(transform),
    };

    const variants = cva(
        "min-h-[300px] w-[350px] max-w-full bg-primary-foreground flex flex-col flex-1 snap-center",
        {
            variants: {
                dragging: {
                    default: "border-2 border-transparent",
                    over: "ring-2 opacity-30",
                    overlay: "ring-2 ring-primary",
                },
            },
        }
    );

    return (
        <div
            ref={setNodeRef}
            {...attributes}
            {...listeners}
            style={style}
            className={cn("bg-muted rounded-lg p-4 flex flex-col", variants({
                dragging: isOverlay ? "overlay" : isDragging ? "over" : undefined,
            }))}>
            <h2 className="font-semibold text-lg mb-4 flex items-center justify-between">
                {column.name}
                <span className="bg-primary/10 text-primary rounded-full px-2 py-1 text-xs">
                  {tasks.length}
                </span>
            </h2>

            <ScrollArea className="max-h-[calc(100dvh-260px)] flex flex-grow flex-col pr-2">
                <div className="flex flex-grow flex-col gap-2 p-2">
                    <SortableContext items={tasksIds}>
                        {tasks.map((task) => (
                            <TaskCard key={task.id} task={task}/>
                        ))}
                    </SortableContext>
                </div>
            </ScrollArea>
        </div>
    );
}

export function BoardContainer({children}: { children: React.ReactNode }) {
    const dndContext = useDndContext();

    const variations = cva("px-2 md:px-0 flex lg:justify-center pb-4", {
        variants: {
            dragging: {
                default: "snap-x snap-mandatory",
                active: "snap-none",
            },
        },
    });

    return (
        <ScrollArea
            className={cn("w-full flex-1", variations({
                dragging: dndContext.active ? "active" : "default",
            }))}
        >
            <div className="px-4 flex-1 flex gap-4 items-start flex-row justify-center">
                {children}
            </div>
            <ScrollBar orientation="horizontal"/>
        </ScrollArea>
    );
}
