import { NavLink } from 'react-router-dom'
import { useAuthStore } from '../../stores/useAuthStore'
import '../home/HomePage.css'
import '../characters/CharactersPage.css'
import './WizardPage.css'
import { BasicInfoShellStep } from './steps/BasicInfoShellStep'
import { PlaceholderStep } from './steps/PlaceholderStep'
import { useCreationWizardStore, wizardSteps } from './wizardStore'

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
              <p>Черновик мастера с локальным состоянием между шагами.</p>
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
            <div className="wizard-layout">
              <ol className="wizard-steps" aria-label="Шаги создания персонажа">
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

              <section className="wizard-card" aria-live="polite">
                {currentStep.id === 'basic-info' ? (
                  <BasicInfoShellStep draftName={draftName} onDraftNameChange={setDraftName} />
                ) : (
                  <PlaceholderStep
                    description={currentStep.description}
                    stepNumber={currentStepIndex + 1}
                    title={currentStep.title}
                  />
                )}

                <footer className="wizard-actions">
                  <button className="text-button" disabled={isFirstStep} type="button" onClick={goToPreviousStep}>
                    Назад
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
