import { render, screen } from '@testing-library/react'
import { expect, test } from 'vitest'
import { App } from './App'

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
