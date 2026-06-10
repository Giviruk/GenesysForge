import type { ValidationSeverity } from './ValidationSeverity'

export type ValidationMessageDto = {
  code: string
  message: string
  severity: ValidationSeverity
  path: string | null
}
