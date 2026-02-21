import type { Bookmark } from "@/api/bookmarks/schema"
import type { DialogAction, NotificationType, Nullable, SortBookmarksBy, ToastAction } from "@/types"
import { create } from "zustand"
import { persist } from "zustand/middleware"

export type GlobalStore = {
    headerTitle: string,

    sortBookmarksBy: SortBookmarksBy,
    setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => void,

    tagFilters: string[],
    addTagFilter: (tag: string) => void,
    removeTagFilter: (tag: string) => void,

    filterArchivedBookmarks: boolean,
    setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean) => void,

    isDialogOpen: boolean,
    dialogAction: Nullable<DialogAction>,
    setIsDialogOpen: (dialogAction: Nullable<DialogAction>) => void,

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
            headerTitle: "All bookmarks",

            sortBookmarksBy: "recently-added",
            setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => set({ sortBookmarksBy }),

            tagFilters: [],
            addTagFilter: (tag: string) => set((store) => {
                let headerTitle = store.headerTitle
                if (store.tagFilters.length > 0) {
                    const prevSuffix = `tagged with [${store.tagFilters.join(", ")}]`
                    headerTitle = headerTitle.slice(0, -prevSuffix.length)
                }
                const tagFilters = [...store.tagFilters, tag]
                const suffix = `tagged with [${tagFilters.join(", ")}]`
                headerTitle = `${headerTitle} ${suffix}`
                return { headerTitle, tagFilters }
            }),
            removeTagFilter: (tag: string) => set((store) => {
                let headerTitle = store.headerTitle
                if (store.tagFilters.length > 0) {
                    const prevSuffix = `tagged with [${store.tagFilters.join(", ")}]`
                    headerTitle = headerTitle.slice(0, -prevSuffix.length)
                }
                const tagFilters = store.tagFilters.filter((t) => t !== tag)
                const suffix = `tagged with [${tagFilters.join(", ")}]`
                headerTitle = tagFilters.length === 0 ? headerTitle : `${headerTitle} ${suffix}`
                return { headerTitle, tagFilters }
            }),

            filterArchivedBookmarks: false,
            setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean) => set((store) => {
                const prevPrefix = store.filterArchivedBookmarks ? "Archived" : "All"
                let headerTitle = store.headerTitle.slice(prevPrefix.length + 1, store.headerTitle.length)
                const prefix = filterArchivedBookmarks ? "Archived" : "All"
                headerTitle = `${prefix} ${headerTitle}`
                return { headerTitle, filterArchivedBookmarks }
            }),

            isDialogOpen: false,
            dialogAction: null,
            setIsDialogOpen: (dialogAction: Nullable<DialogAction>) => set(() => {
                const dialogOpenAndAction = getIsDialogOpen({ dialogAction })
                return { isDialogOpen: dialogOpenAndAction.isDialogOpen, dialogAction: dialogOpenAndAction.dialogAction }
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


export function getIsDialogOpen({ dialogAction }: { dialogAction: Nullable<DialogAction> }): { isDialogOpen: boolean, dialogAction: Nullable<DialogAction> } {
    if (dialogAction === "delete") {
        return { isDialogOpen: true, dialogAction: "delete" }
    }
    if (dialogAction === "archive") {
        return { isDialogOpen: true, dialogAction: "archive" }
    }
    if (dialogAction === "unarchive") {
        return { isDialogOpen: true, dialogAction: "unarchive" }
    }
    return { isDialogOpen: false, dialogAction: null }
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
