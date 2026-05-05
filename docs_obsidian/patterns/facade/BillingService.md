---
name: BillingService
description: Pricing subsystem behind the facade — Facade 'SubSystem' #2.
tags:
  - pattern/facade
  - role/concrete
  - layer/application
---

# BillingService

`class BillingService` in `RentalShop.Application.Facades`.

## Pattern role
**SubSystem (subsystem #2)** in the Facade GoF pattern.

## Domain role
Billing subsystem — applies the active pricing [[PricingCalculator]] and returns the line total.

## Depends on
- [[PricingCalculator]] (Strategy.Context)

## Related
- [[Facade-Overview]]
