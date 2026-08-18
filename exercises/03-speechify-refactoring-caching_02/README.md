# Exercise 03 — Billing Refactor & Caching (advanced)

A 50-minute, test-driven refactoring kata modelled on the Speechify Refactoring (C#) test.

Unlike [exercise 01](../01-speechify-refactoring-caching/), nothing here is pre-designed for you.
You get working, ugly code and a brief. Every seam is your call.

---

## The brief

`Speechify.Billing.Legacy.BillingEngine` is in production. It charges customers and
produces receipts. It works, as far as anyone knows.

It is also slow, unsafe under load, and impossible to test without global state.

**Your job:** implement `Speechify.Billing.BillingComposition.Create(...)` so that it returns
an `IBillingService` which

1. behaves **identically** to the legacy engine, and
2. stops hammering the FX rate provider.

Do not edit anything under `src/Speechify.Billing.Legacy/`. It is your oracle.

---

## Getting started

```bash
dotnet test exercises/03-speechify-refactoring-caching_02/SpeechifyKata.slnx
```

Or open `SpeechifyKata.slnx` in Visual Studio.

The starting state is deliberate:

| Suite | Tests | State | Meaning |
|---|---:|---|---|
| `CharacterizationTests` | 69 | **green** | Your safety net. Must never go red. |
| `EquivalenceTests` | 60 | red | Legacy vs yours, field for field. |
| `CachingTests` | 14 | red | The performance half of the brief. |
| `EdgeCaseTests` | 22 | red | Validation and boundaries. |
| `DesignTests` | 3 | 2 green, 1 red | Guardrails. The green two fail if you take a shortcut. |

168 tests total, ~0.3s to run. Nothing from any other exercise is in this solution.

---

## What you are allowed to change

**Preserve** every behaviour the characterization tests pin — including the parts that
look wrong. There are several. Finding them is the point; "fixing" them silently is how
you fail a refactoring test.

**One sanctioned improvement:** input validation. The legacy engine throws
`NullReferenceException` from deep inside risk scoring when the customer id is null.
`EdgeCaseTests` requires `ArgumentException` instead. That is the only place you may diverge.

---

## Contract

`src/Speechify.Billing/Contract.cs` defines the outer boundary and nothing else:
`ChargeRequest`, `ChargeReceipt`, `IBillingService`, and the `BillingComposition.Create`
factory the tests build you through.

There is deliberately **no** `IClock`, no `IFeeCalculator`, no cache interface. Designing
those is the exercise. `TimeProvider` is the BCL abstraction — use it for every time read.

---

## Caching requirements

The legacy cache keys on **currency + amount**, so it only helps when the identical amount
is billed twice. In practice it never hits, it grows without bound, and it is a plain
`Dictionary` written from multiple threads.

Your cache must:

- key on the **normalised currency code** — `"eur"`, `" eur "` and `"EUR"` are one entry
- honour the TTL passed to `Create`
- **expire exactly at the TTL** — an entry is valid while `age < ttl`, so at precisely
  `ttl` it is already stale
- keep per-currency entries independent
- hold no static state, so two services never share a cache
- survive concurrent access

**Stretch:** collapse concurrent misses into a single upstream call (single-flight), rather
than letting 64 threads stampede the provider.

---

## Notes on the legacy code

Some things you will find, in no particular order, and not an exhaustive list:

- one very long method doing validation, fees, discounts, risk, FX and rounding
- a static rate dictionary that is never invalidated and is shared across instances
- `DateTime.Now` read in the middle of business logic, driving a weekend surcharge
- a second copy of the fee rules in `EstimateFee` that **does not agree** with what
  `ProcessCharge` actually charges — the customer is quoted one number and billed another
- two different rounding modes, one of which will change results if you "unify" them
- a swallowed exception that silently bills at a 1:1 exchange rate
- risk scoring nested five levels deep
- an eight-parameter positional method
- `ToLower()` where an ordinal comparison belongs

---

## Ground rules

- 50 minutes, timed.
- Commit in small steps with clear messages. Push before the clock runs out.
- Optimise for something a reviewer can read and you can explain out loud.
- Leave a short note in `DECISIONS.md` covering what you changed, what you deliberately
  left alone, and what you would do with another hour.

See [TIMEBOX.md](TIMEBOX.md) for a prioritised schedule and [RUBRIC.md](RUBRIC.md) to score
yourself afterwards.
