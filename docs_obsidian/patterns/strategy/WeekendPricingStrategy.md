---
name: WeekendPricingStrategy
description: Weekend pricing (rate × days × 1.5) — Strategy ConcreteStrategy B.
tags:
  - pattern/strategy
  - role/concrete
  - layer/application
---

# WeekendPricingStrategy

`class WeekendPricingStrategy : IPricingStrategy` in `RentalShop.Application.Services`.

## Pattern role
**ConcreteStrategy (variant B)** in the Strategy GoF pattern.

## Domain role
Weekend pricing — base rate × days × 1.5 weekend multiplier.

## Implements
- [[IPricingStrategy]]

## Related
- [[Strategy-Overview]]
