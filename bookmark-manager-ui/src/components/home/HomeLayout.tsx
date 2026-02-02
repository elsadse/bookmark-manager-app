import { type JSX, useState } from "react"
import { Outlet } from "react-router"
import { Header } from "@/components/home/Header"
import { SideBar } from "@/components/home/SideBar"
import { AddBookmark } from "@/components/modals/Modals"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import { useShallow } from "zustand/shallow"
import { ArchiveDialog, DeleteDialog, UnArchiveDialog } from "@/components/dialog/Dialogs"

export function HomeLayout(): JSX.Element {
    const [isMobileSidebarOpen, setMobileSidebarOpen] = useState<boolean>(false)
    const [isAddBookmarkOpen, setIsAddBookmarkOpen] = useState<boolean>(false)
    const { isDialogOpen, setIsDialogOpen, dialogAction } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            isDialogOpen: store.isDialogOpen,
            setIsDialogOpen: store.setIsDialogOpen,
            dialogAction: store.dialogAction
        })))

    return (
        <div className="flex flex-row p-0">
            <div className="max-xl:hidden">
                <SideBar />
            </div>
            <div className={`w-full relative flex flex-col gap-y-2.5 px-0 pt-16.25 md:pt-19.5`}>
                {isMobileSidebarOpen &&
                    <div className="fixed inset-0 z-10 bg-[#131313]/70">
                        <SideBar onClose={() => setMobileSidebarOpen(false)} />
                    </div>
                }
                {isAddBookmarkOpen &&
                    <div className="fixed inset-0 z-10 flex items-center justify-center bg-[#131313]/70">
                        <AddBookmark onClose={() => setIsAddBookmarkOpen(false)} />
                    </div>
                }
                {isDialogOpen && dialogAction === "delete" &&
                    <div className="fixed inset-0 z-10 flex items-center justify-center bg-[#131313]/70">
                        <DeleteDialog onClose={() => setIsDialogOpen(false)} />
                    </div>
                }
                {isDialogOpen && dialogAction === "archive" &&
                    <div className="fixed inset-0 z-10 flex items-center justify-center bg-[#131313]/70">
                        <ArchiveDialog onClose={() => setIsDialogOpen(false)} />
                    </div>
                }
                {isDialogOpen && dialogAction === "unarchive" &&
                    <div className="fixed inset-0 z-10 flex items-center justify-center bg-[#131313]/70">
                        <UnArchiveDialog onClose={() => setIsDialogOpen(false)} />
                    </div>
                }
                <Header onMenuClick={() => setMobileSidebarOpen(true)} onAddClick={() => setIsAddBookmarkOpen(true)} />
                <Outlet />
            </div>
        </div>
    )
}