import { ApiError } from './authApi'
import type { ValidationResultResponse } from './validation/ValidationResultResponse'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? '/api'

export async function validateCharacter(accessToken: string, characterId: string): Promise<ValidationResultResponse> {
  const response = await fetch(`${API_BASE_URL}/characters/${characterId}/validation`, {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new ApiError(response.status, await readErrorMessage(response))
  }

  return response.json() as Promise<ValidationResultResponse>
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
