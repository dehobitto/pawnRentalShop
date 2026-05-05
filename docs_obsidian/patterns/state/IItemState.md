---
name: IItemState
description: Lifecycle-state contract for rentable items — State 'State'.
tags:
  - pattern/state
  - role/interface
  - layer/domain
---

# IItemState

`interface IItemState` in `RentalShop.Domain.States`.

## Pattern role
**State** in the State GoF pattern.

## Domain role
Encapsulates which transitions are legal from the current state and prevents invalid operations (e.g. renting an already-rented item).

## Implemented by
- [[AvailableState]]
- [[RentedState]]
- [[UnderRepairState]]

## Used by
- [[ItemLifecycle]]

## Related
- [[State-Overview]]
