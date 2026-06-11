import { useMutation } from '@tanstack/react-query'
import { useEffect, useMemo } from 'react'
import { NavLink } from 'react-router-dom'
import { createCharacterDraft } from '../../api/charactersApi'
import type { CharacterSkillResponse } from '../../api/characters/CharacterSkillResponse'
import type { RuleDefinitionDto } from '../../api/rules/RuleDefinitionDto'
import type { RuleEntityDto } from '../../api/rules/RuleEntityDto'
import { useAuthStore } from '../../stores/useAuthStore'
import { useWorkspaceStore } from '../../stores/useWorkspaceStore'
import { useRuleCatalog, useRulesets } from '../rules/useRulesets'
import '../home/HomePage.css'
import '../characters/CharactersPage.css'
import './WizardPage.css'
import { useCreationWizardStore, wizardSteps } from './wizardStore'

const characteristicLabels: Record<string, string> = {
  brawn: 'Сила',
  agility: 'Ловкость',
  intellect: 'Интеллект',
  cunning: 'Хитрость',
  willpower: 'Воля',
  presence: 'Харизма',
}

const characteristicOrder = ['brawn', 'agility', 'intellect', 'cunning', 'willpower', 'presence']

const derivedStatLabels: Record<string, { label: string; caption: string }> = {
  woundThreshold: { label: 'Раны', caption: 'Порог' },
  strainThreshold: { label: 'Усталость', caption: 'Порог' },
  soak: { label: 'Поглощение', caption: 'Значение' },
  defense: { label: 'Защита', caption: 'Ближн. / Дальн.' },
}

const skillCategoryLabels: Record<string, string> = {
  general: 'Общие',
  social: 'Социальные',
  combat: 'Боевые',
  knowledge: 'Знания',
}

const skillCategoryOrder = ['general', 'social', 'combat', 'knowledge']

type ArchetypeProfile = {
  characteristics?: Record<string, number>
  derivedStats?: Record<string, number>
}

type CareerProfile = {
  careerSkillKeys?: string[]
  careerSkillRanksToAssign?: number
}

type SkillProfile = {
  category?: string
  characteristic?: string
}

type SkillViewModel = {
  entity: RuleEntityDto
  category: string
  characteristic: string
  isCareerSkill: boolean
  rank: number
}

