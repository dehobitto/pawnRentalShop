---
name: Template Method — Overview
description: Pattern overview — fixed receipt/contract algorithm with variable header & footer steps.
tags:
  - pattern/template-method
  - layer/application
  - role/overview
---

# Template Method — Overview

**Pattern role.** Defines the skeleton of an algorithm in a method, deferring some steps to subclasses. Subclasses redefine certain steps without changing the algorithm's structure.

**Domain role.** Locked-in document-generation flow. Every rental document goes header → body → footer; only the header & footer differ between a receipt and a contract.

## Type roster
- [[DocumentRenderer]] — AbstractClass (`RentalShop.Application.Services`)
  - [[ReceiptDocument]] — ConcreteClass A (short, customer-facing)
  - [[ContractDocument]] — ConcreteClass B (long, legally formatted)

## Wiring
[[DocumentService]] (Facade subsystem) holds a [[DocumentRenderer]] and switches between [[ReceiptDocument]] and [[ContractDocument]] depending on the use-case.

## Related
- [[_index]] · [[overview]]
