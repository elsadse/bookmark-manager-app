import { BookmarkList } from "@/components/home/BookmarkList";
import { Header } from "@/components/home/Header";
import { SideBar } from "@/components/home/SideBar";

export function HomePage() {
    return (
        <div className="flex flex-row">
            <div className="hidden xl:block">
                <SideBar page="Home"/>
            </div>
            <div className="flex flex-col">
                <Header />
                <BookmarkList listTitle="All bookmarks" isBookmarkPin={true} isBookmarkAichived={false} />
            </div>
        </div>
    )
}