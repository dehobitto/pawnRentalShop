# Project Structure — Rental Shop Management System

> High-level architectural map. Updated implicitly with every significant change.

---

## 1. Solution Layout (Phase 3 — Domain-Driven)

```
КП/
├── claude.md                       # Active working memory (read first)
├── project_structure.md            # THIS FILE — architectural map
├── patterns_examples/              # Source of canonical naming (read-only)
├── docs_obsidian/                  # External memory graph (one .md per type)
│   ├── _index.md                   # Map-of-Content; entry node
│   ├── architecture/
│   │   ├── overview.md
│   │   └── pattern-collisions.md
│   └── patterns/                   # Organised by GoF pattern (academic view)
│       ├── factory-method/         # 7 pages: 1 overview + Item/Tool/Gear × {Item,Factory}
│       ├── builder/                # 6 pages
│       ├── composite/              # 4 pages
│       ├── proxy/                  # 4 pages
│       ├── facade/                 # 6 pages
│       ├── state/                  # 6 pages
│       ├── strategy/               # 6 pages
│       ├── observer/               # 5 pages
│       ├── template-method/        # 4 pages
│       └── command/                # 6 pages
└── RentalShop/                     # C# domain project (Domain-Driven layout)
    ├── RentalShop.csproj
    ├── Program.cs                  # Composition root
    ├── Domain/
    │   ├── Entities/               # RentalItem, ToolItem, GearItem, RentalOrder,
    │   │                           # PackageComponent, RentalLineItem, RentalPackage
    │   └── States/                 # IItemState, ItemLifecycle, AvailableState,
    │                               # RentedState, UnderRepairState
    ├── Application/
    │   ├── Factories/              # ItemFactory, ToolFactory, GearFactory  (Factory Method)
    │   ├── Builders/               # OrderBuilder, OrderDirector,
    │   │                           # StandardOrderBuilder, PremiumOrderBuilder  (Builder)
    │   ├── Services/               # IPricingStrategy + 3 strategies + PricingCalculator    (Strategy)
    │   │                           # WaitlistPublisher, ItemWaitlist,
    │   │                           # IWaitlistSubscriber, CustomerSubscriber                (Observer)
    │   │                           # DocumentRenderer, ReceiptDocument, ContractDocument    (Template Method)
    │   ├── Commands/               # CashierCommand, RentItemCommand, ReturnItemCommand,
    │   │                           # CashierTerminal, CashierConsole                        (Command)
    │   └── Facades/                # RentalShopFacade,
    │                               # InventoryService, BillingService, PaymentService,
    │                               # DocumentService                                        (Facade)
    └── Infrastructure/
        └── Repositories/           # ICatalogRepository,
                                    # InMemoryCatalogRepository, CachingCatalogRepository    (Proxy)
```

## 2. Layer Map

| Layer | Folder | Responsibility |
|---|---|---|
| **Composition root** | `Program.cs` | Wire everything together, run domain demo for each pattern |
| **Domain** | `Domain/Entities`, `Domain/States` | Rental entities, item lifecycle states |
| **Application** | `Application/Factories`, `Builders`, `Services`, `Commands`, `Facades` | Use-cases, services, orchestrators |
| **Infrastructure** | `Infrastructure/Repositories` | Data access (mock in-memory + caching proxy) |

## 3. Namespace Strategy

Domain-driven; collisions resolved by giving each type a meaningful, domain-specific name (`RentalItem` vs `RentalOrder`, `ICatalogRepository` vs `WaitlistPublisher`, `ItemLifecycle` vs `PricingCalculator`).

```
RentalShop.Domain.Entities
RentalShop.Domain.States
RentalShop.Application.Factories
RentalShop.Application.Builders
RentalShop.Application.Services
RentalShop.Application.Commands
RentalShop.Application.Facades
RentalShop.Infrastructure.Repositories
```

