# FrameworkV2 Overview and Working Notes

이 문서는 `Assets/DoughFramework/FrameworkV2` 트리의 현재 구조/의도/사용 패턴을 요약하고, 이 범위에서 작업 시 따라야 할 실무 지침을 제공합니다. 적용 범위는 본 폴더와 모든 하위 폴더입니다.

## 핵심 개념 요약

- Core ↔ UnityIntegration 분리
  - `Core/*`: 순수 C# 로직. UnityEngine 참조 금지.
  - `UnityIntegration/*`: MonoBehaviour, Service, Editor 유틸 등 Unity 의존 레이어.
- 수명 관리 `ILifecycle`
  - `Initialize()`, `Ready()`, `Uninitialize()` + 상태 프로퍼티(`IsInitialized`, `IsReady`).
- 식별/별칭
  - `Identity`: 자동 GUID(`N`) 생성. 외부 주입 금지(자동 할당만 허용).
  - `AliasSet`: 간단 리스트 컨테이너(중복 허용, 선형 탐색, Ordinal 비교).
- 이벤트
  - `EventAbility`: 키 기반 이벤트 등록/실행(일반/제네릭 payload). 필요 시 Ability로 부착하여 사용.
  - `HashKey.FromName(string)`: 이름→정수 키(FNV-1a 해시).
  - `SubscriptionSet` + `AddTo(...)`: 구독/정리 유틸리티.
- 커맨드(입력/출력 파이프)
  - `ICommand`: 마커 인터페이스.
  - 입력: `HandlesCommandAttribute(Type)` + `AbilityCommandCache`로 Ability 메서드를 리플렉션 등록.
  - `CommandAbility`: 입력 큐 드레인, 타입별 팬아웃, 출력 큐/이벤트(`OutputAvailable`) 관리.
  - 출력: Presence가 출력 큐를 드레인하고 `[HandlesOutputCommand]`로 처리.

## 디렉토리 구조 요약

- Core/Common
  - `ILifecycle`, `Identity`, `IIdentifiable`, `AliasSet`, `SubscriptionSet`, `SubscriptionExtensions`.
- Core/Event
  - `HashKey`: 문자열→정수 키(FNV-1a).
- Core/Command
  - `ICommand`, `HandlesCommandAttribute`, `AbilityCommandCache`(입력 핸들러 캐시).
- Core/Ability
  - `Ability`: 라이프사이클 베이스, `AbilityResolver`(로컬 조회 전용) 보유.
  - `IAbilityResolver`: `HasAbility<T>()`/`GetAbility<T>()`(로컬 AbilitySet 조회).
  - `AbilitySet`: 추가/삭제/조회/수명, Resolver 주입/해제 관리.
  - `AbilityAttribute`, `AbilityAttributeCache`, `AbilityAttributeInstaller`: 클래스 Attribute 기반 Ability 자동 부착.
- `Common/CommandAbility`, `Common/EventAbility`, `Common/BuildRealmAbility`, `Common/SpawnEntityAbility`.
- 시간(Clock)
  - `ClockAbility`: Realm 단위 시간 스냅샷 보관/조회. UnityIntegration의 `GameRoot.Update()`가 프레임마다 `TimeSnapshot`을 트리에 푸시합니다.
  - `TimeSnapshot`/`TimeDomain`: Core에서 Unity API 없이 시간을 표현.
- Core/Realm
- `Realm`: Children/Abilities/Aliases. 기본 Attribute로 `BuildRealmAbility`, `SpawnEntityAbility` 자동 부착.
  - `UpstreamAbilityResolver`: 상위 컨텍스트 Resolver 보관(상향 조회 포인트 전달용).
- Core/RealmBuilder
  - `RealmBuilder.Build(parent)`: Realm 생성/구성만 수행(부착 금지).
  - `CommonBuilder`: 예시 빌더.
