import { useEffect } from 'react'
import { useRuleCatalog, useRulesets } from './useRulesets'

type RulesetSelectProps = {
  value: string
  onChange: (rulesetId: string) => void
}

export function RulesetSelect({ value, onChange }: RulesetSelectProps) {
  const rulesetsQuery = useRulesets()
  const catalogQuery = useRuleCatalog(value)

  useEffect(() => {
    const firstRuleset = rulesetsQuery.data?.[0]
    const hasSelectedRuleset = rulesetsQuery.data?.some((ruleset) => ruleset.id === value)
    if (firstRuleset && (!value || !hasSelectedRuleset)) {
      onChange(firstRuleset.id)
    }
  }, [onChange, rulesetsQuery.data, value])

  if (rulesetsQuery.isPending) {
    return (
      <div className="ruleset-panel" aria-live="polite">
        <span className="ruleset-label">Набор правил</span>
        <p className="muted">Загружаем доступные наборы правил...</p>
      </div>
    )
  }

  if (rulesetsQuery.isError) {
    return (
      <div className="ruleset-panel" aria-live="polite">
        <span className="ruleset-label">Набор правил</span>
        <p className="form-error">Не удалось загрузить наборы правил.</p>
        <button className="text-button" type="button" onClick={() => void rulesetsQuery.refetch()}>
          Повторить
        </button>
      </div>
    )
  }

  const rulesets = rulesetsQuery.data ?? []

  if (rulesets.length === 0) {
    return (
      <div className="ruleset-panel" aria-live="polite">
        <span className="ruleset-label">Набор правил</span>
        <p className="muted">Пока нет доступных наборов правил.</p>
      </div>
    )
  }

  return (
    <div className="ruleset-panel">
      <label>
        Набор правил
        <select value={value} onChange={(event) => onChange(event.target.value)}>
          {rulesets.map((ruleset) => (
            <option key={ruleset.id} value={ruleset.id}>
              {ruleset.name} v{ruleset.version}
            </option>
          ))}
        </select>
      </label>

      {catalogQuery.isPending ? <p className="muted">Загружаем каталог выбранного набора...</p> : null}
      {catalogQuery.isError ? (
        <p className="form-error">Не удалось загрузить каталог выбранного набора.</p>
      ) : null}
      {catalogQuery.data ? (
        <p className="ruleset-summary">
          Источников: {catalogQuery.data.sourceBooks.length}. Версий: {catalogQuery.data.sourceVersions.length}.
          Элементов правил: {catalogQuery.data.entities.length}.
        </p>
      ) : null}
    </div>
  )
}
