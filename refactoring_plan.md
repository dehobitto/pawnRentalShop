# RentalShop — Refactoring Plan
**Principles:** SOLID · DRY · KISS · Clean Code  
**Constraint:** All 10 GoF patterns are SACRED and must not be broken.

---

## Step 1 — Domain Entities & State Layer
*Files: `PackageComponent`, `RentalLineItem`, `RentalPackage`, `ItemLifecycle`, `ItemWaitlist`*

### Changes
| # | What | Why |
|---|---|---|
| 1.1 | Extract `protected static string Indent(int depth)` into `PackageComponent` base class | DRY: the `new string(' ', depth * 2)` expression is copy-pasted in both `RentalLineItem.GetDisplayLines` and `RentalPackage.GetDisplayLines`. Both subclasses call it; it belongs in the shared base. |
| 1.2 | Extract `private const int IndentWidth = 2` in `PackageComponent` | KISS/magic number: the literal `2` appears implicitly in the indent expression. One named constant. |
| 1.3 | Extract `private string StateName(IItemState state) => state.GetType().Name` helper into `ItemLifecycle` | DRY: `_state.GetType().Name` is evaluated twice inside the `CurrentState` setter (once for `{From}`, once for `{To}`). A single private helper eliminates the duplication without touching the log message structure. |
| 1.4 | Change `ItemWaitlist.Sku` from a public mutable field to `public string Sku { get; init; }` | SOLID/Clean Code: a public field on a domain object is an encapsulation hole. SKU should be set at construction and never reassigned. |

---

## Step 2 — Factory Hierarchy DRY
*Files: `ItemFactory`, `ToolFactory`, `GearFactory`*

### Changes
| # | What | Why |
|---|---|---|
| 2.1 | Introduce intermediate abstract class `TemplatedItemFactory : ItemFactory` | DRY: `ToolFactory` and `GearFactory` are byte-for-byte identical in structure — both have `private int _counter`, both use `Interlocked.Increment(ref _counter)`, both cycle `(n - 1) % Templates.Length`, both format `$"PREFIX-{n:D3}"`. This logic lives once in `TemplatedItemFactory`. |
| 2.2 | In `TemplatedItemFactory`: declare `protected abstract (string Name, decimal Price)[] Templates { get; }` and `protected abstract string SkuPrefix { get; }` | The only things that differ between the two factories are the template data and the prefix string. Subclasses supply those; the base class owns the algorithm. |
| 2.3 | Implement `Create()` (auto-template) in `TemplatedItemFactory` using the two abstracts above | Removes the duplicate counter+format logic from both concrete factories. |
| 2.4 | Implement `Create(string sku, string name, decimal price)` in `TemplatedItemFactory` by delegating to a new `protected abstract RentalItem BuildItem(string sku, string name, decimal price)` | The user-defined variant differs only in which concrete domain type to instantiate (`ToolItem` vs `GearItem`). Each subclass provides `BuildItem`; the base class owns the delegation signature. |
| 2.5 | Reduce `ToolFactory` and `GearFactory` to just the three abstract overrides (`Templates`, `SkuPrefix`, `BuildItem`) | Each factory shrinks from ~20 lines to ~10 lines of pure data + one-liner `BuildItem`. The Factory Method pattern is fully preserved: concrete creators still decide the concrete product. |

---

## Step 3 — Magic Values → Constants & Enums
*Files: `RentalDbContext`, `RentalShopFacade`, `CreateItemViewModel`, `RentFormViewModel`, `StandardOrderBuilder`, `PremiumOrderBuilder`, `WeekendPricingStrategy`, `LoyaltyPricingStrategy`*

