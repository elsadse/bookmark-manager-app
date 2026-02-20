import type { Nullable } from "@/types";
import { createContext } from "react";

export type Theme = "light" | "dark" ;

export type ThemeContextType= {
    theme: Theme,
    setTheme: (theme: Theme) => void,
    isDark: boolean
}

export const ThemeContext = createContext<Nullable<ThemeContextType>>(null)