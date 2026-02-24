import { ThemeContext, type Theme } from "@/context/ThemeContext";
import { useLocalStorage } from "@/hooks/useLocalStorage";
import { useEffect, type ReactNode } from "react";

export function ThemeProvider({ children }: { children: ReactNode }) {
    const { value, setLocalStorageValue } = useLocalStorage<Theme>("hs_theme")

    useEffect((): void => {
        const theme: Theme = value ?? (window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light")
        if (theme === "dark") {
            document.documentElement.classList.add("dark")
        } else {
            document.documentElement.classList.remove("dark")
        }

        if (value) {
            setLocalStorageValue(theme)
        }
    }, [value])

    return (
        <ThemeContext.Provider value={{ theme: value!, setTheme: setLocalStorageValue }}>
            {children}
        </ThemeContext.Provider>
    )
}