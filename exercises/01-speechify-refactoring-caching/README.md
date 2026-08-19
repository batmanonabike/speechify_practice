# Exercise 01 — Speechify Refactoring & Caching (warm-up)

A short, guided drill on caching mechanics and DI wiring.

**This is a warm-up, not a rehearsal.** The interfaces, the decorator shape, the enum and
the DI constructors are already written for you — and choosing those is the part a real
refactoring test actually grades. Expect 15–25 minutes, not 50.

For the full-fidelity, 50-minute version with genuinely legacy code and no pre-designed
seams, use [exercise 03](../03-speechify-refactoring-caching_02/).

## Tech baseline
- .NET SDK pinned in `global.json`
- Classic Visual Studio solution file: `SpeechifyPractice.sln`

## Projects
- `src/SpeechifyPractice.Legacy` — the shipped implementation, and the behavioural oracle
- `src/SpeechifyPractice.Refactor` — where you work
- `tests/SpeechifyPractice.Refactor.Tests`

## Practice goal
Implement the three TODOs in `SpeechifyPractice.Refactor` so the tests pass:
1. `PaymentFeeCalculator.Calculate`
2. `CachedCurrencyRateClient.GetUsdRate`
3. `PaymentCheckoutService.Checkout`

58 tests. Two are green at the start; the rest are yours.

| Suite | Tests | Covers |
|---|---:|---|
| `PaymentFeeCalculatorTests` | 8 | Card, bank transfer cap, **wallet**, zero amount, undefined enum value |
| `CachedCurrencyRateClientTests` | 19 | TTL reuse and expiry, **the exact-TTL boundary**, per-currency isolation, code normalisation, failed lookups, constructor validation, concurrency |
| `PaymentCheckoutServiceTests` | 17 | Orchestration, validation, risk-band boundaries |
| `LegacyEquivalenceTests` | 14 | Legacy vs refactored, field for field, plus the caching win |

## Defined behaviour

Two things the original tests left ambiguous, now pinned:

- **TTL boundary** — an entry is valid while `age < ttl`, so at exactly the TTL it is stale.
- **Currency normalisation** — `"eur"`, `"EUR"`, `"Eur"` and `" eur "` are one cache entry.

## Constraints (simulate interview)
- Timebox: 50 minutes for the real thing; this one should take far less
- Focus on SOLID, KISS, DRY, YAGNI
- Do not change test expectations unless clearly wrong
- Keep refactors incremental and explainable
- Push your work before the window expires

## Real test logistics to remember
- You schedule the test ahead of time and connect your GitHub account
- Access is only available within 30 minutes of the scheduled start time
- Pre-test checks may start screen recording, webcam sharing, and screen sharing
- The real test repo is created privately and you must push your code before access is revoked

## Run locally
From Visual Studio: open `SpeechifyPractice.sln`, build, run all tests.

From the repository root:
```bash
dotnet test exercises/01-speechify-refactoring-caching/SpeechifyPractice.sln
```

This solution contains only this exercise's three projects, so the run is this
exercise's signal and nothing else.

## Suggested workflow
1. Make one test pass at a time.
2. Commit in small steps with clear messages.
3. After green tests, do one readability pass.
4. Stretch: `PaymentCheckoutService.ComputeRiskBand` is `public static`, so it cannot be
   substituted. Extract an `IRiskAssessor` — the equivalence tests will hold you honest.
