import iconSwitchVertical from "@/assets/images/icon-switch-vert.svg"
import iconSwitchVerticalDark from "@/assets/images/icon-switch-vert-dark.svg"
import iconLeading from "@/assets/images/icon-leading.svg"
import iconLeadingDark from "@/assets/images/icon-leading-dark.svg"
import iconVisitCount from "@/assets/images/icon-visit-count.svg"
import iconTime from "@/assets/images/icon-time.svg"
import iconDate from "@/assets/images/icon-date.svg"
import iconVisitCountDark from "@/assets/images/icon-visit-count-dark.svg"
import iconTimeDark from "@/assets/images/icon-time-dark.svg"
import iconDateDark from "@/assets/images/icon-date-dark.svg"
import iconPin from "@/assets/images/icon-pin.svg"
import iconPinDark from "@/assets/images/icon-pin-dark.svg"
import iconVisit from "@/assets/images/icon-visit.svg"
import iconCopy from "@/assets/images/icon-copy.svg"
import iconUnpin from "@/assets/images/icon-unpin.svg"
import iconEdit from "@/assets/images/icon-edit.svg"
import iconArchive from "@/assets/images/icon-archive.svg"
import iconUnarchive from "@/assets/images/icon-unarchive.svg"
import iconDelete from "@/assets/images/icon-delete.svg"
import iconCheck from "@/assets/images/icon-check.svg"
import { useEffect, useState } from "react"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { fetchBookmarks, togglePin } from "@/api/bookmarks"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import { useShallow } from "zustand/shallow"
import type { Bookmark } from "@/api/bookmarks/schema"
import { visitBookmark } from "@/api/visits"
import { useAuthContext } from "@/hooks/useAuthContext"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"

export function BookmarkList() {
    const { sortBookmarksBy, tagFilters, filterArchivedBookmarks, headerTitle } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            headerTitle: store.headerTitle,
            sortBookmarksBy: store.sortBookmarksBy,
            tagFilters: store.tagFilters,
            filterArchivedBookmarks: store.filterArchivedBookmarks,
            isToastOpen: store.isToastOpen,
            toastDescription: store.toastDescription,
        }))
    )
    const [isSortByDropdownOpen, setIsSortByDropdownOpen] = useState(false)

    const { data: bookmarks, isLoading, isError, error } = useQuery({
        queryKey: ["bookmarks"],
        queryFn: fetchBookmarks,
        select: (data: Bookmark[]): Bookmark[] =>
            [
                ...data
                    .filter((bookmark: Bookmark): boolean => filterArchivedBookmarks ? bookmark.isArchived : !bookmark.isArchived)
                    .filter((bookmark: Bookmark): boolean => tagFilters.every((filter: string): boolean => bookmark.tags.includes(filter)))
            ].sort((a: Bookmark, b: Bookmark): number => {
                const primarySort = Number(b.isPinned) - Number(a.isPinned)
                if (primarySort !== 0) return primarySort
                if (sortBookmarksBy === "recently-added") {
                    return b.creationTime.getTime() - a.creationTime.getTime()
                } else if (sortBookmarksBy === "most-visited") {
                    return b.visitsCount - a.visitsCount
                } else if (sortBookmarksBy === "recently-visited") {
                    return (b.lastVisitTime?.getTime() ?? 0) - (a.lastVisitTime?.getTime() ?? 0)
                }
                return 0
            })
    })

    // Gestion de la déconnexion automatique
    const { logout } = useAuthContext()
    useEffect(() => {
        /*console.log(error instanceof UnauthorizedApiError)
        console.log(error)
        console.log((error as any)?.response?.status)*/
        if (isError && error instanceof UnauthorizedApiError) {
            logout()
        }
    }, [error, isError, logout])


    function handleClickSortBy() {
        setIsSortByDropdownOpen(!isSortByDropdownOpen)
    }

    return (
        <div className="flex flex-col gap-y-5 px-4 pt-6 pb-16 h-screen overflow-y-auto">
            <div className="flex flex-row justify-between items-center gap-x-4">
                <div className="flex flex-col md:flex-row">
                    <span className="text-preset-2 md:text-preset-1 text-neutral-900 dark:text-neutral-0">{headerTitle}</span>
                    {/*{tagFilters.length !== 0 && (
                        <div className="flex flex-row">
                            {tagFilters.map((tag, index) => (
                                <span key={index} className="text-preset-2 md:text-preset-1 text-teal-700">
                                    &nbsp;{tag}
                                    {index < tagFilters.length - 1 && ","}&nbsp;
                                </span>
                            ))}
                        </div>
                    )}
                    {searchQuery.length > 0 && (
                        <span className="text-preset-2 md:text-preset-1 text-teal-700">
                            &nbsp;{'"' + searchQuery + '"'}
                        </span>
                    )}*/}
                </div>
                <button onClick={handleClickSortBy}
                    className={`relative flex justify-center 
                    items-center gap-x-4 px-3 py-2.5 bg-neutral-0 dark:bg-neutral-d-800
                    rounded-8 border border-neutral-400 dark:border-neutral-d-400 cursor-pointer hover:bg-neutral-100
                    ${isSortByDropdownOpen ? "ring ring-teal-700" : ""}
                    `}>
                    <img src={iconSwitchVertical} className="dark:hidden" alt="icon switch" />
                    <img src={iconSwitchVerticalDark} className="hidden dark:block" alt="icon switch" />
                    <span className="text-preset-3 text-neutral-900 dark:text-neutral-0">Sort by</span>
                    {isSortByDropdownOpen && <SortByDropdown />}
                </button>

            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-8 pb-8 overflow-y-auto">
                {isLoading ?
                    <p className="px-3 text-preset-2 text-neutral-800">Loading bookmarks...</p> :
                    bookmarks?.map((bookmark, index) => (
                        <BookmarkListCard key={index} bookmark={bookmark} />
                    ))}
            </div>
        </div>
    )
}

