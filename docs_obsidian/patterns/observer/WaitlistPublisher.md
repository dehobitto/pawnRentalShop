---
name: WaitlistPublisher
description: Owns the waitlist and broadcasts updates — Observer 'Subject'.
tags:
  - pattern/observer
  - role/abstract
  - layer/application
---

# WaitlistPublisher

`abstract class WaitlistPublisher` in `RentalShop.Application.Services`.

## Pattern role
**Subject** in the Observer GoF pattern — owns the list of observers and exposes attach/detach/notify operations.

## Domain role
Base class for items that maintain a customer waitlist. Provides `Subscribe` / `Unsubscribe` / `NotifyAll`.

## Subclassed by
- [[ItemWaitlist]]

## Notifies
- [[IWaitlistSubscriber]] (i.e. every [[CustomerSubscriber]] subscribed)

## Related
- [[Observer-Overview]] · [[pattern-collisions]] (this Subject ≠ [[ICatalogRepository]])
