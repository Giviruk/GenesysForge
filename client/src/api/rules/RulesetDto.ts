import type { ApiId } from '../common/ApiId'

export type RulesetDto = {
  id: ApiId
  name: string
  version: string
  description: string | null
}
