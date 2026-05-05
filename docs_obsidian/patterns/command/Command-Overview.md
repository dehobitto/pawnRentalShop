---
name: Command — Overview
description: Pattern overview — cashier actions, queueable & undoable.
tags:
  - pattern/command
  - layer/application
  - role/overview
---

# Command — Overview

**Pattern role.** Encapsulates a request as an object, thereby letting you parameterise clients with different requests, queue or log requests, and support undoable operations.

**Domain role.** Cashier actions (Rent / Return) become first-class objects so they can be queued, audited, and undone.

## Type roster
- [[CashierCommand]] — abstract Command (`RentalShop.Application.Commands`)
  - [[RentItemCommand]] — ConcreteCommand A (Rent)
  - [[ReturnItemCommand]] — ConcreteCommand B (Return)
- [[CashierTerminal]] — Receiver (commits the action)
- [[CashierConsole]] — Invoker (stages, runs, undoes)

## Wiring
`Program.cs` constructs a [[CashierTerminal]] and a [[CashierConsole]], stages [[RentItemCommand]] / [[ReturnItemCommand]] objects, then calls `UndoLast()` to demonstrate the undo stack.

## Notable extension
- `Undo()` — added because the brief explicitly requires undo support; the canonical example does not include it.

## Related
- [[_index]] · [[overview]]
