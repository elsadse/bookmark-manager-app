import { useEffect, useState } from "react"

export function DarkModeToggle() {
    const [darkMode, setDarkMode] = useState(false)

    useEffect(() => {
        if (localStorage.getItem('hs_theme') === 'dark' ||
            (!('hs_theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
            setDarkMode(true)
            document.documentElement.classList.add('dark')
        } else {
            setDarkMode(false)
            document.documentElement.classList.remove('dark')
        }
    }, [])

    function toggleDarkMode() {
        if (darkMode) {
            document.documentElement.classList.remove('dark')
            localStorage.setItem('hs_theme', 'light')
            setDarkMode(false)
        } else {
            document.documentElement.classList.add('dark')
            localStorage.setItem('hs_theme', 'dark')
            setDarkMode(true)
        }
    }

    return (
        <button type="button" onClick={toggleDarkMode}
            className="hs-dropdown-toggle hs-dark-mode group flex items-center text-gray-600 hover:text-blue-600 focus:outline-hidden focus:text-blue-600 font-medium dark:text-neutral-400 dark:hover:text-neutral-500 dark:focus:text-neutral-500" aria-haspopup="menu" aria-expanded="false" aria-label="Dropdown">
            <svg className="hs-dark-mode-active:hidden block size-4" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M12 3a6 6 0 0 0 9 9 9 9 0 1 1-9-9Z"></path>
            </svg>
            <svg className="hs-dark-mode-active:block hidden size-4" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <circle cx="12" cy="12" r="4"></circle>
                <path d="M12 2v2"></path>
                <path d="M12 20v2"></path>
                <path d="m4.93 4.93 1.41 1.41"></path>
                <path d="m17.66 17.66 1.41 1.41"></path>
                <path d="M2 12h2"></path>
                <path d="M20 12h2"></path>
                <path d="m6.34 17.66-1.41 1.41"></path>
                <path d="m19.07 4.93-1.41 1.41"></path>
            </svg>
        </button>
    )
}