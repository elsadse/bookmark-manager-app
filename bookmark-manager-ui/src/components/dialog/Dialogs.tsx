import { deleteBookmark, toggleArchive } from "@/api/bookmarks"
import { CloseIcon } from "@/components/icons/CloseIcon"
import { useGlobalStore, type GlobalStore } from "@/hooks/useGlobalStore"
import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useShallow } from "zustand/shallow"


export function ToggleArchiveDialog({ onClose }: { onClose: () => void }) {
    const queryClient = useQueryClient()
    const { bookmarkSelected, setBookmarkSelected, setIsNotificationOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            bookmarkSelected: store.bookmarkSelected,
            setBookmarkSelected: store.setBookmarkSelected,
            setIsNotificationOpen: store.setIsNotificationOpen,
        }))
    )
    const { mutate: toggleArchiveFn } = useMutation({
        mutationFn: toggleArchive,
        onSuccess: async () => {
            await Promise.all([
                queryClient.invalidateQueries({ queryKey: ["bookmarks"] }),
                queryClient.invalidateQueries({ queryKey: ["tags"] })
            ])
            onClose()
            setBookmarkSelected(null)
            setIsNotificationOpen(true, bookmarkSelected?.isArchived ? "bookmark-restored" : "bookmark-archived")
        }
    })

    function handleToggleArchive() {
        toggleArchiveFn({ bookmarkId: bookmarkSelected!.bookmarkId })
    }

    return (
        <Dialog title={bookmarkSelected?.isArchived ? "UnArchive bookmark" : "Archive bookmark"} titleButton={bookmarkSelected?.isArchived ? "Unarchive" : "Archive"} onClose={onClose} onClickDialog={handleToggleArchive}
            isDelete={false} description={bookmarkSelected?.isArchived ? `Move back to your active list` : `Are you sure you want to archive`}
            titleBookmark={bookmarkSelected!.title}
        />
    )
}

export function DeleteDialog({ onClose }: { onClose: () => void }) {
    const queryClient = useQueryClient()
    const { mutate: deleteFn } = useMutation({
        mutationFn: deleteBookmark,
        onSuccess: async () => {
            await Promise.all([
                queryClient.invalidateQueries({ queryKey: ["bookmarks"] }),
                queryClient.invalidateQueries({ queryKey: ["tags"] })
            ])
            onClose()
            setBookmarkSelected(null)
            setIsNotificationOpen(true, "bookmark-deleted")
        }
    })

    const { bookmarkSelected, setBookmarkSelected, setIsNotificationOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            bookmarkSelected: store.bookmarkSelected,
            setBookmarkSelected: store.setBookmarkSelected,
            setIsNotificationOpen: store.setIsNotificationOpen
        }))
    )

    function handleDelete() {
        deleteFn({ bookmarkId: bookmarkSelected!.bookmarkId })
    }

    return (
        <Dialog title="Delete bookmark" titleButton="Delete Permanently" onClose={onClose} onClickDialog={handleDelete}
            isDelete={true} description={`Are you sure you want to delete`} titleBookmark={bookmarkSelected!.title}
        />
    )
}

export function Dialog({ titleBookmark, title, description, titleButton, isDelete, onClose, onClickDialog }: { titleBookmark: string, title: string, description: string, titleButton: string, isDelete: boolean, onClose: () => void, onClickDialog: () => void }) {

    return (
        <>
            <div className="fixed inset-0 bg-[#131313] opacity-63 h-screen w-screen z-10" />
            <div className="absolute w-85 md:w-112.5 flex flex-col gap-y-6 p-6 rounded-12 bg-neutral-0 z-10">
                <div className="flex flex-col gap-y-2">
                    <div onClick={onClose}
                        className="absolute right-0 top-0 size-8 flex justify-center items-center gap-x-1 cursor-pointer">
                        <CloseIcon className="size-5" />
                    </div>
                    <div className="flex flex-col gap-y-2">
                        <span className="text-preset-1 text-neutral-900">{title}</span>
                        <p className="text-preset-4-md text-neutral-800">
                            {description}
                            <span className="font-bold text-preset-3"> {titleBookmark}</span> ?
                        </p>
                    </div>
                </div>
                <div className="flex flex-row justify-end gap-x-8">
                    <button onClick={onClose}
                        className="flex justify-center items-center gap-x-1 px-4 py-3 rounded-8 bg-neutral-0 border border-neutral-400 cursor-pointer">
                        <span className="text-center px-0.5 text-neutral-900">Cancel</span>
                    </button>
                    <button onClick={onClickDialog}
                        className={`flex justify-center items-center 
                        gap-x-1 px-4 py-3 rounded-8 cursor-pointer
                        ${isDelete ? "bg-red-800" : "bg-teal-700"}`}>
                        <span className="text-center px-0.5 text-neutral-0 dark:text-neutral-d-0">{titleButton}</span>
                    </button>
                </div>
            </div>
        </>
    )
}