export function BookmarkListCard({ bookmark }: { bookmark: Bookmark }) {

    return (
        <div className="rounded-8 bg-neutral-0 dark:bg-neutral-d-800 h-68">
            <BookmarkListCardContainer bookmark={bookmark} />
            <div className="h-1O.25 flex flex-row justify-between items-center gap-x-2 px-4 py-3 border-t border-neutral-300 dark:border-neutral-d-500">
                <div className="flex flex-row gap-x-4">
                    <BookmarkListCardFooterInfo icon={iconVisitCount} iconDark={iconVisitCountDark} information={bookmark.visitsCount.toString()} />
                    <BookmarkListCardFooterInfo icon={iconTime} iconDark={iconTimeDark} information={bookmark.creationTime.toLocaleDateString("en-US", { month: "short", day: "numeric" })} />
                    {bookmark.lastVisitTime !== null &&
                        <BookmarkListCardFooterInfo icon={iconDate} iconDark={iconDateDark} information={bookmark.lastVisitTime.toLocaleDateString("en-US", { month: "short", day: "numeric" })} />
                    }
                </div>
                {!bookmark.isArchived && bookmark.isPinned && <img src={iconPin} className="size-4 dark:hidden" alt="icon pin" />}
                {!bookmark.isArchived && bookmark.isPinned && <img src={iconPinDark} className="size-4 hidden dark:block" alt="icon pin" />}
                {bookmark.isArchived &&
                    <span className="text-center text-preset-5 text-neutral-800 dark:text-neutral-d-100 bg-neutral-100 dark:bg-neutral-d-600 rounded-4 px-1.5">Archived</span>
                }
            </div>
        </div>
    )
}


