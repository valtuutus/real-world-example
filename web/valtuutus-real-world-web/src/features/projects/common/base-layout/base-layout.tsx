import {SidebarInset, SidebarProvider,} from "@/components/ui/sidebar"
import {AppSidebar} from "@/features/projects/common/base-layout/app-sidebar.tsx";
import {Outlet} from "react-router";

export const BaseLayout = () => {
    return (
        <SidebarProvider className="h-dvh">
            <AppSidebar/>
            <SidebarInset className="flex h-dvh overflow-hidden">
                <Outlet/>
            </SidebarInset>
        </SidebarProvider>
    )
}