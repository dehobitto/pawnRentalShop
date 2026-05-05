---
name: RentalPackage
description: Bundle of items (and sub-bundles) — Composite 'Composite'.
tags:
  - pattern/composite
  - role/concrete
  - layer/domain
  - entity
---

# RentalPackage

`class RentalPackage : PackageComponent` in `RentalShop.Domain.Entities`.

## Pattern role
**Composite** in the Composite GoF pattern — a node that holds children and aggregates their behaviour.

## Domain role
A rental package that bundles several items (or other packages). Its total price is the sum of its children's prices.

## Inherits
- [[PackageComponent]]

## Children may be
- [[RentalLineItem]]
- [[RentalPackage]]

## Related
- [[Composite-Overview]]
