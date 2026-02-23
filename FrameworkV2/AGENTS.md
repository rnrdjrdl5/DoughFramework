# FrameworkV2 Overview and Working Notes

This document summarizes the current structure/intent/usage patterns of the `Assets/DoughFramework/FrameworkV2` tree and provides practical guidelines for working in this scope. The scope applies to this folder and all subfolders.

## Core Concepts Summary

- Core is structured to allow Unity references.
- Realm/Entity/Ability are `MonoBehaviour`-based, while Ability itself is a pure C# object.
- Lifecycle management `ILifecycle`
  - `Initialize()`, `Ready()`, `Uninitialize()` + state properties (`IsInitialized`, `IsReady`).
  - Lifecycle calls are manual, not automatic.
- Director/Element
  - `Director` is a MonoBehaviour coordinator that assembles `Element` units and wires Abilities.
  - `Element` is a lightweight feature unit attached to a Director, with optional lifecycle hooks.
- The Transform hierarchy is the basis of the Realm/Entity tree.
- Entity plays the GameObject role; the `WorldEntity` concept was removed.
- All existing Service features were converted to Ability and are attached to RootRealm.

## Directory Structure Summary

- Core/Common
  - `ILifecycle`, `Identity`, `IIdentifiable`, `AliasSet`, `SubscriptionSet`, `SubscriptionExtensions`.
- Core/Event
  - `HashKey`: string → int key (FNV-1a).
- Core/Ability
  - `Ability`: pure C# + `ILifecycle` base.
  - `AbilityHost`: host base that manages Ability list/lifecycle/resolver.
  - `IAbilityResolver`: `HasAbility<T>()`/`GetAbility<T>()` (local Ability lookup only).
  - `IAbilityTick`: interface for Abilities that need per-frame Tick.
  - `AbilityAttribute`, `AbilityAttributeCache`, `AbilityAttributeInstaller`: auto-attach Abilities based on class attributes.
  - `Common/BuildRealmAbility`, `Common/SpawnEntityAbility`, `Common/EventAbility`.
- Time (Clock)
  - `ClockAbility`: stores/queries Realm-level time snapshots.
- Core/Realm
  - `Realm`: holds Children/Abilities/Aliases. Defaults to auto-attaching `BuildRealmAbility`, `SpawnEntityAbility`, `ClockAbility` via attribute.
- Core/RealmBuilder
  - `RealmBuilder.Build(parent)`: creates/configures Realm only (no attaching).
  - `CommonBuilder`: sample builder.
- Core/Entity
  - `Entity`: holds Aliases/Abilities; the Entity itself plays the GameObject role.
- Core/Director
  - `Director`: resolves a Host `Entity`, builds `Element` list, and wires Abilities.
- Core/Element
  - `Element`: Director-attached feature unit with `Initialize/Ready/Uninitialize` hooks.
- UnityIntegration/Service
  - `UIAbility`, `InputAbility`, `SpawnAbility`, `ObjectPoolAbility` are provided as Abilities.
  - Attach to RootRealm to use.
- UnityIntegration/Editor
  - `FrameworkViewerWindow`: structure text dump UI. Uses `Inspection/FrameworkStructureText`.
- Inspection
  - `FrameworkStructureText`: text dump for Realm/Entities/Abilities/aliases.

## Execution Flow

- Entity registration
  - `SpawnEntityAbility` scans Entities under the Realm Transform and manages the list.
- Realm tree
  - `BuildRealmAbility` builds Realms with `RealmBuilder.Build(parent)` and attaches them via `owner.AddChild(...)`.
  - `Realm` refreshes child Realm/Entity caches on `OnTransformChildrenChanged()`.
- Root Ability
  - UI/input/spawn/pool features are used via Abilities attached to RootRealm.
- Director/Element flow
  - `Director.Awake()` resolves Host, calls `BuildElements()` then `WireElements()`, then optionally `Initialize()`.
  - `Director.Start()` optionally calls `Ready()`, and `OnDestroy()` optionally calls `Uninitialize()`.
  - `Element` instances are registered via `AddElement(...)` and receive lifecycle callbacks in order.
- Tick
  - `GameRoot.Update()` calls `RootRealm.Tick()` to update Abilities with Tick capability.

## Coding Guide (Within This Scope)

- ID policy allows only auto-generated IDs. Do not inject IDs directly except via `Identity`.
- For Ability collaboration, prefer local lookup via Resolver.
- If upstream context collaboration is needed, pass/inject via the owner's `UpstreamAbilityResolver`.
- `RealmBuilder.Build(parent)` does not attach. Attachment is performed by `BuildRealmAbility.Build(owner, builder)`.

### Ability Management Rules

- Use only owner (`Entity`, `Realm`) APIs to add/remove/query Abilities.
- Example: `entity.AddAbility<FooAbility>()`, `realm.GetAbility<SpawnEntityAbility>()`.
- The single source of truth for an Ability is the owner (`AbilityHost`).
- You can auto-attach default Abilities via `AbilityAttribute`.

## Writing/Style Rules

- Follow the coding style in `Assets/DoughFramework/AI/Convention/`.
- Do not write unnecessary comments. If comments are needed, write them concisely in Korean.

### Priority

- User/system/developer instructions > this document > convention documents.
- If there is a nested AGENTS.md in the same tree, the lower-path document takes precedence.

## Operations Tips/Notes

- You can inspect the current Realm/Entity/Ability configuration with `FrameworkViewerWindow`.

# FrameworkV2 Agent Notes

## Content Placement
- New content units must be created under `FrameworkV2/Core/Content`.
