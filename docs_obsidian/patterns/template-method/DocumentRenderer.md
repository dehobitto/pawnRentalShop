---
name: DocumentRenderer
description: Skeleton of the rental-document algorithm — Template Method 'AbstractClass'.
tags:
  - pattern/template-method
  - role/abstract
  - layer/application
---

# DocumentRenderer

`abstract class DocumentRenderer` in `RentalShop.Application.Services`.

## Pattern role
**AbstractClass** in the Template Method GoF pattern.

## Domain role
Defines the fixed algorithm for producing a rental document (`Render` = `RenderHeader` → `RenderBody` → `RenderFooter`). Concrete subclasses fill in the variable steps.

## Subclassed by
- [[ReceiptDocument]]
- [[ContractDocument]]

## Used by
- [[DocumentService]] (Facade subsystem)

## Related
- [[TemplateMethod-Overview]]
