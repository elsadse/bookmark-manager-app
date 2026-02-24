import { z } from "zod"

export const TagApiResponseSchema = z.object({
    tagId: z.number(),
    name: z.string(),
    count: z.number(),
    archivedCount: z.number(),
})
export type Tag = z.infer<typeof TagApiResponseSchema>