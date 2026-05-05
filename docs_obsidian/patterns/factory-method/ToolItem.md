---
name: ToolItem
description: Tool rental entity (drill, saw, hammer-set) — Factory Method ConcreteProduct.
tags:
  - pattern/factory-method
  - role/concrete
  - layer/domain
  - entity
---

# ToolItem

`class ToolItem : RentalItem` in `RentalShop.Domain.Entities`.

## Pattern role
**ConcreteProduct (variant A)** in the Factory Method GoF pattern.

## Domain role
A tool rental entity — drill, saw, hammer-set, etc.

## Inherits
- [[RentalItem]]

## Created by
- [[ToolFactory]]

## Related
- [[FactoryMethod-Overview]]
