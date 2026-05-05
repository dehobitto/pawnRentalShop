---
name: PricingCalculator
description: Pricing engine that holds the active pricing strategy — Strategy 'Context'.
tags:
  - pattern/strategy
  - role/orchestrator
  - layer/application
---

# PricingCalculator

`class PricingCalculator` in `RentalShop.Application.Services`.

## Pattern role
**Context** in the Strategy GoF pattern — holds a strategy reference and exposes a stable call-site that delegates to it.

## Domain role
Holds the active [[IPricingStrategy]] and gives the rest of the domain a single, stable entry point for price calculation.

## Holds
- [[IPricingStrategy]] (one of [[StandardPricingStrategy]] / [[WeekendPricingStrategy]] / [[LoyaltyPricingStrategy]])

## Used by
- [[BillingService]] (Facade subsystem)

## Related
- [[Strategy-Overview]] · [[pattern-collisions]] (this Context ≠ [[ItemLifecycle]])
