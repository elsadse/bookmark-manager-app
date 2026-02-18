import { useAuthContext } from "@/hooks/useAuthContext"
import type { JSX } from "react"
import { Navigate, Outlet } from "react-router"

export function ProtectedRoute(): JSX.Element {
    const { authenticatedUser } = useAuthContext()
    
    if (authenticatedUser === null) {
        return <Navigate to="login" replace/>
    } 

    return <Outlet/>
}