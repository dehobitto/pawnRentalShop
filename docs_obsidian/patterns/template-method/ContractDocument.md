---
name: ContractDocument
description: Long, legally-formatted rental contract — Template Method ConcreteClass B.
tags:
  - pattern/template-method
  - role/concrete
  - layer/application
---

# ContractDocument

`class ContractDocument : DocumentRenderer` in `RentalShop.Application.Services`.

## Pattern role
**ConcreteClass (variant B)** in the Template Method GoF pattern.

## Domain role
Rental-contract document — long, legally-formatted, with full T&C and signature lines. Used by [[DocumentService]] for the "process rental" flow.

## Inherits
- [[DocumentRenderer]]

## Related
- [[TemplateMethod-Overview]]
