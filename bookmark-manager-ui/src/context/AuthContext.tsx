import type { User } from "@/types"
import { createContext } from "react"

export type AuthContextType = {
    authUser: User|null,
    login: (email: string, password: string) => void,
}

export const AuthContext = createContext<AuthContextType| undefined>(undefined)

