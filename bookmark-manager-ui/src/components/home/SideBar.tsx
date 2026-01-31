import iconHome from "@/assets/images/icon-home.svg"
import iconArchived from "@/assets/images/icon-archive.svg"
import iconArchivedDark from "@/assets/images/icon-archive-dark.svg"
import iconClose from "@/assets/images/icon-close.svg"
import { Logo } from "@/components/auth/FormContainerSignIn"
import { useState } from "react"
import { useFilterTagsContext } from "@/context/FilterTagsContext"

export function SideBar({ onClose}: {onClose?: () => void}) {
    const [selectedItem, setSelectedItem] = useState<"Home" | "Archived">("Home")
    const availableTags = [
        { name: "AI", count: 1 },
        { name: "Community", count: 5 },
        { name: "Compatibility", count: 1 },
        { name: "CSS", count: 6 },
        { name: "Design", count: 1 },
        { name: "React", count: 3 },
        { name: "JavaScript", count: 4 },
        { name: "TypeScript", count: 2 },
        { name: "Tailwind", count: 3 },
        { name: "Learning", count: 2 }
    ]

    return (
        <div className="flex flex-col gap-y-10 bg-neutral-0 dark:bg-neutral-d-800 border border-neutral-300 dark:border-neutral-d-500 w-74 h-screen max-h-screen overflow-y-auto">
            <div className="relative flex flex-col gap-y-5 px-5 pt-5 pb-2.5">
                {onClose && (
                    <div onClick={onClose}
                        className="absolute right-0 top-0 size-8 flex justify-center items-center gap-x-1 cursor-pointer">
                        <img src={iconClose} className="size-5" alt="icon close" />
                    </div>

                )}
                <Logo />
            </div>
            <div className="flex flex-col gap-y-4 px-4 pb-5">
                <div className="flex flex-col gap-y-1">
                    <div
                        onClick={() => {
                            if(selectedItem==="Archived") setSelectedItem("Home")
                        }}
                        className={`flex flex-row items-center gap-x-2 px-3 py-2 rounded-6 
                        cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-600
                        ${selectedItem === "Home" ? 'bg-neutral-100 dark:bg-neutral-d-600 border border-neutral-100 dark:border-neutral-d-500 text-neutral-900 dark:text-neutral-0 ring ring-teal-700' : 'text-neutral-800 dark:text-neutral-d-100'}`}>
                        <div className="flex flex-row items-center gap-x-2">
                            <img src={iconHome} className="size-5" alt="icon navigation" />
                            <span className="text-preset-3">Home</span>
                        </div>
                    </div>
                    <div
                        onClick={() => {
                            if(selectedItem==="Home")setSelectedItem("Archived")
                        }}
                        className={`flex flex-row items-center gap-x-2 px-3 py-2 rounded-6 
                        cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-600 
                        ${selectedItem === "Archived" ? 'bg-neutral-100 dark:bg-neutral-d-600 border border-neutral-100 dark:border-neutral-d-500 text-neutral-900 dark:text-neutral-0 ring ring-teal-700' : 'text-neutral-800 dark:text-neutral-d-100'}`}>
                        <div className="flex flex-row items-center gap-x-2">
                            <img src={iconArchived} className="size-5 dark:hidden" alt="icon navigation" />
                            <img src={iconArchivedDark} className="size-5 hidden dark:block" alt="icon navigation" />
                            <span className="text-preset-3">Archived</span>
                        </div>
                    </div>
                </div>
                <div>
                    <span className="h-5.25 items-center px-3 pb-1 text-[#34D4D4D] text-xs font-bold">TAGS</span>
                    <div>
                        {availableTags.map(tag => (
                            <ContentItemNavigationSideBar
                                key={tag.name}
                                text={tag.name}
                                numberBadge={tag.count}
                            />
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}

export function ContentItemNavigationSideBar({ text, numberBadge }: { text: string, numberBadge: number }) {
    const { selectedTagsList, addTag, deleteTag } = useFilterTagsContext()
    const isChecked = selectedTagsList.includes(text)

    function handleChangeInput(event: React.ChangeEvent<HTMLInputElement>) {
        if (event.target.checked) {
            addTag(text);
        } else {
            deleteTag(text);
        }
    }

    return (
        <div className="flex justify-between px-3 py-2.5">
            <div className="flex flex-row items-center gap-x-2">
                <input type="checkbox" checked={isChecked} onChange={handleChangeInput}
                    className={`size-4 border border-neutral-500 cursor-pointer
                         ${isChecked ? "accent-teal-700" : ""}`}
                />
                <span className="text-preset-3 text-neutral-800"> {text} </span>
            </div>
            <div className="items-center px-2 pb-0.5 rounded-full bg-neutral-100 border border-neutral-300">
                <span className="text-neutral-800 text-xs font-bold">{numberBadge}</span>
            </div>
        </div>
    )
}