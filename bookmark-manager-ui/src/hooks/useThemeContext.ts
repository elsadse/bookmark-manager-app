import { ThemeContext } from "@/context/ThemeContext";
import { useContext } from "react";

export function useThemeContext() {
  const context = useContext(ThemeContext);
  if (context === null) {
    throw new Error("useTheme must be used within a ThemeProvider");
  }
  return context;
}