- Core/Entity
  - `Entity`: Aliases/Abilities 보유, `UpstreamAbilityResolver` 필드 보관.
  - `LocalEntity`, `WorldEntity([Ability(typeof(CommandAbility))])`.
  - `WorldEntity.Execute(ICommand)`: `CommandAbility` 보장(없으면 추가) 후 실행.
- UnityIntegration/Presence
  - `Presence`(MonoBehaviour): 출력 드레인 + `[HandlesOutputCommand]` 핸들러 호출.
  - `PresenceOutputCommandCache`, `HandlesOutputCommandAttribute`, 파생 베이스 `Avatar`, `UI`.
  - UI 레이어: `Core`, `Panel`(비모달), `Modal`, `System` 정렬 규칙 사용.
  - `HeadlessPresence`(Example): MonoBehaviour 없는 출력 드레인 구현.
- UnityIntegration/Service
  - `IService`/`Service`: 라이프사이클 보조.
  - `TypeRegistry`: 인터페이스 타입 기반 조회(조회 전용, IService 비인지).
  - `ServiceSet` + `IServiceResolver`: 씬/자식에서 `IService` 자동 수집/등록(발견 순서 보존), 라이프사이클 호출 API, 외부에는 인터페이스 기반 O(1) 조회 제공.
  - `ObjectPoolService`: 간단 오브젝트 풀.
  - `UIService(IUIService)`: Realm별 Core/Panel/Modal과 전역 System UI를 관리, Panel/Modal 스택 정렬 및 활성 Realm 상태 보유.
  - `GameRoot`: 캐시 초기화(`AbilityCommandCache.Clear`, `PresenceOutputCommandCache.Clear`, `AbilityAttributeCache.Clear`) 및 서비스 초기화, 루트 Realm 생성. `Services: IServiceResolver` 노출.
- UnityIntegration/Editor
  - `FrameworkViewerWindow`(메뉴: `Dough/Framework Viewer`): 구조 텍스트 덤프 UI. `Inspection/FrameworkStructureText` 사용.
- UnityIntegration 기타
  - `WorldEntityBehaviour`, `WorldEntitySpawner`: 엔티티 바인딩/프리팹 스폰 보조.
- Inspection
  - `FrameworkStructureText`: Realm/Entities/Abilities/별칭 텍스트 덤프.
- Example
  - 명령/출력/헤드리스 프레즌스/Unity 예제(`UnityIntegration/Example/*`).

## 동작 흐름

- 입력(Presence → Entity)
  - Presence/사용자 코드에서 `WorldEntity.Execute(cmd)` 호출 → `CommandAbility`가 큐 드레인 → `HandlesCommand` 메서드 팬아웃 → Ability 내부 상태 변경/출력 발생.
- 출력(Entity → Presence)
  - Ability가 `CommandAbility.EmitOutput(cmd)` 호출 → Presence가 큐를 드레인하며 `[HandlesOutputCommand]` 메서드 호출. Core에서는 예외를 삼키고, Presence에서는 Unity 로그로 예외 보고.
- 이벤트(선택)
  - `EventAbility.Register(...)`, `Execute(...)` 제공. 기본 장착은 아님(필요 시 Ability로 부착하여 사용).
- Realm/Registry/Upstream
- `SpawnEntityAbility`: `AddLocalEntity`/`AddWorldEntity` 시 엔티티에 `UpstreamAbilityResolver` 주입, 수명 보장(Initialize/Ready/Uninitialize).
  - `BuildRealmAbility`: `RealmBuilder.Build(parent)`가 반환한 Realm을 `owner.AddChild(...)`로 부착하며, 새 Realm에 상위 `AbilityResolver`를 전달.

### 최근 변경(Realm/Entity 관련)

- BuildRealmAbility
  - 새 Realm이 부모에 부착된 직후 `RealmBuilt(Realm child)` 이벤트를 발생시킵니다.
