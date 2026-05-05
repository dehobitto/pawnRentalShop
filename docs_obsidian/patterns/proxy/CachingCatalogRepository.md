---
name: CachingCatalogRepository
description: Caching surrogate around the in-memory catalog — Proxy 'Proxy'.
tags:
  - pattern/proxy
  - role/concrete
  - layer/infrastructure
---

# CachingCatalogRepository

`class CachingCatalogRepository : ICatalogRepository` in `RentalShop.Infrastructure.Repositories`.

## Pattern role
**Proxy** in the Proxy GoF pattern — a surrogate that controls access to the real subject (here, by caching).

## Domain role
Caching wrapper around [[InMemoryCatalogRepository]]. Avoids repeated catalog lookups for the same SKU within a session.

## Implements
- [[ICatalogRepository]]

## Wraps
- [[InMemoryCatalogRepository]]

## Related
- [[Proxy-Overview]]
