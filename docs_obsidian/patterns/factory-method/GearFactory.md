---
name: GearFactory
description: Factory for gear entities — Factory Method ConcreteCreator (variant B).
tags:
  - pattern/factory-method
  - role/concrete
  - layer/application
---

# GearFactory

`class GearFactory : ItemFactory` in `RentalShop.Application.Factories`.

## Pattern role
**ConcreteCreator (variant B)** in the Factory Method GoF pattern.

## Domain role
Produces [[GearItem]] instances with auto-numbered SKUs (`GEAR-001`, `GEAR-002`, …).

## Inherits
- [[ItemFactory]]

## Produces
- [[GearItem]]

## Related
- [[FactoryMethod-Overview]]
