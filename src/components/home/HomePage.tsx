import { BookmarkList } from "@/components/home/BookmarkList";
import { Header } from "@/components/home/Header";
import { SideBar } from "@/components/home/SideBar";

export function HomePage() {
    return (
        <div className="flex flex-row">
            <div className="hidden xl:block">
                <SideBar />
            </div>
            <div className="flex flex-col">
                <Header />
                <BookmarkList listTitle="All bookmarks"/>
            </div>
        </div>
    )
}