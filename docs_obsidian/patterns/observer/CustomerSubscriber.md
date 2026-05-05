---
name: CustomerSubscriber
description: A specific customer waiting for a specific item — Observer 'ConcreteObserver'.
tags:
  - pattern/observer
  - role/concrete
  - layer/application
  - entity
---

# CustomerSubscriber

`class CustomerSubscriber : IWaitlistSubscriber` in `RentalShop.Application.Services`.

## Pattern role
**ConcreteObserver** in the Observer GoF pattern.

## Domain role
A specific customer waiting for a specific item. On notification it reads the current [[ItemWaitlist]].`Status` and dispatches the update (here, just a console line).

## Implements
- [[IWaitlistSubscriber]]

## Reads
- [[ItemWaitlist]]

## Related
- [[Observer-Overview]]
