import { type JSX, useRef, useState } from "react"
import { Outlet } from "react-router"
import { Header } from "@/components/home/Header"
import { useCloseModal } from "@/hooks/useCloseModal"
import { SideBar } from "@/components/home/SideBar"
import { AddBookmark } from "@/components/modals/Modals"

export function HomeLayout(): JSX.Element {
    const ref = useRef<HTMLDivElement>(null)
    const [isMobileSidebarOpen, setMobileSidebarOpen] = useState<boolean>(false)
    const [isAddBookmarkOpen, setIsAddBookmarkOpen] = useState<boolean>(false)

    useCloseModal(ref, () => {
        setMobileSidebarOpen(false)
        setIsAddBookmarkOpen(false)
    })

    return (
        <div className="flex flex-row p-0">
            <div className="max-xl:hidden">
                <SideBar />
            </div>
            <div className={`w-full relative flex flex-col gap-y-2.5 px-0 pt-16.25 md:pt-19.5`} ref={ref}>
                {isMobileSidebarOpen &&
                    <div className="fixed inset-0 z-10 bg-[#131313]/70" ref={ref}>
                        <SideBar onClose={() => setMobileSidebarOpen(false)} />
                    </div>
                }
                {isAddBookmarkOpen &&
                    <div className="fixed inset-0 z-10 flex items-center justify-center bg-[#131313]/70" ref={ref}>
                        <AddBookmark onClose={() => setIsAddBookmarkOpen(false)} />
                    </div>
                }
                <Header onMenuClick={() => setMobileSidebarOpen(true)} onAddClick={() => setIsAddBookmarkOpen(true)} />
                <Outlet />
            </div>
        </div>
    )
}