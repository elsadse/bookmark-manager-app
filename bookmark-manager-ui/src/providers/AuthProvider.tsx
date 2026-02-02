import { type ReactNode, useState } from "react"
import type { Nullable } from "@/types"
import { useNavigate } from "react-router"
import { authLogin, authLogout, authRegister } from "@/api/auth"
import type { AuthApiResponse } from "@/api/auth/schema"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"
import type { UnauthorizedApiResponse } from "@/api/errors/schema"
import { useLocalStorage } from "@/hooks/useLocalStorage"
import { AuthContext, type AuthenticatedUser } from "@/context/AuthContext"

export function AuthProvider({ children }: { children: ReactNode }): ReactNode {
    const { value, setLocalStorageValue } = useLocalStorage<AuthenticatedUser>("AuthenticatedUser")
    const [authenticatedUser, setAuthenticatedUser] = useState<Nullable<AuthenticatedUser>>(value)
    const [error, setError] = useState<Nullable<UnauthorizedApiResponse>>(null)

    const navigate = useNavigate()

    function login(email: string, password: string): void {
        authLogin({ email, password })
            .then((response: AuthApiResponse) => {
                setAuthenticatedUser(response)
                setLocalStorageValue(response)

                navigate("/", { replace: true })
            })
            .catch((error) => {
                if (error instanceof UnauthorizedApiError) {
                    setError(error.response)
                }
            })
    }

    function logout(): void {
        authLogout()
            .catch((error) => {
                console.log("Logout failed:", error)
            })
            .finally(() => {
                setAuthenticatedUser(null)
                setLocalStorageValue(null)

                navigate("login", { replace: true })
            })
    }

    function register(fullname: string, email: string, password: string): void {
        authRegister({ fullname, email, password })
            .then((response: AuthApiResponse) => {
                setAuthenticatedUser(response)
                setLocalStorageValue(response)

                navigate("/", { replace: true })
            })
    }

    return (
        <AuthContext.Provider value={{ authenticatedUser, login, logout, register, error }}>
            {children}
        </AuthContext.Provider>
    )
}