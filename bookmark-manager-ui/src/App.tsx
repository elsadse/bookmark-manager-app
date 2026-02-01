import { AuthLayout } from '@/components/auth/AuthLayout'
import { FormContainerSignIn } from '@/components/auth/FormContainerSignIn'
import { FormContainerSignUp } from '@/components/auth/FormContainerSignUp'
import { BookmarkList } from '@/components/home/BookmarkList'
import { HomeLayout } from '@/components/home/HomeLayout'
import { ProtectedRoute } from '@/components/ProtectedRoute'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { Route, Routes } from 'react-router'

const queryClient = new QueryClient()

export function App() {

  return (
    <QueryClientProvider client={queryClient}>
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
    </QueryClientProvider>
  )
}
