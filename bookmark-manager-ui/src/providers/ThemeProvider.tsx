import { ThemeContext, type Theme } from "@/context/ThemeContext";
import { useLocalStorage } from "@/hooks/useLocalStorage";
import { useEffect, type ReactNode } from "react";

export function ThemeProvider({ children }: { children: ReactNode }) {
    function initialTheme(): Theme {
        const savedTheme = localStorage.getItem("hs_theme") as Theme | null
        if (savedTheme === "light" || savedTheme === "dark") {
            return savedTheme
        }
        if (window.matchMedia("(prefers-color-scheme: dark)").matches) {
            return "dark"
        }
        return "light"
    }

    const { value, setLocalStorageValue } = useLocalStorage<Theme>("hs_theme", initialTheme())

    useEffect(() => {
        if (value === "dark") {
            document.documentElement.classList.add("dark")
        } else {
            document.documentElement.classList.remove("dark")
        }

        if (value) {
            setLocalStorageValue(value)
        }

    }, [value])

    return (
        <ThemeContext.Provider value={{ theme: value??"light", setTheme: setLocalStorageValue }}>
            {children}
        </ThemeContext.Provider>
    )
}