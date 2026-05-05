---
name: UnderRepairState
description: "Under Repair" — item is unavailable for rental. Concrete State C (extension over canonical A/B).
tags:
  - pattern/state
  - role/concrete
  - layer/domain
---

# UnderRepairState

`class UnderRepairState : IItemState` in `RentalShop.Domain.States`.

## Pattern role
**ConcreteState (variant C)** in the State GoF pattern. *Extension over the canonical A/B set — added to fulfil the brief's three-state requirement.*

## Domain role
"Under Repair" — the item is unavailable for rental. Handling a repair-completed event transitions back to [[AvailableState]].

## Implements
- [[IItemState]]

## Transitions to
- [[AvailableState]] (on repair completed)

## Related
- [[State-Overview]]
