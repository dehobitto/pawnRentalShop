---
name: DocumentService
description: Document-rendering subsystem behind the facade — Facade 'SubSystem' #4.
tags:
  - pattern/facade
  - role/concrete
  - layer/application
---

# DocumentService

`class DocumentService` in `RentalShop.Application.Facades`.

## Pattern role
**SubSystem (subsystem #4)** in the Facade GoF pattern.

## Domain role
Receipt / contract subsystem — drives the [[DocumentRenderer]] Template-Method document generator. The active renderer is swapped at runtime depending on whether the use-case is a rental (contract) or a return (receipt).

## Depends on
- [[DocumentRenderer]] (Template Method abstract)

## Renderers used
- [[ContractDocument]] (rental)
- [[ReceiptDocument]] (return)

## Related
- [[Facade-Overview]]
