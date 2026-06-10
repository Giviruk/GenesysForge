import { useEffect, useId, useState } from 'react'
import { useRuleCatalog, useRulesets } from './useRulesets'

type RulesetSelectProps = {
  value: string
  onChange: (rulesetId: string) => void
}

export function RulesetSelect({ value, onChange }: RulesetSelectProps) {
  const labelId = useId()
  const triggerId = useId()
  const listboxId = useId()
  const [isOpen, setIsOpen] = useState(false)
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

  const selectedRuleset = rulesets.find((ruleset) => ruleset.id === value) ?? rulesets[0]

  function handleSelect(rulesetId: string) {
    onChange(rulesetId)
    setIsOpen(false)
  }

  return (
    <div className="ruleset-panel">
      <div className="ruleset-selector">
        <span id={labelId} className="ruleset-label">
          Набор правил
        </span>
        <button
          id={triggerId}
          aria-controls={isOpen ? listboxId : undefined}
          aria-expanded={isOpen}
          aria-haspopup="listbox"
          aria-labelledby={`${labelId} ${triggerId}`}
          className="ruleset-trigger"
          role="combobox"
          type="button"
          onClick={() => setIsOpen((current) => !current)}
          onKeyDown={(event) => {
            if (event.key === 'Escape') {
              setIsOpen(false)
            }
          }}
        >
          <span>{selectedRuleset.name} v{selectedRuleset.version}</span>
          <span className="ruleset-chevron" aria-hidden="true">
            ▾
          </span>
        </button>

        {isOpen ? (
          <div id={listboxId} className="ruleset-options" role="listbox" aria-labelledby={labelId}>
            {rulesets.map((ruleset) => (
              <button
                key={ruleset.id}
                aria-selected={ruleset.id === selectedRuleset.id}
                className={ruleset.id === selectedRuleset.id ? 'ruleset-option selected' : 'ruleset-option'}
                role="option"
                type="button"
                onClick={() => handleSelect(ruleset.id)}
              >
                <span>{ruleset.name}</span>
                <small>v{ruleset.version}</small>
              </button>
            ))}
          </div>
        ) : null}
      </div>

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
