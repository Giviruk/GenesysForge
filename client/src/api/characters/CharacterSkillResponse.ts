import type { ApiId } from '../common/ApiId'

export type CharacterSkillResponse = {
  ruleEntityId: ApiId
  rank: number
  xpSpent: number
  isCareerSkill: boolean
  updatedAt: string
}
