import { InputField } from "@/components/auth/FormContainerSignIn"
import iconClose from "@/assets/images/icon-close.svg"
import { useState } from "react"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { addBookmark } from "@/api/bookmarks"
import { useShallow } from "zustand/shallow"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import { fetchTagCount } from "@/api/tags"
import type { TagCount } from "@/api/tags/schema"

export function AddBookmark({ onClose }: { onClose: () => void }) {

    const queryClient = useQueryClient()
    const { mutate: addBookmarkFn } = useMutation({
        mutationFn: addBookmark,
        onSuccess: async () => await Promise.all([
            queryClient.invalidateQueries({ queryKey: ["bookmarks"] }),
            queryClient.invalidateQueries({ queryKey: ["tags"] })
        ])
    })
    const { setIsNotificationOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsNotificationOpen: store.setIsNotificationOpen,
        }))
    )

    function handleAddBookmark(data: { title: string, description: string, url: string, tags: string[] }) {
        addBookmarkFn(data)
        onClose()
        setIsNotificationOpen(true, "bookmark-added")
    }

    return (
        <BookmarkForm onClose={onClose} titleButton="Add Bookmark" onsubmit={handleAddBookmark}
            titleForm="Add a Bookmark" descriptionForm="Save a link with details to keep your collection organized."
        />
    )
}

export function EditBookmark({ onClose }: { onClose: () => void }) {

    function handleEditBookmark() { }

    return (
        <BookmarkForm onClose={onClose} titleButton="Save Bookmark" onsubmit={handleEditBookmark}
            titleForm="Edit bookmark" descriptionForm="Update your saved link details — change the title, description, URL, or tags anytime."
        />
    )
}

type BookmarkFormProps = {
    onsubmit: (data: {
        title: string,
        description: string,
        url: string,
        tags: string[]
    }) => void,
    onClose: () => void,
    titleForm: string,
    descriptionForm: string,
    titleButton: string
}


export function BookmarkForm({ onsubmit, onClose, titleForm, descriptionForm, titleButton }: BookmarkFormProps) {
    const [title, setTitle] = useState("")
    const [description, setDescription] = useState("")
    const [url, setUrl] = useState("")

    function handleSubmit(e: React.FormEvent) {
        e.preventDefault()
        onsubmit({
            title,
            description,
            url,
            tags: tags.map(t => t.trim())
        })
    }

    const [tags, setTags] = useState<string[]>([])
    const [currentTag, setCurrentTag] = useState<string>("")
    const { data: tagSuggestions } = useQuery({
        queryKey: ["tags"],
        queryFn: fetchTagCount,
        select: (tags: TagCount[]): TagCount[] =>
            [...tags].sort((a: TagCount, b: TagCount): number => a.name.localeCompare(b.name))
    })

    return (
        <>
            <div className="relative flex flex-col w-85.75 md:w-142.5 gap-y-8 px-5 py-6 md:p-8 rounded-16 bg-neutral-0">
                <div className="flex flex-col gap-y-2">
                    <div onClick={onClose}
                        className="absolute right-4 top-4 size-8 flex justify-center items-center gap-x-1 rounded-8 border border-neutral-400 cursor-pointer">
                        <img src={iconClose} className="size-5" alt="icon close" />
                    </div>
                    <span className="text-preset-1 text-neutral-900">{titleForm}</span>
                    <span className="text-preset-4-md text-neutral-800"> {descriptionForm}</span>
                </div>
                <form onSubmit={handleSubmit} className="flex flex-col gap-y-8">
                    <div className="flex flex-col gap-y-5">
                        <InputField
                            name="title"
                            onChange={e => setTitle(e.target.value)}
                            value={title}
                            typeInput="text"
                            labelInput="Title*"
                        />
                        <div className="flex flex-col gap-y-1.5">
                            <div className="flex flex-col gap-y-1.5">
                                <span className="text-preset-4">Description*</span>
                                <textarea
                                    name="description"
                                    onChange={e => setDescription(e.target.value)}
                                    value={description}
                                    className="p-3 rounded-8 border border-neutral-500 focus:outline-none "
                                />
                            </div>
                            <div className="flex justify-end gap-x-2.5">
                                <span className="text-preset-5 text-neutral-800">0/280</span>
                            </div>
                        </div>
                        <InputField
                            name="url"
                            onChange={e => setUrl(e.target.value)}
                            value={url}
                            typeInput="url"
                            labelInput="Website Url*"
                        />
                        <div className="flex flex-col gap-y-1.5">
                            <span className="text-preset-4">Tags*</span>
                            {
                                tags.map((tag: string, index: number) => (
                                    <div key={index}
                                        className="bg-teal-700 text-neutral-0 px-2 py-1 rounded-8 flex items-center gap-x-1.5 text-preset-4">
                                        {tag}
                                        <button
                                            onClick={(): void => setTags(tags.filter((_: string, i: number): boolean => i !== index))}
                                            className="text-neutral-0 cursor-pointer"
                                        >
                                            ×
                                        </button>
                                    </div>
                                ))
                            }
                            <input className="p-3 rounded-8 border border-neutral-500 dark:border-neutral-d-300 dark:bg-neutral-d-600 focus:outline-none
                focus:ring ring-teal-700 cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-500"
                                type="text"
                                value={currentTag}
                                onChange={(e): void => setCurrentTag(e.target.value)}
                                onKeyDown={(e): void => {
                                    if (e.key === "," || e.key === "Enter") {
                                        e.preventDefault()
                                        if (currentTag.trim()) {
                                            setTags([...tags, currentTag.trim()])
                                            setCurrentTag("")
                                        }
                                    }
                                }}
                                onBlur={(): void => {
                                    if (currentTag.trim()) {
                                        setTags([...tags, currentTag.trim()])
                                        setCurrentTag("")
                                    }
                                }}
                                list="SUGGESTIONS_LIST_ID"
                            />
                            <datalist id="SUGGESTIONS_LIST_ID">
                                {tagSuggestions
                                    ?.filter((s) => s.name.toLowerCase().includes(currentTag.toLowerCase().trim()))
                                    ?.map((suggestion) => (
                                        <option key={suggestion.id} value={suggestion.name} />
                                    ))}
                            </datalist>
                        </div>
                    </div>
                    <div className="flex flex-row justify-end gap-x-4">
                        <button onClick={onClose} type="button"
                            className="w-35.5 flex justify-center items-center gap-x-1 px-4 py-3 rounded-8 bg-neutral-0 border border-neutral-400 cursor-pointer">
                            <span className="text-center px-0.5 text-neutral-900">Cancel</span>
                        </button>
                        <button type="submit"
                            className="flex justify-center items-center gap-x-1 px-4 py-3 rounded-8 bg-teal-700 cursor-pointer">
                            <span className="text-center px-0.5 text-neutral-0">{titleButton}</span>
                        </button>
                    </div>
                </form>
            </div>
        </>
    )
}