export function BookmarkListCardContainer({ bookmark }: { bookmark: Bookmark }) {
    const [isBookmarkActionDropdownOpen, setIsBookmarkActionDropdownOpen] = useState(false)

    const urlObject = new URL(bookmark.url)
    const formattedUrl = urlObject.pathname === "/"
        ? `${urlObject.host}${urlObject.search}`
        : `${urlObject.host}${urlObject.pathname}${urlObject.search}`

    const { setBookmarkSelected } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setBookmarkSelected: store.setBookmarkSelected
        }))
    )

    function handleActionDropdown() {
        setIsBookmarkActionDropdownOpen(!isBookmarkActionDropdownOpen)
        setBookmarkSelected(bookmark)
    }

    return (
        <div className="h-57.75 flex flex-col gap-y-4 p-4 rounded-10">
            <div className="flex flex-row justify-between gap-x-3">
                <div className="flex flex-row gap-x-3">
                    <div className="size-11 flex items-center rounded-8 border border-neutral-100 dark:border-neutral-d-500">
                        <img src={`https://www.faviconextractor.com/favicon/${urlObject.host}`} className="w-11 h-11" alt="icon logo" />
                    </div>
                    <div className="flex flex-col gap-1">
                        <span className="text-preset-2 text-neutral-900 dark:text-neutral-0">{bookmark.title}</span>
                        <span className="text-preset-5 text-neutral-800 dark:text-neutral-d-100">{formattedUrl}</span>
                    </div>
                </div>
                <div onClick={handleActionDropdown}
                    className={`${isBookmarkActionDropdownOpen ? "ring ring-teal-700" : ""}
                        relative flex justify-center items-center size-8 gap-x-1 rounded-8 
                      bg-neutral-0 dark:bg-neutral-d-800 border border-neutral-400 dark:border-neutral-d-500 cursor-pointer hover:bg-neutral-100
                        `}>
                    <img src={iconLeading} className="size-5 dark:hidden" alt="icon logo" />
                    <img src={iconLeadingDark} className="size-5 hidden dark:block" alt="icon logo" />
                    {isBookmarkActionDropdownOpen &&
                        <BookmarkActionDropdown pinned={bookmark.isPinned} id={bookmark.bookmarkId} url={bookmark.url} archived={bookmark.isArchived} />
                    }
                </div>
            </div>
            <div className="h-px bg-neutral-300 dark:bg-neutral-d-500" />
            <span className="text-preset-4-md text-neutral-800 dark:text-neutral-d-100 text-left">
                {bookmark.description}
            </span>
            <div className="flex flex-row gap-x-2">
                {bookmark.tags.map((tag, index) => (
                    <span key={index} className="text-center text-preset-5 text-neutral-800 dark:text-neutral-d-100 bg-neutral-100 dark:bg-neutral-d-600 rounded-4 px-2 py-0.5">{tag.charAt(0).toUpperCase() + tag.slice(1).toLowerCase()}</span>
                ))}
            </div>
        </div>
    )
}

export function BookmarkListCardFooterInfo({ icon, iconDark, information }: { icon: string, information: string, iconDark: string }) {

    return (
        <div className="flex flex-row justify-center items-center gap-x-1.5">
            <img src={icon} className="size-3 dark:hidden" alt="icon" />
            <img src={iconDark} className="size-3 hidden dark:block" alt="icon" />
            <span className="text-preset-5 text-left text-neutral-800 dark:text-neutral-d-100">{information}</span>
        </div>
    )
}


