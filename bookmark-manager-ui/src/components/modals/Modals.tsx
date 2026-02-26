import { InputField } from "@/components/auth/FormContainerSignIn"
import { useState } from "react"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { addBookmark, updateBookmark } from "@/api/bookmarks"
import { useShallow } from "zustand/shallow"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import type { Nullable } from "@/types"
import type { ErrorApiResponse } from "@/api/errors/schema"
import { ApiError } from "@/api/errors/ApiError"
import { CloseIcon } from "@/components/icons/CloseIcon"
import { LoadingIcon } from "@/components/icons/LoadingIcon"
import type { Bookmark } from "@/api/bookmarks/schema"
import { fetchTags } from "@/api/tags"
import type { Tag } from "@/api/tags/schema"

export function AddBookmark({ onClose }: { onClose: () => void }) {

    const queryClient = useQueryClient()
    const { mutate, isPending, error: mutationError } = useMutation({
        mutationFn: addBookmark,
        onSuccess: async () => {
            await Promise.all([
                queryClient.invalidateQueries({ queryKey: ["bookmarks"] }),
                queryClient.invalidateQueries({ queryKey: ["tags"] })
            ])
            onClose()
            setIsNotificationOpen(true, "bookmark-added")
        },
        /*onError: (err) => {
            console.error("Erreur complète de la mutation :", err)
            console.log("Response de l’erreur :", (err as any).response)
        },*/
    })
    const error: Nullable<ErrorApiResponse> = mutationError instanceof ApiError ? mutationError.response : null

    const { setIsNotificationOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsNotificationOpen: store.setIsNotificationOpen,
        }))
    )

    function handleAddBookmark(data: { title: string, description: string, url: string, tags: string[] }) {
        mutate(data)
    }

    return (
        <BookmarkForm onClose={onClose} titleButton="Add Bookmark" onsubmit={handleAddBookmark}
            titleForm="Add a Bookmark" descriptionForm="Save a link with details to keep your collection organized."
            error={error} isPending={isPending}
        />
    )
}

export function EditBookmark({ onClose }: { onClose: () => void }) {
    const queryClient = useQueryClient()
    const { setIsNotificationOpen, bookmarkSelected } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsNotificationOpen: store.setIsNotificationOpen,
            bookmarkSelected: store.bookmarkSelected
        }))
    )
    const { mutate, isPending, error: mutationError } = useMutation({
        mutationFn: updateBookmark,
        onSuccess: async () => {
            await Promise.all([
                queryClient.invalidateQueries({ queryKey: ["bookmarks"] }),
                queryClient.invalidateQueries({ queryKey: ["tags"] })
            ])
            onClose()
            setIsNotificationOpen(true, "bookmark-edited")
        },
    })
    const error: Nullable<ErrorApiResponse> = mutationError instanceof ApiError ? mutationError.response : null

    function handleEditBookmark(data: { title: string, description: string, url: string, tags: string[] }) {
        if (bookmarkSelected)
            mutate({ ...data, bookmarkId: bookmarkSelected.bookmarkId })
    }

    return (
        <BookmarkForm onClose={onClose} titleButton="Save Bookmark" onsubmit={handleEditBookmark} bookmark={bookmarkSelected}
            titleForm="Edit bookmark" descriptionForm="Update your saved link details — change the title, description, URL, or tags anytime."
            error={error} isPending={isPending}
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
    titleButton: string,
    error?: Nullable<ErrorApiResponse>,
    isPending: boolean,
    bookmark?: Nullable<Bookmark>
}


