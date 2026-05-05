---
name: ToolFactory
description: Factory for tool entities — Factory Method ConcreteCreator (variant A).
tags:
  - pattern/factory-method
  - role/concrete
  - layer/application
---

# ToolFactory

`class ToolFactory : ItemFactory` in `RentalShop.Application.Factories`.

## Pattern role
**ConcreteCreator (variant A)** in the Factory Method GoF pattern.

## Domain role
Produces [[ToolItem]] instances with auto-numbered SKUs (`TOOL-001`, `TOOL-002`, …) drawn from a small in-memory template pool.

## Inherits
- [[ItemFactory]]

## Produces
- [[ToolItem]]

## Related
- [[FactoryMethod-Overview]]
