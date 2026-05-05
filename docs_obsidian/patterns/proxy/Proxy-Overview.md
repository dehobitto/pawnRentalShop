---
name: Proxy — Overview
description: Pattern overview — caching wrapper around the mock catalog repository.
tags:
  - pattern/proxy
  - layer/infrastructure
  - role/overview
---

# Proxy — Overview

**Pattern role.** Provides a surrogate or placeholder for another object to control access to it.

**Domain role.** Caches catalog reads from the mock repository so repeat lookups for the same SKU don't hit the (simulated) DB twice within a session.

## Type roster
- [[ICatalogRepository]] — Subject (`RentalShop.Infrastructure.Repositories`)
- [[InMemoryCatalogRepository]] — RealSubject
- [[CachingCatalogRepository]] — Proxy

## Wiring
`Program.cs` seeds the [[InMemoryCatalogRepository]] from Factory Method output, wraps it in a [[CachingCatalogRepository]], and hands the cached interface to [[InventoryService]] / [[RentalShopFacade]].

## Related
- [[_index]] · [[overview]] · [[pattern-collisions]] (was the canonical "Subject")
