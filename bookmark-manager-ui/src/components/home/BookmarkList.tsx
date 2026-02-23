import { useEffect, useRef, useState, type ComponentType } from "react"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { fetchBookmarks, togglePin } from "@/api/bookmarks"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import { useShallow } from "zustand/shallow"
import type { Bookmark } from "@/api/bookmarks/schema"
import { visitBookmark } from "@/api/visits"
import { useAuthContext } from "@/hooks/useAuthContext"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"
import { useCloseDropdown } from "@/hooks/useCloseDropdown"
import { SwitchVerticalIcon } from "@/components/icons/SwitchVerticalIcon"
import { PinIcon } from "@/components/icons/PinIcon"
import { LeadingIcon } from "@/components/icons/LeadingIcon"
import { CheckIcon } from "@/components/icons/CheckIcon"
import { VisitIcon } from "@/components/icons/VisitIcon"
import { CopyIcon } from "@/components/icons/CopyIcon"
import { UnpinIcon } from "@/components/icons/UnpinIcon"
import { EditIcon } from "@/components/icons/EditIcon"
import { ArchivedIcon } from "@/components/icons/ArchivedIcon"
import { UnarchivedIcon } from "@/components/icons/UnarchivedIcon"
import { DeleteIcon } from "@/components/icons/DeleteIcon"
import { VisitCountIcon } from "@/components/icons/VisitCountIcon"
import { TimeIcon } from "@/components/icons/TimeIcon"
import { DateIcon } from "@/components/icons/DateIcon"
import { LoadingIcon } from "@/components/icons/LoadingIcon"

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
    const ref = useRef<HTMLButtonElement>(null)
    useCloseDropdown(ref, (): void => setIsSortByDropdownOpen(false))

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
        <div className="flex flex-col gap-y-5 px-4 pt-6 pb-16 h-full overflow-y-auto">
            <div className="flex flex-row justify-between items-center gap-x-4">
                <span className="text-preset-2 md:text-preset-1 text-neutral-900">{headerTitle}</span>
                <button onClick={handleClickSortBy}
                    className={`relative flex flex-row justify-between
                    items-center gap-x-1 px-3 py-2.5 bg-neutral-0 
                    rounded-8 border border-neutral-400 cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-600
                    ${isSortByDropdownOpen ? "ring ring-teal-700 dark:ring-neutral-d-0" : ""}
                    `} ref={ref}>
                    <SwitchVerticalIcon className="w-5 h-5" />
                    <span className="text-preset-3 text-neutral-900">Sort by</span>
                    {isSortByDropdownOpen && <SortByDropdown />}
                </button>
            </div>
            <div className="flex flex-wrap gap-8 pt-8 items-center justify-center">
                {isLoading ?
                    <LoadingIcon className="size-20 stroke-neutral-500" /> :
                    bookmarks?.map((bookmark, index) => (
                        <BookmarkListCard key={index} bookmark={bookmark} />
                    ))
                }
                {bookmarks?.length === 0 &&
                    <p className="px-3 text-preset-2 text-neutral-800">No bookmarks to display</p>
                }
            </div>
        </div>
    )
}

export function BookmarkListCard({ bookmark }: { bookmark: Bookmark }) {

    return (
        <div className="rounded-8 bg-neutral-0 dark:bg-neutral-d-800 h-68 w-84.5">
            <BookmarkListCardContainer bookmark={bookmark} />
            <div className="h-1O.25 flex flex-row justify-between items-center gap-x-2 px-4 py-3 border-t border-neutral-300 dark:border-neutral-d-500">
                <div className="flex flex-row gap-x-4">
                    <BookmarkListCardFooterInfo Icon={VisitCountIcon} information={bookmark.visitsCount.toString()} />
                    <BookmarkListCardFooterInfo Icon={TimeIcon} information={bookmark.lastVisitTime === null ? "Never" : bookmark.lastVisitTime.toLocaleDateString("en-US", { month: "short", day: "numeric" })} />
                    <BookmarkListCardFooterInfo Icon={DateIcon} information={bookmark.creationTime.toLocaleDateString("en-US", { month: "short", day: "numeric" })} />
                </div>
                {!bookmark.isArchived && bookmark.isPinned && <PinIcon className="size-3" />}
                {bookmark.isArchived &&
                    <span className="text-center text-preset-5 text-neutral-800 dark:text-neutral-d-100 bg-neutral-100 dark:bg-neutral-d-600 rounded-4 px-1.5">Archived</span>
                }
            </div>
        </div>
    )
}


