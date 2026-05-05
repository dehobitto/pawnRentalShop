---
name: OrderBuilder
description: Abstract step-by-step rental-order builder — Builder 'Builder'.
tags:
  - pattern/builder
  - role/abstract
  - layer/application
---

# OrderBuilder

`abstract class OrderBuilder` in `RentalShop.Application.Builders`.

## Pattern role
**Builder** in the Builder GoF pattern.

## Domain role
Contract for rental-order assemblers. Different builders produce different order flavours (standard, premium, corporate) while the [[OrderDirector]]'s script stays untouched.

## Steps
- `AddItems()` — line items / rented goods
- `AddExtras()` — insurance, deposit, delivery
- `GetOrder()` — hand back the finished [[RentalOrder]]

## Subclassed by
- [[StandardOrderBuilder]]
- [[PremiumOrderBuilder]]

## Related
- [[Builder-Overview]]
