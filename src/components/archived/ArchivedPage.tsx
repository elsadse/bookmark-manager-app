import { BookmarkList } from "@/components/home/BookmarkList";
import { Header } from "@/components/home/Header";
import { SideBar } from "@/components/home/SideBar";
import { useFilterTagsContext } from "@/context/FilterTagsContext";
import { useState } from "react";

export function ArchivedPage() {
    const [isMobileSidebarOpen, setIsMobileSidebarOpen] = useState(false)
    const { selectedTagsList } = useFilterTagsContext()

    function getTitleBookmarkList(): string {
        if (selectedTagsList.length === 0) {
            return "Archived bookmarks"
        }
        return "Bookmarks tagged:"
    }

    return (
        <div className="relative flex flex-row">
            <div className="hidden xl:block">
                <SideBar page="Archived" />
            </div>
            {isMobileSidebarOpen && (
                <>
                    <div onClick={() => setIsMobileSidebarOpen(false)}
                        className="absolute right-0 bg-[#131313] opacity-63 h-screen w-screen"
                    />
                    <div className="absolute">
                        <SideBar
                            page="Archived"
                            onClose={() => setIsMobileSidebarOpen(false)}
                            showCloseButton={true}
                        />
                    </div>
                </>
            )}
            <div className="flex flex-col">
                <Header onMenuClick={() => setIsMobileSidebarOpen(true)} />
                <BookmarkList listTitle={getTitleBookmarkList()} isOnArchivedPage={true} />
            </div>
        </div>
    )
}