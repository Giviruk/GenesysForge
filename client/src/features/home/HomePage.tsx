import { useQuery } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { NavLink } from 'react-router-dom'
import { getProjectStatus } from '../../api/projectStatus'
import type { CreateCharacterDraftRequest } from '../../api/characters/CreateCharacterDraftRequest'
import { useAuthStore } from '../../stores/useAuthStore'
import { useWorkspaceStore } from '../../stores/useWorkspaceStore'
import './HomePage.css'

const navigation = [
  { id: 'characters', label: 'Персонажи' },
  { id: 'rulesets', label: 'Наборы правил' },
  { id: 'settings', label: 'Настройки' },
] as const

const quickActions = ['Создать черновик', 'Импортировать правила', 'Проверить контракты']

const demoRulesets = [
  { id: '11111111-1111-1111-1111-111111111111', name: 'Демо Genesys' },
  { id: '22222222-2222-2222-2222-222222222222', name: 'Своя песочница' },
] as const

export function HomePage() {
  const activeSection = useWorkspaceStore((state) => state.activeSection)
  const lastDraftName = useWorkspaceStore((state) => state.lastDraftName)
  const setActiveSection = useWorkspaceStore((state) => state.setActiveSection)
  const setLastDraftName = useWorkspaceStore((state) => state.setLastDraftName)
  const session = useAuthStore((state) => state.session)
  const clearSession = useAuthStore((state) => state.clearSession)

  const statusQuery = useQuery({
    queryKey: ['project-status'],
    queryFn: getProjectStatus,
  })

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<CreateCharacterDraftRequest>({
    defaultValues: {
      name: '',
      rulesetId: demoRulesets[0].id,
    },
  })

  function handleCreateDraft(values: CreateCharacterDraftRequest) {
    setLastDraftName(values.name.trim())
    reset({ name: '', rulesetId: values.rulesetId })
  }

  return (
    <main className="app-shell">
      <aside className="sidebar" aria-label="Основная навигация">
        <div className="brand">
          <span className="brand-mark">GF</span>
          <span>Genesys Forge</span>
        </div>

        <nav className="sidebar-nav">
          {navigation.map((item) => (
            <button
              key={item.id}
              className={activeSection === item.id ? 'nav-item active' : 'nav-item'}
              type="button"
              onClick={() => setActiveSection(item.id)}
            >
              {item.label}
            </button>
          ))}
        </nav>
      </aside>

      <section className="workspace">
        <header className="topbar">
          <NavLink to="/" className="topbar-link">
            Панель
          </NavLink>
          <div className="topbar-actions">
            {session ? (
              <>
                <span className="environment">Вы вошли как {session.user.displayName}</span>
                <button className="text-button" type="button" onClick={clearSession}>
                  Выйти
                </button>
              </>
            ) : (
              <>
                <NavLink to="/login" className="topbar-link">
                  Войти
                </NavLink>
                <NavLink to="/register" className="environment">
                  Создать аккаунт
                </NavLink>
              </>
            )}
          </div>
        </header>

        <section className="hero-panel" aria-labelledby="welcome-heading">
          <div>
            <h1 id="welcome-heading">Создавайте персонажей на основе правил.</h1>
            <p>
              Основа интерфейса уже подключена к маршрутам, серверному состоянию,
              локальному рабочему пространству и формам. Следующие шаги плана смогут
              опираться на реальные API-контракты.
            </p>
          </div>
          <div className="hero-actions">
            {quickActions.map((action) => (
              <button key={action} className="primary-action" type="button">
                {action}
              </button>
            ))}
          </div>
        </section>

        <section className="content-grid">
          <div className="status-grid" aria-label="Статус проекта">
            {statusQuery.isPending ? (
              <p className="muted">Загружаем статус проекта...</p>
            ) : (
              statusQuery.data?.map((item) => (
                <article className="status-card" key={item.label}>
                  <span>{item.label}</span>
                  <strong>{item.value}</strong>
                  <p>{item.detail}</p>
                </article>
              ))
            )}
          </div>

          <form className="draft-card" onSubmit={handleSubmit(handleCreateDraft)}>
            <h2>Создать черновик</h2>
            <label>
              Имя персонажа
              <input
                {...register('name', {
                  required: 'Укажите имя персонажа',
                  minLength: { value: 2, message: 'Введите минимум 2 символа' },
                })}
                placeholder="Asha Vorn"
              />
            </label>
            {errors.name ? <p className="form-error">{errors.name.message}</p> : null}

            <label>
              Набор правил
              <select {...register('rulesetId')}>
                {demoRulesets.map((ruleset) => (
                  <option key={ruleset.id} value={ruleset.id}>
                    {ruleset.name}
                  </option>
                ))}
              </select>
            </label>

            <button className="submit-button" type="submit">
              Сохранить локальный черновик
            </button>

            {lastDraftName ? (
              <p className="success-note">Последний локальный черновик: {lastDraftName}</p>
            ) : (
              <p className="muted">Черновики хранятся локально, пока API персонажей еще не готов.</p>
            )}
          </form>
        </section>
      </section>
    </main>
  )
}
