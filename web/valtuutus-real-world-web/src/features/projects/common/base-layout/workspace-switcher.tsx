import {useEffect, useState} from "react"
import {ChevronsUpDown, GalleryVerticalEnd, Plus} from "lucide-react"

import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuShortcut,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {SidebarMenu, SidebarMenuButton, SidebarMenuItem, useSidebar,} from "@/components/ui/sidebar"
import {useWorkspaceState, WorkspaceInfo} from "@/features/projects/common/state/workspace-state-context.tsx";
import {Avatar, AvatarFallback} from "@/components/ui/avatar.tsx";
import {useWorkspaceService} from "@/features/projects/common/workspace-service.ts";

export function WorkspaceSwitcher() {
    const {isMobile} = useSidebar()
    const {workspace, changeWorkspace} = useWorkspaceState()
    const [workspaces, setWorkspaces] = useState<WorkspaceInfo[]>([])
    const {getWorkspaces} = useWorkspaceService()

    useEffect(() => {
        getWorkspaces()
            .then(setWorkspaces)
    }, []);

    if (!workspace) {
        return null
    }

    return (
        <SidebarMenu>
            <SidebarMenuItem>
                <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                        <SidebarMenuButton
                            size="lg"
                            className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground"
                        >
                            <Avatar
                                className="aspect-square size-10 rounded-lg">
                                <AvatarFallback
                                    className="aspect-square size-10 rounded-lg bg-sidebar-primary/50 text-sidebar-primary-foreground">
                                    <GalleryVerticalEnd className="size-4"/>
                                </AvatarFallback>
                            </Avatar>
                            <div className="grid flex-1 text-left text-sm leading-tight">
                                <span className="truncate font-semibold">
                                    {workspace?.name}
                                </span>
                                {/*<span className="truncate text-xs">{company?.slug}</span>*/}
                            </div>
                            <ChevronsUpDown className="ml-auto"/>
                        </SidebarMenuButton>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent
                        className="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
                        align="start"
                        side={isMobile ? "bottom" : "right"}
                        sideOffset={4}
                    >
                        <DropdownMenuLabel className="text-xs text-muted-foreground">
                            Workspaces
                        </DropdownMenuLabel>
                        {workspaces.map((workspace, index) => (
                            <DropdownMenuItem
                                key={workspace.name}
                                onClick={() => changeWorkspace(workspace)}
                                className="gap-2 p-2"
                            >
                                <Avatar
                                    className="aspect-square size-6 rounded-sm">
                                    <AvatarFallback
                                        className="aspect-square size-6 rounded-sm bg-sidebar-primary/50 text-sidebar-primary-foreground">
                                        <GalleryVerticalEnd className="size-4"/>
                                    </AvatarFallback>
                                </Avatar>
                                {workspace.name}
                                <DropdownMenuShortcut>⌘{index + 1}</DropdownMenuShortcut>
                            </DropdownMenuItem>
                        ))}
                        <DropdownMenuSeparator/>
                        <DropdownMenuItem className="gap-2 p-2">
                            <div className="flex size-6 items-center justify-center rounded-md border bg-background">
                                <Plus className="size-4"/>
                            </div>
                            <div className="font-medium text-muted-foreground">Add workspace</div>
                        </DropdownMenuItem>
                    </DropdownMenuContent>
                </DropdownMenu>
            </SidebarMenuItem>
        </SidebarMenu>
    )
}
