---
name: CashierConsole
description: Stages, runs, and undoes cashier commands — Command 'Invoker'.
tags:
  - pattern/command
  - role/orchestrator
  - layer/application
---

# CashierConsole

`class CashierConsole` in `RentalShop.Application.Commands`.

## Pattern role
**Invoker** in the Command GoF pattern — holds a reference to a command and triggers it.

## Domain role
Cashier console that stages and runs cashier commands while remembering them for undo. Decouples the UI button-press from the actual operation, which is what makes undo / redo / replay possible.

## Holds
- staged [[CashierCommand]]
- LIFO history of executed [[CashierCommand]]s (for `UndoLast`)

## Related
- [[Command-Overview]]
