---
name: Strategy — Overview
description: Pattern overview — swappable rental-pricing rules.
tags:
  - pattern/strategy
  - layer/application
  - role/overview
---

# Strategy — Overview

**Pattern role.** Defines a family of algorithms, encapsulates each one, and makes them interchangeable. Strategy lets the algorithm vary independently from clients that use it.

**Domain role.** Pricing engine — Standard / Weekend / Loyalty rules can be swapped at runtime without touching callers.

## Type roster
- [[IPricingStrategy]] — Strategy interface (`RentalShop.Application.Services`)
  - [[StandardPricingStrategy]] — ConcreteStrategy A
  - [[WeekendPricingStrategy]] — ConcreteStrategy B
  - [[LoyaltyPricingStrategy]] — ConcreteStrategy C
- [[PricingCalculator]] — Context (holds active strategy, exposes `Calculate`)

## Wiring
[[BillingService]] → [[PricingCalculator]] → active [[IPricingStrategy]].

## Related
- [[_index]] · [[overview]] · [[pattern-collisions]] (this Context ≠ [[ItemLifecycle]])
