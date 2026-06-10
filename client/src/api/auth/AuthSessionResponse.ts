import type { UserProfileDto } from './UserProfileDto'

export type AuthSessionResponse = {
  accessToken: string
  expiresAt: string
  user: UserProfileDto
}
