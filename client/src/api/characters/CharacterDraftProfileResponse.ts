import type { ApiId } from '../common/ApiId'

export type CharacterDraftProfileResponse = {
  archetypeId: ApiId | null
  careerId: ApiId | null
  careerSkillRanksToAssign: number
}
