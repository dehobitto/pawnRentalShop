---
name: IWaitlistSubscriber
description: Contract for any subscriber to a waitlist event — Observer 'Observer'.
tags:
  - pattern/observer
  - role/interface
  - layer/application
---

# IWaitlistSubscriber

`interface IWaitlistSubscriber` in `RentalShop.Application.Services`.

## Pattern role
**Observer** in the Observer GoF pattern.

## Domain role
Contract for "interested customers" subscribed to a waitlisted item. Implementations turn `OnItemAvailable()` into a real-world side-effect (email, SMS, …).

## Implemented by
- [[CustomerSubscriber]]

## Notified by
- [[WaitlistPublisher]] (via `NotifyAll`)

## Related
- [[Observer-Overview]]