export function WizardPage() {
  const session = useAuthStore((state) => state.session)
  const clearSession = useAuthStore((state) => state.clearSession)
  const selectedRulesetId = useWorkspaceStore((state) => state.selectedRulesetId)
  const setSelectedRulesetId = useWorkspaceStore((state) => state.setSelectedRulesetId)
  const currentStepId = useCreationWizardStore((state) => state.currentStepId)
  const draftName = useCreationWizardStore((state) => state.draftName)
  const selectedArchetypeId = useCreationWizardStore((state) => state.selectedArchetypeId)
  const selectedCareerId = useCreationWizardStore((state) => state.selectedCareerId)
  const setCurrentStep = useCreationWizardStore((state) => state.setCurrentStep)
  const setDraftName = useCreationWizardStore((state) => state.setDraftName)
  const setSelectedArchetypeId = useCreationWizardStore((state) => state.setSelectedArchetypeId)
  const setSelectedCareerId = useCreationWizardStore((state) => state.setSelectedCareerId)
  const resetWizard = useCreationWizardStore((state) => state.reset)

  const rulesetsQuery = useRulesets()
  const activeRulesetId = selectedRulesetId ?? rulesetsQuery.data?.[0]?.id ?? ''
  const catalogQuery = useRuleCatalog(activeRulesetId)
  const catalog = catalogQuery.data

  const currentStepIndex = Math.max(
    wizardSteps.findIndex((step) => step.id === currentStepId),
    0,
  )
  const currentStep = wizardSteps[currentStepIndex]
  const isFirstStep = currentStepIndex === 0
  const isLastStep = currentStepIndex === wizardSteps.length - 1

  const archetypes = useMemo(
    () => catalog?.entities.filter((entity) => entity.entityType === 'archetype') ?? [],
    [catalog],
  )
  const careers = useMemo(
    () => catalog?.entities.filter((entity) => entity.entityType === 'career') ?? [],
    [catalog],
  )
  const definitions = catalog?.definitions ?? []

  const selectedArchetype = archetypes.find((entity) => entity.id === selectedArchetypeId) ?? archetypes[0]
  const selectedCareer = careers.find((entity) => entity.id === selectedCareerId) ?? careers[0]
  const createDraftMutation = useMutation({
    mutationFn: () =>
      createCharacterDraft(session?.accessToken ?? '', {
        name: draftName.trim(),
        rulesetId: activeRulesetId,
        archetypeId: selectedArchetype?.id ?? null,
        careerId: selectedCareer?.id ?? null,
      }),
  })
  const archetypeProfile = parseDefinition<ArchetypeProfile>(definitions, selectedArchetype?.id, 'starting-profile')
  const careerProfile = parseDefinition<CareerProfile>(definitions, selectedCareer?.id, 'career-profile')
  const careerSkillKeys = new Set(careerProfile?.careerSkillKeys ?? [])
  const createdSkills = createDraftMutation.data?.skills ?? []
  const characteristics = archetypeProfile?.characteristics ?? createDefaultCharacteristics()
  const derivedStats = archetypeProfile?.derivedStats ?? createDefaultDerivedStats()
  const skillGroups = createSkillGroups(catalog?.entities ?? [], definitions, careerSkillKeys, createdSkills)

  useEffect(() => {
    const firstRuleset = rulesetsQuery.data?.[0]
    if (!selectedRulesetId && firstRuleset) {
      setSelectedRulesetId(firstRuleset.id)
    }
  }, [rulesetsQuery.data, selectedRulesetId, setSelectedRulesetId])

  useEffect(() => {
    if (!selectedArchetypeId && archetypes[0]) {
      setSelectedArchetypeId(archetypes[0].id)
    }
  }, [archetypes, selectedArchetypeId, setSelectedArchetypeId])

  useEffect(() => {
    if (!selectedCareerId && careers[0]) {
      setSelectedCareerId(careers[0].id)
    }
  }, [careers, selectedCareerId, setSelectedCareerId])

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

  function handleRulesetChange(nextRulesetId: string) {
    setSelectedRulesetId(nextRulesetId)
    setSelectedArchetypeId('')
    setSelectedCareerId('')
    createDraftMutation.reset()
  }

  function handleCreateDraft() {
    if (!canCreateDraft) {
      return
    }

    createDraftMutation.mutate()
  }

  const displayName = draftName.trim() || 'Новый персонаж'
  const initials = displayName
    .split(' ')
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part[0]?.toUpperCase())
    .join('')
  const canCreateDraft =
    Boolean(session) &&
    draftName.trim().length >= 2 &&
    activeRulesetId.length > 0 &&
    Boolean(selectedArchetype) &&
    Boolean(selectedCareer) &&
    !createDraftMutation.isPending

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
              <p>
                Выберите набор правил, архетип и карьеру. Лист сразу показывает стартовые параметры,
                карьерные навыки и данные, которые попадут в черновик.
              </p>
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

                {rulesetsQuery.isPending ? <p className="muted">Загружаем наборы правил...</p> : null}
                {catalogQuery.isPending && activeRulesetId ? <p className="muted">Загружаем каталог правил...</p> : null}
                {rulesetsQuery.isError || catalogQuery.isError ? (
                  <p className="form-error">Не удалось загрузить данные правил для создания персонажа.</p>
                ) : null}

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
                        onChange={(event) => {
                          setDraftName(event.target.value)
                          createDraftMutation.reset()
                        }}
                      />
                    </label>

                    <label className="sheet-field-row">
                      <span>Набор правил</span>
                      <select value={activeRulesetId} onChange={(event) => handleRulesetChange(event.target.value)}>
                        {(rulesetsQuery.data ?? []).map((ruleset) => (
                          <option key={ruleset.id} value={ruleset.id}>
                            {ruleset.name} v{ruleset.version}
                          </option>
                        ))}
                      </select>
                    </label>

                    <label className="sheet-field-row">
                      <span>Архетип</span>
                      <select
                        value={selectedArchetype?.id ?? ''}
                        onChange={(event) => {
                          setSelectedArchetypeId(event.target.value)
                          setCurrentStep('origin')
                          createDraftMutation.reset()
                        }}
                      >
                        {archetypes.map((archetype) => (
                          <option key={archetype.id} value={archetype.id}>
                            {archetype.name}
                          </option>
                        ))}
                      </select>
                    </label>

                    <label className="sheet-field-row">
                      <span>Карьера</span>
                      <select
                        value={selectedCareer?.id ?? ''}
                        onChange={(event) => {
                          setSelectedCareerId(event.target.value)
                          setCurrentStep('career')
                          createDraftMutation.reset()
                        }}
                      >
                        {careers.map((career) => (
                          <option key={career.id} value={career.id}>
                            {career.name}
                          </option>
                        ))}
                      </select>
                    </label>

                    <div className="sheet-field-row">
                      <span>Карьерные ранги</span>
                      <div className="sheet-static-value">
                        Выберите {careerProfile?.careerSkillRanksToAssign ?? 0} карьерных навыка для стартовой прокачки
                      </div>
                    </div>
                  </div>

                  <aside className="sheet-panel sheet-portrait" aria-label="Образ персонажа">
                    <h2>Образ</h2>
                    <div className="portrait-mark" aria-hidden="true">
                      {initials || 'GF'}
                    </div>
                    <p>{displayName}</p>
                    {selectedArchetype ? <small>{selectedArchetype.name}</small> : null}
                    {selectedCareer ? <small>{selectedCareer.name}</small> : null}
                  </aside>
                </section>

                <section className="sheet-derived-grid" aria-label="Производные параметры">
                  {createDerivedCards(derivedStats).map((stat) => (
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
                    {characteristicOrder.map((key) => (
                      <article className="characteristic-token" key={key}>
                        <span>{characteristics[key] ?? 2}</span>
                        <strong>{characteristicLabels[key]}</strong>
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
                      <article className="skill-table" key={group.category}>
                        <h3>{skillCategoryLabels[group.category]}</h3>
                        <div className="skill-row skill-row-head">
                          <span>Навык</span>
                          <span>Кар.</span>
                          <span>Ранг</span>
                          <span>Пул</span>
                        </div>
                        {group.skills.map((skill) => (
                          <div className="skill-row" key={skill.entity.id}>
                            <span>
                              {skill.entity.name} ({getCharacteristicShortLabel(skill.characteristic)})
                            </span>
                            <span>{skill.isCareerSkill ? 'Да' : 'Нет'}</span>
                            <span>{skill.rank}</span>
                            <span className="dice-pool" aria-label={`Пул ${skill.entity.name}`}>
                              {createDicePool(characteristics[skill.characteristic] ?? 2, skill.rank).map((die, index) => (
                                <span className={`die ${die}`} key={`${skill.entity.id}-${index}`} />
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
                  <button className="submit-button" disabled={!canCreateDraft} type="button" onClick={handleCreateDraft}>
                    {createDraftMutation.isPending ? 'Создаем...' : 'Создать черновик'}
                  </button>
                  <button className="submit-button" disabled={isLastStep} type="button" onClick={goToNextStep}>
                    Далее
                  </button>
                </footer>

                {createDraftMutation.isSuccess ? (
                  <p className="success-note">
                    Черновик создан: {createDraftMutation.data.name}. Навыков назначено: {createDraftMutation.data.skills.length}.
                  </p>
                ) : null}
                {createDraftMutation.isError ? (
                  <p className="form-error">Не удалось создать черновик. Проверьте выбранные данные и попробуйте еще раз.</p>
                ) : null}
              </section>
            </div>
          )}
        </section>
      </section>
    </main>
  )
}

function parseDefinition<TDefinition>(
  definitions: RuleDefinitionDto[],
  ruleEntityId: string | undefined,
  definitionKey: string,
) {
  if (!ruleEntityId) {
    return null
  }

  const definition = definitions.find(
    (candidate) => candidate.ruleEntityId === ruleEntityId && candidate.key === definitionKey,
  )
  if (!definition) {
    return null
  }

  try {
    return JSON.parse(definition.contentJson) as TDefinition
  } catch {
    return null
  }
}

function createDefaultCharacteristics() {
  return Object.fromEntries(characteristicOrder.map((key) => [key, 2]))
}

function createDefaultDerivedStats() {
  return {
    woundThreshold: 10,
    strainThreshold: 10,
    soak: 2,
    meleeDefense: 0,
    rangedDefense: 0,
  }
}

function createDerivedCards(derivedStats: Record<string, number>) {
  return [
    {
      label: derivedStatLabels.woundThreshold.label,
      value: derivedStats.woundThreshold ?? 10,
      caption: derivedStatLabels.woundThreshold.caption,
    },
    {
      label: derivedStatLabels.strainThreshold.label,
      value: derivedStats.strainThreshold ?? 10,
      caption: derivedStatLabels.strainThreshold.caption,
    },
    {
      label: derivedStatLabels.soak.label,
      value: derivedStats.soak ?? 2,
      caption: derivedStatLabels.soak.caption,
    },
    {
      label: derivedStatLabels.defense.label,
      value: `${derivedStats.meleeDefense ?? 0} / ${derivedStats.rangedDefense ?? 0}`,
      caption: derivedStatLabels.defense.caption,
    },
  ]
}

function createSkillGroups(
  entities: RuleEntityDto[],
  definitions: RuleDefinitionDto[],
  careerSkillKeys: Set<string>,
  createdSkills: CharacterSkillResponse[],
) {
  const createdSkillByRuleEntityId = new Map(createdSkills.map((skill) => [skill.ruleEntityId, skill]))
  const skills = entities
    .filter((entity) => entity.entityType === 'skill')
    .map<SkillViewModel>((entity) => {
      const profile = parseDefinition<SkillProfile>(definitions, entity.id, 'skill-profile')
      const createdSkill = createdSkillByRuleEntityId.get(entity.id)
      return {
        entity,
        category: profile?.category ?? 'general',
        characteristic: profile?.characteristic ?? 'intellect',
        isCareerSkill: createdSkill?.isCareerSkill ?? careerSkillKeys.has(entity.key),
        rank: createdSkill?.rank ?? 0,
      }
    })
    .sort((left, right) => left.entity.name.localeCompare(right.entity.name, 'ru'))

  return skillCategoryOrder.map((category) => ({
    category,
    skills: skills.filter((skill) => skill.category === category),
  }))
}

function getCharacteristicShortLabel(characteristic: string) {
  const label = characteristicLabels[characteristic] ?? characteristic
  return label.slice(0, 3).toUpperCase()
}

function createDicePool(characteristicValue: number, rank: number) {
  const upgradedDice = Math.min(characteristicValue, rank)
  const abilityDice = Math.max(characteristicValue, rank) - upgradedDice
  return [
    ...Array.from({ length: upgradedDice }, () => 'proficiency'),
    ...Array.from({ length: abilityDice }, () => 'ability'),
  ]
}
