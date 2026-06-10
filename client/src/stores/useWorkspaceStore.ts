import { create } from 'zustand'

type WorkspaceState = {
  activeSection: 'characters' | 'rulesets' | 'settings'
  lastDraftName: string
  selectedRulesetId: string | null
  setActiveSection: (section: WorkspaceState['activeSection']) => void
  setLastDraftName: (name: string) => void
  setSelectedRulesetId: (rulesetId: string) => void
}

export const useWorkspaceStore = create<WorkspaceState>((set) => ({
  activeSection: 'characters',
  lastDraftName: '',
  selectedRulesetId: null,
  setActiveSection: (section) => set({ activeSection: section }),
  setLastDraftName: (name) => set({ lastDraftName: name }),
  setSelectedRulesetId: (rulesetId) => set({ selectedRulesetId: rulesetId }),
}))
