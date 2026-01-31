import type { ConflictApiResponse } from "@/api/errors/schema"

export class ConflictApiError extends Error {
    readonly response: ConflictApiResponse

    constructor(response: ConflictApiResponse) {
        super(response.detail)
        this.name = response.title
        this.response = response
    }
}