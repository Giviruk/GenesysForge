import type { CharacterSummaryResponse } from '../../api/characters/CharacterSummaryResponse'

type CharacterCardProps = {
  character: CharacterSummaryResponse
}

const statusLabels: Record<CharacterSummaryResponse['status'], string> = {
  Draft: 'Черновик',
  Active: 'Активен',
  Archived: 'В архиве',
}

const dateFormatter = new Intl.DateTimeFormat('ru-RU', {
  day: '2-digit',
  month: 'long',
  year: 'numeric',
})

export function CharacterCard({ character }: CharacterCardProps) {
  return (
    <article className="character-card">
      <div>
        <h2>{character.name}</h2>
        <p>Обновлен {dateFormatter.format(new Date(character.updatedAt))}</p>
      </div>
      <span className="character-status">{statusLabels[character.status]}</span>
    </article>
  )
}
