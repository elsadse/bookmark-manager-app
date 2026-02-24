import type { Bookmark } from "@/api/bookmarks/schema"
import type { Action, NotificationType, Nullable, SortBookmarksBy, ToastAction } from "@/types"
import { create } from "zustand"
import { persist } from "zustand/middleware"

export type GlobalStore = {
    headerTitle: string,

    sortBookmarksBy: SortBookmarksBy,
    setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => void,

    searchQuery: string,
    setSearchQuery: (searchQuery: string) => void,

    tagFilters: string[],
    addTagFilter: (tag: string) => void,
    removeTagFilter: (tag: string) => void,

    filterArchivedBookmarks: boolean,
    setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean) => void,

    isDialogOrModalOpen: boolean,
    action: Nullable<Action>,
    setIsDialogOrModalOpen: (action: Nullable<Action>) => void,

    bookmarkSelected: Nullable<Bookmark>,
    setBookmarkSelected: (bookmarkSelected: Nullable<Bookmark>) => void,

    isToastOpen: boolean,
    toastDescription: Nullable<string>,
    setIsToastOpen: (toastAction: Nullable<ToastAction>) => void

    notificationType: Nullable<NotificationType>
    isNotificationOpen: boolean
    setIsNotificationOpen: (isNotificationOpen: boolean, notificationType: Nullable<NotificationType>) => void
}

export const useGlobalStore = create<GlobalStore>()(
    persist(
        (set) => ({
            headerTitle: "Bookmarks",

            sortBookmarksBy: "recently-added",
            setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => set({ sortBookmarksBy }),

            searchQuery: "",
            setSearchQuery: (searchQuery: string) => set((store) => {
                const {tagFilters, filterArchivedBookmarks} = store
                const headerTitle = buildHeaderTitle({tagFilters, filterArchivedBookmarks, searchQuery})

                return { searchQuery, headerTitle }
            }),

            tagFilters: [],
            addTagFilter: (tag: string) => set((store) => {
                const tagFilters = [...store.tagFilters, tag]
                const {filterArchivedBookmarks, searchQuery} = store
                const headerTitle = buildHeaderTitle({tagFilters, filterArchivedBookmarks, searchQuery})
                return { headerTitle, tagFilters }
            }),
            removeTagFilter: (tag: string) => set((store) => {
                const tagFilters = store.tagFilters.filter((t) => t !== tag)
                const {filterArchivedBookmarks, searchQuery} = store
                const headerTitle = buildHeaderTitle({tagFilters, filterArchivedBookmarks, searchQuery})
                return { headerTitle, tagFilters }
            }),

            filterArchivedBookmarks: false,
            setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean) => set((store) => {
                const {tagFilters, searchQuery} = store
                const headerTitle = buildHeaderTitle({tagFilters, filterArchivedBookmarks, searchQuery})
                return { headerTitle, filterArchivedBookmarks }
            }),

            isDialogOrModalOpen: false,
            action: null,
            setIsDialogOrModalOpen: (action: Nullable<Action>) => set(() => {
                const dialogOpenAndAction = getIsDialogOrModalOpen({ action })
                return { isDialogOrModalOpen: dialogOpenAndAction.isDialogOrModalOpen, action: dialogOpenAndAction.action }
            }),

            bookmarkSelected: null,
            setBookmarkSelected: (bookmarkSelected: Nullable<Bookmark>) => set({ bookmarkSelected }),

            isToastOpen: false,
            toastDescription: null,
            setIsToastOpen: (toastAction: Nullable<ToastAction>) => set(() => {
                const { isToastOpen, toastDescription } = getIsToastOpenAndToastDescription(toastAction)
                return { isToastOpen, toastDescription }
            }),

            notificationType: null,
            isNotificationOpen: false,
            setIsNotificationOpen: (isOpen: boolean, type: Nullable<NotificationType> = null) => set({
                isNotificationOpen: isOpen,
                notificationType: type,
            }),

        }),
        {
            name: "global-storage"
        }
    )
)


export function buildHeaderTitle({ tagFilters, filterArchivedBookmarks, searchQuery }: {
    tagFilters: string[],
    filterArchivedBookmarks: boolean,
    searchQuery: string
}): string {
    const parts: string[] = []

    if (searchQuery.length > 1) {
        parts.push("Search")
    }

    if (filterArchivedBookmarks) {
        parts.push("Archives")
    }

    parts.push("Bookmarks")

    if (searchQuery.length > 1) {
        parts.push(`"${searchQuery}"`)
    }

    if (tagFilters.length > 0) {
        parts.push(`Tagged with: [${tagFilters.join(', ')}]`)
    }

    return parts.join(" > ")
}

export function getIsDialogOrModalOpen({ action }: { action: Nullable<Action> }): { isDialogOrModalOpen?: boolean, action: Nullable<Action> } {
    if (action === "delete") {
        return { isDialogOrModalOpen: true, action: "delete" }
    }
    if (action === "archive") {
        return { isDialogOrModalOpen: true, action: "archive" }
    }
    if (action === "unarchive") {
        return { isDialogOrModalOpen: true, action: "unarchive" }
    }
    if (action === "edit") {
        return { isDialogOrModalOpen: true, action: "edit" }
    }
    return { isDialogOrModalOpen: false, action: null }
}

export function getIsToastOpenAndToastDescription(toastAction: Nullable<ToastAction>): { isToastOpen: boolean, toastDescription: Nullable<string> } {
    if (toastAction == "bookmark-added") {
        return { isToastOpen: true, toastDescription: "Bookmark added successfully." }
    }
    if (toastAction == "bookmark-edited") {
        return { isToastOpen: true, toastDescription: "Changes saved." }
    }
    if (toastAction == "bookmark-link-copied") {
        return { isToastOpen: true, toastDescription: "Link copied to clipboard." }
    }
    if (toastAction == "bookmark-pinned") {
        return { isToastOpen: true, toastDescription: "Bookmark pinned to top." }
    }
    if (toastAction == "bookmark-archived") {
        return { isToastOpen: true, toastDescription: "Bookmark archived." }
    }
    if (toastAction == "bookmark-unarchived") {
        return { isToastOpen: true, toastDescription: "Bookmark restored." }
    }
    if (toastAction == "bookmark-deleted") {
        return { isToastOpen: true, toastDescription: "Bookmark deleted." }
    }
    return { isToastOpen: false, toastDescription: null }
}
