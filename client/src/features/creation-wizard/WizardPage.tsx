import { NavLink } from 'react-router-dom'
import { useAuthStore } from '../../stores/useAuthStore'
import '../home/HomePage.css'
import '../characters/CharactersPage.css'
import './WizardPage.css'
import { useCreationWizardStore, wizardSteps } from './wizardStore'

const characteristics = [
  { label: 'Сила', value: 2 },
  { label: 'Ловкость', value: 2 },
  { label: 'Интеллект', value: 2 },
  { label: 'Хитрость', value: 2 },
  { label: 'Воля', value: 2 },
  { label: 'Харизма', value: 2 },
]

const derivedStats = [
  { label: 'Раны', value: 10, caption: 'Порог' },
  { label: 'Усталость', value: 10, caption: 'Порог' },
  { label: 'Поглощение', value: 2, caption: 'Значение' },
  { label: 'Защита', value: '0 / 0', caption: 'Ближн. / Дальн.' },
]

const skillGroups = [
  {
    title: 'Общие',
    skills: [
      { name: 'Атлетика (СИЛ)', career: false, rank: 0, dice: ['ability', 'ability'] },
      { name: 'Внимание (ВОЛ)', career: false, rank: 0, dice: ['ability', 'ability'] },
      { name: 'Стойкость (ВОЛ)', career: true, rank: 1, dice: ['proficiency', 'ability'] },
      { name: 'Тактика (ИНТ)', career: true, rank: 1, dice: ['proficiency', 'ability'] },
    ],
  },
  {
    title: 'Боевые',
    skills: [
      { name: 'Драка (СИЛ)', career: false, rank: 0, dice: ['ability', 'ability'] },
      { name: 'Холодное оружие (СИЛ)', career: true, rank: 1, dice: ['proficiency', 'ability'] },
      { name: 'Стрельба (ЛОВ)', career: false, rank: 0, dice: ['ability', 'ability'] },
    ],
  },
  {
    title: 'Социальные',
    skills: [
      { name: 'Переговоры (ХАР)', career: false, rank: 0, dice: ['ability', 'ability'] },
      { name: 'Лидерство (ХАР)', career: false, rank: 0, dice: ['ability', 'ability'] },
    ],
  },
]

