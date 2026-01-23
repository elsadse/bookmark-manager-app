import { useBookmarkList } from "@/context/BookmarkListContext"
import type { Bookmark } from "@/types"
import { createContext, useContext, useState, type ReactNode } from "react"

type FilterTagsContextType = {
    selectedTagsList: string[],
    addTag: (tag: string) => void,
    deleteTag: (tag: string) => void,
    filteredBookmarkList: Bookmark[],
    filteredBookmarkListArchived: Bookmark[]
}

const FilterTagsContext = createContext<FilterTagsContextType | undefined>(undefined)

export function FilterTagsContextProvider({ children }: { children: ReactNode }) {
    const [selectedTagsList, setSelectedTagsList] = useState<string[]>([])
    const {bookmarkList, bookmarkListArchived}= useBookmarkList()

    function addTag(tag: string) {
        const newSelectedTagsList = [...selectedTagsList]
        newSelectedTagsList.push(tag)
        setSelectedTagsList(newSelectedTagsList)
    }

    function deleteTag(tag: string) {
        const newSelectedTagsList = [...selectedTagsList]
        setSelectedTagsList(newSelectedTagsList.filter(item => item !== tag))
    }

    const filteredBookmarkList = getFilteredByTagsList(bookmarkList, selectedTagsList)

    const filteredBookmarkListArchived = getFilteredByTagsList(bookmarkListArchived, selectedTagsList)

    return (
        <FilterTagsContext.Provider value={{ 
            selectedTagsList, addTag, deleteTag, 
            filteredBookmarkList, filteredBookmarkListArchived
        }}>
            {children}
        </FilterTagsContext.Provider>
    )
}

export function useFilterTagsContext() {
    const context = useContext(FilterTagsContext)
    if (!context) throw new Error("useFilterTagsContext must be used within FilterTagsContextProvider")
    return context
}

function getFilteredByTagsList(list: Bookmark[], tags: string[]){
    if(tags.length>0){
        const filteredList= list.filter(bookmark => bookmark.tags.some(tag => tags.includes(tag)))
        console.log(filteredList)
        return filteredList
    } 
    return list
}