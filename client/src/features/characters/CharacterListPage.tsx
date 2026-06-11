import { useQuery } from '@tanstack/react-query'
import { NavLink } from 'react-router-dom'
import { listMyCharacters } from '../../api/charactersApi'
import { useAuthStore } from '../../stores/useAuthStore'
import { CharacterCard } from './CharacterCard'
import '../home/HomePage.css'
import './CharactersPage.css'

export function CharacterListPage() {
  const session = useAuthStore((state) => state.session)
  const clearSession = useAuthStore((state) => state.clearSession)
  const charactersQuery = useQuery({
    enabled: Boolean(session),
    queryKey: ['characters', session?.user.id],
    queryFn: () => listMyCharacters(session?.accessToken ?? ''),
  })

  return (
    <main className="app-shell">
      <aside className="sidebar" aria-label="Основная навигация">
        <div className="brand">
          <span className="brand-mark">GF</span>
          <span>Genesys Forge</span>
        </div>

        <nav className="sidebar-nav">
          <NavLink to="/characters" className={({ isActive }) => (isActive ? 'nav-item active' : 'nav-item')}>
            Персонажи
          </NavLink>
          <NavLink to="/" className="nav-item">
            Наборы правил
          </NavLink>
          <button className="nav-item" type="button">
            Настройки
          </button>
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

        <section className="characters-page" aria-labelledby="characters-heading">
          <header className="characters-header">
            <div>
              <h1 id="characters-heading">Персонажи</h1>
              <p>Черновики и готовые листы, привязанные к вашему аккаунту.</p>
            </div>
            <NavLink to="/" className="primary-action">
              Создать черновик
            </NavLink>
          </header>

          {!session ? (
            <section className="characters-empty" aria-live="polite">
              <h2>Войдите, чтобы увидеть персонажей</h2>
              <p>Список персонажей хранится в аккаунте, поэтому для просмотра нужна активная сессия.</p>
              <NavLink to="/login" className="submit-button">
                Войти
              </NavLink>
            </section>
          ) : null}

          {session && charactersQuery.isPending ? (
            <section className="characters-empty" aria-live="polite">
              <p className="muted">Загружаем ваших персонажей...</p>
            </section>
          ) : null}

          {session && charactersQuery.isError ? (
            <section className="characters-empty" aria-live="polite">
              <h2>Не удалось загрузить персонажей</h2>
              <p>Проверьте сессию или попробуйте повторить запрос.</p>
              <button className="text-button" type="button" onClick={() => void charactersQuery.refetch()}>
                Повторить
              </button>
            </section>
          ) : null}

          {session && charactersQuery.data?.length === 0 ? (
            <section className="characters-empty" aria-live="polite">
              <h2>Пока нет персонажей</h2>
              <p>Создайте первый черновик, а следующие шаги мастера постепенно наполнят его правилами.</p>
              <NavLink to="/" className="submit-button">
                Создать черновик
              </NavLink>
            </section>
          ) : null}

          {session && charactersQuery.data && charactersQuery.data.length > 0 ? (
            <section className="characters-grid" aria-label="Ваши персонажи">
              {charactersQuery.data.map((character) => (
                <CharacterCard key={character.id} character={character} />
              ))}
            </section>
          ) : null}
        </section>
      </section>
    </main>
  )
}
