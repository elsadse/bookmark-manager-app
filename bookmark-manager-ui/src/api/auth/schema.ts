import { z } from "zod"

export const ApiResponseSchema = z.object({
    fullname: z.string(),
    email: z.string()
})

export type ApiResponse = z.infer<typeof ApiResponseSchema>