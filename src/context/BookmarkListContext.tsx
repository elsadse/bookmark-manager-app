import type { Bookmark } from "@/types"
import { createContext, useContext, useState, type ReactNode } from "react"
import bookmarkData from "@/data/data.json"

type BookmarkListContextType = {
    bookmarkList: Bookmark[],
    bookmarkListArchived: Bookmark[],
    setBookmarkList: (bookmarkList: Bookmark[])=>void,
    setBookmarkListArchived: (bookmarkListArchived: Bookmark[])=>void,
    searchQuery: string
    setSearchQuery: (searchQuery: string)=>void
}
const BookmarkListContext = createContext<BookmarkListContextType | undefined>(undefined)

export function BookmarkListContextProvider({ children }: { children: ReactNode }) {
    const [bookmarkList, setBookmarkList] = useState<Bookmark[]>(bookmarkData.bookmarks)
    const [bookmarkListArchived, setBookmarkListArchived] = useState<Bookmark[]>(bookmarkData.bookmarks.filter(bookmark => bookmark.isArchived))
    const [searchQuery, setSearchQuery]= useState<string>("")

    return (
        <BookmarkListContext.Provider value={{ 
            bookmarkList, bookmarkListArchived,
            setBookmarkList, setBookmarkListArchived, 
            searchQuery, setSearchQuery
        }}>
            {children}
        </BookmarkListContext.Provider>
    )
}

export function useBookmarkList() {
    const context = useContext(BookmarkListContext)
    if (!context) throw new Error("useBookmarkList must be used within BookmarkListContextProvider")
    return context
}