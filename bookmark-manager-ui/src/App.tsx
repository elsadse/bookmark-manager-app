import { ArchivedPage } from '@/components/archived/ArchivedPage'
import { FormContainerForgotPassword } from '@/components/connexion/FormContainerForgotPassword'
import { FormContainerResetPassword } from '@/components/connexion/FormContainerResetPassword'
import { FormContainerSignIn } from '@/components/connexion/FormContainerSignIn'
import { FormContainerSignUp } from '@/components/connexion/FormContainerSignUp'
import { HomePage } from '@/components/home/HomePage'
import { BookmarkListContextProvider } from '@/context/BookmarkListContext'
import { FilterTagsContextProvider } from '@/context/FilterTagsContext'
import { Route, Routes, useLocation } from 'react-router'

export function App() {
  const location = useLocation()
  const centeredRoutes = ['/bookmark-manager-app/', '/bookmark-manager-app/signUp', '/bookmark-manager-app/forgotPassword', '/bookmark-manager-app/resetPassword']
  const isCenteredRoute = centeredRoutes.includes(location.pathname)

  return (
    <BookmarkListContextProvider>
      <FilterTagsContextProvider>
        <div className={`${isCenteredRoute ? 'min-h-screen flex justify-center items-center gap-y-2.5 px-4 md:p-0' : ''}`}>
          <Routes>
            <Route path="/bookmark-manager-app/" element={<FormContainerSignIn />} />
            <Route path="/bookmark-manager-app/signUp" element={<FormContainerSignUp />} />
            <Route path="/bookmark-manager-app/forgotPassword" element={<FormContainerForgotPassword />} />
            <Route path="/bookmark-manager-app/ResetPassword" element={<FormContainerResetPassword />} />
            <Route path="/bookmark-manager-app/home" element={<HomePage />} />
            <Route path="/bookmark-manager-app/archived" element={<ArchivedPage />} />
          </Routes>
        </div>
      </FilterTagsContextProvider>
    </BookmarkListContextProvider>
  )
}
