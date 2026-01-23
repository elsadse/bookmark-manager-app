import { ArchiveDialog, DeleteDialog, UnArchiveDialog } from "@/components/dialog/Dialogs";
import { BookmarkList } from "@/components/home/BookmarkList";
import { Header } from "@/components/home/Header";
import { SideBar } from "@/components/home/SideBar";
import { AddBookmark } from "@/components/modals/Modals";
import { EditBookmark } from "@/components/modals/Modals";
import { useBookmarkList } from "@/context/BookmarkListContext";
import { useFilterTagsContext } from "@/context/FilterTagsContext";
import { useState } from "react";

export function HomePage() {
    const [isMobileSidebarOpen, setIsMobileSidebarOpen] = useState(false)
    const [isAddBookmarkFormOpen, setIsAddBookmarkFormOpen] = useState(false)
    const [isEditBookmarkFormOpen, setIsEditBookmarkFormOpen] = useState(false)
    const [isArchivedBookmarkDialogOpen, setIsArchivedBookmarkDialogOpen] = useState(false)
    const [isUnArchivedBookmarkDialogOpen, setIsUnArchivedBookmarkDialogOpen] = useState(false)
    const [isDeleteBookmarkDialogOpen, setIsDeleteBookmarkDialogOpen] = useState(false)
    const isDivCenterItems = isAddBookmarkFormOpen || isEditBookmarkFormOpen || isArchivedBookmarkDialogOpen || isUnArchivedBookmarkDialogOpen || isDeleteBookmarkDialogOpen
    const { selectedTagsList } = useFilterTagsContext()
    const { searchQuery } = useBookmarkList()

    function getTitleBookmarkList(): string {
        if (selectedTagsList.length > 0) {
            return "Bookmarks tagged:"
        }
        if (searchQuery.length > 0) {
            return "Results For:"
        }
        return "All Bookmarks"
    }

    return (
        <div className={`relative flex flex-row ${isDivCenterItems ? "justify-center items-center " : ""}`}>
            <div className="hidden xl:block">
                <SideBar page="Home" />
            </div>
            {isMobileSidebarOpen && (
                <>
                    <div onClick={() => setIsMobileSidebarOpen(false)}
                        className="absolute right-0 bg-[#131313] opacity-63 h-screen w-screen"
                    />
                    <div className="absolute">
                        <SideBar
                            page="Home"
                            onClose={() => setIsMobileSidebarOpen(false)}
                            showCloseButton={true}
                        />
                    </div>
                </>
            )}

            {isAddBookmarkFormOpen && <AddBookmark onClose={() => setIsAddBookmarkFormOpen(false)} />}
            {isEditBookmarkFormOpen && <EditBookmark onClose={() => setIsEditBookmarkFormOpen(false)} />}
            {isArchivedBookmarkDialogOpen && <ArchiveDialog onClose={() => setIsArchivedBookmarkDialogOpen(false)} />}
            {isUnArchivedBookmarkDialogOpen && <UnArchiveDialog onClose={() => setIsUnArchivedBookmarkDialogOpen(false)} />}
            {isDeleteBookmarkDialogOpen && <DeleteDialog onClose={() => setIsDeleteBookmarkDialogOpen(false)} />}
            <div className="flex flex-col xl:w-286">
                <Header onMenuClick={() => setIsMobileSidebarOpen(true)} />
                <BookmarkList listTitle={getTitleBookmarkList()} isOnArchivedPage={false} />
            </div>
        </div>
    )
}