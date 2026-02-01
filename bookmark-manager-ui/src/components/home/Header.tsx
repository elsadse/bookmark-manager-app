import iconMenu from "@/assets/images/icon-menu-hamburger.svg"
import iconMenuDark from "@/assets/images/icon-menu-hamburber-dark.svg"
import iconSearch from "@/assets/images/icon-search.svg"
import iconSearchDark from "@/assets/images/icon-search-dark.svg"
import iconAdd from "@/assets/images/icon-add.svg"
import iconAvatar from "@/assets/images/image-avatar.webp"
import iconTheme from "@/assets/images/icon-theme.svg"
import iconThemeLight from "@/assets/images/icon-light-theme.svg"
import iconThemeDark from "@/assets/images/icon-dark-theme.svg"
import iconLogout from "@/assets/images/icon-logout.svg"
import { useState } from "react"
import { useAuthContext } from "@/hooks/useAuthContext"

export function Header({ onMenuClick, onAddClick }: { onMenuClick: () => void, onAddClick:()=>void }) {
    const [isProfileDropdownOpen, setIsProfileDropdownOpen] = useState(false)
    const [searchQuery, setSearchQuery] = useState("")
    //const { searchQuery, setSearchQuery } = useBookmarkList()

    return (
        <div className="absolute top-0 w-full flex justify-center sm:items-start md:justify-between gap-x-2.5 md:gap-x-auto px-4 py-3 md:px-8 md:py-4 bg-neutral-0 dark:bg-neutral-d-800 border border-neutral-300 dark:border-neutral-d-500">
            <div className="flex flex-row justify-center gap-x-2.5 md:gap-x-4">
                <div
                    onClick={onMenuClick}
                    className="flex flex-row justify-center items-center xl:hidden gap-x-1 p-2.5 md:p-3 rounded-8 bg-neutral-0 border border-neutral-400 cursor-pointer dark:hidden">
                    <img src={iconMenu} className="w-5 h-5" alt="icon menu" />
                </div>
                <div
                    onClick={onMenuClick}
                    className="flex flex-row justify-center items-center xl:hidden gap-x-1 p-2.5 md:p-3 rounded-8 bg-neutral-d-800 border border-neutral-d-400 cursor-pointer hidden dark:block">
                    <img src={iconMenuDark} className="w-5 h-5" alt="icon menu" />
                </div>
                <div className={`flex flex-row justify-center items-center dark:bg-neutral-d-500
                    gap-x-1.5 md:gap-x-2 md:p-3 border border-neutral-300 dark:border-neutral-d-400
                    rounded-8 cursor-pointer hover:bg-neutral-100 
                   ${searchQuery.length > 0 ? "ring ring-teal-700" : ""} `}>
                    <img src={iconSearch} className="w-5 h-5 dark:hidden" alt="icon search" />
                    <img src={iconSearchDark} className="w-5 h-5 hidden dark:block" alt="icon search" />
                    <input type="text" value={searchQuery} onChange={(e) => setSearchQuery(e.target.value)}
                        className="placeholder:text-preset-4-md focus:outline-none" placeholder="Search by title..."
                    />
                </div>
            </div>
            <div className="flex flex-row justify-center items-center gap-x-2.5 md:gap-4">
                <button onClick={onAddClick}
                    className="flex justify-center items-center 
                    gap-1 p-2.5 md:px-4 md:py-3 rounded-8 bg-teal-700 
                    border-none dark:border border-neutral-d-400 cursor-pointer ring ring-teal-700">
                    <img src={iconAdd} className="w-5 h-5" alt="icon add" />
                    <span className="hidden md:inline text-neutral-0 text-preset-3 text-center md:px-0.5">Add Bookmark</span>
                </button>
                <div onClick={() => setIsProfileDropdownOpen(!isProfileDropdownOpen)}
                    className={`flex justify-center gap-x-1.25 rounded-45.45 cursor-pointer ${isProfileDropdownOpen ? "rounded-full ring ring-teal-700" : ""}`}>
                    <img src={iconAvatar} className="h-10 w-10" alt="icon avatar" />
                </div>
                {isProfileDropdownOpen && <ProfileMenuDropdown />}
            </div>
        </div>
    )
}

export function ProfileMenuDropdown() {
    const { logout } = useAuthContext()

    return (
        <div className="absolute top-15 right-8 md:top-17 
            w-62 flex flex-col gap-y-4 rounded-8 bg-neutral-0 border 
            border-neutral-100 z-20">
            <div className="flex flex-row items-center gap-x-3 px-4 py-3 border-b border-[#E9EAEB]">
                <div className="flex items-center gap-x-1.25 rounded-45.45 ">
                    <img src={iconAvatar} className="h-10 w-10" alt="icon avatar" />
                </div>
                <div className="flex flex-col">
                    <span className="text-preset-4 text-neutral-900">Emily Carter</span>
                    <span className="text-preset-4-md text-neutral-800">emily101@email.com</span>
                </div>
            </div>
            <div className="flex items-center justify-between flex-row  px-4">
                <div className="flex items-center flex-row gap-x-2">
                    <img src={iconTheme} className="size-4" alt="icon theme" />
                    <span className="text-preset-4 text-neutral-800">Theme</span>
                </div>
                <div className="flex flex-row p-0.5 rounded-4 bg-neutral-300 border border-neutral-300 cursor-pointer hover:ring ring-teal-700">
                    <div className="flex justify-center items-center px-2 py-1.5 rounded-4 bg-neutral-0">
                        <img src={iconThemeLight} className="size-3.5" alt="icon light theme" />
                    </div>
                    <div className="flex justify-center items-center px-2 py-1.5 rounded-4 bg-neutral-300">
                        <img src={iconThemeDark} className="size-3.5" alt="icon light dark" />
                    </div>
                </div>
            </div>
            <div onClick={logout}
                className="flex flex-row gap-x-2 px-4 py-3 border-t border-[#E9EAEB] cursor-pointer">
                <img src={iconLogout} className="size-4" alt="icon logout" />
                <span className="text-preset-4 text-neutral-800">Logout</span>
            </div>
        </div>
    )
}