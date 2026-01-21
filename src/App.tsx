import { FormContainerForgotPassword } from '@/components/connexion/FormContainerForgotPassword'
import { FormContainerResetPassword } from '@/components/connexion/FormContainerResetPassword'
import { FormContainerSignIn } from '@/components/connexion/FormContainerSignIn'
import { FormContainerSignUp } from '@/components/connexion/FormContainerSignUp'
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
      <Route path="/bookmark-manager-app/" element={<FormContainerSignIn />} />
      <Route path="/bookmark-manager-app/signUp" element={<FormContainerSignUp />} />
      <Route path="/bookmark-manager-app/forgotPassword" element={<FormContainerForgotPassword />} />
      <Route path="/bookmark-manager-app/ResetPassword" element={<FormContainerResetPassword />} />
    </Routes>
  )
}

async function loadPreline() {
  return import('preline/dist/index.js')
}