### Changes
| # | What | Why |
|---|---|---|
| 3.1 | In `RentalDbContext.RehydrateState`, replace bare string literals `"RentedState"`, `"UnderRepairState"` with `nameof(RentedState)`, `nameof(UnderRepairState)` | Safety: a rename of any state class would silently break deserialization. `nameof()` is a compile-time constant valid as a switch arm pattern — the compiler will catch the mismatch. |
| 3.2 | Introduce `enum ItemCategory { Tool, Gear }` in `Domain/Entities` (or `Models`) | Clean Code / SOLID: `CreateCatalogItemAsync` currently uses `category == "Gear"` magic string comparison. An enum makes the contract explicit, prevents typos, and enables compiler exhaustiveness checking. |
| 3.3 | Update `CreateItemViewModel.Category` from `string` to `ItemCategory`; update `CreateCatalogItemAsync` signature accordingly | Closes the magic string path end-to-end: view → controller → facade → factory. |
| 3.4 | In `RentFormViewModel`, replace `Range(1, 365)` literals with named constants `MinRentalDays = 1`, `MaxRentalDays = 365` defined in a `RentalConstants` static class | Magic numbers: 365 appears as a `[Range]` value with no explanation. A named constant communicates intent and is easy to change if business rules evolve. |
| 3.5 | In `CreateItemViewModel`, replace `Range(0.01, 9999.99)` with `MinItemPrice = 0.01` / `MaxItemPrice = 9999.99` from the same `RentalConstants` class | Same magic-number issue. |
| 3.6 | In `StandardOrderBuilder` and `PremiumOrderBuilder`, extract hardcoded price strings (`"Deposit: €50"`, `"Insurance: €15"`, `"Delivery: €10"`) to `private const string` fields | KISS: these strings appear as literals inside method bodies with no explanation of where the values come from. Named constants clarify intent without redesigning the Builder pattern. |
| 3.7 | Rename `LoyaltyPricingStrategy.LoyaltyDiscount` (value `0.85m`) to `LoyaltyMultiplier` | Clean Code: `0.85` is a multiplier, not a discount rate. The current name says "discount" but the value is already the post-discount factor, which confuses readers. |

---

## Step 4 — Facade Constructor & Guard Clauses
*Files: `RentalShopFacade`, `Program.cs`, `InventoryService`, `PaymentService`*

### Changes
| # | What | Why |
|---|---|---|
| 4.1 | Introduce a `record DocumentRenderers(DocumentRenderer Contract, DocumentRenderer Receipt)` value object | SOLID / Clean Code: the facade constructor currently takes two consecutive `DocumentRenderer` parameters, which are easy to swap accidentally and have no labels at the call site. A named record collapses two parameters into one self-documenting object. |
| 4.2 | Update `RentalShopFacade` constructor to accept `DocumentRenderers` instead of two loose `DocumentRenderer` parameters; update `Program.cs` and test helper accordingly | Reduces constructor arity from 9 → 8 parameters; makes `_contractRenderer` / `_receiptRenderer` access read as `_renderers.Contract` / `_renderers.Receipt`. |
| 4.3 | Add `Task<bool> ExistsAsync(string sku, CancellationToken ct)` to `InventoryService` | Clean Code / KISS: `CreateCatalogItemAsync` currently calls `_inventory.ReserveAsync(sku, ct) is not null` to check existence, which is semantically wrong (ReserveAsync is a read-for-write operation). A dedicated `ExistsAsync` makes the intent explicit and avoids misleading use of an unrelated method. |
| 4.4 | In `RentalShopFacade.ProcessReturnAsync`, replace the literal `0m` in `CaptureAsync(0m, ct)` with `decimal.Zero` | Clean Code: `0m` is a magic number whose intent (no charge on return, balance already settled) is not obvious. `decimal.Zero` self-documents that the zero is intentional. |
| 4.5 | Add a guard clause in `PaymentService.CaptureAsync`: throw `ArgumentOutOfRangeException` if `amount < 0` | SOLID/correctness: the current implementation silently accepts negative amounts. A domain service that handles money must reject nonsensical inputs at its boundary. |

---

## Step 5 — Controllers & ViewModels Clean Code
*Files: `ItemController`, `RentalController`, `CatalogItemViewModel`*

