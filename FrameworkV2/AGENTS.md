# FrameworkV2 Overview and Working Notes

이 문서는 `Assets/DoughFramework/FrameworkV2` 트리의 현재 구조/의도/사용 패턴을 요약하고, 이 범위에서 작업 시 따라야 할 실무 지침을 제공합니다. 적용 범위는 본 폴더와 모든 하위 폴더입니다.

## 핵심 개념 요약

- Core는 Unity 참조가 가능한 구조입니다.
- Realm/Entity/Ability는 `MonoBehaviour` 기반이며, Ability는 순수 C# 객체로 동작합니다.
- 수명 관리 `ILifecycle`
  - `Initialize()`, `Ready()`, `Uninitialize()` + 상태 프로퍼티(`IsInitialized`, `IsReady`).
  - 수명 호출은 자동이 아닌 수동 호출입니다.
- Transform 계층이 Realm/Entity 트리의 기준입니다.
- Entity는 GameObject 역할을 수행하며 `WorldEntity` 개념은 제거되었습니다.
- 기존 Service 기능은 모두 Ability로 전환되었고, RootRealm에 부착해 사용합니다.

## 디렉토리 구조 요약

- Core/Common
  - `ILifecycle`, `Identity`, `IIdentifiable`, `AliasSet`, `SubscriptionSet`, `SubscriptionExtensions`.
- Core/Event
  - `HashKey`: 문자열→정수 키(FNV-1a).
- Core/Ability
  - `Ability`: 순수 C# + `ILifecycle` 베이스.
  - `AbilityHost`: Ability 목록/수명/Resolver를 관리하는 호스트 베이스.
  - `IAbilityResolver`: `HasAbility<T>()`/`GetAbility<T>()`(로컬 Ability만 조회).
  - `IAbilityTick`: 매 프레임 Tick이 필요한 Ability용 인터페이스.
  - `AbilityAttribute`, `AbilityAttributeCache`, `AbilityAttributeInstaller`: 클래스 Attribute 기반 Ability 자동 부착.
  - `Common/BuildRealmAbility`, `Common/SpawnEntityAbility`, `Common/EventAbility`.
- 시간(Clock)
  - `ClockAbility`: Realm 단위 시간 스냅샷 보관/조회.
- Core/Realm
  - `Realm`: Children/Abilities/Aliases 보유. 기본 Attribute로 `BuildRealmAbility`, `SpawnEntityAbility`, `ClockAbility` 자동 부착.
- Core/RealmBuilder
  - `RealmBuilder.Build(parent)`: Realm 생성/구성만 수행(부착 금지).
  - `CommonBuilder`: 예시 빌더.
- Core/Entity
  - `Entity`: Aliases/Abilities 보유, Entity 자체가 GameObject 역할.
- UnityIntegration/Service
  - `UIAbility`, `InputAbility`, `SpawnAbility`, `ObjectPoolAbility`는 Ability로 제공.
  - RootRealm에 부착해 사용합니다.
- UnityIntegration/Editor
  - `FrameworkViewerWindow`: 구조 텍스트 덤프 UI. `Inspection/FrameworkStructureText` 사용.
- Inspection
  - `FrameworkStructureText`: Realm/Entities/Abilities/별칭 텍스트 덤프.

## 동작 흐름

- Entity 등록
  - `SpawnEntityAbility`가 Realm 하위 Transform에서 Entity를 스캔해 목록을 관리합니다.
- Realm 트리
  - `BuildRealmAbility`가 `RealmBuilder.Build(parent)`로 생성한 Realm을 `owner.AddChild(...)`로 부착합니다.
  - `Realm`은 `OnTransformChildrenChanged()`에서 자식 Realm/Entity 캐시를 갱신합니다.
- Root Ability
  - UI/입력/스폰/풀 기능은 RootRealm에 부착된 Ability를 통해 사용합니다.
- Tick
  - `GameRoot.Update()`가 `RootRealm.Tick()`을 호출해 Tick 능력을 가진 Ability를 업데이트합니다.

## 코딩 가이드(이 범위에서)

- ID 정책은 자동 생성만 허용. `Identity` 외 직접 Id 주입 금지.
- Ability 협력은 Resolver 기반 로컬 조회를 우선 사용합니다.
- 상위 컨텍스트 협력이 필요하면 소유자 측 `UpstreamAbilityResolver`로 전달/주입합니다.
- `RealmBuilder.Build(parent)`는 부착을 하지 않습니다. 부착은 `BuildRealmAbility.Build(owner, builder)`가 수행합니다.

### Ability 관리 규칙

- Ability 추가/삭제/조회는 소유자(`Entity`, `Realm`) API만 사용합니다.
- 예) `entity.AddAbility<FooAbility>()`, `realm.GetAbility<SpawnEntityAbility>()`.
- Ability의 단일 소스 오브 트루스는 소유자(`AbilityHost`)입니다.
- `AbilityAttribute`를 통해 기본 Ability를 자동 부착할 수 있습니다.

## 작성/스타일 규칙

- 코딩 스타일은 `Assets/DoughFramework/AI/Convention/` 문서를 준수합니다.
- 불필요한 주석은 작성하지 않습니다. 주석이 필요할 경우 한글로 간결히 작성합니다.

### 우선순위

- 사용자/시스템/개발자 지시 > 본 문서 > 컨벤션 문서.
- 동일 트리 내 중첩 AGENTS.md가 있을 경우, 더 하위 경로의 문서가 우선합니다.

## 운영 팁/주의사항

- `FrameworkViewerWindow`로 현재 Realm/Entity/Ability 구성을 점검할 수 있습니다.

# FrameworkV2 Agent Notes

## Content Placement
- New content units must be created under `FrameworkV2/Core/Content`.
