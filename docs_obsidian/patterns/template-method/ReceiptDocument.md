---
name: ReceiptDocument
description: Short, customer-facing receipt — Template Method ConcreteClass A.
tags:
  - pattern/template-method
  - role/concrete
  - layer/application
---

# ReceiptDocument

`class ReceiptDocument : DocumentRenderer` in `RentalShop.Application.Services`.

## Pattern role
**ConcreteClass (variant A)** in the Template Method GoF pattern.

## Domain role
Receipt document — short, customer-facing, focused on totals and payment. Used by [[DocumentService]] for the "process return" flow.

## Inherits
- [[DocumentRenderer]]

## Related
- [[TemplateMethod-Overview]]
