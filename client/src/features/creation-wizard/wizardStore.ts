import { create } from 'zustand'

export const wizardSteps = [
  {
    id: 'basic-info',
    title: 'Основное',
    description: 'Имя персонажа и набор правил.',
  },
  {
    id: 'origin',
    title: 'Происхождение',
    description: 'Вид, архетип и стартовая идея.',
  },
  {
    id: 'career',
    title: 'Карьера',
    description: 'Будущая роль и карьерные навыки.',
  },
  {
    id: 'review',
    title: 'Проверка',
    description: 'Сводка перед созданием черновика.',
  },
] as const

export type WizardStepId = (typeof wizardSteps)[number]['id']

type CreationWizardState = {
  currentStepId: WizardStepId
  draftName: string
  reset: () => void
  setCurrentStep: (stepId: WizardStepId) => void
  setDraftName: (name: string) => void
}

export const useCreationWizardStore = create<CreationWizardState>((set) => ({
  currentStepId: 'basic-info',
  draftName: '',
  reset: () => set({ currentStepId: 'basic-info', draftName: '' }),
  setCurrentStep: (stepId) => set({ currentStepId: stepId }),
  setDraftName: (name) => set({ draftName: name }),
}))
