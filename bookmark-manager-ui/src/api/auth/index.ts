import { ApiResponseSchema, type ApiResponse } from "@/api/auth/schema";
import type { Result } from "@/api/util";

export async function authLogin({ email, password }: { email: string, password: string }): Promise<Result<ApiResponse>> {
    const response = await fetch('http://localhost:5160/api/auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            email,
            password
        }),
        credentials: 'include'
    });
    if (![200, 400, 401, 500].includes(response.status)) {
        return { success: false, error: new Error(`Unexpected status code: ${response.status}`) }
    }
    console.log(response.json())
    const parsedResponse = ApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        return { success: false, error: parsedResponse.error }
    }
    return { success: true, data: parsedResponse.data }
}