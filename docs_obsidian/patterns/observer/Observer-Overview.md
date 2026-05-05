---
name: Observer — Overview
description: Pattern overview — waitlist notifications when a rental item becomes available.
tags:
  - pattern/observer
  - layer/application
  - role/overview
---

# Observer — Overview

**Pattern role.** Defines a one-to-many dependency so that when one object changes state, all dependents are notified and updated automatically.

**Domain role.** Waitlist — when a returned item flips to "Available", every customer on the waitlist is notified.

## Type roster
- [[WaitlistPublisher]] — abstract Subject (`RentalShop.Application.Services`)
  - [[ItemWaitlist]] — ConcreteSubject (one per waitlisted SKU)
- [[IWaitlistSubscriber]] — Observer interface
  - [[CustomerSubscriber]] — ConcreteObserver

## Wiring
`Program.cs` builds an [[ItemWaitlist]], `Subscribe`s a few [[CustomerSubscriber]]s, then sets `Status` — the setter auto-broadcasts.

## Related
- [[_index]] · [[overview]] · [[pattern-collisions]] (this Subject ≠ [[ICatalogRepository]])
