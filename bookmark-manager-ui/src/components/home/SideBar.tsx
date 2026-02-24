import { Logo } from "@/components/auth/FormContainerSignIn"
import { useEffect, useState } from "react"
import { useQuery } from "@tanstack/react-query"
import { useShallow } from "zustand/shallow"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import { useAuthContext } from "@/hooks/useAuthContext"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"
import { CloseIcon } from "@/components/icons/CloseIcon"
import { HomeIcon } from "@/components/icons/HomeIcon"
import { ArchivedIcon } from "@/components/icons/ArchivedIcon"
import { LoadingIcon } from "@/components/icons/LoadingIcon"
import { CheckIcon } from "@/components/icons/CheckIcon"
import type { Bookmark } from "@/api/bookmarks/schema"
import { fetchBookmarks } from "@/api/bookmarks"

function buildTag2count({ bookmarks }: { bookmarks: Bookmark[] | undefined }): Map<string, number> {
    const tag2count: Map<string, number> = new Map()
    if (!bookmarks) return tag2count

    for (const bookmark of bookmarks) {
        for (const tag of bookmark.tags) {
            tag2count.set(tag, (tag2count.get(tag) ?? 0) + 1)
        }
    }

    return tag2count
}

export function SideBar({ onClose }: { onClose?: () => void }) {
    const { setFilterArchivedBookmarks, filterArchivedBookmarks, tagFilters, searchQuery } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setFilterArchivedBookmarks: store.setFilterArchivedBookmarks,
            filterArchivedBookmarks: store.filterArchivedBookmarks,
            searchQuery: store.searchQuery,
            tagFilters: store.tagFilters,
        }))
    )

    const { data: bookmarks, isFetching, isError, error } = useQuery({
        queryKey: ["bookmarks", searchQuery],
        queryFn: async (): Promise<Bookmark[]> => fetchBookmarks(searchQuery),
        select: (data: Bookmark[]): Bookmark[] =>
            [
                ...data
                    .filter((bookmark: Bookmark): boolean => filterArchivedBookmarks ? bookmark.isArchived : !bookmark.isArchived)
                    .filter((bookmark: Bookmark): boolean => tagFilters.every((filter: string): boolean => bookmark.tags.includes(filter)))
            ]
    })
    const tag2count = buildTag2count({ bookmarks })

    const [selectedItem, setSelectedItem] = useState<"Home" | "Archived">(() => {
        return filterArchivedBookmarks ? "Archived" : "Home"
    })

    // Gestion de la déconnexion automatique
    const { logout } = useAuthContext()
    useEffect(() => {
        if (isError && error instanceof UnauthorizedApiError) {
            logout()
        }
    }, [error, isError, logout])

    return (
        <div className="flex flex-col gap-y-10 bg-neutral-0 dark:bg-neutral-d-800 border border-neutral-300 dark:border-neutral-d-500 w-74 h-screen overflow-y-auto">
            <div className="relative flex flex-col gap-y-5 px-5 pt-5 pb-2.5">
                {onClose && (
                    <div onClick={onClose}
                        className="absolute right-0 top-0 size-8 flex justify-center items-center gap-x-1 cursor-pointer">
                        <CloseIcon className="size-5" />
                    </div>

                )}
                <Logo />
            </div>
            <div className="flex flex-col gap-y-4 px-4 pb-5">
                <div className="flex flex-col gap-y-1">
                    <div
                        onClick={() => {
                            if (selectedItem === "Archived") {
                                setSelectedItem("Home")
                                setFilterArchivedBookmarks(false)
                            }
                        }}
                        className={`flex flex-row items-center gap-x-2 px-3 py-2 rounded-6 
                        cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-600
                        ${selectedItem === "Home" ? 'bg-neutral-100 dark:bg-neutral-d-600 border border-neutral-100 dark:border-neutral-d-500 text-neutral-900 ring ring-teal-700 dark:ring-neutral-d-0' : 'text-neutral-800'}`}>
                        <div className="flex flex-row items-center gap-x-2">
                            <HomeIcon className="size-5" />
                            <span className="text-preset-3">Home</span>
                        </div>
                    </div>
                    <div
                        onClick={() => {
                            if (selectedItem === "Home") {
                                setSelectedItem("Archived")
                                setFilterArchivedBookmarks(true)
                            }
                        }}
                        className={`flex flex-row items-center gap-x-2 px-3 py-2 rounded-6 
                        cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-600 
                        ${selectedItem === "Archived" ? 'bg-neutral-100 dark:bg-neutral-d-600 border border-neutral-100 dark:border-neutral-d-500 text-neutral-900 ring ring-teal-700 dark:ring-neutral-d-0' : 'text-neutral-800'}`}>
                        <div className="flex flex-row items-center gap-x-2">
                            <ArchivedIcon className="size-5" />
                            <span className="text-preset-3">Archived</span>
                        </div>
                    </div>
                </div>
                <div className="">
                    <span className="h-5.25 items-center px-3 pb-1 text-[#4D4D4D] text-xs font-bold dark:text-neutral-d-100">TAGS</span>
                    <div className="">
                        {isFetching ?
                            <LoadingIcon className="size-12 stroke-neutral-500" /> :
                            Array.from(tag2count.entries())
                                .sort((a: [string, number], b: [string, number]): number => a[0].localeCompare(b[0]))
                                .map(([tag, count]: [string, number]) =>
                                    <ContentItemNavigationSideBar
                                        key={tag}
                                        text={tag}
                                        numberBadge={count}
                                        inputId={tag}
                                    />
                                )
                        }
                    </div>
                </div>
            </div>
        </div>
    )
}

export function ContentItemNavigationSideBar({ text, numberBadge, inputId }: { text: string, numberBadge: number, inputId: string }) {
    const { tagFilters, addFilter, removeFilter } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            tagFilters: store.tagFilters,
            addFilter: store.addTagFilter,
            removeFilter: store.removeTagFilter,
        }))
    )
    const isChecked = tagFilters.includes(text)

    function handleChangeInput(event: React.ChangeEvent<HTMLInputElement>) {
        if (event.target.checked) {
            addFilter(text);
        } else {
            removeFilter(text);
        }
    }

    return (
        <div className="flex justify-between px-3 py-2.5">
            <div className="relative flex flex-row items-center gap-x-2">
                <input id={inputId}
                    type="checkbox" checked={isChecked} onChange={handleChangeInput}
                    className={`appearance-none checked:bg-teal-700 size-4 rounded-sm border border-neutral-500 cursor-pointer `}
                />
                {isChecked && (
                    <CheckIcon className="absolute top-1.45 size-4 stroke-neutral-d-0 fill-none cursor-pointer" onClick={() => removeFilter(text)} />
                )}
                <label htmlFor={inputId} className="text-preset-3 text-neutral-800 cursor-pointer dark:text-neutral-d-100"> {text} </label>
            </div>
            <div className="items-center px-2 pb-0.5 rounded-full bg-neutral-100 dark:bg-neutral-d-600 border border-neutral-300 dark:border-neutral-d-300">
                <span className="text-neutral-800 dark:text-neutral-d-100 text-xs font-bold">{numberBadge}</span>
            </div>
        </div>
    )
}