---
name: OrderDirector
description: Drives the builder through the canonical construction sequence — Builder 'Director'.
tags:
  - pattern/builder
  - role/orchestrator
  - layer/application
---

# OrderDirector

`class OrderDirector` in `RentalShop.Application.Builders`.

## Pattern role
**Director** in the Builder GoF pattern.

## Domain role
Orchestrates the step-by-step assembly of any rental order — calls `AddItems()` first, then `AddExtras()` — letting the concrete builder decide what each step actually contributes.

## Drives
- [[OrderBuilder]] (and its concrete variants)

## Related
- [[Builder-Overview]]
