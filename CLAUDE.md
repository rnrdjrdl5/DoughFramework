# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Unity JRPG 게임 개발 프레임워크. Unity 6000.0.51f1 기반, C# 사용.

## Build & Development

- **Unity 프로젝트**: Unity Editor에서 직접 열기 (자동 컴파일)
- **솔루션 파일**: `DoughFramework.sln` (Visual Studio/Rider)
- **테스트**: Unity Test Framework 사용 (Window > General > Test Runner)

### Assembly Definitions
- `JRPGProject.Common.asmdef` - 메인 프레임워크 코드
- `JRPGProject.Generated.asmdef` - 자동 생성 코드
- `JRPGProject.ExcelProcessor.Editor.asmdef` - 에디터 전용 Excel 처리

## Architecture

### Entry-Universe-Module 초기화 패턴
```
Entry (MonoBehaviour) - 게임 시작점
  └── Universe (싱글톤) - 게임 월드 관리
      └── Environment - 모듈 컨테이너
          └── Modules[] (ObjectPoolModule, StageModule, LocalDataModule, SoundModule 등)
```
모듈 조회: `Environment.GetModule<ModuleType>()`

### Actor-Trait 컴포지션 시스템
```
Actor (MonoBehaviour, IEnvironment) - 게임 객체 베이스
  ├── ActorDatas[] - 커스텀 데이터 (ActorDataAttribute로 등록)
  ├── Traits[] - 기능 컴포넌트 (StateRunnerTrait, FlowRunnerTrait, ProcesserTrait 등)
  └── Children - 자식 액터
```
Actor는 상속 대신 Trait 조합으로 기능 확장.

### State 머신
```
BaseState → State<OwnerType> → SubState<OwnerType>
```
- 생명주기: `OnEnterState()` → `OnUpdateState()` / `OnFixedUpdateState()` → `OnExitState()`
- 비동기 지원: `OnEnterStateAsync()` (UniTask)

### Flow 시스템
계층적 흐름 제어. `Flow<OwnerType>` 상속하여 순차/루프 실행 구현.
- `OnEnterFlow()`, `OnExitFlow()` 오버라이드

### BehaviourTree
AI용 행동트리. `BaseNode` 상속 (DecoratorNode, CompositeNode, LeafNode).
- 상태: `BTNodeState` (None, Running, Success, Failure)

### Processer 시스템
로직 처리기. Attribute 기반 자동 등록.
```csharp
[Processer]
public class MyProcesser : Processer { }

[UpdateProcesser]
public class MyUpdateProcesser : UpdateProcesser { }
```
생명주기: `Initialize()` → `Ready()` → `Uninitialize()`

### Panel (UI) 시스템
`Panel : Actor` - UI 패널은 Actor 확장.
- `PanelElements[]`, `InteractionEvent`, `DataSet` 포함
- `PanelTrait`로 패널 동작 관리

## Coding Style

코드 작성 시 `AI/Convention/` 참고할 것.

## Key Patterns

1. **생명주기 분리**: `Initialize()` (설정) → `Ready()` (시작) 패턴 일관 적용
2. **제네릭 Owner**: `State<T>`, `Flow<T>` 등에서 Owner 타입 명시
3. **Attribute 기반 등록**: `ProcesserAttribute`, `UpdateProcesserAttribute`, `ActorDataAttribute`
4. **오브젝트 풀링**: `ObjectPoolModule`로 GC 최적화, `ListPool<T>` 활용

## Dependencies

- **UniTask**: 비동기 처리 (async/await)
- **DOTween/DOTweenPro**: 트윈 애니메이션
- **Addressables**: 에셋 로드
- **Input System**: 입력 처리
- **URP**: 렌더링

## Code Location

| 모듈 | 경로 |
|------|------|
| Entry/Universe | `Assets/Script/Entry/` |
| Actor | `Assets/Script/Actor/` |
| Trait | `Assets/Script/Trait/` |
| State | `Assets/Script/State/` |
| Flow | `Assets/Script/Flow/` |
| BehaviourTree | `Assets/Script/BehaviourTree/` |
| Processer | `Assets/Script/Processer/` |
| Panel | `Assets/Script/Panel/` |
| Data | `Assets/Script/Data/` |
| Generated | `Assets/Generated/` |

## AI Documentation Convention

주제별 문서는 `AI/[주제명]/` 디렉토리에 3개의 MD 파일로 관리:

```
AI/
├── Addressable/
│   ├── Addressable.MD          # 개요/메인 문서
│   ├── Addressable_Spec.MD     # 상세 명세
│   └── Addressable_History.MD  # 변경 이력
├── [다른주제]/
│   ├── [다른주제].MD
│   ├── [다른주제]_Spec.MD
│   └── [다른주제]_History.MD
└── ...
```

**파일별 역할:**
| 파일 | 역할 | 기록할 내용 |
|------|------|-------------|
| `[주제].MD` | 개요/메인 | 전체적인 설명, 핵심 개념, 사용 방법 |
| `[주제]_Spec.MD` | 상세 명세 | 구체적인 구현 사항, 파라미터, 인터페이스, 규칙 |
| `[주제]_History.MD` | 변경 이력 | 언제, 무엇이 변경되었는지 히스토리 |

**참고 규칙**: "AI/[주제]/ 참고해" 요청 시 → 해당 폴더의 3개 MD 파일 모두 읽을 것

**기록 규칙:**
- 대화 내용을 각 파일의 역할에 맞게 분류하여 기록
- `[주제].MD`, `[주제]_Spec.MD` → 사용자가 "업데이트해달라" 요청 시 갱신
- `[주제]_History.MD` → 변경이 있을 때마다 **항상 누적 기록**
