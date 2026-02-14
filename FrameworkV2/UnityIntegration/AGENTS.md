# UnityIntegration Notes

This document summarizes the current structure/intent/usage patterns within `FrameworkV2/UnityIntegration`.

## Core

- Core Realm/Entity/Ability are `MonoBehaviour`-based, while Ability is a pure C# object.
- UnityIntegration is responsible for service/editor/input/utility layers.
- Service features are provided as Abilities and attached to RootRealm.

## Services (Abilities)

- `UIAbility(IUIAbility)`: Realm-level Core/Panel/Modal and global System UI management.
- `InputAbility(IInputAbility)`: Routes world input after checking UI hit status.
- `SpawnAbility(ISpawnAbility)`: Prefab spawning and UI root attachment.
- `ObjectPoolAbility`: Simple object pool.

## UI

- UI roots are separated by Realm.
- `UIAbility` manages Panel/Modal stack ordering and active Realm state.

## Input

- `InputAbility` checks UI hit status and routes world input.

## GameRoot

- Creates the root Realm, attaches RootAbilities, and calls `Initialize/Ready`.
- `GameRoot.Update()` calls `RootRealm.Tick()` to update Abilities with Tick capability.

## Editor

- `FrameworkViewerWindow`: Displays structure text using `Inspection/FrameworkStructureText`.
