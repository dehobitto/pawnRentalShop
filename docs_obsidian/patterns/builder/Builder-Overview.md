---
name: Builder — Overview
description: Pattern overview — step-by-step assembly of complex rental orders.
tags:
  - pattern/builder
  - layer/application
  - role/overview
---

# Builder — Overview

**Pattern role.** Separates the construction of a complex object from its representation so the same construction process can produce different representations.

**Domain role.** Assembles rental orders piece-by-piece — line items first, then extras (insurance, deposits, delivery, discounts).

## Type roster
- [[OrderDirector]] — Director (`RentalShop.Application.Builders`)
- [[OrderBuilder]] — abstract Builder
  - [[StandardOrderBuilder]] — ConcreteBuilder (standard rental)
  - [[PremiumOrderBuilder]] — ConcreteBuilder (premium rental)
- [[RentalOrder]] — Product (`RentalShop.Domain.Entities`)

## Wiring
`Program.cs` constructs an [[OrderDirector]] and drives a [[StandardOrderBuilder]] / [[PremiumOrderBuilder]] through `AddItems()` → `AddExtras()`, then calls `GetOrder()`.

## Related
- [[_index]] · [[overview]] · [[pattern-collisions]]
