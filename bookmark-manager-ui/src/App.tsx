import { AuthLayout } from '@/components/auth/AuthLayout'
import { FormContainerSignIn } from '@/components/auth/FormContainerSignIn'
import { FormContainerSignUp } from '@/components/auth/FormContainerSignUp'
import { BookmarkList } from '@/components/home/BookmarkList'
import { HomeLayout } from '@/components/home/HomeLayout'
import { ProtectedRoute } from '@/components/ProtectedRoute'
import { BookmarkListContextProvider } from '@/context/BookmarkListContext'
import { FilterTagsContextProvider } from '@/context/FilterTagsContext'
import { Route, Routes } from 'react-router'

export function App() {

  return (
    <BookmarkListContextProvider>
      <FilterTagsContextProvider>
        <Routes>
          <Route element={<ProtectedRoute />}>
            <Route element={<HomeLayout />}>
              <Route index element={<BookmarkList />} />
            </Route>
          </Route>

          <Route element={<AuthLayout />}>
            <Route path="login" element={<FormContainerSignIn />} />
            <Route path="register" element={<FormContainerSignUp />} />
          </Route>
        </Routes>
      </FilterTagsContextProvider>
    </BookmarkListContextProvider>
  )
}
