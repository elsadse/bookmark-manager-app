import { ArchivedPage } from '@/components/archived/ArchivedPage'
import { FormContainerForgotPassword } from '@/components/connexion/FormContainerForgotPassword'
import { FormContainerResetPassword } from '@/components/connexion/FormContainerResetPassword'
import { FormContainerSignIn } from '@/components/connexion/FormContainerSignIn'
import { FormContainerSignUp } from '@/components/connexion/FormContainerSignUp'
import { HomePage } from '@/components/home/HomePage'
import { PrivateRoute } from '@/components/PivateRoute'
import { PublicRoute } from '@/components/PublicRoute'
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
            <Route path="/bookmark-manager-app/" element={<PublicRoute><FormContainerSignIn /></PublicRoute>} />
            <Route path="/bookmark-manager-app/signUp" element={<PublicRoute><FormContainerSignUp /></PublicRoute>} />
            <Route path="/bookmark-manager-app/forgotPassword" element={<PublicRoute><FormContainerForgotPassword /></PublicRoute>} />
            <Route path="/bookmark-manager-app/ResetPassword" element={<PublicRoute><FormContainerResetPassword /></PublicRoute>} />

            <Route path="/bookmark-manager-app/home" element={<PrivateRoute><HomePage /></PrivateRoute>} />
            <Route path="/bookmark-manager-app/archived" element={<PrivateRoute><ArchivedPage /></PrivateRoute>} />
          </Routes>
        </div>
      </FilterTagsContextProvider>
    </BookmarkListContextProvider>
  )
}
