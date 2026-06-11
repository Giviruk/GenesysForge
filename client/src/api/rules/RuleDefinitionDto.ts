import type { ApiId } from '../common/ApiId'

export type RuleDefinitionDto = {
  id: ApiId
  ruleEntityId: ApiId
  sourceVersionId: ApiId
  key: string
  contentJson: string
}
