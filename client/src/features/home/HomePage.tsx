import { useQuery } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { NavLink } from 'react-router-dom'
import { getProjectStatus } from '../../api/projectStatus'
import type { DraftCharacterForm } from '../../api/types'
import { useWorkspaceStore } from '../../stores/useWorkspaceStore'
import './HomePage.css'

const navigation = [
  { id: 'characters', label: 'Characters' },
  { id: 'rulesets', label: 'Rulesets' },
  { id: 'settings', label: 'Settings' },
] as const

const quickActions = ['Create draft', 'Import ruleset', 'Review contracts']

export function HomePage() {
  const activeSection = useWorkspaceStore((state) => state.activeSection)
  const lastDraftName = useWorkspaceStore((state) => state.lastDraftName)
  const setActiveSection = useWorkspaceStore((state) => state.setActiveSection)
  const setLastDraftName = useWorkspaceStore((state) => state.setLastDraftName)

  const statusQuery = useQuery({
    queryKey: ['project-status'],
    queryFn: getProjectStatus,
  })

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<DraftCharacterForm>({
    defaultValues: {
      name: '',
      ruleset: 'Genesys Demo',
    },
  })

  function handleCreateDraft(values: DraftCharacterForm) {
    setLastDraftName(values.name.trim())
    reset({ name: '', ruleset: values.ruleset })
  }

  return (
    <main className="app-shell">
      <aside className="sidebar" aria-label="Primary">
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
            Dashboard
          </NavLink>
          <span className="environment">Local MVP</span>
        </header>

        <section className="hero-panel" aria-labelledby="welcome-heading">
          <div>
            <h1 id="welcome-heading">Build characters from a rules-driven core.</h1>
            <p>
              Frontend skeleton is wired for routing, server state, local workspace state,
              and forms. The next plan items can now attach real API contracts.
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
          <div className="status-grid" aria-label="Project status">
            {statusQuery.isPending ? (
              <p className="muted">Loading project status...</p>
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
            <h2>Create a draft</h2>
            <label>
              Character name
              <input
                {...register('name', {
                  required: 'Name is required',
                  minLength: { value: 2, message: 'Use at least 2 characters' },
                })}
                placeholder="Asha Vorn"
              />
            </label>
            {errors.name ? <p className="form-error">{errors.name.message}</p> : null}

            <label>
              Ruleset
              <select {...register('ruleset')}>
                <option>Genesys Demo</option>
                <option>Custom Sandbox</option>
              </select>
            </label>

            <button className="submit-button" type="submit">
              Save local draft
            </button>

            {lastDraftName ? (
              <p className="success-note">Last local draft: {lastDraftName}</p>
            ) : (
              <p className="muted">Drafts are local until the character API arrives.</p>
            )}
          </form>
        </section>
      </section>
    </main>
  )
}
