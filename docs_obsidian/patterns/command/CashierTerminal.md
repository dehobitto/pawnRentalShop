---
name: CashierTerminal
description: Cashier terminal that commits the transaction — Command 'Receiver'.
tags:
  - pattern/command
  - role/concrete
  - layer/application
---

# CashierTerminal

`class CashierTerminal` in `RentalShop.Application.Commands`.

## Pattern role
**Receiver** in the Command GoF pattern — the object that actually performs the work.

## Domain role
Cashier terminal / domain service that commits the transaction. Holds the real domain logic; commands delegate to it via `Commit()`.

## Used by
- [[RentItemCommand]]
- [[ReturnItemCommand]]

## Related
- [[Command-Overview]]
