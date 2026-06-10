import type { ApiId } from '../common/ApiId'

export type ExportCharacterPdfResponse = {
  characterId: ApiId
  fileName: string
  contentType: string
}
