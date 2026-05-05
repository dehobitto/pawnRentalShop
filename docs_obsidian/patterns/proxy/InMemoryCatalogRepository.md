---
name: InMemoryCatalogRepository
description: Mock in-memory rental catalog — Proxy 'RealSubject'.
tags:
  - pattern/proxy
  - role/concrete
  - layer/infrastructure
---

# InMemoryCatalogRepository

`class InMemoryCatalogRepository : ICatalogRepository` in `RentalShop.Infrastructure.Repositories`.

## Pattern role
**RealSubject** in the Proxy GoF pattern — the actual resource the proxy stands in for.

## Domain role
Mock in-memory rental catalog repository. Stands in for what would be a database in production. Treated as the canonical source of truth for catalog data.

## Implements
- [[ICatalogRepository]]

## Wrapped by
- [[CachingCatalogRepository]]

## Related
- [[Proxy-Overview]]