export function BookmarkListCardContainer({ bookmark }: { bookmark: Bookmark }) {
    const [isBookmarkActionDropdownOpen, setIsBookmarkActionDropdownOpen] = useState(false)
    const ref = useRef<HTMLDivElement>(null)
    useCloseDropdown(ref, (): void => setIsBookmarkActionDropdownOpen(false))

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
                        <span className="text-preset-2 text-neutral-900 ">{bookmark.title}</span>
                        <span className="text-preset-5 text-neutral-800">{formattedUrl}</span>
                    </div>
                </div>
                <div onClick={handleActionDropdown} ref={ref}
                    className={`${isBookmarkActionDropdownOpen ? "ring ring-teal-700 dark:ring-neutral-d-0" : ""}
                        relative flex justify-center items-center size-8 gap-x-1 rounded-8 
                      bg-neutral-0 border border-neutral-400 dark:border-neutral-d-500 cursor-pointer hover:bg-neutral-100
                        `}>
                    <LeadingIcon className="size-5" />
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

export function BookmarkListCardFooterInfo({ Icon, information }: { Icon: ComponentType<{ className?: string }>, information: string }) {

    return (
        <div className="flex flex-row justify-center items-center gap-x-1.5">
            <Icon className="size-3" />
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

    const { setIsDialogOrModalOpen, setIsToastOpen, setIsNotificationOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsNotificationOpen: store.setIsNotificationOpen,
            setIsDialogOrModalOpen: store.setIsDialogOrModalOpen,
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
        setIsDialogOrModalOpen("archive")
    }

    function handleUnarchive() {
        setIsDialogOrModalOpen("unarchive")
    }

    function handleCopy() {
        navigator.clipboard.writeText(url)
        setIsToastOpen("bookmark-link-copied")
        setIsNotificationOpen(true, "bookmark-link-copied")
    }

    function handleEdit() {
        setIsDialogOrModalOpen("edit")
    }

    function handleDelete() {
        setIsDialogOrModalOpen("delete")
    }

    return (
        <div className="absolute top-10 right-0 w-50 flex flex-col gap-y-1 p-2 rounded-8 bg-neutral-0 border border-neutral-100">
            <BookmarkActionDropdownMenu Icon={VisitIcon} text="Visit" onClick={handleVisit} />
            <BookmarkActionDropdownMenu Icon={CopyIcon} text="Copy Url" onClick={handleCopy} />
            {!archived && pinned && <BookmarkActionDropdownMenu Icon={UnpinIcon} text="Unpin" onClick={handlePin} />}
            {!archived && !pinned && <BookmarkActionDropdownMenu Icon={PinIcon} text="Pin" onClick={handlePin} />}
            {!archived && <BookmarkActionDropdownMenu Icon={EditIcon} text="Edit" onClick={handleEdit} />}
            {!archived && <BookmarkActionDropdownMenu Icon={ArchivedIcon} text="Archive" onClick={handleArchive} />}
            {archived && <BookmarkActionDropdownMenu Icon={UnarchivedIcon} text="Unarchive" onClick={handleUnarchive} />}
            {archived && <BookmarkActionDropdownMenu Icon={DeleteIcon} text="Delete Permanently" onClick={handleDelete} />}
        </div>
    )
}

export function BookmarkActionDropdownMenu({ Icon, text, onClick }: { Icon: ComponentType<{ className?: string }>, text: string, onClick: () => void }) {

    return (
        <div onClick={onClick}
            className="flex flex-row items-center gap-x-2.5 p-2 rounded-8 cursor-pointer hover:ring ring-teal-700 dark:ring-neutral-d-0">
            <Icon className="size-4" />
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
        <div className="fixed right-5 top-38 w-50 flex flex-col gap-y-p-2 rounded-8 bg-neutral-0 border border-neutral-100 z-10">
            <div onClick={(): void => {
                if (sortBookmarksBy !== "recently-added") {
                    setSortBookmarksBy("recently-added")
                }
            }}
                className="flex flex-row p-4 justify-between rounded-6 hover:ring ring-teal-700 dark:ring-neutral-d-0">
                <span className="text-preset-4 text-neutral-800">Recently added</span>
                {sortBookmarksBy === "recently-added" && <CheckIcon className="w-4 h-4" />}
            </div>
            <div onClick={(): void => {
                if (sortBookmarksBy !== "recently-visited") {
                    setSortBookmarksBy("recently-visited")
                }
            }}
                className="flex flex-row p-4 justify-between rounded-6 hover:ring ring-teal-700 dark:ring-neutral-d-0">
                <span className="text-preset-4 text-neutral-800">Recently visited</span>
                {sortBookmarksBy === "recently-visited" && <CheckIcon className="w-4 h-4" />}
            </div>
            <div onClick={(): void => {
                if (sortBookmarksBy !== "most-visited") {
                    setSortBookmarksBy("most-visited")
                }
            }}
                className="flex flex-row p-4 justify-between rounded-6 hover:ring ring-teal-700 dark:ring-neutral-d-0">
                <span className="text-preset-4 text-neutral-800">Most visited</span>
                {sortBookmarksBy === "most-visited" && <CheckIcon className="w-4 h-4" />}
            </div>
        </div>
    )
}