### Changes
| # | What | Why |
|---|---|---|
| 5.1 | Extract `private static string ToDisplayStateName(IItemState state)` in `ItemController` (or as a static method on `CatalogItemViewModel`) to replace `state.GetType().Name.Replace("State", "")` | Clean Code: inline string manipulation inside a LINQ projection is hard to read and test. A named helper makes the transformation obvious. |
| 5.2 | Inline `BuildRentFormAsync()` and `BuildReturnFormAsync()` in `RentalController` — replace each with a direct `new RentFormViewModel { Items = await LoadItemSelectListAsync() }` | KISS: both private methods are single-expression wrappers with no logic. They add a navigation hop without adding clarity. The line they save is not worth the indirection. |
| 5.3 | In `RentalController.LoadItemSelectListAsync`, extract the display-string format `"{i.Sku} — {i.Name} (€{i.BasePricePerDay}/day)"` into a `private static string ItemSelectLabel(RentalItem i)` helper | Clean Code: a formatting expression inside a LINQ lambda is noise. A named helper is readable at a glance and easy to change. |
| 5.4 | Flatten the two identical POST action patterns (validate → load items on error → return view; or succeed → TempData → redirect) into a private `HandleFormResult` helper or at minimum align the two methods' guard-clause structure | DRY / readability: `Rent(POST)` and `Return(POST)` share identical error-path and success-path code. At a minimum, ensure both use the same guard-clause style (fail-fast return) rather than nested `if/else`. |

---

## Step 6 — Observer, Strategy & Command Layer Naming
*Files: `WaitlistPublisher`, `CustomerSubscriber`, `ItemWaitlist`, `CashierConsole`, `WeekendPricingStrategy`*

### Changes
| # | What | Why |
|---|---|---|
| 6.1 | Add a duplicate-subscriber guard to `WaitlistPublisher.Subscribe`: check `_subscribers.Contains(subscriber)` (inside the lock) before adding | SOLID / correctness: nothing currently prevents the same `IWaitlistSubscriber` from being added twice, which would fire `OnItemAvailable` twice per event for that observer. |
| 6.2 | In `WaitlistPublisher.NotifyAll`, wrap each `subscriber.OnItemAvailable()` in a try/catch that logs the exception and continues | Robustness / SRP: one failing observer must not abort the notification of all remaining observers. The publisher's responsibility is notification, not exception propagation. |
| 6.3 | Rename `CustomerSubscriber.OnItemAvailable` log message from `"Waitlist notification sent to {CustomerName}"` to `"Waitlist notification logged for {CustomerName} — item {Sku} is available"` | Clean Code: the current message says "sent" which implies an external dispatch that does not occur. The rename makes the mock nature of the implementation explicit. |
| 6.4 | In `WeekendPricingStrategy`, add an XML doc comment clarifying that the multiplier is applied uniformly (not day-of-week aware) | Clean Code / documentation: the class name implies weekend-specific behaviour. A single sentence in the doc-comment prevents confusion for readers who expect a `DateTime.DayOfWeek` check. |
| 6.5 | In `CashierConsole.UndoLastAsync`, rename the local variable `command` (popped from history) to `lastCommand` | Clean Code: inside a method whose purpose is undo, `command` is ambiguous — it could refer to the staged command or the history item. `lastCommand` removes the ambiguity in a three-line method. |
| 6.6 | Add `ArgumentNullException.ThrowIfNull` guards to `RentItemCommand`, `ReturnItemCommand`, and `CashierConsole` constructors for their required parameters | SOLID / fail-fast: these constructors accept objects that are always dereferenced. Null-checking at construction time surfaces DI misconfiguration immediately rather than at first use. |

---

## Execution Order Rationale
Steps proceed from the innermost layers outward (Domain → Factories → Cross-cutting values → Facade → Controllers → Peripheral services) so that each step builds on already-clean lower layers and can be verified by the test suite independently.

**Test suite gate:** after each step, `dotnet test` must still pass 21/21.
