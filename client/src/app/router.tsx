import { createBrowserRouter } from 'react-router-dom'
import { LoginPage } from '../features/auth/LoginPage'
import { CharacterListPage } from '../features/characters/CharacterListPage'
import { WizardPage } from '../features/creation-wizard/WizardPage'
import { RegisterPage } from '../features/auth/RegisterPage'
import { HomePage } from '../features/home/HomePage'

export function createAppRouter() {
  return createBrowserRouter([
    {
      path: '/',
      element: <HomePage />,
    },
    {
      path: '/characters',
      element: <CharacterListPage />,
    },
    {
      path: '/characters/new',
      element: <WizardPage />,
    },
    {
      path: '/login',
      element: <LoginPage />,
    },
    {
      path: '/register',
      element: <RegisterPage />,
    },
  ])
}
