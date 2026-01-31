import { InputField } from "@/components/auth/FormContainerSignIn"
import iconClose from "@/assets/images/icon-close.svg"
import { useState } from "react"

export function AddBookmark({ onClose }: { onClose: () => void }) {

    return (
        <BookmarkForm onClose={onClose} titleButton="Add Bookmark"
            titleForm="Add a Bookmark" descriptionForm="Save a link with details to keep your collection organized."
        />
    )
}

export function EditBookmark({ onClose }: { onClose: () => void }) {

    return (
        <BookmarkForm onClose={onClose} titleButton="Save Bookmark"
            titleForm="Edit bookmark" descriptionForm="Update your saved link details — change the title, description, URL, or tags anytime."
        />
    )
}


export function BookmarkForm({ onClose, titleForm, descriptionForm, titleButton }: { onClose: () => void, titleForm: string, descriptionForm: string, titleButton: string }) {
    const [title, setTitle] = useState("")
    const [description, setDescription] = useState("")
    const [url, setUrl] = useState("")
    const [tags, setTags] = useState<string>("")

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
                        <input type="text"
                            name="tags"
                            onChange={e => setTags(e.target.value)}
                            value={tags}
                            className="p-3 rounded-8 border border-neutral-500 focus:outline-none placeholder:text-preset-4" placeholder="e.g. design, learning, tools"
                        />
                    </div>
                </div>
                <div className="flex flex-row justify-end gap-x-4">
                    <button className="w-35.5 flex justify-center items-center gap-x-1 px-4 py-3 rounded-8 bg-neutral-0 border border-neutral-400">
                        <span className="text-center px-0.5 text-neutral-900">Cancel</span>
                    </button>
                    <button className="flex justify-center items-center gap-x-1 px-4 py-3 rounded-8 bg-teal-700">
                        <span className="text-center px-0.5 text-neutral-0">{titleButton}</span>
                    </button>
                </div>
            </div>
        </>
    )
}
