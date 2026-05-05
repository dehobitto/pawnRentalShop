---
name: Factory Method — Overview
description: Pattern overview — instantiates rental entities (tools / gear) via concrete factories.
tags:
  - pattern/factory-method
  - layer/application
  - role/overview
---

# Factory Method — Overview

**Pattern role.** Defines an interface for creating an object but lets subclasses decide which class to instantiate.

**Domain role.** Instantiates rental entities for the catalog (tools, gear, …) without leaking the instantiation decision to callers.

## Type roster
- [[ItemFactory]] — abstract Creator (`RentalShop.Application.Factories`)
  - [[ToolFactory]] — ConcreteCreator (variant A)
  - [[GearFactory]] — ConcreteCreator (variant B)
- [[RentalItem]] — abstract Product (`RentalShop.Domain.Entities`)
  - [[ToolItem]] — ConcreteProduct (variant A)
  - [[GearItem]] — ConcreteProduct (variant B)

## Wiring
`Program.cs` instantiates [[ToolFactory]] / [[GearFactory]] and seeds the [[InMemoryCatalogRepository]] with their output.

## Related
- [[_index]] · [[overview]] · [[pattern-collisions]]
