---
name: State — Overview
description: Pattern overview — item lifecycle (Available, Rented, Under Repair).
tags:
  - pattern/state
  - layer/domain
  - role/overview
---

# State — Overview

**Pattern role.** Allows an object to alter its behaviour when its internal state changes; the object appears to change its class.

**Domain role.** Manages the lifecycle of a rentable item (Available → Rented → Under Repair) so invalid operations (renting a rented item, returning an item under repair) become impossible by construction.

## Type roster
- [[IItemState]] — State interface (`RentalShop.Domain.States`)
  - [[AvailableState]] — ConcreteState A (Available)
  - [[RentedState]] — ConcreteState B (Rented)
  - [[UnderRepairState]] — ConcreteState C (Under Repair) *(extension over canonical A/B set — see [[claude]] notes)*
- [[ItemLifecycle]] — Context (owns the current state of one item)

## Wiring
`Program.cs` constructs an [[ItemLifecycle]] in [[AvailableState]], then drives it through `Advance()` and direct `CurrentState` assignment.

## Related
- [[_index]] · [[overview]] · [[pattern-collisions]] (this Context ≠ [[PricingCalculator]])
