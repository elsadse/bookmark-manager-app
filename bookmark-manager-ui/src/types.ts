export type Nullable<T> = T | null

export type SortBookmarksBy = "recently-added" | "most-visited" | "recently-visited"
export type Action = "delete" | "archive" | "unarchive" | "edit"
export type ToastAction = "bookmark-added"
    | "bookmark-edited"
    | "bookmark-link-copied"
    | "bookmark-pinned"
    | "bookmark-archived"
    | "bookmark-unarchived"
    | "bookmark-deleted"

export type NotificationType =
    | ToastAction
    | "bookmark-pinned"
    | "bookmark-restored"
    | "bookmark-unpinned"