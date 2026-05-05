---
name: RentItemCommand
description: "Rent" cashier action — Command ConcreteCommand A.
tags:
  - pattern/command
  - role/concrete
  - layer/application
---

# RentItemCommand

`class RentItemCommand : CashierCommand` in `RentalShop.Application.Commands`.

## Pattern role
**ConcreteCommand (variant A)** in the Command GoF pattern.

## Domain role
"Rent" cashier action — encapsulates everything needed to hand an item to a customer so the operation can be queued or undone.

## Inherits
- [[CashierCommand]]

## Related
- [[Command-Overview]]
