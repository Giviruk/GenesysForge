import type { RuleDefinitionDto } from './RuleDefinitionDto'
import type { RuleEntityDto } from './RuleEntityDto'
import type { RulesetDto } from './RulesetDto'
import type { RuleSourceVersionDto } from './RuleSourceVersionDto'
import type { SourceBookDto } from './SourceBookDto'

export type RuleCatalogResponse = {
  rulesets: RulesetDto[]
  sourceBooks: SourceBookDto[]
  sourceVersions: RuleSourceVersionDto[]
  entities: RuleEntityDto[]
  definitions: RuleDefinitionDto[]
}
