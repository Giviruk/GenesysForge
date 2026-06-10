import type { ApiId } from '../common/ApiId'

export type RuleEntityDto = {
  id: ApiId
  rulesetId: ApiId
  entityType: string
  key: string
  name: string
  description: string | null
}
