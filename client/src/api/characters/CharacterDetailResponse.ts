import type { RuleSnapshotDto } from '../rules/RuleSnapshotDto'
import type { CalculatedCharacterStatsDto } from './CalculatedCharacterStatsDto'
import type { CharacterDraftProfileResponse } from './CharacterDraftProfileResponse'
import type { CharacterSkillResponse } from './CharacterSkillResponse'
import type { CharacterSummaryResponse } from './CharacterSummaryResponse'

export type CharacterDetailResponse = CharacterSummaryResponse & {
  ruleSnapshot: RuleSnapshotDto | null
  calculatedStats: CalculatedCharacterStatsDto | null
  draftProfile: CharacterDraftProfileResponse | null
  skills: CharacterSkillResponse[]
}
