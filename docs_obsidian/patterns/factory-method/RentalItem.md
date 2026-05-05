---
name: RentalItem
description: Abstract base for any rentable entity — Factory Method 'Product'.
tags:
  - pattern/factory-method
  - role/abstract
  - layer/domain
  - entity
---

# RentalItem

`abstract class RentalItem` in `RentalShop.Domain.Entities`.

## Pattern role
**Product** in the Factory Method GoF pattern.

## Domain role
Base abstraction for any rentable entity in the catalog (tool, gear, vehicle, …). Lets the rest of the domain treat catalog items uniformly.

## Subclassed by
- [[ToolItem]]
- [[GearItem]]

## Used by
- [[ICatalogRepository]] (lookup result type)
- [[InventoryService]], [[RentalShopFacade]]

## Related
- [[FactoryMethod-Overview]]
