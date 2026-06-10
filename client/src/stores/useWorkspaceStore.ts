import { create } from 'zustand'

type WorkspaceState = {
  activeSection: 'characters' | 'rulesets' | 'settings'
  lastDraftName: string
  setActiveSection: (section: WorkspaceState['activeSection']) => void
  setLastDraftName: (name: string) => void
}

export const useWorkspaceStore = create<WorkspaceState>((set) => ({
  activeSection: 'characters',
  lastDraftName: '',
  setActiveSection: (section) => set({ activeSection: section }),
  setLastDraftName: (name) => set({ lastDraftName: name }),
}))
