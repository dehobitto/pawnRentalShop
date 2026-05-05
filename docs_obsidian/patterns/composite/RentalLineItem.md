---
name: RentalLineItem
description: Single rentable item — Composite 'Leaf'.
tags:
  - pattern/composite
  - role/concrete
  - layer/domain
  - entity
---

# RentalLineItem

`class RentalLineItem : PackageComponent` in `RentalShop.Domain.Entities`.

## Pattern role
**Leaf** in the Composite GoF pattern — a terminal node with no children.

## Domain role
A single rentable item priced on its own (one drill, one tent). Cannot contain other items.

## Inherits
- [[PackageComponent]]

## Related
- [[Composite-Overview]]
