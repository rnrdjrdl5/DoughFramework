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
- 언더스코어 prefix 미사용

## 주석

- 구조적 주석: `// NOTE :`, `// TBD`
- 한국어/영어 혼용 허용
- XML 문서 주석은 필요시에만
