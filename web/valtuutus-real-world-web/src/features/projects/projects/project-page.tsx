import { Separator } from "@/components/ui/separator";
import { SidebarTrigger } from "@/components/ui/sidebar";
import {KanbanBoard} from "@/features/projects/projects/components/kanban/kanban-board.tsx";
import {Breadcrumb, BreadcrumbItem, BreadcrumbList, BreadcrumbPage} from "@/components/ui/breadcrumb.tsx";
import {House} from "lucide-react";


export const ProjectPage = () => {
    return (
        <div className="w-full flex flex-col overflow-hidden relative">
            <header
                className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-[[data-collapsible=icon]]/sidebar-wrapper:h-12">
                <div className="flex items-center gap-2 px-4">
                    <SidebarTrigger className="-ml-1"/>
                    <Separator orientation="vertical" className="mr-2 h-4"/>
                    <Breadcrumb>
                        <BreadcrumbList>
                            <BreadcrumbItem>
                                <BreadcrumbPage
                                    className="relative flex cursor-default select-none items-center gap-2 rounded-sm px-2 py-1.5 text-sm outline-none transition-colors focus:bg-accent focus:text-accent-foreground data-[disabled]:pointer-events-none data-[disabled]:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0">
                                    <House/>
                                    Página inicial
                                </BreadcrumbPage>
                            </BreadcrumbItem>
                        </BreadcrumbList>
                    </Breadcrumb>
                </div>
            </header>
            <div className="mt-8 sm:mt-0 flex flex-col sm:px-8 pb-4 w-full">
                <div className="p-4">
                    <h1>Hello from project page</h1>
                    {/*<div className="flex items-center justify-between gap-4 w-full flex-wrap">*/}
                    {/*</div>*/}
                </div>
            </div>

            {/*<div className="flex-1">*/}
                <KanbanBoard/>
            {/*</div>*/}
        </div>
    )
}