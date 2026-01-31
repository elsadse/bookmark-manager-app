import { InputField } from "@/components/auth/FormContainerSignIn"
import iconClose from "@/assets/images/icon-close.svg"

export function AddBookmark({ onClose }: { onClose: () => void }) {

    return (
        <BookmarkForm onClose={onClose} titleButton="Add Bookmark"
            title="Add a Bookmark" description="Save a link with details to keep your collection organized."
        />
    )
}

export function EditBookmark({ onClose }: { onClose: () => void }) {

    return (
        <BookmarkForm onClose={onClose} titleButton="Save Bookmark"
            title="Edit bookmark" description="Update your saved link details — change the title, description, URL, or tags anytime."
        />
    )
}


export function BookmarkForm({ onClose, title, description, titleButton }: { onClose: () => void, title: string, description: string, titleButton: string }) {

    return (
        <>
            <div className="fixed inset-0 bg-[#131313] opacity-63 h-screen w-screen" />
            <div className="absolute flex flex-col w-85.75 md:w-142.5 gap-y-8 px-5 py-6 md:p-8 rounded-16 bg-neutral-0">
                <div className="flex flex-col gap-y-2">
                    <div className="">
                        <div onClick={onClose}
                            className="absolute right-4 top-4 size-8 flex justify-center items-center gap-x-1 rounded-8 border border-neutral-400 cursor-pointer">
                            <img src={iconClose} className="size-5" alt="icon close" />
                        </div>
                        <span className="text-preset-1 text-neutral-900">{title}</span>
                    </div>
                    <span className="text-preset-4-md text-neutral-800"> {description}</span>
                </div>
                <div className="flex flex-col gap-y-5">
                    <InputField typeInput="text" labelInput="Title*" />
                    <div className="flex flex-col gap-y-1.5">
                        <div className="flex flex-col gap-y-1.5">
                            <span className="text-preset-4">Description*</span>
                            <textarea className="p-3 rounded-8 border border-neutral-500 focus:outline-none " />
                        </div>
                        <div className="flex justify-end gap-x-2.5">
                            <span className="text-preset-5 text-neutral-800">0/280</span>
                        </div>
                    </div>
                    <InputField typeInput="url" labelInput="Website Url*" />
                    <div className="flex flex-col gap-y-1.5">
                        <span className="text-preset-4">Tags*</span>
                        <input type="text" className="p-3 rounded-8 border border-neutral-500 focus:outline-none placeholder:text-preset-4" placeholder="e.g. design, learning, tools" />
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
