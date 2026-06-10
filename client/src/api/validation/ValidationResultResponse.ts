import type { ValidationMessageDto } from './ValidationMessageDto'

export type ValidationResultResponse = {
  isValid: boolean
  messages: ValidationMessageDto[]
}
