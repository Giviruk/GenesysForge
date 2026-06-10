import { ApiError } from './authApi'
import type { RuleCatalogResponse } from './rules/RuleCatalogResponse'
import type { RulesetDto } from './rules/RulesetDto'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? '/api'

export async function getRulesets(): Promise<RulesetDto[]> {
  return getJson<RulesetDto[]>('/rules/rulesets')
}

export async function getRuleCatalog(rulesetId: string): Promise<RuleCatalogResponse> {
  return getJson<RuleCatalogResponse>(`/rules/${rulesetId}/catalog`)
}

async function getJson<TResponse>(path: string): Promise<TResponse> {
  const response = await fetch(`${API_BASE_URL}${path}`)

  if (!response.ok) {
    throw new ApiError(response.status, await readErrorMessage(response))
  }

  return response.json() as Promise<TResponse>
}

async function readErrorMessage(response: Response): Promise<string> {
  const fallback = `Запрос завершился ошибкой ${response.status}`

  try {
    const payload = (await response.json()) as { title?: string; detail?: string }
    return payload.detail ?? payload.title ?? fallback
  } catch {
    return fallback
  }
}
