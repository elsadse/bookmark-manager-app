import { type ComponentType, type JSX, useEffect } from "react"
import type { NotificationType, Nullable } from "@/types"
import { CheckIcon } from "@/components/icons/CheckIcon"
import { CopyIcon } from "@/components/icons/CopyIcon"
import { PinIcon } from "@/components/icons/PinIcon"
import { UnpinIcon } from "@/components/icons/UnpinIcon"
import { ArchivedIcon } from "@/components/icons/ArchivedIcon"
import { UnarchivedIcon } from "@/components/icons/UnarchivedIcon"
import { DeleteIcon } from "@/components/icons/DeleteIcon"
import { CloseIcon } from "@/components/icons/CloseIcon"

export function NotificationContainer({ notificationType, closeNotification }: {
    notificationType: Nullable<NotificationType>,
    closeNotification: () => void
}): JSX.Element {

    useEffect(() => {
        const timeout = setTimeout(closeNotification, 5000)
        return () => clearTimeout(timeout)
    }, [closeNotification])

    const { Icon, text } = getLogoAndTextNotificationFrom(notificationType)

    return (
        <div
            className="w-85 h-10.25 flex flex-row gap-x-2 px-3 py-2.5 items-center justify-between rounded-8 bg-neutral-0 dark:bg-neutral-d-500 border border-neutral-300">
            <Icon className="w-5 h-5 dark:fill-neutral-d-500"/>
            <p className="text-preset-4-md text-neutral-900">{text}</p>
            <button onClick={closeNotification} className="cursor-pointer">
                <CloseIcon className="w-4 h-4"/>
            </button>
        </div>
    )
}

type NotificationConfig = {
  Icon: ComponentType<{ className?: string }>;
  text: string;
}

function getLogoAndTextNotificationFrom(type: Nullable<NotificationType>): NotificationConfig {
    switch (type) {
        case "bookmark-added":
            return { Icon: CheckIcon, text: "Bookmark added successfully." }
        case "bookmark-edited":
            return { Icon: CheckIcon, text: "Bookmark updated successfully" }
        case "bookmark-link-copied":
            return { Icon: CopyIcon, text: "Bookmark link copied to clipboard." }
        case "bookmark-pinned":
            return { Icon: PinIcon, text: "Bookmark pinned to the top." }
        case "bookmark-unpinned":
            return { Icon: UnpinIcon, text: "Bookmark unpinned from the top." }
        case "bookmark-archived":
            return { Icon: ArchivedIcon, text: "Bookmark archived." }
        case "bookmark-restored":
            return { Icon: UnarchivedIcon, text: "Bookmark restored." }
        case "bookmark-deleted":
            return { Icon: DeleteIcon, text: "Bookmark permanently deleted." }
        default:
            return { Icon: CheckIcon, text: "Action executed successfully." }
    }
}