---
name: ReturnItemCommand
description: "Return" cashier action — Command ConcreteCommand B.
tags:
  - pattern/command
  - role/concrete
  - layer/application
---

# ReturnItemCommand

`class ReturnItemCommand : CashierCommand` in `RentalShop.Application.Commands`.

## Pattern role
**ConcreteCommand (variant B)** in the Command GoF pattern.

## Domain role
"Return" cashier action — encapsulates everything needed to take an item back so the operation can be queued or undone (e.g. if the item turns out to be damaged on inspection).

## Inherits
- [[CashierCommand]]

## Related
- [[Command-Overview]]
