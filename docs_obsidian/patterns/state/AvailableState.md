---
name: AvailableState
description: "Available" — item is on the shelf and may be rented. Concrete State A.
tags:
  - pattern/state
  - role/concrete
  - layer/domain
---

# AvailableState

`class AvailableState : IItemState` in `RentalShop.Domain.States`.

## Pattern role
**ConcreteState (variant A)** in the State GoF pattern.

## Domain role
"Available" — the item is on the shelf and may be rented. Handling a rent event transitions to [[RentedState]].

## Implements
- [[IItemState]]

## Transitions to
- [[RentedState]] (on rent)

## Related
- [[State-Overview]]
