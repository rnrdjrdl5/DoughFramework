# Convention - 코딩 스타일 개요

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

## 주석

- 구조적 주석: `// NOTE :`, `// TBD`
- 한국어/영어 혼용 허용
- XML 문서 주석은 필요시에만

## 멤버 변수 선언 규칙

- private 키워드는 사용하지 않습니다. (암시적 private 적용)
- 멤버 변수 선언 순서는 public → private(표기 생략) 입니다.
- 같은 범주에서는 컨테이너(Dictionary, List 등) → 기본형(int, string, float 등) 순서로 선언합니다.
- public 블록과 private 블록 사이에는 공백 한 줄을 둡니다.
- 멤버 변수 초기화에서 `new`를 사용할 때는 타겟-타입 `new()`만 사용합니다. (예: `List<string> datas = new();`)

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
