export type Nullable<T> = T | null

export type SortBookmarksBy = "recently-added" | "most-visited" | "recently-visited"
export type DialogAction = "delete" | "archive" | "unarchive"
export type ToastAction = "bookmark-added"
    | "bookmark-edited"
    | "bookmark-link-copied"
    | "bookmark-pinned"
    | "bookmark-archived"
    | "bookmark-unarchived"
    | "bookmark-deleted"

export type NotificationType =
    | ToastAction
    | "bookmark-updated"
    | "bookmark-pinned"
    | "bookmark-restored"
    | "bookmark-unpinned"