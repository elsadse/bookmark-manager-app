import { type AuthApiResponse, AuthApiResponseSchema } from "@/api/auth/schema"
import { BadRequestApiResponseSchema, ConflictApiResponseSchema, UnauthorizedApiResponseSchema } from "@/api/errors/schema"
import { BadRequestApiError } from "@/api/errors/BadRequestApiError"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"
import { ConflictApiError } from "@/api/errors/ConflictApiError"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function authLogin({ email, password }: { email: string, password: string }): Promise<AuthApiResponse> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
        credentials: "include"
    })
    if (response.status === 400) {
        const parsedResponse = BadRequestApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new BadRequestApiError(parsedResponse.data)
    }
    if (response.status === 401) {
        const parsedResponse = UnauthorizedApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new UnauthorizedApiError(parsedResponse.data)
    }
    if (response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }

    const parsedResponse = AuthApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        throw parsedResponse.error
    }

    return parsedResponse.data
}

export async function authLogout(): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/logout`, {
        method: "POST"
    })
    if (response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
}

export async function authRegister({ fullname, email, password }: { fullname: string, email: string, password: string }): Promise<AuthApiResponse> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ fullname, email, password }),
        credentials: "include"
    })
    if (response.status === 400) {
        const parsedResponse = BadRequestApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new BadRequestApiError(parsedResponse.data)
    }
    if (response.status === 409) {
        const parsedResponse = ConflictApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new ConflictApiError(parsedResponse.data)
    }
    if (response.status !== 201) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }

    const parsedResponse = AuthApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        throw parsedResponse.error
    }

    return parsedResponse.data
}