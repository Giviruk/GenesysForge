import type { ApiId } from '../common/ApiId'

export type CreateCharacterDraftRequest = {
  name: string
  rulesetId: ApiId
  archetypeId?: ApiId | null
  careerId?: ApiId | null
}
