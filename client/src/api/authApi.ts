import type { AuthSessionResponse } from './auth/AuthSessionResponse'
import type { LoginRequest } from './auth/LoginRequest'
import type { RegisterRequest } from './auth/RegisterRequest'
import type { RegisterResponse } from './auth/RegisterResponse'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? '/api'

export class ApiError extends Error {
  public readonly status: number

  public constructor(status: number, message: string) {
    super(message)
    this.status = status
  }
}

export async function register(request: RegisterRequest): Promise<RegisterResponse> {
  return postJson<RegisterResponse>('/auth/register', request)
}

export async function login(request: LoginRequest): Promise<AuthSessionResponse> {
  return postJson<AuthSessionResponse>('/auth/login', request)
}

async function postJson<TResponse>(path: string, body: unknown): Promise<TResponse> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body),
  })

  if (!response.ok) {
    throw new ApiError(response.status, await readErrorMessage(response))
  }

  return response.json() as Promise<TResponse>
}

async function readErrorMessage(response: Response): Promise<string> {
  const fallback = `Request failed with status ${response.status}`

  try {
    const payload = (await response.json()) as { title?: string; detail?: string }
    return payload.detail ?? payload.title ?? fallback
  } catch {
    return fallback
  }
}
