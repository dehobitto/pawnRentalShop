---
name: ItemLifecycle
description: Owns the current lifecycle state of a single rentable item — State 'Context'.
tags:
  - pattern/state
  - role/orchestrator
  - layer/domain
  - entity
---

# ItemLifecycle

`class ItemLifecycle` in `RentalShop.Domain.States`.

## Pattern role
**Context** in the State GoF pattern — the object whose behaviour changes as its current [[IItemState]] changes.

## Domain role
Owns the current lifecycle state of a single rentable item. Callers invoke `Advance()`; what actually happens is decided by the active state.

## Holds
- [[IItemState]] (one of [[AvailableState]] / [[RentedState]] / [[UnderRepairState]])

## Related
- [[State-Overview]] · [[pattern-collisions]] (this Context ≠ [[PricingCalculator]])
