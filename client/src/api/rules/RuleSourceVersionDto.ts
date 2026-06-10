import type { ApiId } from '../common/ApiId'

export type RuleSourceVersionDto = {
  id: ApiId
  sourceBookId: ApiId
  version: string
  isActive: boolean
}
