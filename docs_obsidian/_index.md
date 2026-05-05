---
name: Rental Shop — Knowledge Graph Index
description: Map-of-Content for the Rental Shop Management System. Entry node for all pattern docs.
tags:
  - moc
  - layer/domain
  - project/rental-shop
---

# Rental Shop — Knowledge Graph Index

External memory for the **Rental Shop Management System**. Every interface, abstract class, and concrete class declared in `RentalShop/` has a sibling page here. Use it as a navigable graph — click wikilinks instead of grepping the codebase.

The C# project is organised by **layer** (Domain / Application / Infrastructure) with domain-specific class names. This Obsidian graph is still organised by **GoF pattern** so the academic mapping stays one click away — every type page calls out its pattern role.

## Architecture
- [[overview]] — high-level architecture overview
- [[pattern-collisions]] — historical name-collision strategy (now resolved by domain-specific names)

## Patterns

### Creational
- [[FactoryMethod-Overview]] → instantiates rental entities → [[ItemFactory]]
- [[Builder-Overview]] → assembles complex rental orders → [[OrderDirector]]

### Structural
- [[Composite-Overview]] → uniform pricing for items vs. packages → [[PackageComponent]]
- [[Proxy-Overview]] → caches catalog reads → [[ICatalogRepository]]
- [[Facade-Overview]] → single API for the UI → [[RentalShopFacade]]

### Behavioural
- [[State-Overview]] → item lifecycle (Available / Rented / Under Repair) → [[ItemLifecycle]]
- [[Strategy-Overview]] → swappable pricing rules → [[PricingCalculator]]
- [[Observer-Overview]] → waitlist notifications → [[ItemWaitlist]]
- [[TemplateMethod-Overview]] → rental document generation → [[DocumentRenderer]]
- [[Command-Overview]] → cashier actions, queueable & undoable → [[CashierConsole]]

## Tag conventions
- `pattern/<slug>` — which GoF pattern the page belongs to
- `role/abstract` `role/concrete` `role/orchestrator` `role/interface` `role/overview` — type's role inside the pattern
- `layer/domain` `layer/application` `layer/infrastructure` — C# project layer the page lives in
- `entity` — applied to types that represent rental-shop business entities
