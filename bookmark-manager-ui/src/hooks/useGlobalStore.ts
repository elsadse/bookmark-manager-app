import { create } from "zustand"

export type SortBookmarksBy = "recently-added" | "most-visited" | "recently-visited"

export type GlobalStore = {
    headerTitle: string,
    sortBookmarksBy: SortBookmarksBy,
    setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => void,
    tagFilters: string[],
    addTagFilter: (tag: string) => void,
    removeTagFilter: (tag: string) => void,
    filterArchivedBookmarks: boolean,
    setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean) => void,
    isDeleteDialogOpen: boolean,
    setIsDeleteDialogOpen: (isDeleteDialogOpen: boolean) => void
}

export const useGlobalStore = create<GlobalStore>((set) => ({
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
    isDeleteDialogOpen: false,
    setIsDeleteDialogOpen: (isDeleteDialogOpen: boolean) => set({ isDeleteDialogOpen })
}))
