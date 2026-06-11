type PlaceholderStepProps = {
  description: string
  stepNumber: number
  title: string
}

export function PlaceholderStep({ description, stepNumber, title }: PlaceholderStepProps) {
  return (
    <div className="wizard-step-panel">
      <span className="wizard-kicker">Шаг {stepNumber}</span>
      <h2>{title}</h2>
      <p>{description}</p>
      <p className="muted">Содержимое этого шага появится в следующих пунктах плана.</p>
    </div>
  )
}
