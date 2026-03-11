# Convention - Coding Style

C# coding conventions for the DoughFramework project.

## Naming Rules

| Target | Rule | Example |
|------|------|------|
| Class/Struct | PascalCase | `BaseNode`, `Panel`, `EventListener` |
| Method | PascalCase | `OnEnterNode()`, `GetModule<T>()` |
| Property | PascalCase | `Environment`, `Canvas`, `Parent` |
| Private field | camelCase | `parentNode`, `targetDataSet` |
| Parameter | camelCase | `prefabPath`, `initData` |
| Constant | PascalCase | `DefaultHierarchyLevel` |
| Enum | PascalCase | `BTNodeState.Running` |

## Callback/Event Methods

Use the `On` prefix:
- `OnEnterState()`, `OnUpdateState()`, `OnExitState()`
- `OnEnterFlow()`, `OnExitFlow()`
- `OnEnterNode()`, `OnUpdateNode()`, `OnExitNode()`

## Access Modifier Principles

- **Fields**: private (default) + exposed via public properties
- **Properties**: public get, private set
- **For inheritance**: protected virtual/override

## Formatting

- **Indentation**: 4 spaces
- **Braces**: Allman style (start on a new line)
- No underscore (`_`) prefix (disallowed for all variables/fields)

## Property Style

### Arrow expression (read-only)
```csharp
public Environment Environment => environment;
public Canvas Canvas => canvas;
```

### get/set split
```csharp
public Environment Environment { get; private set; }
public int PanelCustomOffset { get; protected set; }
```

### Protect collections with IReadOnlyList
```csharp
public IReadOnlyList<Trait> Traits => traits;
public IReadOnlyList<IActorData> ActorDatas => actorDatas;
```

No underscore (`_`) backing fields:
```csharp
// before
int _value;
public int Value => _value;

// after
int value;
public int Value => value;
```

## Field Initialization

Use `new()` expressions:
```csharp
List<Panel> panels = new();
List<Trait> traits = new();
```

Member variable initialization rule (required)
- When creating objects in class/struct member variables, use target-typed `new()` instead of `new Type()`.

Incorrect example
```csharp
List<string> datas = new List<string>();
Dictionary<string, int> map = new Dictionary<string, int>();
```

Correct example
```csharp
List<string> datas = new();
Dictionary<string, int> map = new();
```

No-underscore example
```csharp
// before
readonly List<string> _datas = new();

// after
readonly List<string> datas = new();
```

## Generic Patterns

### Where constraints
```csharp
where T : class              // reference type
where T : new()              // parameterless constructor
where T : Trait              // base class inheritance
where T : IData              // interface implementation
where T : class, IActorData  // composite constraint
```

### Generic methods
```csharp
public TElement GetPanelElement<TElement>() where TElement : PanelElement
public TraitType GetTrait<TraitType>() where TraitType : Trait
```

### Type checking
```csharp
typeof(TElement).IsAssignableFrom(element.GetType())
```

## Async (UniTask)

### Async suffix
```csharp
public virtual async UniTask OnEnterStateAsync()
{
    await base.OnEnterStateAsync();
}
```

### Forget() - Fire and forget
```csharp
activatedState.OnEnterStateAsync().Forget();
```

### CancellationToken
```csharp
CancellationTokenSource source = new();
public CancellationToken Token => source.Token;
```

## Switch Expression

```csharp
return operate switch
{
    "+" => leftValue + rightValue,
    "-" => leftValue - rightValue,
    _ => throw new Exception($"Unknown operator {operate}")
};
```

## Null Handling

### Null-conditional operator
```csharp
OnEnter?.Invoke();
startRefresher?.Refresh();
```

### Explicit null check
```csharp
if (listener == null) return;
```

### TryGetValue
```csharp
if (listeners.TryGetValue(key, out var listenerList))
{
    listenerList.Remove(listener);
}
```

## LINQ Patterns

