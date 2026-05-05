---
name: Facade — Overview
description: Pattern overview — single API the UI calls (cross-pattern hub).
tags:
  - pattern/facade
  - layer/application
  - role/overview
---

# Facade — Overview

**Pattern role.** Provides a unified, higher-level interface to a set of interfaces in a subsystem, making the subsystem easier to use.

**Domain role.** Single entry point the UI talks to. Exposes high-level use-cases ("process a rental", "process a return") and orchestrates inventory, billing, payment, and document subsystems behind the scenes.

## Type roster
- [[RentalShopFacade]] — Facade (`RentalShop.Application.Facades`)
- [[InventoryService]] — SubSystem #1 (catalog reservation)
- [[BillingService]] — SubSystem #2 (pricing)
- [[PaymentService]] — SubSystem #3 (payment capture)
- [[DocumentService]] — SubSystem #4 (receipt / contract rendering)

## Cross-pattern wiring
- [[InventoryService]] → [[CachingCatalogRepository]] → [[InMemoryCatalogRepository]] (Proxy)
- [[BillingService]] → [[PricingCalculator]] → [[IPricingStrategy]] (Strategy)
- [[DocumentService]] → [[DocumentRenderer]] → [[ReceiptDocument]] / [[ContractDocument]] (Template Method)

## Public API
- `ProcessRental(sku, days)` — reserve, price, charge, contract.
- `ProcessReturn(sku)` — reserve (lookup), settle, receipt.

## Related
- [[_index]] · [[overview]]
