import iconMenu from "@/assets/images/icon-menu-hamburger.svg"
import iconSearch from "@/assets/images/icon-search.svg"
import iconAdd from "@/assets/images/icon-add.svg"
import iconAvatar from "@/assets/images/image-avatar.webp"

export function Header({ onMenuClick }: { onMenuClick: () => void }) {

    return (
        <div className="w-full flex justify-center sm:items-start md:justify-between gap-x-2.5 md:gap-x-auto px-4 py-3 md:px-8 md:py-4 bg-neutral-0 border border-neutral-300">
            <div className="flex flex-row justify-center gap-x-2.5 md:gap-x-4">
                <div
                    onClick={onMenuClick}
                    className="flex flex-row justify-center items-center xl:hidden gap-x-1 p-2.5 md:p-3 rounded-8 bg-neutral-0 border border-neutral-400 cursor-pointer">
                    <img src={iconMenu} className="w-5 h-5" alt="icon menu" />
                </div>
                <div className="flex flex-row justify-center items-center gap-x-1.5 md:gap-x-2 md:p-3 border border-neutral-300 rounded-8">
                    <img src={iconSearch} className="w-5 h-5" alt="icon search" />
                    <input type="text" className="placeholder:text-preset-4-md focus:outline-none" placeholder="Search by title..." />
                </div>
            </div>
            <div className="flex flex-row justify-center items-center gap-x-2.5 md:gap-4">
                <button className="flex justify-center items-center gap-1 p-2.5 md:px-4 md:py-3 rounded-8 bg-teal-700 border-none">
                    <img src={iconAdd} className="w-5 h-5" alt="icon add" />
                    <span className="hidden md:inline text-neutral-0 text-preset-3 text-center md:px-0.5">Add Bookmark</span>
                </button>
                <button className="flex justify-center gap-x-1.25 rounded-45.45 ">
                    <img src={iconAvatar} className="h-10 w-10" alt="icon avatar" />
                </button>
            </div>
        </div>
    )
}