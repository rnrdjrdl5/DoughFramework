# Ability Directory Summary

This directory provides the base structure for the Ability system.

## Key Points

- `Ability` is a pure C# object and is based on `ILifecycle`.
- `AbilityHost` manages Ability registration/query/lifecycle.
- You can auto-attach default Abilities to a Host type via `AbilityAttribute`.
- Ability lifecycle is controlled by the Host's `Initialize/Ready/Uninitialize`, not Unity events.
- Abilities that need per-frame calls implement `IAbilityTick`.

## Usage Rules

- When adding an Ability, use the Host's `AddAbility<T>()` or `AddAbility(Ability)`.
- Abilities receive the Host-owned GameObject.
- Upstream context lookup is performed via `UpstreamAbilityResolver`.
