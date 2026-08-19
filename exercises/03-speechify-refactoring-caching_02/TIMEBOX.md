# 50-minute plan

There is deliberately more work here than fits. Triage is part of the skill.
Work top to bottom and stop when the clock stops.

---

## 0–5 · Read and run

- `dotnet test SpeechifyKata.slnx`. Confirm characterization is green, everything else red.
- Skim `BillingEngine.ProcessCharge` once, top to bottom. Do not start editing.
- Note the two rounding modes and the `EstimateFee` duplicate. Those are the traps.

**Commit:** nothing yet.

## 5–12 · Get a working seam

- Implement `BillingComposition.Create` returning a service that does the job *badly* —
  a direct transcription of the legacy method into your own class is fine.
- Goal is `EquivalenceTests` green, not beauty. You now have a safety net for everything after.

**Commit:** "Port billing logic behind IBillingService".

## 12–22 · Extract the obvious seams

- Fee calculation out into its own type. Keep the two rounding modes distinct — they are
  not the same rule and the tests know it.
- Risk scoring out into its own type. Replace the nested `if`/`else` with guard clauses or
  a switch expression.
- Named constants for `0.029`, `0.30`, `5`, `500`, `100`, `"new_"`.
- `StringComparison.OrdinalIgnoreCase` in place of `ToLower()`.

**Commit** after each extraction. Run the tests each time.

## 22–35 · The cache

This is the half of the brief people run out of time for. Do not leave it until last.

- Cache by **normalised currency code**, not by currency + amount.
- TTL from the `rateTtl` parameter, time from the injected `TimeProvider`.
- Expiry boundary: valid while `age < ttl`.
- Instance state, not static.

**Commit:** "Add TTL rate cache".

## 35–43 · Concurrency and edges

- Make the cache safe under concurrent access.
- `EdgeCaseTests`: validation, null request, blank inputs, batch ordering.
- **Stretch:** single-flight so 64 concurrent misses cost one upstream call.

**Commit:** "Thread-safe cache + input validation".

## 43–50 · Land it

- Full test run.
- `DECISIONS.md`: what you changed, what you deliberately left alone and why, what is next.
- **Push.** Access is revoked at 50:00 and unpushed work counts as incomplete.

---

## If you are behind

Ship in this order. A green, well-explained partial beats a broken whole.

1. `EquivalenceTests` green — behaviour preserved
2. Basic TTL cache — the brief's second half
3. Fee and risk extracted — the SOLID story
4. `EdgeCaseTests` — polish
5. Single-flight and `DesignTests` — stretch

## Traps, stated plainly

- **Two rounding modes are not a bug to fix.** `ProcessCharge` rounds to even, `EstimateFee`
  rounds away from zero. Switch `ProcessCharge` over to away-from-zero to "tidy up" and
  **9 of the 44 charge rows go red** — 8 through the fee and one (1 USD wallet, weekend)
  through the surcharge alone.
- **`EstimateFee` quotes a different number from what `ProcessCharge` takes on 7 of its
  14 rows.** The customer is quoted one fee and billed another. Real defect, shipped
  behaviour, pinned by the tests. Decide deliberately; do not "fix" it by accident.
- **The bank transfer cap is applied before rounding in one path and after in the other.**
- **The swallowed exception is pinned behaviour.** A failed rate lookup bills at 1:1.
- **Do not reference the legacy assembly** from `Speechify.Billing`. `DesignTests` checks.
