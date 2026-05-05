---
name: InventoryService
description: Catalog-reservation subsystem behind the facade — Facade 'SubSystem' #1.
tags:
  - pattern/facade
  - role/concrete
  - layer/application
---

# InventoryService

`class InventoryService` in `RentalShop.Application.Facades`.

## Pattern role
**SubSystem (subsystem #1)** in the Facade GoF pattern.

## Domain role
Inventory subsystem — checks item availability via the caching [[ICatalogRepository]] and "reserves" stock. Hidden from the UI by the [[RentalShopFacade]].

## Depends on
- [[ICatalogRepository]] (cache-first lookup)

## Returns
- [[RentalItem]]

## Related
- [[Facade-Overview]]
