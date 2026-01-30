import { Navigate } from "react-router"
import { useAuth } from "@/hooks/useAuth"
import type { JSX } from "react"

export function PrivateRoute({ children }: { children: JSX.Element }) {
  const { authUser } = useAuth()

  if (!authUser) {
    return <Navigate to="/bookmark-manager-app/" replace />
  }

  return children
}
