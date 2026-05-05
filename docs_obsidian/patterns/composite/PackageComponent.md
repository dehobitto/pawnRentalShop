---
name: PackageComponent
description: Common abstraction for both single items and packages — Composite 'Component'.
tags:
  - pattern/composite
  - role/abstract
  - layer/domain
  - entity
---

# PackageComponent

`abstract class PackageComponent` in `RentalShop.Domain.Entities`.

## Pattern role
**Component** in the Composite GoF pattern.

## Domain role
Common abstraction for both leaves (single items) and composites (packages). Defines `Add` / `Remove` / `Display(int)` / `GetPrice()`.

## Subclassed by
- [[RentalLineItem]] (Leaf)
- [[RentalPackage]] (Composite)

## Related
- [[Composite-Overview]]
