import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, expect, test } from 'vitest'
import { App } from './App'

beforeEach(() => {
  sessionStorage.clear()
  window.history.pushState({}, '', '/')
})

test('renders the frontend starter screen', async () => {
  render(<App />)

  expect(
    screen.getByRole('heading', {
      name: /создавайте персонажей на основе правил/i,
    }),
  ).toBeInTheDocument()

  expect(await screen.findByText('.NET 10')).toBeInTheDocument()
  expect(screen.getByLabelText(/имя персонажа/i)).toBeInTheDocument()
})

test('validates the login form', async () => {
  const user = userEvent.setup()
  render(<App />)

  await user.click(screen.getByRole('link', { name: /войти/i }))
  expect(await screen.findByRole('heading', { name: /вход/i })).toBeInTheDocument()

  await user.click(screen.getByRole('button', { name: /войти/i }))

  expect(await screen.findByText(/укажите email/i)).toBeInTheDocument()
  expect(screen.getByText(/укажите пароль/i)).toBeInTheDocument()
})

test('validates the register form', async () => {
  const user = userEvent.setup()
  render(<App />)

  await user.click(screen.getByRole('link', { name: /создать аккаунт/i }))
  expect(await screen.findByRole('heading', { name: /создание аккаунта/i })).toBeInTheDocument()

  await user.click(screen.getByRole('button', { name: /создать аккаунт/i }))

  expect(await screen.findByText(/укажите отображаемое имя/i)).toBeInTheDocument()
  expect(screen.getByText(/укажите email/i)).toBeInTheDocument()
  expect(screen.getByText(/укажите пароль/i)).toBeInTheDocument()
})
