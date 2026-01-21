import { FormContainerForgotPassword } from '@/components/connexion/FormContainerForgotPassword'
import { FormContainerResetPassword } from '@/components/connexion/FormContainerResetPassword'
import { FormContainerSignIn } from '@/components/connexion/FormContainerSignIn'
import { FormContainerSignUp } from '@/components/connexion/FormContainerSignUp'
import { DarkModeToggle } from '@/DarkModeToggle'
import { useEffect } from 'react'
import { Route, Routes, useLocation } from 'react-router'

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
    <Routes>
      <Route path="/" element={<FormContainerSignIn />} />
      <Route path="/signUp" element={<FormContainerSignUp />} />
      <Route path="/forgotPassword" element={<FormContainerForgotPassword />} />
      <Route path="/ResetPassword" element={<FormContainerResetPassword />} />
    </Routes>
  )
}

async function loadPreline() {
  return import('preline/dist/index.js')
}