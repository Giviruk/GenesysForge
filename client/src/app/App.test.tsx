import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, expect, test, vi } from 'vitest'
import { useCreationWizardStore } from '../features/creation-wizard/wizardStore'
import { useAuthStore } from '../stores/useAuthStore'
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
const demoSession = {
  accessToken: 'test-token',
  expiresAt: new Date(Date.now() + 60 * 60 * 1000).toISOString(),
  user: {
    id: '20000000-0000-0000-0000-000000000001',
    email: 'player@example.com',
    displayName: 'Игрок',
  },
}
const demoCharacters = [
  {
    id: '30000000-0000-0000-0000-000000000001',
    name: 'Мира Вейл',
    status: 'Draft',
    rulesetId: demoRulesetId,
    updatedAt: '2026-06-11T08:00:00Z',
  },
]
const demoCreatedCharacter = {
  id: '30000000-0000-0000-0000-000000000002',
  name: 'Мира Вейл',
  status: 'Draft',
  rulesetId: demoRulesetId,
  updatedAt: '2026-06-11T08:00:00Z',
  ruleSnapshot: {
    rulesetId: demoRulesetId,
    createdAt: '2026-06-11T08:00:00Z',
    sourceVersionIds: ['10000000-0000-0000-0000-000000000003'],
    ruleEntityIds: [
      '10000000-0000-0000-0000-000000000004',
      '10000000-0000-0000-0000-00000000000e',
    ],
    ruleDefinitionIds: [
      '10000000-0000-0000-0000-000000000005',
      '10000000-0000-0000-0000-00000000000f',
    ],
  },
  calculatedStats: {
    availableXp: 0,
    spentXp: 0,
    characteristics: {},
    derivedStats: {},
  },
  draftProfile: {
    archetypeId: '10000000-0000-0000-0000-000000000004',
    careerId: '10000000-0000-0000-0000-00000000000e',
    careerSkillRanksToAssign: 4,
  },
  skills: [
    {
      ruleEntityId: '10000000-0000-0000-0000-000000000010',
      rank: 0,
      xpSpent: 0,
      isCareerSkill: true,
      updatedAt: '2026-06-11T08:00:00Z',
    },
  ],
}
const demoValidationResult = {
  isValid: true,
  messages: [
    {
      code: 'character.career.starting_ranks.unassigned',
      message: 'Назначьте 4 стартовых ранга карьерных навыков.',
      severity: 'Warning',
      path: 'skills',
    },
  ],
}

let rulesetsStatus = 200
let rulesetsResponse = demoRulesets
let charactersStatus = 200
let charactersResponse = demoCharacters

beforeEach(() => {
  sessionStorage.clear()
  window.history.pushState({}, '', '/')
  useWorkspaceStore.setState({
    activeSection: 'characters',
    lastDraftName: '',
    selectedRulesetId: null,
  })
  useAuthStore.setState({ session: null })
  useCreationWizardStore.setState({
    currentStepId: 'basic-info',
    draftName: '',
    selectedArchetypeId: null,
    selectedCareerId: null,
  })
  rulesetsStatus = 200
  rulesetsResponse = demoRulesets
  charactersStatus = 200
  charactersResponse = demoCharacters
  vi.stubGlobal(
    'fetch',
    vi.fn(async (input: RequestInfo | URL, init?: RequestInit) => {
      const url = typeof input === 'string' || input instanceof URL ? input.toString() : input.url
      const method = init?.method ?? (input instanceof Request ? input.method : 'GET')

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
            {
              id: '10000000-0000-0000-0000-00000000000e',
              rulesetId: demoRulesetId,
              entityType: 'career',
              key: 'warrior',
              name: 'Воин',
              description: null,
            },
          ],
          definitions: [],
        })
      }

      if (url.endsWith('/api/characters') && method === 'POST') {
        return Response.json(demoCreatedCharacter, { status: 201 })
      }

      if (url.endsWith(`/api/characters/${demoCreatedCharacter.id}/validation`)) {
        return Response.json(demoValidationResult)
      }

      if (url.endsWith('/api/characters')) {
        return Response.json(charactersResponse, { status: charactersStatus })
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
  expect(await screen.findByRole('combobox', { name: /набор правил/i })).toHaveTextContent(
    /Демо-набор Genesys Forge v1\.0/i,
  )
  expect(await screen.findByText(/элементов правил: 2/i)).toBeInTheDocument()
  expect(screen.getByLabelText(/имя персонажа/i)).toBeInTheDocument()
})

