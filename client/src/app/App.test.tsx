import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, expect, test, vi } from 'vitest'
import { useWorkspaceStore } from '../stores/useWorkspaceStore'
import { App } from './App'

const demoRulesetId = '10000000-0000-0000-0000-000000000001'
const demoRulesets = [
  {
    id: demoRulesetId,
    name: 'Демо-набор Genesys Forge',
    version: '1.0',
    description: 'Открытый демо-набор для проверки rules-driven данных.',
  },
]

let rulesetsStatus = 200
let rulesetsResponse = demoRulesets

beforeEach(() => {
  sessionStorage.clear()
  window.history.pushState({}, '', '/')
  useWorkspaceStore.setState({
    activeSection: 'characters',
    lastDraftName: '',
    selectedRulesetId: null,
  })
  rulesetsStatus = 200
  rulesetsResponse = demoRulesets
  vi.stubGlobal(
    'fetch',
    vi.fn(async (input: RequestInfo | URL) => {
      const url = typeof input === 'string' || input instanceof URL ? input.toString() : input.url

      if (url.endsWith('/api/rules/rulesets')) {
        return Response.json(rulesetsResponse, { status: rulesetsStatus })
      }

      if (url.endsWith(`/api/rules/${demoRulesetId}/catalog`)) {
        return Response.json({
          rulesets: [
            {
              id: demoRulesetId,
              name: 'Демо-набор Genesys Forge',
              version: '1.0',
              description: 'Открытый демо-набор для проверки rules-driven данных.',
            },
          ],
          sourceBooks: [
            {
              id: '10000000-0000-0000-0000-000000000002',
              rulesetId: demoRulesetId,
              key: 'demo-core',
              name: 'Демо-правила',
            },
          ],
          sourceVersions: [
            {
              id: '10000000-0000-0000-0000-000000000003',
              sourceBookId: '10000000-0000-0000-0000-000000000002',
              version: '1.0',
              isActive: true,
            },
          ],
          entities: [
            {
              id: '10000000-0000-0000-0000-000000000004',
              rulesetId: demoRulesetId,
              entityType: 'archetype',
              key: 'guardian',
              name: 'Страж',
              description: null,
            },
          ],
        })
      }

      return Response.json({ title: 'Not found' }, { status: 404 })
    }),
  )
})

test('renders the frontend starter screen', async () => {
  render(<App />)

  expect(screen.getByText(/загружаем доступные наборы правил/i)).toBeInTheDocument()

  expect(
    screen.getByRole('heading', {
      name: /создавайте персонажей на основе правил/i,
    }),
  ).toBeInTheDocument()

  expect(await screen.findByText('.NET 10')).toBeInTheDocument()
  expect(await screen.findByRole('combobox', { name: /набор правил/i })).toHaveValue(demoRulesetId)
  expect(await screen.findByText(/элементов правил: 1/i)).toBeInTheDocument()
  expect(screen.getByLabelText(/имя персонажа/i)).toBeInTheDocument()
})

test('renders the empty ruleset state', async () => {
  rulesetsResponse = []

  render(<App />)

  expect(await screen.findByText(/пока нет доступных наборов правил/i)).toBeInTheDocument()
})

test('renders the ruleset error state', async () => {
  rulesetsStatus = 500

  render(<App />)

  expect(
    await screen.findByText(/не удалось загрузить наборы правил/i, undefined, { timeout: 3_000 }),
  ).toBeInTheDocument()
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
