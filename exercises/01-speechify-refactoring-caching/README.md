# Speechify-Style Refactoring Practice (C#)

This repo is a timed-practice simulation based on your test brief:
- Refactor legacy C# code
- Add caching for performance
- Keep behavior stable

This is exercise 01 inside a larger practice repository.

## Tech baseline
- .NET SDK pinned in `global.json`
- Uses latest installed SDK and C# language version
- Classic Visual Studio solution file: `SpeechifyPractice.sln`

## Projects
- `src/SpeechifyPractice.Legacy`
- `src/SpeechifyPractice.Refactor`
- `tests/SpeechifyPractice.Refactor.Tests`

## Practice goal
Implement the TODOs in `SpeechifyPractice.Refactor` so tests pass:
1. `PaymentFeeCalculator.Calculate`
2. `CachedCurrencyRateClient.GetUsdRate`
3. `PaymentCheckoutService.Checkout`

## Constraints (simulate interview)
- Timebox: 50 minutes
- Focus on SOLID, KISS, DRY, YAGNI
- Do not change test expectations unless clearly wrong
- Keep refactors incremental and explainable

## Run locally
From Visual Studio:
1. Open `SpeechifyPractice.sln`
2. Build solution
3. Run all tests in Test Explorer

From CLI:
```bash
dotnet test SpeechifyPractice.sln
```

From the repository root:
```bash
dotnet test exercises/01-speechify-refactoring-caching/SpeechifyPractice.sln
```

## Suggested workflow
1. Make one test pass at a time.
2. Commit in small steps with clear messages.
3. After green tests, do one readability pass.
4. If time remains, add one extra edge-case test.
