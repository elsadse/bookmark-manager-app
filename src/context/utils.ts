import type { Bookmark } from "@/types"

export function getFilteredByTagsList(list: Bookmark[], tags: string[]){
    if(tags.length>0){
        const filteredList= list.filter(bookmark => bookmark.tags.some(tag => tags.includes(tag)))
        console.log(filteredList)
        return filteredList
    } 
    return list
}

