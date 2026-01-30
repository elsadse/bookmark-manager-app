import { authLogin } from "@/api/auth";
import { ApiResponseSchema } from "@/api/auth/schema";
import { AuthContext } from "@/context/AuthContext";
import type { User } from "@/types";
import { useState, type ReactNode } from "react";
import { useNavigate } from "react-router";

export function AuthProvider({ children }: { children: ReactNode }) {
    const [authUser, setAuthUser] = useState<User | null>(null)
    const navigate = useNavigate()

    async function login(email: string, password: string) {
        const response = await authLogin({ email, password })
        if (!response.success) {
            console.log("Login failed:", response.error);
            return
        }
        const parsedResult = ApiResponseSchema.safeParse(response.data);
        if (parsedResult.success) {
            setAuthUser(parsedResult.data);
            navigate("/bookmark-manager-app/home")
        }
    }

    return (
        <AuthContext.Provider value={{ authUser, login }}>
            {children}
        </AuthContext.Provider>
    )
}