test('renders the empty ruleset state', async () => {
  rulesetsResponse = []

  render(<App />)

  expect(await screen.findByText(/пока нет доступных наборов правил/i)).toBeInTheDocument()
})

test('renders the character list for the signed in user', async () => {
  useAuthStore.setState({ session: demoSession })
  window.history.pushState({}, '', '/characters')

  render(<App />)

  expect(await screen.findByRole('heading', { name: /персонажи/i })).toBeInTheDocument()
  expect(await screen.findByText('Мира Вейл')).toBeInTheDocument()
  expect(screen.getByText('Черновик')).toBeInTheDocument()
  expect(screen.getByRole('link', { name: /создать черновик/i })).toBeInTheDocument()
})

test('renders the empty character list state', async () => {
  useAuthStore.setState({ session: demoSession })
  charactersResponse = []
  window.history.pushState({}, '', '/characters')

  render(<App />)

  expect(await screen.findByText(/пока нет персонажей/i)).toBeInTheDocument()
  expect(screen.getAllByRole('link', { name: /создать черновик/i }).length).toBeGreaterThan(0)
})

test('opens the creation wizard from the character list', async () => {
  const user = userEvent.setup()
  useAuthStore.setState({ session: demoSession })
  window.history.pushState({}, '', '/characters')

  render(<App />)

  await user.click(await screen.findByRole('link', { name: /создать черновик/i }))

  expect(await screen.findByRole('heading', { name: /создание персонажа/i })).toBeInTheDocument()
  expect(screen.getByRole('button', { name: /далее/i })).toBeInTheDocument()
})

test('keeps local wizard state between steps', async () => {
  const user = userEvent.setup()
  useAuthStore.setState({ session: demoSession })
  window.history.pushState({}, '', '/characters/new')

  render(<App />)

  await user.type(await screen.findByLabelText(/имя персонажа/i), 'Мира Вейл')
  await user.click(screen.getByRole('button', { name: /далее/i }))
  expect(await screen.findByRole('heading', { name: /происхождение/i })).toBeInTheDocument()

  await user.click(screen.getByRole('button', { name: /назад/i }))

  expect(await screen.findByLabelText(/имя персонажа/i)).toHaveValue('Мира Вейл')
})

test('validates the wizard basic info form before creating a draft', async () => {
  const user = userEvent.setup()
  useAuthStore.setState({ session: demoSession })
  window.history.pushState({}, '', '/characters/new')

  render(<App />)

  const createButton = await screen.findByRole('button', { name: /создать черновик/i })
  await waitFor(() => expect(createButton).not.toBeDisabled())
  await user.click(createButton)

  expect(await screen.findByText(/укажите имя персонажа/i)).toBeInTheDocument()
})

test('shows validation warnings after creating a draft', async () => {
  const user = userEvent.setup()
  useAuthStore.setState({ session: demoSession })
  window.history.pushState({}, '', '/characters/new')

  render(<App />)

  await user.type(await screen.findByLabelText(/имя персонажа/i), 'Мира Вейл')
  const createButton = await screen.findByRole('button', { name: /создать черновик/i })
  await waitFor(() => expect(createButton).not.toBeDisabled())
  await user.click(createButton)

  expect(await screen.findByText(/черновик создан: мира вейл/i)).toBeInTheDocument()
  expect(await screen.findByText(/назначьте 4 стартовых ранга карьерных навыков/i)).toBeInTheDocument()
  expect(screen.getByText(/можно продолжать/i)).toBeInTheDocument()
})

test('asks anonymous users to sign in before using the creation wizard', async () => {
  window.history.pushState({}, '', '/characters/new')

  render(<App />)

  expect(await screen.findByText(/войдите, чтобы создать персонажа/i)).toBeInTheDocument()
  expect(screen.getAllByRole('link', { name: /^войти$/i }).length).toBeGreaterThan(0)
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
