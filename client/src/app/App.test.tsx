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
      name: /build characters from a rules-driven core/i,
    }),
  ).toBeInTheDocument()

  expect(await screen.findByText('.NET 10')).toBeInTheDocument()
  expect(screen.getByLabelText(/character name/i)).toBeInTheDocument()
})

test('validates the login form', async () => {
  const user = userEvent.setup()
  render(<App />)

  await user.click(screen.getByRole('link', { name: /sign in/i }))
  expect(await screen.findByRole('heading', { name: /sign in/i })).toBeInTheDocument()

  await user.click(screen.getByRole('button', { name: /sign in/i }))

  expect(await screen.findByText(/email is required/i)).toBeInTheDocument()
  expect(screen.getByText(/password is required/i)).toBeInTheDocument()
})

test('validates the register form', async () => {
  const user = userEvent.setup()
  render(<App />)

  await user.click(screen.getByRole('link', { name: /create account/i }))
  expect(await screen.findByRole('heading', { name: /create account/i })).toBeInTheDocument()

  await user.click(screen.getByRole('button', { name: /create account/i }))

  expect(await screen.findByText(/display name is required/i)).toBeInTheDocument()
  expect(screen.getByText(/email is required/i)).toBeInTheDocument()
  expect(screen.getByText(/password is required/i)).toBeInTheDocument()
})
