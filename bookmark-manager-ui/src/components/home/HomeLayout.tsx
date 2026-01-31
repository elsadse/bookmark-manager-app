import { type JSX, useRef, useState } from "react"
import { Outlet } from "react-router"
import { Header } from "@/components/home/Header"
import { useCloseModal } from "@/hooks/useCloseModal"
import { SideBar } from "@/components/home/SideBar"

export function HomeLayout(): JSX.Element {
    const ref = useRef<HTMLDivElement>(null)
    const [isMobileSidebarOpen, setMobileSidebarOpen] = useState<boolean>(false)

    useCloseModal(ref, (): void => setMobileSidebarOpen(false))

    return (
        <div className="flex flex-row p-0">
            <div className="max-xl:hidden">
                <SideBar/>
            </div>
            <div className={`w-full relative flex flex-col gap-y-2.5 px-0 pt-16.25 md:pt-19.5`} ref={ref}>
                {
                    isMobileSidebarOpen
                    && <div className="absolute top-0 left-0 z-10" ref={ref}>
                        <SideBar onClose={() => setMobileSidebarOpen(false)}/>
                    </div>
                }
                <div className={isMobileSidebarOpen ? "opacity-50" : ""}>
                    <Header onMenuClick={() => setMobileSidebarOpen(true)}/>
                    <Outlet/>
                </div>
            </div>
        </div>
    )
}