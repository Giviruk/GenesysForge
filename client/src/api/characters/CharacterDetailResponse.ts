import type { RuleSnapshotDto } from '../rules/RuleSnapshotDto'
import type { CalculatedCharacterStatsDto } from './CalculatedCharacterStatsDto'
import type { CharacterSummaryResponse } from './CharacterSummaryResponse'

export type CharacterDetailResponse = CharacterSummaryResponse & {
  ruleSnapshot: RuleSnapshotDto | null
  calculatedStats: CalculatedCharacterStatsDto | null
}
