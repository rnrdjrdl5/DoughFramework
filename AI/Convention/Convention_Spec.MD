# Convention_Spec - 코딩 스타일 상세 명세

## 1. 프로퍼티 스타일

### 화살표 표현식 (읽기 전용)
```csharp
public Environment Environment => environment;
public Canvas Canvas => canvas;
```

### get/set 분리
```csharp
public Environment Environment { get; private set; }
public int PanelCustomOffset { get; protected set; }
```

### IReadOnlyList로 컬렉션 보호
```csharp
public IReadOnlyList<Trait> Traits => traits;
public IReadOnlyList<IActorData> ActorDatas => actorDatas;
```

## 2. 필드 초기화

`new()` 표현식 사용:
```csharp
List<Panel> panels = new();
List<Trait> traits = new();
```

## 3. 제네릭 패턴

### Where 제약조건
```csharp
where T : class              // 참조 타입
where T : new()              // 매개변수 없는 생성자
where T : Trait              // 기본 클래스 상속
where T : IData              // 인터페이스 구현
where T : class, IActorData  // 복합 조건
```

### 제네릭 메서드
```csharp
public TElement GetPanelElement<TElement>() where TElement : PanelElement
public TraitType GetTrait<TraitType>() where TraitType : Trait
```

### 타입 체킹
```csharp
typeof(TElement).IsAssignableFrom(element.GetType())
```

## 4. 비동기 (UniTask)

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

## 5. Switch 표현식

```csharp
return operate switch
{
    "+" => leftValue + rightValue,
    "-" => leftValue - rightValue,
    _ => throw new Exception($"Unknown operator {operate}")
};
```

## 6. Null 처리

### 널 조건 연산자
```csharp
OnEnter?.Invoke();
startRefresher?.Refresh();
```

### 명시적 null 체크
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

## 7. LINQ 패턴

```csharp
return traits.Where(t => typeof(T).IsAssignableFrom(t.GetType()))
    .Cast<T>()
    .FirstOrDefault();
```

## 8. Attribute

### SerializeField
```csharp
[SerializeField] List<PanelElement> panelElements;
[SerializeField] Canvas canvas;
```

### 커스텀 Attribute + Reflection
```csharp
[ActorData(Type = typeof(DataType))]

var types = GetType()
    .GetCustomAttributes(typeof(ActorDataAttribute), true)
    .Cast<ActorDataAttribute>()
    .Select(attr => attr.Type);
```

## 9. Partial 클래스

기능별 파일 분할:
```
Universe.cs
Universe.Resources.cs
Universe.Addressable.cs
```

## 10. 메모리 풀링

```csharp
public class Parameter : IDisposable, MemoryPoolModule.IMemoryPool
{
    public void ReleasePool() => Dispose();

    public void Dispose()
    {
        parameters.Clear();
        MemoryPoolModule.Release(this);
    }
}
```

## 11. Using문 정렬

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;      // 서드파티
using UnityEngine;                   // Unity
```

순서: System → System.* → 서드파티 → UnityEngine

## 12. 메서드 오버로딩

```csharp
public void AllocateObject(int count)
public void AllocateObject(int count, GameObject prefab)
public GameObject AllocateObject()
public GameObject AllocateObject(GameObject prefab)
```
