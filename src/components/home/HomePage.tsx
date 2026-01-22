import { BookmarkList } from "@/components/home/BookmarkList";
import { Header } from "@/components/home/Header";
import { SideBar } from "@/components/home/SideBar";
import { useEffect, useState } from "react";

export function HomePage() {
    const [isMobileSidebarOpen, setIsMobileSidebarOpen] = useState(false)

    useEffect(() => {
        if (isMobileSidebarOpen) {
            document.body.style.overflow = 'hidden'
        }
        return () => {
            document.body.style.overflow = 'auto'
        }
    }, [isMobileSidebarOpen])

    return (
        <div className="relative flex flex-row">
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
            <div className="flex flex-col">
                <Header onMenuClick={() => setIsMobileSidebarOpen(true)} />
                <BookmarkList listTitle="All bookmarks" isBookmarkPin={true} isBookmarkAichived={false} />
            </div>
        </div>
    )
}