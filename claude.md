# Claude — Active Working Memory

> Active context for the **Rental Shop Management System** course project.
> This file is the live scratchpad. Read it first at the start of every session.

---

## 1. Mission
Build the **domain logic** of a Rental Shop Management System in C# (.NET) that demonstrates **10 GoF design patterns**. The implementation now follows a **production-ready Domain-Driven layout** — generic pattern names (`Context`, `ConcreteStateB`, `MethodA`) were retired in Phase 3 in favour of domain-specific identifiers (`PricingCalculator`, `RentedState`, `ProcessRental`). The pattern role for every type is preserved in its XML doc-comment.

## 2. Hard Rules (Phase 3 onwards)
1. **Domain-specific names** — every class, interface, and method has a meaningful identifier. The codebase reads like a real enterprise app.
2. **Pattern role lives in XML doc-comments** — every type/interface/method opens with `/// <summary>Represents the '<role>' in the <pattern> GoF pattern.</summary>`-style attribution.
3. **Domain-Driven folder layout** — `Domain/`, `Application/`, `Infrastructure/` (no `Patterns/` folder).
4. **In-memory mock repositories** (no EF, no SQL).
5. **SOLID** — small, decoupled classes; one type per file.
6. **Memory discipline** — every interface/abstract/concrete class gets a sibling `.md` in `/docs_obsidian/patterns/<slug>/` (organised by pattern for academic traceability) with wikilinks and tags.

## 3. Current Phase
**Phase 3 — Domain-Driven Refactor.** ✅ Complete. Build green (0 warnings, 0 errors). Demo flow preserved end-to-end against the new names.

Phase 1 deliverables (skeleton): ✅ done.
Phase 2 deliverables (concrete implementations): ✅ done.
Phase 3 deliverables (domain-driven refactor):
- [x] Dismantle `Patterns/` folder; reorganise into `Domain/`, `Application/`, `Infrastructure/`.
- [x] Rename every generic GoF identifier to a domain-specific one.
- [x] Add `/// <summary>Represents the '<role>' in the <pattern> GoF pattern.</summary>` to every renamed type/method.
- [x] Update every `using` and namespace.
- [x] Update Obsidian graph (54 type pages + 10 overviews + 1 index + 2 architecture).
- [x] Update `claude.md` and `project_structure.md`.
- [x] Build verified: `dotnet build` → 0 errors, 0 warnings.

## 4. Pattern → Domain-Driven Type Map (cheat-sheet)

| # | Pattern | Pattern roles | Domain-driven types |
|---|---|---|---|
| 1 | Factory Method | Product / Creator (+ concretes A, B) | `RentalItem` / `ItemFactory` (+ `ToolItem`, `GearItem`, `ToolFactory`, `GearFactory`) |
| 2 | Builder | Director / Builder / Product | `OrderDirector` / `OrderBuilder` / `RentalOrder` (+ `Standard`/`PremiumOrderBuilder`) |
| 3 | Composite | Component / Leaf / Composite | `PackageComponent` / `RentalLineItem` / `RentalPackage` |
| 4 | Proxy | Subject / RealSubject / Proxy | `ICatalogRepository` / `InMemoryCatalogRepository` / `CachingCatalogRepository` |
| 5 | Facade | Facade / SubSystem × 4 | `RentalShopFacade` / `InventoryService`, `BillingService`, `PaymentService`, `DocumentService` |
| 6 | State | State / Context / ConcreteState A,B,C | `IItemState` / `ItemLifecycle` / `AvailableState`, `RentedState`, `UnderRepairState` |
| 7 | Strategy | Strategy / Context / ConcreteStrategy A,B,C | `IPricingStrategy` / `PricingCalculator` / `Standard`/`Weekend`/`LoyaltyPricingStrategy` |
| 8 | Observer | Subject / ConcreteSubject / Observer / ConcreteObserver | `WaitlistPublisher` / `ItemWaitlist` / `IWaitlistSubscriber` / `CustomerSubscriber` |
| 9 | Template Method | AbstractClass / ConcreteClass A,B | `DocumentRenderer` / `ReceiptDocument`, `ContractDocument` |
| 10 | Command | Command / ConcreteCommand A,B / Receiver / Invoker | `CashierCommand` / `RentItemCommand`, `ReturnItemCommand` / `CashierTerminal` / `CashierConsole` |

## 5. Naming Collisions (resolved by the rename)
Earlier phases relied on namespace separation for three colliding canonical names. Phase 3 eliminated the collisions outright by giving each type a domain-specific name.

| Old canonical | Pattern A → new name | Pattern B → new name |
|---|---|---|
| `Product` | Factory Method → `RentalItem` | Builder → `RentalOrder` |
| `Subject` | Proxy → `ICatalogRepository` | Observer → `WaitlistPublisher` |
| `Context` | State → `ItemLifecycle` | Strategy → `PricingCalculator` |

## 6. Resolved Architectural Decisions
- **State pattern** — three states (Available / Rented / Under Repair) per the brief; the third state was an extension over the canonical A/B example. ✅
- **Command pattern** — split the single canonical `ConcreteCommand` into Rent / Return commands (`RentItemCommand`, `ReturnItemCommand`); added `Undo()` because the brief requires it. ✅

## 6a. Open Questions
*(none — Phase 3 complete and approved)*

## 6b. Notable Signature Extensions (vs. canonical examples)
Documented inside the relevant XML doc-comments. The Phase 3 rename does not change these — the signatures are still the same domain-driven shape, just under new names.
- **`ICatalogRepository.Find(sku)`** — extended from canonical `void Request()` (catalog lookup needs a key and a result).
- **`IPricingStrategy.Calculate(baseRate, days)` / `PricingCalculator.Calculate`** — extended from canonical `void AlgorithmInterface()` (pricing needs inputs and a price out).
- **`PackageComponent.GetPrice()`** — added alongside canonical `Display(int)` (price totalling is the brief's stated reason for using Composite).
- **`CashierCommand.Undo()`** — added because the brief explicitly requires undo.
- **Facade subsystem methods** — extended with parameters where each subsystem consumes real data from the prior step.

## 7. Recent Activity Log
- Phase 1: extracted canonical names from `.docx` examples; generated Phase-1 skeleton + Obsidian graph.
- Phase 2: implemented all 10 patterns; wired the Facade end-to-end; demo verified.
- Phase 3 (this refactor):
  - Dismantled `RentalShop/Patterns/` and reorganised into `Domain/Entities`, `Domain/States`, `Application/{Factories,Builders,Services,Commands,Facades}`, `Infrastructure/Repositories`.
  - Renamed every type/method to a domain-specific identifier; added GoF-role XML doc-comments to every renamed member.
  - Rewrote `Program.cs` against the new types.
  - Regenerated the Obsidian graph (54 type pages + 10 overviews + 1 index + 2 architecture pages); wikilinks now use the new names.
  - Updated `claude.md` and `project_structure.md` to reflect the new architecture.
  - Build verified (`dotnet build` → 0 warnings, 0 errors).