export function WizardPage() {
  const session = useAuthStore((state) => state.session)
  const clearSession = useAuthStore((state) => state.clearSession)
  const currentStepId = useCreationWizardStore((state) => state.currentStepId)
  const draftName = useCreationWizardStore((state) => state.draftName)
  const setCurrentStep = useCreationWizardStore((state) => state.setCurrentStep)
  const setDraftName = useCreationWizardStore((state) => state.setDraftName)
  const resetWizard = useCreationWizardStore((state) => state.reset)

  const currentStepIndex = Math.max(
    wizardSteps.findIndex((step) => step.id === currentStepId),
    0,
  )
  const currentStep = wizardSteps[currentStepIndex]
  const isFirstStep = currentStepIndex === 0
  const isLastStep = currentStepIndex === wizardSteps.length - 1

  function goToPreviousStep() {
    if (!isFirstStep) {
      setCurrentStep(wizardSteps[currentStepIndex - 1].id)
    }
  }

  function goToNextStep() {
    if (!isLastStep) {
      setCurrentStep(wizardSteps[currentStepIndex + 1].id)
    }
  }

  const displayName = draftName.trim() || 'Новый персонаж'
  const initials = displayName
    .split(' ')
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part[0]?.toUpperCase())
    .join('')

  return (
    <main className="app-shell">
      <aside className="sidebar" aria-label="Основная навигация">
        <div className="brand">
          <span className="brand-mark">GF</span>
          <span>Genesys Forge</span>
        </div>

        <nav className="sidebar-nav">
          <NavLink to="/characters" className="nav-item">
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
          <NavLink to="/characters" className="topbar-link">
            Персонажи
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

        <section className="wizard-page" aria-labelledby="wizard-heading">
          <header className="characters-header">
            <div>
              <h1 id="wizard-heading">Создание персонажа</h1>
              <p>Заполняйте лист персонажа сверху вниз. Черновик пока хранится локально до подключения шага сохранения.</p>
            </div>
            <button className="text-button" type="button" onClick={resetWizard}>
              Сбросить
            </button>
          </header>

          {!session ? (
            <section className="characters-empty" aria-live="polite">
              <h2>Войдите, чтобы создать персонажа</h2>
              <p>Черновики персонажей будут сохраняться в аккаунте, поэтому мастер требует активную сессию.</p>
              <NavLink to="/login" className="submit-button">
                Войти
              </NavLink>
            </section>
          ) : (
            <div className="wizard-layout sheet-layout">
              <div className="sheet-toolbar" aria-label="Навигация создания персонажа">
                <ol className="wizard-steps sheet-step-tabs" aria-label="Разделы листа персонажа">
                {wizardSteps.map((step, index) => (
                  <li key={step.id}>
                    <button
                      className={step.id === currentStepId ? 'wizard-step active' : 'wizard-step'}
                      type="button"
                      onClick={() => setCurrentStep(step.id)}
                    >
                      <span>{index + 1}</span>
                      <strong>{step.title}</strong>
                      <small>{step.description}</small>
                    </button>
                  </li>
                ))}
                </ol>

              </div>

              <section className="character-sheet" aria-live="polite">
                <div className="sheet-current-step">
                  <span>Активный раздел</span>
                  <h2>{currentStep.title}</h2>
                  <p>{currentStep.description}</p>
                </div>

                <section className="sheet-identity" aria-labelledby="sheet-character-heading">
                  <div className="sheet-panel sheet-main-panel">
                    <div className="sheet-title-row">
                      <h2 id="sheet-character-heading">Персонаж</h2>
                      <button className="sheet-small-button" type="button" onClick={resetWizard}>
                        Новый лист
                      </button>
                    </div>

                    <label className="sheet-field-row">
                      <span>Имя персонажа</span>
                      <input
                        value={draftName}
                        placeholder="Например, Артур Лейвин"
                        onChange={(event) => setDraftName(event.target.value)}
                      />
                    </label>

                    <div className="sheet-field-row">
                      <span>Архетип</span>
                      <button className="sheet-select-row" type="button" onClick={() => setCurrentStep('origin')}>
                        <strong>Выбрать происхождение</strong>
                        <small>Шаг 2</small>
                      </button>
                    </div>

                    <div className="sheet-field-row">
                      <span>Карьера</span>
                      <button className="sheet-select-row" type="button" onClick={() => setCurrentStep('career')}>
                        <strong>Выбрать карьеру</strong>
                        <small>Шаг 3</small>
                      </button>
                    </div>

                    <div className="sheet-field-row">
                      <span>Набор правил</span>
                      <div className="sheet-static-value">Демо-набор Genesys Forge v1.0</div>
                    </div>
                  </div>

                  <aside className="sheet-panel sheet-portrait" aria-label="Образ персонажа">
                    <h2>Образ</h2>
                    <div className="portrait-mark" aria-hidden="true">
                      {initials || 'GF'}
                    </div>
                    <p>{displayName}</p>
                  </aside>
                </section>

                <section className="sheet-derived-grid" aria-label="Производные параметры">
                  {derivedStats.map((stat) => (
                    <article className="derived-card" key={stat.label}>
                      <strong>{stat.label}</strong>
                      <span>{stat.value}</span>
                      <small>{stat.caption}</small>
                    </article>
                  ))}
                </section>

                <section className="sheet-section" aria-labelledby="sheet-characteristics-heading">
                  <h2 id="sheet-characteristics-heading">Характеристики</h2>
                  <div className="characteristics-grid">
                    {characteristics.map((characteristic) => (
                      <article className="characteristic-token" key={characteristic.label}>
                        <span>{characteristic.value}</span>
                        <strong>{characteristic.label}</strong>
                      </article>
                    ))}
                  </div>
                </section>

                <section className="sheet-section sheet-critical" aria-labelledby="sheet-critical-heading">
                  <h2 id="sheet-critical-heading">Критические травмы</h2>
                  <div className="critical-row">
                    <strong>Критическая травма</strong>
                    <span>Описание появится после сохранения листа и развития персонажа.</span>
                  </div>
                </section>

                <section className="sheet-section" aria-labelledby="sheet-skills-heading">
                  <h2 id="sheet-skills-heading">Навыки</h2>
                  <div className="skills-grid">
                    {skillGroups.map((group) => (
                      <article className="skill-table" key={group.title}>
                        <h3>{group.title}</h3>
                        <div className="skill-row skill-row-head">
                          <span>Навык</span>
                          <span>Кар.</span>
                          <span>Ранг</span>
                          <span>Пул</span>
                        </div>
                        {group.skills.map((skill) => (
                          <div className="skill-row" key={skill.name}>
                            <span>{skill.name}</span>
                            <span>{skill.career ? 'Да' : 'Нет'}</span>
                            <span>{skill.rank}</span>
                            <span className="dice-pool" aria-label={`Пул ${skill.name}`}>
                              {skill.dice.map((die, dieIndex) => (
                                <span className={`die ${die}`} key={`${skill.name}-${dieIndex}`} />
                              ))}
                            </span>
                          </div>
                        ))}
                      </article>
                    ))}
                  </div>
                </section>

                <footer className="sheet-footer-actions">
                  <button className="text-button" disabled={isFirstStep} type="button" onClick={goToPreviousStep}>
                    Назад
                  </button>
                  <button className="submit-button" disabled type="button">
                    Создать черновик
                  </button>
                  <button className="submit-button" disabled={isLastStep} type="button" onClick={goToNextStep}>
                    Далее
                  </button>
                </footer>
              </section>
            </div>
          )}
        </section>
      </section>
    </main>
  )
}
