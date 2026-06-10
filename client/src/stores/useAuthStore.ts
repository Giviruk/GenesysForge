import { create } from 'zustand'
import type { AuthSessionResponse } from '../api/auth/AuthSessionResponse'

const STORAGE_KEY = 'genesys-forge-auth-session'

type AuthState = {
  session: AuthSessionResponse | null
  setSession: (session: AuthSessionResponse) => void
  clearSession: () => void
}

export const useAuthStore = create<AuthState>((set) => ({
  session: readStoredSession(),
  setSession: (session) => {
    writeStoredSession(session)
    set({ session })
  },
  clearSession: () => {
    sessionStorage.removeItem(STORAGE_KEY)
    set({ session: null })
  },
}))

function readStoredSession() {
  const rawSession = sessionStorage.getItem(STORAGE_KEY)
  if (!rawSession) {
    return null
  }

  try {
    const session = JSON.parse(rawSession) as AuthSessionResponse
    if (new Date(session.expiresAt).getTime() <= Date.now()) {
      sessionStorage.removeItem(STORAGE_KEY)
      return null
    }

    return session
  } catch {
    sessionStorage.removeItem(STORAGE_KEY)
    return null
  }
}

function writeStoredSession(session: AuthSessionResponse) {
  sessionStorage.setItem(STORAGE_KEY, JSON.stringify(session))
}
