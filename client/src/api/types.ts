export type ApiId = string

export type RegisterRequest = {
  email: string
  password: string
  displayName: string
}

export type RegisterResponse = {
  userId: ApiId
  email: string
  displayName: string
}

export type LoginRequest = {
  email: string
  password: string
}

export type UserProfileDto = {
  id: ApiId
  email: string
  displayName: string
}

export type AuthSessionResponse = {
  accessToken: string
  expiresAt: string
  user: UserProfileDto
}

export type CharacterStatus = 'Draft' | 'Active' | 'Archived'

export type CreateCharacterDraftRequest = {
  name: string
  rulesetId: ApiId
}

export type UpdateCharacterBasicInfoRequest = {
  name: string
}

export type CharacterSummaryResponse = {
  id: ApiId
  name: string
  status: CharacterStatus
  rulesetId: ApiId
  updatedAt: string
}

export type CharacterDetailResponse = CharacterSummaryResponse & {
  ruleSnapshot: RuleSnapshotDto | null
  calculatedStats: CalculatedCharacterStatsDto | null
}

export type CalculatedCharacterStatsDto = {
  availableXp: number
  spentXp: number
  characteristics: Record<string, number>
  derivedStats: Record<string, number>
}

export type RulesetDto = {
  id: ApiId
  name: string
  version: string
  description: string | null
}

export type SourceBookDto = {
  id: ApiId
  rulesetId: ApiId
  key: string
  name: string
}

export type RuleSourceVersionDto = {
  id: ApiId
  sourceBookId: ApiId
  version: string
  isActive: boolean
}

export type RuleEntityDto = {
  id: ApiId
  rulesetId: ApiId
  entityType: string
  key: string
  name: string
  description: string | null
}

export type RuleCatalogResponse = {
  rulesets: RulesetDto[]
  sourceBooks: SourceBookDto[]
  sourceVersions: RuleSourceVersionDto[]
  entities: RuleEntityDto[]
}

export type RuleSnapshotDto = {
  rulesetId: ApiId
  createdAt: string
  sourceVersionIds: ApiId[]
  ruleEntityIds: ApiId[]
}

export type ValidationSeverity = 'Error' | 'Warning'

export type ValidationMessageDto = {
  code: string
  message: string
  severity: ValidationSeverity
  path: string | null
}

export type ValidationResultResponse = {
  isValid: boolean
  messages: ValidationMessageDto[]
}

export type ExportCharacterPdfRequest = {
  characterId: ApiId
}

export type ExportCharacterPdfResponse = {
  characterId: ApiId
  fileName: string
  contentType: string
}
