import type { Nullable } from "@/types"
import { useEffect, useState } from "react"
import { useNavigate } from "react-router"

export function useLocalStorage<T>(key: string, initialValue: Nullable<T> = null): {
    value: Nullable<T>,
    setLocalStorageValue: (newValue: Nullable<T>) => void,
    getLocalStorageValue: () => Nullable<T>
} {
    const [value, setValue] = useState<Nullable<T>>(() => {
        const storedValue = localStorage.getItem(key)
        if (storedValue === null) return initialValue

        try {
            return JSON.parse(storedValue) as Nullable<T>
        } catch {
            localStorage.removeItem(key)
            return initialValue
        }
    })
    const navigate = useNavigate()

    function setLocalStorageValue(newValue: Nullable<T>): void {
        if (key !== "hs_theme" || newValue !== null) {
            setValue(newValue)
            localStorage.setItem(key, JSON.stringify(newValue))
        }
    }

    function getLocalStorageValue(): Nullable<T> {
        const storedValue = localStorage.getItem(key)
        if (storedValue === null) return initialValue
        return JSON.parse(storedValue) as Nullable<T>
    }

    useEffect(() => {
        const timeout: number = setTimeout((): void => {
            setLocalStorageValue(null)
            navigate("login", { replace: true })
        }, 60 * 60 * 1000)

        return (): void => clearTimeout(timeout)
    }, [setLocalStorageValue])

    return { value, setLocalStorageValue, getLocalStorageValue }
}