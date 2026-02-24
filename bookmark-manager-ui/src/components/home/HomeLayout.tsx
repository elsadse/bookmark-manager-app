import { type JSX, useState } from "react"
import { Outlet } from "react-router"
import { Header } from "@/components/home/Header"
import { SideBar } from "@/components/home/SideBar"
import { AddBookmark, EditBookmark } from "@/components/modals/Modals"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import { useShallow } from "zustand/shallow"
import { DeleteDialog, ToggleArchiveDialog } from "@/components/dialog/Dialogs"
import { NotificationContainer } from "@/components/Notification"

export function HomeLayout(): JSX.Element {
    const [isMobileSidebarOpen, setMobileSidebarOpen] = useState<boolean>(false)
    const [isAddBookmarkOpen, setIsAddBookmarkOpen] = useState<boolean>(false)
    const { setIsDialogOrModalOpen, action, bookmarkSelected, setIsNotificationOpen, isNotificationOpen, notificationType } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsDialogOrModalOpen: store.setIsDialogOrModalOpen,
            action: store.action,
            bookmarkSelected: store.bookmarkSelected,
            isNotificationOpen: store.isNotificationOpen,
            setIsNotificationOpen: store.setIsNotificationOpen,
            notificationType: store.notificationType
        })))

    return (
        <div className="flex flex-row p-0 max-h-screen">
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
                    <div className="fixed inset-0 z-20 flex items-center justify-center bg-[#131313]/70">
                        <AddBookmark onClose={() => setIsAddBookmarkOpen(false)} />
                    </div>
                }
                {action === "edit" &&
                    <div className="fixed inset-0 z-10 flex items-center justify-center bg-[#131313]/70">
                        <EditBookmark onClose={() => setIsDialogOrModalOpen(null)} />
                    </div>
                }
                {action === "delete" &&
                    <div className="fixed inset-0 z-10 flex items-center justify-center bg-[#131313]/70">
                        <DeleteDialog onClose={() => setIsDialogOrModalOpen(null)} />
                    </div>
                }
                {action === (bookmarkSelected?.isArchived ? "unarchive" : "archive") &&
                    <div className="fixed inset-0 z-10 flex items-center justify-center bg-[#131313]/70">
                        <ToggleArchiveDialog onClose={() => setIsDialogOrModalOpen(null)} />
                    </div>
                }
                {
                    isNotificationOpen
                    && <div className="fixed top-25 right-10 z-50 animate-slide-in-from-top">
                        <NotificationContainer
                            notificationType={notificationType}
                            closeNotification={() => setIsNotificationOpen(false, null)} />
                    </div>
                }
                <Header onMenuClick={() => setMobileSidebarOpen(true)} onAddClick={() => setIsAddBookmarkOpen(true)} />
                <Outlet />
            </div>
        </div>
    )
}