- SpawnEntityAbility
  - WorldEntity 전용 수명 이벤트를 제공합니다: `WorldEntityAdded(WorldEntity)`, `WorldEntityRemoved(WorldEntity)`.
  - LocalEntity에는 대응 GameObject가 없으므로 관련 이벤트는 없습니다.
- GameRoot
  - `RootRealm = BuildRealmTree()` 직후, 전체 Realm 트리를 DFS로 순회하며 RealmPresence를 1:1로 생성합니다.
- RealmPresence(요약, 상세는 UnityIntegration/AGENTS.md 참고)
  - 각 Realm에 대해 하나 생성됩니다. `BuildRealmAbility.RealmBuilt`를 구독해 동적으로 생성되는 자식 Realm에 대해서도 Presence를 만듭니다.
  - `SpawnEntityAbility.WorldEntityAdded/Removed`를 구독하여 표현 오브젝트(Avatar)를 스폰/반납합니다. 상태 컨테이너(캐시/맵)는 유지하지 않습니다.
  - 기본 World 루트는 제거되었으며, 아바타는 RealmPresence 바로 하위의 `Avatars` 루트에 부착합니다.

## 코딩 가이드(이 범위에서)

- Core는 UnityEngine 참조 금지. Unity 의존 로직은 `UnityIntegration/*`에만 둡니다.
- ID 정책은 자동 생성만 허용. `Identity` 외 직접 Id 주입 금지.
- Ability 간 협력은 Resolver 기반 로컬 조회를 우선 사용합니다.
  - `AbilityResolver.GetAbility<T>()`는 “로컬 AbilitySet”만 조회합니다(상향 검색 아님).
  - 상위 컨텍스트 협력이 필요하면 소유자 측 `UpstreamAbilityResolver`로 전달/주입하세요.
- 리플렉션 캐시 초기화는 `GameRoot.Awake()`에서 수행합니다(필수: 도메인 리로드/씬 재시작 안정성).
- `RealmBuilder.Build(parent)`는 부착을 하지 않습니다. 부착은 `BuildRealmAbility.Build(owner, builder)`가 수행합니다.

### Ability 관리 규칙

- Ability 추가/삭제/조회는 소유자(`Entity`, `Realm`) API만 사용합니다.
- 예) `entity.AddAbility(new Foo())`, `realm.GetAbility<SpawnEntityAbility>()`.
- 단일 소스 오브 트루스는 소유자의 `AbilitySet`입니다(Ability가 별도 컨테이너를 유지하지 않음).
- `AbilityAttribute`를 통해 기본 Ability를 자동 부착할 수 있습니다.
- 예) `Realm`은 `BuildRealmAbility`, `SpawnEntityAbility`가 자동 장착; `WorldEntity`는 `CommandAbility` 자동 장착.

## 작성/스타일 규칙

- 코딩 스타일은 `Assets/DoughFramework/AI/Convention/` 문서를 준수합니다.
- 불필요한 주석은 작성하지 않습니다. 주석이 필요할 경우 한글로 간결히 작성합니다.

### 우선순위

- 사용자/시스템/개발자 지시 > 본 문서 > 컨벤션 문서.
- 동일 트리 내 중첩 AGENTS.md가 있을 경우, 더 하위 경로의 문서가 우선합니다.

## 운영 팁/주의사항

- 캐시 클리어 순서: `AbilityCommandCache.Clear()` → `PresenceOutputCommandCache.Clear()` → `AbilityAttributeCache.Clear()`(현재 `GameRoot.Awake()`에서 처리).
- `WorldEntity.Execute(...)`는 `CommandAbility`가 없을 때 자동 추가합니다(입력 파이프 강제 보장).
- Presence 출력 드레인은 프레임마다 수행됩니다(`Presence.Update()`에서 호출). 헤드리스 환경에서는 `HeadlessPresence.Tick()`을 직접 호출하세요.
- `FrameworkViewerWindow`로 현재 Realm/Entity/Ability 구성을 시각 점검하고, `Copy` 버튼으로 텍스트 덤프를 복사할 수 있습니다.
