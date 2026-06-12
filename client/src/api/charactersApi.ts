import { ApiError } from './authApi'
import type { CharacterDetailResponse } from './characters/CharacterDetailResponse'
import type { CharacterSummaryResponse } from './characters/CharacterSummaryResponse'
import type { CreateCharacterDraftRequest } from './characters/CreateCharacterDraftRequest'
import type { UpdateCharacterSkillsRequest } from './characters/UpdateCharacterSkillsRequest'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? '/api'

export async function createCharacterDraft(
  accessToken: string,
  request: CreateCharacterDraftRequest,
): Promise<CharacterDetailResponse> {
  return sendJson<CharacterDetailResponse>('/characters', accessToken, 'POST', request)
}

export async function getCharacter(accessToken: string, characterId: string): Promise<CharacterDetailResponse> {
  return getJson<CharacterDetailResponse>(`/characters/${characterId}`, accessToken)
}

export async function updateCharacterSkills(
  accessToken: string,
  characterId: string,
  request: UpdateCharacterSkillsRequest,
): Promise<CharacterDetailResponse> {
  return sendJson<CharacterDetailResponse>(`/characters/${characterId}/skills`, accessToken, 'PUT', request)
}

export async function listMyCharacters(accessToken: string): Promise<CharacterSummaryResponse[]> {
  return getJson<CharacterSummaryResponse[]>('/characters', accessToken)
}

async function getJson<TResponse>(path: string, accessToken: string): Promise<TResponse> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: createHeaders(accessToken),
  })

  if (!response.ok) {
    throw new ApiError(response.status, await readErrorMessage(response))
  }

  return response.json() as Promise<TResponse>
}

async function sendJson<TResponse>(
  path: string,
  accessToken: string,
  method: 'POST' | 'PUT',
  body: unknown,
): Promise<TResponse> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method,
    headers: createHeaders(accessToken, true),
    body: JSON.stringify(body),
  })

  if (!response.ok) {
    throw new ApiError(response.status, await readErrorMessage(response))
  }

  return response.json() as Promise<TResponse>
}

function createHeaders(accessToken: string, hasJsonBody = false): HeadersInit {
  return {
    ...(hasJsonBody ? { 'Content-Type': 'application/json' } : {}),
    Authorization: `Bearer ${accessToken}`,
  }
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
