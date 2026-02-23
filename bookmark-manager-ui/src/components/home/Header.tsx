import iconAdd from "@/assets/images/icon-add.svg"
import iconAvatar from "@/assets/images/image-avatar.webp"
import { useRef, useState, type ChangeEvent, type KeyboardEvent } from "react"
import { useAuthContext } from "@/hooks/useAuthContext"
import { useThemeContext } from "@/hooks/useThemeContext"
import { useLocalStorage } from "@/hooks/useLocalStorage"
import type { AuthenticatedUser } from "@/context/AuthContext"
import { useCloseDropdown } from "@/hooks/useCloseDropdown"
import { MenuHamburgerIcon } from "@/components/icons/MenuHamburgerIcon"
import { SearchIcon } from "@/components/icons/SearchIcon"
import { ThemeIcon } from "@/components/icons/ThemeIcon"
import { LightThemeIcon } from "@/components/icons/LightThemeIcon"
import { DarkThemeIcon } from "@/components/icons/DarkThemeIcon"
import { LogoutIcon } from "@/components/icons/LogoutIcon"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import { useShallow } from "zustand/shallow"
import { CloseIcon } from "@/components/icons/CloseIcon"

export function Header({ onMenuClick, onAddClick }: { onMenuClick: () => void, onAddClick: () => void }) {
    const [isProfileDropdownOpen, setIsProfileDropdownOpen] = useState(false)
    const ref = useRef<HTMLDivElement>(null)
    useCloseDropdown(ref, (): void => setIsProfileDropdownOpen(false))

    const [searchTerm, setSearchTerm] = useState("")
    const { setSearchQuery } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setSearchQuery: store.setSearchQuery,
        }))
    )

    function handleSearch(event: KeyboardEvent<HTMLInputElement>) {
        if (event.key === "Enter") {
            event.preventDefault()
            setSearchQuery(searchTerm.toLowerCase())
        }
    }

    function handleChange(event: ChangeEvent<HTMLInputElement>): void {
        setSearchTerm(event.target.value)
        if (event.target.value.length === 0) setSearchQuery("")
    }

    return (
        <div className="absolute top-0 w-full flex justify-between gap-x-2.5 md:gap-x-auto px-4 py-3 md:px-8 md:py-4 bg-neutral-0 dark:bg-neutral-d-800 border border-neutral-300 dark:border-neutral-d-500">
            <div className="flex flex-row justify-center gap-x-2.5 md:gap-x-4">
                <div
                    onClick={onMenuClick}
                    className="flex flex-row justify-center items-center xl:hidden gap-x-1 p-2.5 md:p-3 rounded-8 bg-neutral-0 border border-neutral-400 dark:border-neutral-500 cursor-pointer">
                    <MenuHamburgerIcon className="w-5 h-5" />
                </div>
                <div className={`flex flex-row justify-center items-center dark:bg-neutral-d-500
                    gap-x-1.5 md:gap-x-2 md:p-3 border border-neutral-300 dark:border-neutral-d-400
                    rounded-8 cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-400
                   ${searchTerm.length > 0 ? "ring ring-teal-700 dark:ring-neutral-d-0" : ""} `}>
                    <SearchIcon className="w-5 h-5" />
                    <input type="text" value={searchTerm} onKeyDown={handleSearch} onChange={handleChange}
                        className="placeholder:text-preset-4-md focus:outline-none w-full" placeholder="Search by title..."
                    />
                    {searchTerm.length > 0 && <CloseIcon className="w-5 h-5" onClick={() => { setSearchTerm(""); setSearchQuery("") }} />}
                </div>
            </div>
            <div className="relative flex flex-row justify-center items-center gap-x-2.5 md:gap-4" ref={ref}>
                <button onClick={onAddClick}
                    className="flex justify-center items-center 
                    gap-1 p-2.5 md:px-4 md:py-3 rounded-8 bg-teal-700 hover:bg-teal-800
                    border-none dark:border border-neutral-d-400 cursor-pointer focus:ring ring-teal-700 dark:ring-neutral-d-0">
                    <img src={iconAdd} className="w-5 h-5" alt="icon add" />
                    <span className="hidden md:inline text-neutral-0 dark:text-neutral-d-0 text-preset-3 text-center md:px-0.5">Add Bookmark</span>
                </button>
                <div onClick={() => setIsProfileDropdownOpen(!isProfileDropdownOpen)}
                    className={`flex justify-center gap-x-1.25 rounded-45.45 cursor-pointer ${isProfileDropdownOpen ? "rounded-full ring ring-teal-700 dark:ring-neutral-d-0" : ""}`}>
                    <img src={iconAvatar} className="h-10 w-10" alt="icon avatar" />
                </div>
                {isProfileDropdownOpen && <ProfileMenuDropdown />}
            </div>
        </div>
    )
}

export function ProfileMenuDropdown() {
    const { logout } = useAuthContext()
    const { theme, setTheme } = useThemeContext()
    const { getLocalStorageValue } = useLocalStorage<AuthenticatedUser>("AuthenticatedUser")

    function toggleTheme() {
        setTheme(theme === "dark" ? "light" : "dark")
    }

    return (
        <div className="absolute top-15 right-0 
            w-62 flex flex-col gap-y-4 rounded-8 bg-neutral-0 border 
            border-neutral-100 z-10">
            <div className="flex flex-row items-center gap-x-3 px-4 py-3 border-b border-[#E9EAEB]">
                <div className="flex items-center gap-x-1.25 rounded-45.45 ">
                    <img src={iconAvatar} className="h-10 w-10" alt="icon avatar" />
                </div>
                <div className="flex flex-col">
                    <span className="text-preset-4 text-neutral-900">{getLocalStorageValue()?.fullname}</span>
                    <span className="text-preset-4-md text-neutral-800">{getLocalStorageValue()?.email}</span>
                </div>
            </div>
            <div className="flex items-center justify-between flex-row  px-4">
                <div className="flex items-center flex-row gap-x-2">
                    <ThemeIcon className="w-4 h-4" />
                    <span className="text-preset-4 text-neutral-800">Theme</span>
                </div>
                <div onClick={toggleTheme}
                    className="flex flex-row p-0.5 rounded-4 bg-neutral-300 border border-neutral-300 cursor-pointer hover:ring ring-teal-700 dark:ring-neutral-d-0">
                    <div className={`flex justify-center items-center px-2 py-1.5 rounded-4 ${theme === "light" ? "bg-neutral-0" : "bg-neutral-300"}`}>
                        <LightThemeIcon className="w-3.5 h-3.5" />
                    </div>
                    <div className={`flex justify-center items-center px-2 py-1.5 rounded-4 ${theme === "dark" ? "bg-neutral-0" : "bg-neutral-300"}`}>
                        <DarkThemeIcon className="w-3.5 h-3.5" />
                    </div>
                </div>
            </div>
            <div onClick={logout}
                className="flex flex-row gap-x-2 px-4 py-3 border-t border-[#E9EAEB] cursor-pointer">
                <LogoutIcon className="size-4" />
                <span className="text-preset-4 text-neutral-800">Logout</span>
            </div>
        </div>
    )
}