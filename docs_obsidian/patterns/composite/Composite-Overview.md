---
name: Composite — Overview
description: Pattern overview — uniform pricing & rendering for items vs. packages.
tags:
  - pattern/composite
  - layer/domain
  - role/overview
---

# Composite — Overview

**Pattern role.** Composes objects into tree structures so clients can treat individual objects and compositions uniformly.

**Domain role.** Lets the rest of the domain treat a single rentable item and a multi-item rental package the same way when totalling prices or rendering an order tree on a receipt.

## Type roster
- [[PackageComponent]] — abstract Component (`RentalShop.Domain.Entities`)
  - [[RentalLineItem]] — Leaf (single item)
  - [[RentalPackage]] — Composite (bundle of items / sub-packages)

## Wiring
`Program.cs` builds a tree of [[RentalLineItem]] leaves under a [[RentalPackage]] root, then calls `Display(int)` and `GetPrice()` polymorphically through [[PackageComponent]].

## Related
- [[_index]] · [[overview]] · [[pattern-collisions]]
