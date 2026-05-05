---
name: ICatalogRepository
description: Catalog-lookup contract shared by the real repository and its caching proxy — Proxy 'Subject'.
tags:
  - pattern/proxy
  - role/interface
  - layer/infrastructure
---

# ICatalogRepository

`interface ICatalogRepository` in `RentalShop.Infrastructure.Repositories`.

## Pattern role
**Subject** in the Proxy GoF pattern — common interface shared by the real subject and its proxy.

## Domain role
Catalog-lookup contract. Both the in-memory repository and the caching wrapper implement it, so callers (notably [[RentalShopFacade]]) never know whether a hit came from cache or store.

## Implemented by
- [[InMemoryCatalogRepository]]
- [[CachingCatalogRepository]]

## Returns
- [[RentalItem]]

## Related
- [[Proxy-Overview]]
