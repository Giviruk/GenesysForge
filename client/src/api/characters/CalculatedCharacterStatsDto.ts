export type CalculatedCharacterStatsDto = {
  availableXp: number
  spentXp: number
  characteristics: Record<string, number>
  derivedStats: Record<string, number>
}
