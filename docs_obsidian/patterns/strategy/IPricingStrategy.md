---
name: IPricingStrategy
description: Contract for swappable rental-pricing algorithms — Strategy 'Strategy'.
tags:
  - pattern/strategy
  - role/interface
  - layer/application
---

# IPricingStrategy

`interface IPricingStrategy` in `RentalShop.Application.Services`.

## Pattern role
**Strategy** in the Strategy GoF pattern.

## Domain role
Contract for swappable rental-pricing rules. Adding a new pricing scheme means writing a new implementation, never editing existing ones.

## Implemented by
- [[StandardPricingStrategy]]
- [[WeekendPricingStrategy]]
- [[LoyaltyPricingStrategy]]

## Used by
- [[PricingCalculator]]

## Related
- [[Strategy-Overview]]
