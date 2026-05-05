---
name: Pattern Name Collisions (resolved)
description: Historical record of canonical-name collisions; resolved by the domain-driven rename.
tags:
  - architecture
  - naming
---

# Pattern Name Collisions (resolved)

## Status: resolved

The earlier phases of this project used **strict canonical GoF names** taken from `patterns_examples/`, which created three name collisions across patterns. Phase 3 refactored the codebase to a **domain-driven** naming scheme — every class & method now has a domain-specific name and the canonical role lives in the XML doc-comment instead. As a result, **all three collisions are gone** in the C# code.

## Historical collision table

| Canonical symbol | Patterns | Old namespace separation | New domain-specific names |
|---|---|---|---|
| `Product` | Factory Method, Builder | `RentalShop.Patterns.FactoryMethod.Product` / `RentalShop.Patterns.Builder.Product` | [[RentalItem]] / [[RentalOrder]] |
| `Subject` | Proxy, Observer | `RentalShop.Patterns.Proxy.Subject` / `RentalShop.Patterns.Observer.Subject` | [[ICatalogRepository]] / [[WaitlistPublisher]] |
| `Context` | State, Strategy | `RentalShop.Patterns.State.Context` / `RentalShop.Patterns.Strategy.Context` | [[ItemLifecycle]] / [[PricingCalculator]] |

## Why the rename
The brief for Phase 3 explicitly required a "production-ready Domain-Driven C# structure". Generic GoF names (`Context`, `Subject`, `Product`) carry no meaning for a reader of the rental-shop domain. The rename:
- removes ambiguity at the call site (no more `using StrategyContext = …`)
- makes each type self-documenting
- preserves the academic mapping via XML doc-comments and the per-pattern Obsidian pages

## Related
- [[overview]]
- [[_index]]
