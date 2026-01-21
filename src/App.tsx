import { DarkModeToggle } from '@/DarkModeToggle'
import { useEffect } from 'react'
import { useLocation } from 'react-router'

export function App() {
  const location = useLocation()

  useEffect(() => {
    const theme = localStorage.getItem('hs_theme') ||
      (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light')

    if (theme === 'dark') {
      document.documentElement.classList.add('dark')
    } else {
      document.documentElement.classList.remove('dark')
    }
  }, [])

  useEffect(() => {
    async function initPreline() {
      await loadPreline()

      if (
        window.HSStaticMethods &&
        typeof window.HSStaticMethods.autoInit === 'function'
      ) {
        window.HSStaticMethods.autoInit()
      }
    }

    initPreline()
  }, [location.pathname])

  return (
    <>
      <DarkModeToggle />
      <h1>Hello World</h1>
    </>
  )
}

async function loadPreline() {
  return import('preline/dist/index.js')
}