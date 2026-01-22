import { BookmarkList } from "@/components/home/BookmarkList";
import { Header } from "@/components/home/Header";
import { SideBar } from "@/components/home/SideBar";

export function ArchivedPage() {

    return (
        <div className="flex flex-row">
            <div className="hidden xl:block">
                <SideBar page="Archived" />
            </div>
            <div className="flex flex-col">
                <Header />
                <BookmarkList listTitle="Archived bookmarks" isBookmarkPin={false} isBookmarkAichived={true} />
            </div>
        </div>
    )
}