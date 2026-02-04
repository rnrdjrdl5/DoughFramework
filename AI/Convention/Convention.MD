# Convention - 코딩 스타일

DoughFramework 프로젝트의 C# 코딩 컨벤션.

## 네이밍 규칙

| 대상 | 규칙 | 예시 |
|------|------|------|
| 클래스/구조체 | PascalCase | `BaseNode`, `Panel`, `EventListener` |
| 메서드 | PascalCase | `OnEnterNode()`, `GetModule<T>()` |
| 프로퍼티 | PascalCase | `Environment`, `Canvas`, `Parent` |
| private 필드 | camelCase | `parentNode`, `targetDataSet` |
| 파라미터 | camelCase | `prefabPath`, `parameter` |
| 상수 | PascalCase | `DefaultHierarchyLevel` |
| Enum | PascalCase | `BTNodeState.Running` |

## 콜백/이벤트 메서드

`On` prefix 사용:
- `OnEnterState()`, `OnUpdateState()`, `OnExitState()`
- `OnEnterFlow()`, `OnExitFlow()`
- `OnEnterNode()`, `OnUpdateNode()`, `OnExitNode()`

## 접근 제한자 원칙

- **필드**: private (기본) + public 프로퍼티로 노출
- **프로퍼티**: public get, private set
- **상속용**: protected virtual/override

## 포맷팅

- **들여쓰기**: 4 스페이스
- **중괄호**: Allman 스타일 (새 줄에서 시작)
- 언더스코어(`_`) prefix 미사용 (모든 변수/필드에서 금지)

## 프로퍼티 스타일

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

언더스코어(`_`) 백킹필드 금지:
```csharp
// before
int _value;
public int Value => _value;

// after
int value;
public int Value => value;
```

## 필드 초기화

`new()` 표현식 사용:
```csharp
List<Panel> panels = new();
List<Trait> traits = new();
```

멤버 변수 초기화 규칙(필수)
- 클래스/구조체의 멤버 변수에서 객체를 생성할 때는 `new Type()`이 아닌 타겟-타입 `new()`를 사용합니다.

잘못된 예
```csharp
List<string> datas = new List<string>();
Dictionary<string, int> map = new Dictionary<string, int>();
```

올바른 예
```csharp
List<string> datas = new();
Dictionary<string, int> map = new();
```

언더스코어 금지 예시
```csharp
// before
readonly List<string> _datas = new();

// after
readonly List<string> datas = new();
```

## 제네릭 패턴

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

## 비동기 (UniTask)

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

## Switch 표현식

```csharp
return operate switch
{
    "+" => leftValue + rightValue,
    "-" => leftValue - rightValue,
    _ => throw new Exception($"Unknown operator {operate}")
};
```

## Null 처리

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

## LINQ 패턴

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

### 커스텀 Attribute + Reflection
```csharp
[ActorData(Type = typeof(DataType))]

var types = GetType()
    .GetCustomAttributes(typeof(ActorDataAttribute), true)
    .Cast<ActorDataAttribute>()
    .Select(attr => attr.Type);
```

## Partial 클래스

기능별 파일 분할:
```
Universe.cs
Universe.Resources.cs
Universe.Addressable.cs
```

## 메모리 풀링

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

## Using문 정렬

순서: System → System.* → 서드파티 → UnityEngine
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;      // 서드파티
using UnityEngine;                   // Unity
```

## 주석

- 구조적 주석: `// NOTE :`, `// TBD`
- 기본적으로 한글 주석을 작성합니다.
- 클래스 시작부에는 기능 목적을 한 줄 주석으로 명시합니다.
- 함수나 문맥(중요 분기/처리 흐름)에는 읽기 쉬운 설명 주석을 추가합니다.
- 불필요한 주석은 피하되, 가독성을 우선합니다.
- 한국어/영어 혼용 허용
- XML 문서 주석은 필요시에만

## 메서드 오버로딩

```csharp
public void AllocateObject(int count)
public void AllocateObject(int count, GameObject prefab)
public GameObject AllocateObject()
public GameObject AllocateObject(GameObject prefab)
```

## 멤버 변수 선언 규칙

- private 키워드는 사용하지 않습니다. (암시적 private 적용)
- 멤버 변수 선언 순서는 public → private(표기 생략) 입니다.
- 같은 범주에서는 컨테이너(Dictionary, List 등) → 기본형(int, string, float 등) 순서로 선언합니다.
- public 블록과 private 블록 사이에는 공백 한 줄을 둡니다.
- 멤버 변수 초기화에서 `new`를 사용할 때는 타겟-타입 `new()`만 사용합니다. (예: `List<string> datas = new();`)

### 멤버 변수 선언 상세

#### 접근 한정자
- private 키워드는 표기하지 않습니다. (클래스 멤버의 기본 접근자는 private)
- protected/public 등 다른 접근 한정자는 기존 규칙대로 명시합니다.

#### 선언 순서
- 멤버 변수는 그룹을 나누어 선언합니다: `public` 그룹 → 빈 줄 → `private` 그룹(표기 생략)
- 각 그룹 내부에서는 컨테이너(Dictionary, List, HashSet 등) → 기본형/값형(int, string, float 등) 순서로 정렬합니다.
- 모든 멤버/로컬/파라미터 이름에 선행 언더스코어(`_`)를 사용하지 않습니다.

### 언더스코어 금지 상세
- 모든 멤버/로컬 변수/파라미터명에서 선행 언더스코어(`_`)를 사용하지 않습니다.
- 기존 `_fieldName` 패턴은 `fieldName`으로 변경합니다.
- 프로퍼티/백킹필드 조합에서도 언더스코어를 사용하지 않습니다. 예)
  ```csharp
  // before
  int _count;
  public int Count => _count;

  // after
  int count;
  public int Count => count;
  ```

예시
```csharp
public List<Foo> foos;
public int maxCount;

List<Foo> internalFoos;   // private 키워드 생략
int currentCount;          // private 키워드 생략

// 초기화 시 타겟-타입 new() 사용
List<string> datas = new();
Dictionary<string, int> map = new();
```