## 4. Cross-Pattern Wiring (Phase 2 — implemented; rewired in Phase 3 against new names)

```
RentalShopFacade
   ├── (uses) ICatalogRepository → CachingCatalogRepository → InMemoryCatalogRepository
   │       (via InventoryService — Proxy)
   ├── (uses) PricingCalculator → IPricingStrategy
   │       (via BillingService — Strategy)
   ├── (uses) DocumentRenderer (ContractDocument or ReceiptDocument)
   │       (via DocumentService — Template Method)
   └── (uses) PaymentService (mock)

Program.cs also exercises (standalone demos):
   ItemFactory (FactoryMethod), OrderDirector (Builder), PackageComponent (Composite),
   ItemLifecycle (State), ItemWaitlist (Observer), CashierConsole (Command).
```

The Facade's two public methods:
- **`ProcessRental(sku, days)`** — Inventory lookup → Billing pricing → Payment → Contract document.
- **`ProcessReturn(sku)`** — Inventory lookup (cache HIT on second call) → settle → Receipt document.

## 5. Type Inventory (Phase 3 — domain-driven)

| Pattern | Pattern role | Domain-specific type |
|---|---|---|
| Factory Method | Product / ConcreteProduct A,B | `RentalItem`, `ToolItem`, `GearItem` |
|  | Creator / ConcreteCreator A,B | `ItemFactory`, `ToolFactory`, `GearFactory` |
| Builder | Director / Builder / ConcreteBuilder 1,2 | `OrderDirector`, `OrderBuilder`, `StandardOrderBuilder`, `PremiumOrderBuilder` |
|  | Product | `RentalOrder` |
| Composite | Component / Leaf / Composite | `PackageComponent`, `RentalLineItem`, `RentalPackage` |
| Proxy | Subject / RealSubject / Proxy | `ICatalogRepository`, `InMemoryCatalogRepository`, `CachingCatalogRepository` |
| Facade | Facade / SubSystems × 4 | `RentalShopFacade`, `InventoryService`, `BillingService`, `PaymentService`, `DocumentService` |
| State | State / Context / ConcreteState A,B,C | `IItemState`, `ItemLifecycle`, `AvailableState`, `RentedState`, `UnderRepairState` |
| Strategy | Strategy / Context / ConcreteStrategy A,B,C | `IPricingStrategy`, `PricingCalculator`, `StandardPricingStrategy`, `WeekendPricingStrategy`, `LoyaltyPricingStrategy` |
| Observer | Subject / ConcreteSubject / Observer / ConcreteObserver | `WaitlistPublisher`, `ItemWaitlist`, `IWaitlistSubscriber`, `CustomerSubscriber` |
| Template Method | AbstractClass / ConcreteClass A,B | `DocumentRenderer`, `ReceiptDocument`, `ContractDocument` |
| Command | Command / ConcreteCommand A,B / Receiver / Invoker | `CashierCommand`, `RentItemCommand`, `ReturnItemCommand`, `CashierTerminal`, `CashierConsole` |

**Total declared types in Phase 3**: 41 (one Obsidian `.md` each, organised under `docs_obsidian/patterns/<slug>/` for academic traceability).

## 6. Conventions

- **File-per-class** for `.cs` (one `*.cs` file per type — production-style layout).
- **File-per-type** for Obsidian (`docs_obsidian/patterns/<pattern>/<TypeName>.md`).
- **Wikilinks** between Obsidian files use the new domain-driven type name only: `[[RentedState]]`, `[[PricingCalculator]]`.
- **Tags** in YAML frontmatter: `pattern/<pattern-slug>`, `layer/<domain|application|infrastructure>`, `role/<abstract|concrete|orchestrator|interface|overview>`.
- **Pattern traceability** is preserved in **two places**:
  1. The XML doc-comment on every C# type/method names its GoF role explicitly.
  2. Obsidian pages remain organised by GoF pattern under `docs_obsidian/patterns/`.
