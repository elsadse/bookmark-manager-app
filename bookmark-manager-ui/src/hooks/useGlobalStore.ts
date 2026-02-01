import { create } from "zustand"

export type SortBookmarksBy = "recently-added" | "most-visited" | "recently-visited"

export type GlobalStore = {
    sortBookmarksBy: SortBookmarksBy
    setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => void
    tagFilters: string[]
    addTagFilter: (tag: string) => void
    removeTagFilter: (tag: string) => void
}

export const useGlobalStore = create<GlobalStore>((set) => ({
    sortBookmarksBy: "recently-added",
    setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => set({ sortBookmarksBy }),
    tagFilters: [],
    addTagFilter: (tag: string) => set((store) => ({ tagFilters: [...store.tagFilters, tag] })),
    removeTagFilter: (tag: string) => set((store) => ({ tagFilters: store.tagFilters.filter((t) => t !== tag) })),
}))
