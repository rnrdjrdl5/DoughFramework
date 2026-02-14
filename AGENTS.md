# AGENTS

- Read `AI/Convention/Convention.MD` before making code changes.
- Follow the conventions described there.
- The function-structure examples in `AI/Convention/Convention.MD` are for reference when designing initialization flows.
- Orchestration principle: entry points define only high-level flow and delegate concrete logic to lower-level functions.
- Post-work test cases: provide a list of tests the user can run to verify changed docs/code.
- Ability work rule: after completing a requested Ability task, write an Ability summary `AGENTS.md` in that directory.
- Ability example rule: after completing a requested Ability task, write an `Example{AbilityName}` file with usage in that directory. (Prefix `Example` required; choose a name freely to avoid collisions.)
- Example file reference rule: if `Example{Name}.cs` exists in the target directory, read it for usage reference but do not copy the implementation as-is.
