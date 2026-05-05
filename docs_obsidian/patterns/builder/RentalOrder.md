---
name: RentalOrder
description: The fully-assembled rental order — Builder 'Product'.
tags:
  - pattern/builder
  - role/concrete
  - layer/domain
  - entity
---

# RentalOrder

`class RentalOrder` in `RentalShop.Domain.Entities`.

## Pattern role
**Product** in the Builder GoF pattern — the step-by-step-assembled object.

## Domain role
A fully-assembled rental order (line items + extras) produced by an [[OrderBuilder]] under [[OrderDirector]] control.

## Built by
- [[StandardOrderBuilder]]
- [[PremiumOrderBuilder]]

## Related
- [[Builder-Overview]]
