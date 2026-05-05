---
name: Architecture Overview
description: High-level wiring between the 10 GoF patterns in the Rental Shop domain.
tags:
  - architecture
  - layer/domain
---

# Architecture Overview

The system follows a **Domain-Driven layout** — domain logic, application services, and infrastructure are split into separate folders/namespaces. Each GoF pattern still maps to a discrete set of types; the difference is that the types are now named for what they do (`PricingCalculator`, `RentedState`, `RentalShopFacade`) rather than after their pattern role (`Context`, `ConcreteStateB`, `Facade`).

## Layer map

| Layer | Folder | Namespace prefix | What lives here |
|---|---|---|---|
| Domain | `RentalShop/Domain/Entities` | `RentalShop.Domain.Entities` | Rental items, composite nodes, rental orders |
| Domain | `RentalShop/Domain/States` | `RentalShop.Domain.States` | Item lifecycle states |
| Application | `RentalShop/Application/Factories` | `RentalShop.Application.Factories` | Factory Method types |
| Application | `RentalShop/Application/Builders` | `RentalShop.Application.Builders` | Builder types |
| Application | `RentalShop/Application/Services` | `RentalShop.Application.Services` | Strategy, Observer, Template-Method types |
| Application | `RentalShop/Application/Commands` | `RentalShop.Application.Commands` | Command types |
| Application | `RentalShop/Application/Facades` | `RentalShop.Application.Facades` | Facade + its 4 subsystems |
| Infrastructure | `RentalShop/Infrastructure/Repositories` | `RentalShop.Infrastructure.Repositories` | Proxy + mock repository |

## Cross-pattern wiring (Phase 2)

```
[[RentalShopFacade]]
   ├── (uses) [[CachingCatalogRepository]] → [[InMemoryCatalogRepository]]   # Proxy   (via [[InventoryService]])
   ├── (uses) [[PricingCalculator]] → active [[IPricingStrategy]]            # Strategy (via [[BillingService]])
   ├── (uses) [[ContractDocument]] / [[ReceiptDocument]] : [[DocumentRenderer]] # Template Method (via [[DocumentService]])
   └── (uses) [[PaymentService]]                                             # mock payment subsystem

Standalone demos in Program.cs also exercise:
   [[ItemFactory]] (Factory Method) · [[OrderDirector]] (Builder) · [[PackageComponent]] (Composite)
   [[ItemLifecycle]] (State) · [[ItemWaitlist]] (Observer) · [[CashierConsole]] (Command)
```

## Pattern cross-reference

| Domain capability | Pattern | Entry node |
|---|---|---|
| "Make a new rental item" | Factory Method | [[ItemFactory]] |
| "Build an order step-by-step" | Builder | [[OrderDirector]] |
| "Total an item or a package" | Composite | [[PackageComponent]] |
| "Read the catalog (cached)" | Proxy | [[ICatalogRepository]] |
| "UI calls one thing" | Facade | [[RentalShopFacade]] |
| "What can this item do right now?" | State | [[ItemLifecycle]] |
| "How much should we charge?" | Strategy | [[PricingCalculator]] |
| "Who's waiting for this item?" | Observer | [[ItemWaitlist]] |
| "Render a receipt / contract" | Template Method | [[DocumentRenderer]] |
| "Run / queue / undo a cashier action" | Command | [[CashierConsole]] |

## Related
- [[pattern-collisions]] — historical name-collision strategy (resolved by the domain rename)
- [[_index]] — full index
