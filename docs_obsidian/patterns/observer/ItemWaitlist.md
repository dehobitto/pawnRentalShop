---
name: ItemWaitlist
description: Waitlist for a specific SKU; setting Status auto-broadcasts — Observer 'ConcreteSubject'.
tags:
  - pattern/observer
  - role/concrete
  - layer/application
  - entity
---

# ItemWaitlist

`class ItemWaitlist : WaitlistPublisher` in `RentalShop.Application.Services`.

## Pattern role
**ConcreteSubject** in the Observer GoF pattern — stores the state observers care about and triggers notifications on change.

## Domain role
A specific waitlist-enabled rental item (e.g. a popular drill model). Setting `Status` auto-broadcasts via [[WaitlistPublisher]].`NotifyAll()`.

## Inherits
- [[WaitlistPublisher]]

## Read by
- [[CustomerSubscriber]] (during `OnItemAvailable`)

## Related
- [[Observer-Overview]]
