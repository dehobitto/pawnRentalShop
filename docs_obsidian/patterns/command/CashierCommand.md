---
name: CashierCommand
description: Encapsulated cashier action with execute & undo hooks — Command 'Command'.
tags:
  - pattern/command
  - role/abstract
  - layer/application
---

# CashierCommand

`abstract class CashierCommand` in `RentalShop.Application.Commands`.

## Pattern role
**Command** in the Command GoF pattern — declares the execute interface (and an undo hook).

## Domain role
Encapsulates a single cashier action (rent, return) as an object so it can be queued, logged, undone, or replayed. Holds a reference to the [[CashierTerminal]] that does the actual work.

## Subclassed by
- [[RentItemCommand]]
- [[ReturnItemCommand]]

## Targets
- [[CashierTerminal]] (Receiver)

## Run by
- [[CashierConsole]] (Invoker)

## Related
- [[Command-Overview]]
