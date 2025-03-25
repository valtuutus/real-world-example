import {useMemo, useState} from "react";
import {createPortal} from "react-dom";

import {
    DndContext,
    type DragEndEvent,
    type DragOverEvent,
    DragOverlay,
    type DragStartEvent,
    KeyboardSensor,
    MouseSensor,
    TouchSensor,
    useSensor,
    useSensors,
} from "@dnd-kit/core";
import {arrayMove, SortableContext} from "@dnd-kit/sortable";

import {hasDraggableData} from "./utils";
import {BoardColumn, BoardContainer} from "./board-column";
import {TaskCard} from "@/features/projects/projects/components/kanban/task-card.tsx";
import {coordinateGetter} from "@/features/projects/projects/components/kanban/multiple-containers-keyboard-preset.ts";
import {
    ProjectStatusInfo,
    ProjectTaskInfo,
    useProjectState
} from "@/features/projects/projects/common/project-state.tsx";
import {useProjectService} from "../../common/projects-service";
import {useTaskService} from "@/features/projects/projects/common/task-service.ts";


export function KanbanBoard() {
    const {project, tasks: projectTasks} = useProjectState();
    const {updateProjectStatusOrder} = useProjectService();
    const {moveTask} = useTaskService();
    const [columns, setColumns] = useState<ProjectStatusInfo[]>(project.statuses);
    const [tasks, setTasks] = useState<ProjectTaskInfo[]>(projectTasks);
    const columnsId = useMemo(() => columns.map((col) => col.id), [columns]);

    const [activeColumn, setActiveColumn] = useState<ProjectStatusInfo | null>(null);

    const [activeTask, setActiveTask] = useState<ProjectTaskInfo | null>(null);

    const sensors = useSensors(
        useSensor(MouseSensor),
        useSensor(TouchSensor),
        useSensor(KeyboardSensor, {
            coordinateGetter: coordinateGetter,
        })
    );


    return (
        <DndContext
            sensors={sensors}
            onDragStart={onDragStart}
            onDragEnd={onDragEnd}
            onDragOver={onDragOver}
        >
            <BoardContainer>
                <SortableContext items={columnsId}>
                    {columns.map((col) => (
                        <BoardColumn
                            key={col.id}
                            column={col}
                            tasks={tasks.filter((task) => task.statusId === col.id)}
                        />
                    ))}
                </SortableContext>
            </BoardContainer>

            {"document" in window &&
                createPortal(
                    <DragOverlay>
                        {activeColumn && (
                            <BoardColumn
                                isOverlay
                                column={activeColumn}
                                tasks={tasks.filter(
                                    (task) => task.statusId === activeColumn.id
                                )}
                            />
                        )}
                        {activeTask && <TaskCard task={activeTask} isOverlay/>}
                    </DragOverlay>,
                    document.body
                )}
        </DndContext>
    );

    function onDragStart(event: DragStartEvent) {
        if (!hasDraggableData(event.active)) return;
        const data = event.active.data.current;
        if (data?.type === "Column") {
            setActiveColumn(data.column);
            return;
        }

        if (data?.type === "Task") {
            setActiveTask(data.task);
            return;
        }
    }

    async function onDragEnd(event: DragEndEvent) {
        setActiveColumn(null);
        setActiveTask(null);

        const {active, over} = event;
        if (!over) return;

        const activeId = active.id;
        const overId = over.id;

        if (!hasDraggableData(active)) return;

        const activeData = active.data.current;

        if (activeId === overId) return;

        const isActiveAColumn = activeData?.type === "Column";
        if (!isActiveAColumn) return;

        const activeColumnIndex = columns.findIndex((col) => col.id === activeId);
        const overColumnIndex = columns.findIndex((col) => col.id === overId);

        const newOrder = calcNewOrder(columns, activeColumnIndex, overColumnIndex);

        const activeStatus = columns[activeColumnIndex];

        const prevColumns = columns;

        const updatedColumns = [...columns.filter(c => c.id !== activeId), {...activeStatus, order: newOrder}]
            .sort((a, b) => a.order - b.order);
        setColumns(updatedColumns);

        await updateProjectStatusOrder(project.id, activeStatus?.id!, newOrder)
            .catch(() => {
                setColumns(prevColumns);
            })
    }

    function onDragOver(event: DragOverEvent) {
        const {active, over} = event;
        if (!over) return;

        const activeId = active.id;
        const overId = over.id;

        if (activeId === overId) return;

        if (!hasDraggableData(active) || !hasDraggableData(over)) return;

        const activeData = active.data.current;
        const overData = over.data.current;

        const isActiveATask = activeData?.type === "Task";
        const isOverATask = overData?.type === "Task";

        if (!isActiveATask) return;

        // Im dropping a Task over another Task
        if (isActiveATask && isOverATask) {
            const activeTask = tasks.find((t) => t.id === activeId)!;
            const overTask = tasks.find((t) => t.id === overId)!;

            const activeIndex = tasks
                .filter((task) => task.statusId === activeTask.statusId)
                .findIndex((t) => t.id === activeId);

            const overIndex = tasks
                .filter((task) => task.statusId === overTask.statusId)
                .findIndex((t) => t.id === overId);


            const newOrder = calcNewOrder(
                tasks.filter(t => t.statusId == overTask.statusId),
                activeIndex,
                overIndex,
                activeTask.statusId == overTask.statusId
            );

            const oldTasks = tasks;

            const updatedTasks = [...tasks.filter(t => t.id !== activeId), {
                ...activeTask,
                order: newOrder,
                statusId: overTask.statusId
            }]
                .sort((a, b) => a.order - b.order);
            setTasks(updatedTasks);

            moveTask(project.id, activeTask.id, {
                newOrder,
                newStatusId: overTask.statusId,
            })
                .catch(() => {
                    setTasks(oldTasks);
                })
        }

        const isOverAColumn = overData?.type === "Column";

        // Im dropping a Task over a column
        if (isActiveATask && isOverAColumn) {
            const activeTask = tasks.find((t) => t.id === activeId)!;

            const columnTasks = tasks
                .filter((task) => task.statusId === (overId as string));
            
            const lastIndex = columnTasks.reduce((last, t) => last >= t.order ? last : t.order, 0)

            const newOrder = calcNewOrder(columnTasks, 0, lastIndex + 1);

            const updatedTasks = [...tasks.filter(t => t.id !== activeId), {
                ...activeTask,
                order: newOrder,
                statusId: (overId as string)
            }]
                .sort((a, b) => a.order - b.order);

            const oldTasks = tasks;
            setTasks(updatedTasks);

            moveTask(project.id, activeTask.id, {
                newOrder,
                newStatusId: overId as string,
            })
                .catch(() => {
                    setTasks(oldTasks);
                })
        }
    }
}

function calcNewOrder(itens: { order: number }[], oldIndex: number, newIndex: number, sameGroup: boolean = true) {
    console.log(itens, oldIndex, newIndex, sameGroup);
    let prevIndex = newIndex - 1;
    let nextIndex = newIndex;

    const isLastIndex = itens.length - 1 === newIndex;

    if (sameGroup && isLastIndex) {
        prevIndex++;
        nextIndex++;
    } else if (sameGroup && newIndex > oldIndex) {
        prevIndex++;
        nextIndex++;
    }

    const prev = itens[prevIndex];
    const next = itens[nextIndex];

    const prevOrder = prev?.order ?? 0;
    const nextOrder = next?.order ?? (prevOrder + 1);

    return (prevOrder + nextOrder) / 2;
}