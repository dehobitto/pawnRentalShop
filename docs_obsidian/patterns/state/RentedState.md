---
name: RentedState
description: "Rented" — item is out with a customer. Concrete State B.
tags:
  - pattern/state
  - role/concrete
  - layer/domain
---

# RentedState

`class RentedState : IItemState` in `RentalShop.Domain.States`.

## Pattern role
**ConcreteState (variant B)** in the State GoF pattern.

## Domain role
"Rented" — the item is out with a customer. Handling a return event transitions back to [[AvailableState]]; a damage report transitions to [[UnderRepairState]].

## Implements
- [[IItemState]]

## Transitions to
- [[AvailableState]] (on return)
- [[UnderRepairState]] (on damage report)

## Related
- [[State-Overview]]