export function BookmarkActionDropdown({ id, pinned, archived, url }: { id: number, pinned: boolean, archived: boolean, url: string }) {
    const queryClient = useQueryClient()
    const { mutate: visitBookmarkFn } = useMutation({
        mutationFn: visitBookmark,
        onSuccess: async () => await queryClient.invalidateQueries({ queryKey: ["bookmarks"] })
    })
    const { mutate: pinToggleFn } = useMutation({
        mutationFn: togglePin,
        onSuccess: async () => await queryClient.invalidateQueries({ queryKey: ["bookmarks"] })
    })

    const { setIsDialogOpen, setIsToastOpen, setIsNotificationOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsNotificationOpen: store.setIsNotificationOpen,
            setIsDialogOpen: store.setIsDialogOpen,
            setIsToastOpen: store.setIsToastOpen,
        }))
    )

    function handleVisit() {
        visitBookmarkFn({ bookmarkId: id, visitTime: new Date() })
        window.open(url, "_blank", "noopener,noreferrer")
    }

    function handlePin() {
        pinToggleFn({ bookmarkId: id })
    }

    function handleArchive() {
        setIsDialogOpen("archive")
    }

    function handleUnarchive() {
        setIsDialogOpen("unarchive")
    }

    function handleCopy() {
        navigator.clipboard.writeText(url)
        setIsToastOpen("bookmark-link-copied")
        setIsNotificationOpen(true, "bookmark-link-copied")
    }

    function handleEdit() {
        //setIsNotificationOpen(true, "bookmark-edited")
    }

    function handleDelete() {
        setIsDialogOpen("delete")
    }

    return (
        <div className="absolute top-10 right-0 w-50 flex flex-col gap-y-1 p-2 rounded-8 bg-neutral-0 border border-neutral-100">
            <BookmarkActionDropdownMenu icon={iconVisit} text="Visit" onClick={handleVisit} />
            <BookmarkActionDropdownMenu icon={iconCopy} text="Copy Url" onClick={handleCopy} />
            {!archived && pinned && <BookmarkActionDropdownMenu icon={iconUnpin} text="Unpin" onClick={handlePin} />}
            {!archived && !pinned && <BookmarkActionDropdownMenu icon={iconPin} text="Pin" onClick={handlePin} />}
            {!archived && <BookmarkActionDropdownMenu icon={iconEdit} text="Edit" onClick={handleEdit} />}
            {!archived && <BookmarkActionDropdownMenu icon={iconArchive} text="Archive" onClick={handleArchive} />}
            {archived && <BookmarkActionDropdownMenu icon={iconUnarchive} text="Unarchive" onClick={handleUnarchive} />}
            {archived && <BookmarkActionDropdownMenu icon={iconDelete} text="Delete Permanently" onClick={handleDelete} />}
        </div>
    )
}

export function BookmarkActionDropdownMenu({ icon, text, onClick }: { icon: string, text: string, onClick: () => void }) {

    return (
        <div onClick={onClick}
            className="flex flex-row items-center gap-x-2.5 p-2 rounded-8 cursor-pointer hover:ring ring-teal-700">
            <img src={icon} alt="icon" />
            <span className="text-preset-4 text-neutral-800">{text}</span>
        </div>
    )
}

export function SortByDropdown() {
    const { sortBookmarksBy, setSortBookmarksBy } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            sortBookmarksBy: store.sortBookmarksBy,
            setSortBookmarksBy: store.setSortBookmarksBy
        }))
    )

    return (
        <div className="absolute right-0 top-12 w-50 flex flex-col gap-y-p-2 rounded-8 bg-neutral-0 border border-neutral-100 z-10">
            <div onClick={(): void => {
                if (sortBookmarksBy !== "recently-added") {
                    setSortBookmarksBy("recently-added")
                }
            }}
                className="flex flex-row p-4 justify-between rounded-6 hover:ring ring-teal-700">
                <span className="text-preset-4 text-neutral-800">Recently added</span>
                {sortBookmarksBy === "recently-added" && <img src={iconCheck} alt="icon check" />}
            </div>
            <div onClick={(): void => {
                if (sortBookmarksBy !== "recently-visited") {
                    setSortBookmarksBy("recently-visited")
                }
            }}
                className="flex flex-row p-4 justify-between rounded-6 hover:ring ring-teal-700">
                <span className="text-preset-4 text-neutral-800">Recently visited</span>
                {sortBookmarksBy === "recently-visited" && <img src={iconCheck} alt="icon check" />}
            </div>
            <div onClick={(): void => {
                if (sortBookmarksBy !== "most-visited") {
                    setSortBookmarksBy("most-visited")
                }
            }}
                className="flex flex-row p-4 justify-between rounded-6 hover:ring ring-teal-700">
                <span className="text-preset-4 text-neutral-800">Most visited</span>
                {sortBookmarksBy === "most-visited" && <img src={iconCheck} alt="icon check" />}
            </div>
        </div>
    )
}