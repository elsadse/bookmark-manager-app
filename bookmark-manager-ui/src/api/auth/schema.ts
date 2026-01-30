import { z } from "zod"

export const ApiResponseSchema = z.object({
    Fullname: z.string(),
    Email: z.string()
})

export type ApiResponse = z.infer<typeof ApiResponseSchema>