```csharp
return traits.Where(t => typeof(T).IsAssignableFrom(t.GetType()))
    .Cast<T>()
    .FirstOrDefault();
```

## Attribute

### SerializeField
```csharp
[SerializeField] List<PanelElement> panelElements;
[SerializeField] Canvas canvas;
```

### Custom Attribute + Reflection
```csharp
[ActorData(Type = typeof(DataType))]

var types = GetType()
    .GetCustomAttributes(typeof(ActorDataAttribute), true)
    .Cast<ActorDataAttribute>()
    .Select(attr => attr.Type);
```

## Partial Classes

Split files by feature:
```
Universe.cs
Universe.Resources.cs
Universe.Addressable.cs
```

## Memory Pooling

- Use pooling for frequently allocated gameplay objects.
- `IInitData` is lightweight; prefer `EmptyInitData.Instance` when no data is required.

## Using Directive Ordering

Order: System → System.* → third-party → UnityEngine
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;      // third-party
using UnityEngine;                   // Unity
```

## Comments

- Structural comments: `// NOTE :`, `// TBD`
- Write comments in Korean by default.
- At the start of a class, **must** add a single-line comment that clearly states the purpose.
- At the start of a function, **must** add a single-line comment explaining the function's purpose (general methods only).
- However, avoid comments that simply repeat the function name; if the function has no clear intent, it is okay to omit.
- Add extra explanatory comments for important branches/flows.
- Avoid unnecessary comments, but prioritize readability.
- Mixing Korean/English is allowed.
- XML doc comments only when needed.

## Function Structure (Orchestration)

- The closer to the entry point, the more **high-level/rough flow** you define.
- The deeper the function, the more **granular concrete logic** it handles.
- Upper-level functions coordinate order/flow and delegate actual processing to lower-level functions.

Example:
```csharp
// Orchestrator that manages the session initialization flow
public class Session
{
    // High-level initialization entry point called externally
    public void Initialize(SessionConfig config)
    {
        InitializeEnvironment(config);
        InitializeState(config);
    }

    // Coordinates only the flow of internal initialization steps
    void InitializeState(SessionConfig config)
    {
        AssignSessionId();
        LoadModules(config);
        WireEvents();
    }

    // Actual module loading logic
    void LoadModules(SessionConfig config)
    {
        // concrete logic
    }
}
```

## Method Overloading

```csharp
public void AllocateObject(int count)
public void AllocateObject(int count, GameObject prefab)
public GameObject AllocateObject()
public GameObject AllocateObject(GameObject prefab)
```

## Member Variable Declaration Rules

- Do not use the private keyword (implicit private applies).
- Member declaration order is public → private (implicit).
- Within the same group, declare containers (Dictionary, List, etc.) before primitives (int, string, float, etc.).
- Insert a blank line between the public and private groups.
- When using `new` for member initialization, only use target-typed `new()` (e.g., `List<string> datas = new();`).

### Member Variable Declaration Details

#### Access Modifiers
- Do not write the private keyword. (Default access for class members is private.)
- Explicitly write protected/public modifiers per existing rules.

#### Declaration Order
- Split member variables into groups: `public` group → blank line → `private` group (implicit).
- Within each group, order containers (Dictionary, List, HashSet, etc.) before primitives/value types (int, string, float, etc.).
- Do not use a leading underscore (`_`) in any member/local/parameter name.

### No-underscore Details
- Do not use a leading underscore (`_`) in any member/local variable/parameter name.
- Change existing `_fieldName` patterns to `fieldName`.
- Do not use underscores in property/backing-field pairs. Example:
  ```csharp
  // before
  int _count;
  public int Count => _count;

  // after
  int count;
  public int Count => count;
  ```

Example
```csharp
public List<Foo> foos;
public int maxCount;

List<Foo> internalFoos;   // private keyword omitted
int currentCount;          // private keyword omitted

// Use target-typed new() for initialization
List<string> datas = new();
Dictionary<string, int> map = new();
```
