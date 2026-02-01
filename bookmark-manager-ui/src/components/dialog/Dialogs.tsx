import iconClose from "@/assets/images/icon-close.svg"

export function ArchiveDialog({ onClose }: { onClose: () => void }) {

    return (
        <Dialog title="Archive bookmark" titleButton="Archive" onClose={onClose}
            isDelete={false} description="Are you sure you want to archive this bookmark?"
        />
    )
}

export function UnArchiveDialog({ onClose }: { onClose: () => void }) {

    return (
        <Dialog title="UnArchive bookmark" titleButton="Unarchive" onClose={onClose}
            isDelete={false} description="Move this bookmark back to your active list?"
        />
    )
}

export function DeleteDialog({ onClose }: { onClose: () => void }) {

    return (
        <Dialog title="Delete bookmark" titleButton="Delete Permanently" onClose={onClose}
            isDelete={true} description="Are you sure you want to delete this bookmark?"
        />
    )
}

export function Dialog({ title, description, titleButton, isDelete, onClose }: { title: string, description: string, titleButton: string, isDelete: boolean, onClose: () => void }) {

    return (
        <>
            <div className="fixed inset-0 bg-[#131313] opacity-63 h-screen w-screen z-10" />
            <div className="absolute w-85 md:w-112.5 flex flex-col gap-y-6 p-6 rounded-12 bg-neutral-0 z-10">
                <div className="flex flex-col gap-y-2">
                    <div onClick={onClose}
                        className="absolute right-0 top-0 size-8 flex justify-center items-center gap-x-1 cursor-pointer">
                        <img src={iconClose} className="size-5" alt="icon close" />
                    </div>
                    <div className="flex flex-col gap-y-2">
                        <span className="text-preset-1 text-neutral-900">{title}</span>
                        <span className="text-preset-4-md text-neutral-800"> {description}</span>
                    </div>
                </div>
                <div className="flex flex-row justify-end gap-x-8">
                    <button onClick={onClose}
                        className="flex justify-center items-center gap-x-1 px-4 py-3 rounded-8 bg-neutral-0 border border-neutral-400 cursor-pointer">
                        <span className="text-center px-0.5 text-neutral-900">Cancel</span>
                    </button>
                    <button className={`flex justify-center items-center 
                        gap-x-1 px-4 py-3 rounded-8 cursor-pointer
                        ${isDelete ? "bg-red-800" : "bg-teal-700"}`}>
                        <span className="text-center px-0.5 text-neutral-0">{titleButton}</span>
                    </button>
                </div>
            </div>
        </>
    )
}