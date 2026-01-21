import { FormContainerForgotPassword } from '@/components/connexion/FormContainerForgotPassword'
import { FormContainerResetPassword } from '@/components/connexion/FormContainerResetPassword'
import { FormContainerSignIn } from '@/components/connexion/FormContainerSignIn'
import { FormContainerSignUp } from '@/components/connexion/FormContainerSignUp'
import { HomePage } from '@/components/home/HomePage'
import { useEffect } from 'react'
import { Route, Routes, useLocation } from 'react-router'

export function App() {
  const location = useLocation()
  const centeredRoutes = ['/bookmark-manager-app/', '/bookmark-manager-app/signUp', '/bookmark-manager-app/forgotPassword', '/bookmark-manager-app/resetPassword']
  const isCenteredRoute = centeredRoutes.includes(location.pathname)

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
    <div className={`${isCenteredRoute ? 'flex justify-center items-center gap-y-2.5 px-4 md:p-0' : ''}`}>
      <Routes>
        <Route path="/bookmark-manager-app/" element={<FormContainerSignIn />} />
        <Route path="/bookmark-manager-app/signUp" element={<FormContainerSignUp />} />
        <Route path="/bookmark-manager-app/forgotPassword" element={<FormContainerForgotPassword />} />
        <Route path="/bookmark-manager-app/ResetPassword" element={<FormContainerResetPassword />} />
        <Route path="/bookmark-manager-app/home" element={<HomePage />} />
      </Routes>
    </div>
  )
}

async function loadPreline() {
  return import('preline/dist/index.js')
}