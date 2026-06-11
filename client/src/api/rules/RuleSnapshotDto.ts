import type { ApiId } from '../common/ApiId'

export type RuleSnapshotDto = {
  rulesetId: ApiId
  createdAt: string
  sourceVersionIds: ApiId[]
  ruleEntityIds: ApiId[]
  ruleDefinitionIds: ApiId[]
}
