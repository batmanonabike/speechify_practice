# Self-scoring

Score honestly straight after the timer stops, before you fix anything.

## Did it work

| | Points | |
|---|---:|---|
| `CharacterizationTests` still green | 10 | Non-negotiable. Zero for the whole section if red. |
| `EquivalenceTests` green | 10 | |
| `CachingTests` green | 10 | |
| `EdgeCaseTests` green | 5 | |
| `DesignTests` green | 5 | |

## Clean code (the stated grading criteria)

| | Points | |
|---|---:|---|
| **SOLID** — fee, risk, caching and orchestration are separate, substitutable pieces | 8 | One class doing all four scores 0 |
| **DRY** — the fee rules exist once, with the divergence handled explicitly | 6 | |
| **KISS** — a reviewer follows it without you narrating | 6 | |
| **YAGNI** — no speculative interfaces, no config system, no DI container | 5 | Over-engineering costs marks |
| Named constants instead of magic numbers and strings | 3 | |
| Ordinal string comparison, no `ToLower()` | 2 | |

## Engineering discipline

| | Points | |
|---|---:|---|
| Small commits with messages that say why | 5 | One giant commit scores 0 |
| **Pushed before the deadline** | 10 | Unpushed is incomplete, whatever the code looks like |
| `DECISIONS.md` present and honest | 5 | Naming what you left undone counts in your favour |
| No dead code, no commented-out blocks, no stray `Console.WriteLine` | 5 | |

**Total: 95**

---

## Questions to answer out loud

Say these to an empty room. The real test is screen-recorded and the follow-up
conversation is where the marks actually are.

1. Why did you keep the two rounding modes?
2. What is the cache key, and why not currency + amount?
3. What happens when two threads miss on the same currency at the same moment?
4. Why `TimeProvider` and not a static clock?
5. The customer is quoted one fee and charged another. What would you do about it,
   and why did you not do it during the test?
6. What did you deliberately not build, and why?

## Interpreting your score

- **80+** — you would pass and interview well off the back of it.
- **60–79** — solid. Look at which section leaked; it is usually caching or commits.
- **below 60** — re-run the kata cold in a few days. Check whether you spent too long
  making it beautiful before it was correct.
