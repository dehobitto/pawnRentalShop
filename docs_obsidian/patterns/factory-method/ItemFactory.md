---
name: ItemFactory
description: Abstract factory for catalog items — Factory Method 'Creator'.
tags:
  - pattern/factory-method
  - role/abstract
  - layer/application
---

# ItemFactory

`abstract class ItemFactory` in `RentalShop.Application.Factories`.

## Pattern role
**Creator** in the Factory Method GoF pattern. Declares `Create()` (the factory method).

## Domain role
Base contract for catalog item factories. The shop onboards new rental categories by subclassing this factory rather than editing a central instantiation switch.

## Subclassed by
- [[ToolFactory]]
- [[GearFactory]]

## Returns
- [[RentalItem]]

## Related
- [[FactoryMethod-Overview]]
