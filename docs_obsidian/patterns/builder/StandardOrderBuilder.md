---
name: StandardOrderBuilder
description: Standard rental-order assembler — Builder ConcreteBuilder (variant 1).
tags:
  - pattern/builder
  - role/concrete
  - layer/application
---

# StandardOrderBuilder

`class StandardOrderBuilder : OrderBuilder` in `RentalShop.Application.Builders`.

## Pattern role
**ConcreteBuilder (variant 1)** in the Builder GoF pattern.

## Domain role
Assembles a standard rental order — basic line items plus the basic deposit.

## Inherits
- [[OrderBuilder]]

## Produces
- [[RentalOrder]]

## Related
- [[Builder-Overview]]
