# Stat Content Notes

## Summary
- `StatAbility`: handles stat value storage/query/update; no calculation formula.
- `IStatDefinitionProvider`: provides stat definitions (defaults/range) and ID validation.
- `StatDefinition`: stores default value and min/max range.
- `StatChangedPayload`: change event payload.

## Usage
- Implement `IStatDefinitionProvider` externally and inject via `StatAbility.Configure(...)`.
- Receive change events via `OnChangedStat`.

## Example
- See `ExampleStatAbility.cs`.