export function BookmarkForm({ onsubmit, onClose, titleForm, descriptionForm, titleButton, error, isPending, bookmark }: BookmarkFormProps) {
    const [title, setTitle] = useState(bookmark ? bookmark.title : "")
    const [description, setDescription] = useState(bookmark ? bookmark.description : "")
    const [url, setUrl] = useState(bookmark ? bookmark.url : "")
    const maxDescriptionLength: number = 280
    const isOverLimitDescription = description.length > maxDescriptionLength

    function handleSubmit(e: React.FormEvent) {
        e.preventDefault()
        if (description.length <= maxDescriptionLength) {
            onsubmit({
                title,
                description,
                url,
                tags: tags.map(t => t.trim())
            })
        }
    }

    const [tags, setTags] = useState<string[]>(bookmark ? bookmark.tags : [])
    const [currentTag, setCurrentTag] = useState<string>("")
    const { data: tagSuggestions } = useQuery({
        queryKey: ["tags"],
        queryFn: fetchTags,
        select: (tags: Tag[]): Tag[] =>
            [...tags].sort((a: Tag, b: Tag): number => a.name.localeCompare(b.name))
    })

    return (
        <>
            <div className="relative flex flex-col w-85.75 md:w-142.5 gap-y-8 px-5 py-6 md:p-8 rounded-16 bg-neutral-0 max-h-screen overflow-y-auto">
                <div className="flex flex-col gap-y-2">
                    <div onClick={onClose}
                        className="absolute right-4 top-4 size-8 flex justify-center items-center gap-x-1 rounded-8 border border-neutral-400 cursor-pointer">
                        <CloseIcon className="size-5" />
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
                            className={`${error !== null && "errors" in error! && "Title" in error.errors ? "border border-red-600 focus:border-none" : ""}`}
                        />
                        {
                            error !== null && "errors" in error! && "Title" in error.errors &&
                            <div className="flex flex-col gap-y-1.5">
                                {
                                    error.errors["Title"].map((error: string, index: number) => (
                                        <span key={index} className="text-preset-4 text-red-800">{error}</span>
                                    ))
                                }
                            </div>
                        }
                        <div className="flex flex-col gap-y-1.5">
                            <div className="flex flex-col gap-y-1.5">
                                <span className="text-preset-4">Description*</span>
                                <textarea
                                    name="description"
                                    onChange={e => setDescription(e.target.value)}
                                    value={description}
                                    className={`p-3 rounded-8 border border-neutral-500 focus:outline-none focus:ring ring-teal-700 hover:bg-neutral-100 dark:hover:bg-neutral-d-500 dark:bg-neutral-d-600
                                        ${error !== null && "errors" in error! && "Description" in error.errors ? "border border-red-600 focus:border-none" : ""}
                                        `}
                                />
                            </div>
                            <div className="flex justify-end gap-x-2.5">
                                <span className={`text-preset-5 text-neutral-800 ${isOverLimitDescription ? "text-red-600" : ""}`}>
                                    {description.length}/{maxDescriptionLength}
                                </span>
                            </div>
                            {
                                error !== null && "errors" in error! && "Description" in error.errors &&
                                <div className="flex flex-col gap-y-1.5">
                                    {
                                        error.errors["Description"].map((error: string, index: number) => (
                                            <span key={index} className="text-preset-4 text-red-800">{error}</span>
                                        ))
                                    }
                                </div>
                            }
                        </div>
                        <InputField
                            name="url"
                            onChange={e => setUrl(e.target.value)}
                            value={url}
                            typeInput="url"
                            labelInput="Website Url*"
                            className={`${error !== null && "errors" in error! && "Url" in error.errors ? "border border-red-600 focus:border-none" : ""}`}
                        />
                        {
                            error !== null && "errors" in error! && "Url" in error.errors &&
                            <div className="flex flex-col gap-y-1.5">
                                {
                                    error.errors["Url"].map((error: string, index: number) => (
                                        <span key={index} className="text-preset-4 text-red-800">{error}</span>
                                    ))
                                }
                            </div>
                        }
                        <div className="flex flex-col gap-y-1.5">
                            <span className="text-preset-4">Tags (comma or Enter separated)*</span>
                            {tags.length !== 0 &&
                                <div className="flex flex-row gap-x-1.5">
                                    {tags.map((tag: string, index: number) => (
                                        <div key={index}
                                            className="bg-teal-700 text-neutral-0 dark:text-neutral-d-0 px-2 py-1 rounded-8 flex items-center gap-x-1.5 text-preset-4">
                                            {tag}
                                            <button type="button"
                                                onClick={(): void => setTags(tags.filter((_: string, i: number): boolean => i !== index))}
                                                className="text-neutral-0 dark:text-neutral-d-0 cursor-pointer"
                                            >
                                                ×
                                            </button>
                                        </div>
                                    ))}
                                </div>
                            }
                            <input className={`p-3 rounded-8 border border-neutral-500 dark:border-neutral-d-300 dark:bg-neutral-d-600 focus:outline-none
                                focus:ring ring-teal-700 dark:ring-neutral-d-0 cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-500
                                ${error !== null && "errors" in error! && "Tags" in error.errors ? "border-red-600 dark:border-red-600 focus:border-none" : ""}
                                `}
                                type="text"
                                value={currentTag}
                                onChange={(e) => { setCurrentTag(e.target.value) }}
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
                            {
                                error !== null && "errors" in error! && "Tags" in error.errors &&
                                <div className="flex flex-col gap-y-1.5">
                                    {
                                        error.errors["Tags"].map((error: string, index: number) => (
                                            <span key={index} className="text-preset-4 text-red-800">{error}</span>
                                        ))
                                    }
                                </div>
                            }
                            <datalist id="SUGGESTIONS_LIST_ID">
                                {tagSuggestions
                                    ?.filter((s) => s.name.toLowerCase().includes(currentTag.toLowerCase().trim()))
                                    ?.map((suggestion) => (
                                        <option key={suggestion.tagId} value={suggestion.name} />
                                    ))}
                            </datalist>
                        </div>
                    </div>
                    {
                        error !== null && "detail" in error! &&
                        <div className="flex flex-col gap-y-1.5">
                            <span className="text-preset-4 text-red-800">{error.detail}</span>
                        </div>
                    }
                    <div className="flex flex-row justify-end gap-x-4">
                        <button onClick={onClose} type="button"
                            className="w-35.5 flex justify-center items-center gap-x-1 px-4 py-3 rounded-8 bg-neutral-0 border border-neutral-400 cursor-pointer">
                            <span className="text-center px-0.5 text-neutral-900">Cancel</span>
                        </button>
                        <button type="submit" disabled={isPending || isOverLimitDescription}
                            className="flex justify-center items-center gap-x-1 px-4 py-3 rounded-8 bg-teal-700 cursor-pointer disabled:opacity-60 disabled:cursor-not-allowed">
                            {
                                isPending && <LoadingIcon className="w-4 h-4 stroke-neutral-0 dark:stroke-neutral-d-0" />
                            }
                            <span className="text-center px-0.5 text-neutral-0 dark:text-neutral-d-0">{titleButton}</span>
                        </button>
                    </div>
                </form>
            </div>
        </>
    )
}
