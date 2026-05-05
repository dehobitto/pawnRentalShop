---
name: PremiumOrderBuilder
description: Premium rental-order assembler — Builder ConcreteBuilder (variant 2).
tags:
  - pattern/builder
  - role/concrete
  - layer/application
---

# PremiumOrderBuilder

`class PremiumOrderBuilder : OrderBuilder` in `RentalShop.Application.Builders`.

## Pattern role
**ConcreteBuilder (variant 2)** in the Builder GoF pattern.

## Domain role
Assembles a premium rental order — extra gear, insurance, delivery, and a loyalty discount.

## Inherits
- [[OrderBuilder]]

## Produces
- [[RentalOrder]]

## Related
- [[Builder-Overview]]
