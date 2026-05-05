---
name: LoyaltyPricingStrategy
description: Loyalty pricing (15% repeat-customer discount) — Strategy ConcreteStrategy C.
tags:
  - pattern/strategy
  - role/concrete
  - layer/application
---

# LoyaltyPricingStrategy

`class LoyaltyPricingStrategy : IPricingStrategy` in `RentalShop.Application.Services`.

## Pattern role
**ConcreteStrategy (variant C)** in the Strategy GoF pattern.

## Domain role
Loyalty / repeat-customer pricing — applies a 15% discount on top of the standard base rate.

## Implements
- [[IPricingStrategy]]

## Related
- [[Strategy-Overview]]
