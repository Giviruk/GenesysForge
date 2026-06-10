import { useQuery } from '@tanstack/react-query'
import { getRuleCatalog, getRulesets } from '../../api/rulesApi'

export function useRulesets() {
  return useQuery({
    queryKey: ['rulesets'],
    queryFn: getRulesets,
  })
}

export function useRuleCatalog(rulesetId: string) {
  return useQuery({
    enabled: rulesetId.length > 0,
    queryKey: ['rule-catalog', rulesetId],
    queryFn: () => getRuleCatalog(rulesetId),
  })
}
