import type { ApiId } from '../common/ApiId'

export type RegisterResponse = {
  userId: ApiId
  email: string
  displayName: string
}
