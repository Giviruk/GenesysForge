import type { ApiId } from '../common/ApiId'
import type { CharacterStatus } from './CharacterStatus'

export type CharacterSummaryResponse = {
  id: ApiId
  name: string
  status: CharacterStatus
  rulesetId: ApiId
  updatedAt: string
}
