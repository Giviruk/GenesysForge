type BasicInfoShellStepProps = {
  draftName: string
  onDraftNameChange: (name: string) => void
}

export function BasicInfoShellStep({ draftName, onDraftNameChange }: BasicInfoShellStepProps) {
  return (
    <div className="wizard-step-panel">
      <span className="wizard-kicker">Шаг 1</span>
      <h2>Основная информация</h2>
      <p>
        Этот шаг пока хранит данные локально. На следующем пункте плана он будет подключен к созданию draft-персонажа
        через API.
      </p>

      <label className="wizard-field">
        Имя персонажа
        <input
          value={draftName}
          placeholder="Asha Vorn"
          onChange={(event) => onDraftNameChange(event.target.value)}
        />
      </label>
    </div>
  )
}
