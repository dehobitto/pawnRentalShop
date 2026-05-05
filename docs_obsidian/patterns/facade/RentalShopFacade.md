---
name: RentalShopFacade
description: Single high-level API the UI calls — Facade 'Facade'.
tags:
  - pattern/facade
  - role/orchestrator
  - layer/application
---

# RentalShopFacade

`class RentalShopFacade` in `RentalShop.Application.Facades`.

## Pattern role
**Facade** in the Facade GoF pattern.

## Domain role
Single entry point the UI talks to. Provides high-level rental-shop use-cases (`ProcessRental`, `ProcessReturn`) and orchestrates the four subsystems behind the scenes.

## Owns
- [[InventoryService]]
- [[BillingService]]
- [[PaymentService]]
- [[DocumentService]]

## Depends on (injected)
- [[ICatalogRepository]] (Proxy)
- [[PricingCalculator]] (Strategy.Context)
- [[DocumentRenderer]] (Template Method abstract) — both contract & receipt variants

## Public API
- `ProcessRental(string sku, int days)`
- `ProcessReturn(string sku)`

## Related
- [[Facade-Overview]]
