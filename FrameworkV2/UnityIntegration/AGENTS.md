# UnityIntegration — Agent Notes and Rules

이 문서는 `Assets/DoughFramework/FrameworkV2/UnityIntegration` 트리에서 작업할 때 따라야 할 규칙과 구조를 정리합니다. 상위(`FrameworkV2/AGENTS.md`) 지침을 상속하며, 본 문서의 범위 내 항목은 이 디렉터리 하위에 우선 적용됩니다.

## 범위와 레이어링

- Unity 의존 레이어입니다. MonoBehaviour, Editor, Addressables, Resources 등 Unity API 사용 허용.
- Core 레이어(C# 순수 로직) 코드를 UnityIntegration로 옮기지 않습니다. UnityIntegration에서는 Core 타입을 소비만 합니다.

## 수명주기와 서비스 컨테이너

- `IService`/`Service`를 사용해 `Initialize()` → `Ready()` → `Uninitialize()` 순서로 수명 관리합니다.
  - 다건 초기화/해제 호출은 가드되어야 합니다. 실제 로직은 `OnInitialize/OnReady/OnUninitialize`에서 처리.
- `GameRoot`는 다음을 보장합니다.
  - 캐시 초기화(`AbilityCommandCache.Clear`, `PresenceOutputCommandCache.Clear`, `AbilityAttributeCache.Clear`).
  - `ServiceSet` 구성 → `RegisterAllFrom(ServicesRoot or self)` → `InitializeAll()` → `ReadyAll()`.
  - 파괴 시 `UninitializeAll()` → Resolver 분리 → ClearAll().
- `ServiceSet`/`IServiceResolver` 사용 규칙
  - 등록 키는 “인터페이스 타입”만 허용합니다. 구현(클래스) 타입은 등록/조회 대상이 아닙니다(O(1) 조회, 의존 최소화 목적).
  - 교차 의존은 `OnReady()`에서 1회 조회 후 캐시하세요. 핫패스에서 매 프레임 조회하지 않습니다.
  - 필수 의존이면 초기화 단계에서 명확하게 실패(또는 경고+폴백) 정책을 택합니다.

## TypeRegistry

- `TypeRegistry`는 로컬 맵 유틸(인터페이스 키 전용)입니다. 전역/싱글톤 대체로 쓰지 않습니다.
- 새 기능에서 글로벌 서비스 등록/조회에는 `ServiceSet`을 사용하고, `TypeRegistry`는 개별 컴포넌트 내부 캐시 등 제한된 범위에서만 사용합니다.

## Presence 계층

- `Presence`는 출력 드레인 전담 레이어입니다. `AttachService(IServiceResolver)`/`DetachService()`로 Resolver 접근 가능.
- 출력 핸들러는 `[HandlesOutputCommand]` 특성으로 선언하며, 예외는 Unity 로그로 보고합니다.
- `Avatar`, `UI`는 `Presence`의 파생 베이스입니다. 무거운 도메인 로직은 Core Ability로 유지하고 Presence는 바인딩/표현에 집중합니다.

### RealmPresence 규칙(최근 업데이트)

- 생성/수명
  - 각 Realm에 대해 `RealmPresence`를 1:1로 생성합니다. `GameRoot`가 RootRealm 빌드 직후 전체 트리를 DFS로 생성합니다.
  - 부모 Realm의 `BuildRealmAbility.RealmBuilt` 이벤트를 구독하여, 런타임에 생성되는 자식 Realm에 대해서도 즉시 `RealmPresence`를 생성합니다.
- 트랜스폼 구조
  - `Avatars`와 `UI` 루트만 유지합니다. 과거 `World` 루트는 제거되었습니다.
  - 아바타는 항상 `Avatars` 하위에 부착합니다.
- 엔티티 표현(아바타)
  - `SpawnEntityAbility.WorldEntityAdded/Removed`를 구독합니다.
  - Added: 주소 키로 현재는 임시 `"Avatar"` 프리팹을 사용하여 스폰하고 `Avatars` 하위에 부착합니다.
    - 중복 생성 허용: 동일 `WorldEntity`로 여러 아바타가 생성될 수 있습니다(중복 체크 없음).
  - Removed: `Avatars` 하위 `Avatar` 컴포넌트를 스캔하여 `a.Entity == e`인 모든 오브젝트를 풀에 반납/파괴합니다.
  - 컨테이너(딕셔너리/리스트)로 아바타를 관리하지 않습니다. 항상 트리 스캔을 사용합니다.
- 책임 경계
  - Core는 아바타/프리팹/Unity를 모릅니다. RealmPresence는 표현만 담당합니다.
  - 아바타 키 결정은 추후 Unity 쪽 데이터/규칙으로 확장 예정이며, 인터페이스 도입 없이도 운영 가능합니다.

## UI 레이어와 정렬 규칙

- `Presence/UI/UILayer`, `UILayerOrder`, `UISortingOrder`를 사용합니다.
  - 레이어는 `Core`, `Panel`(비모달), `Modal`, `System`으로 구성됩니다.
  - `UISortingOrder`는 `Canvas.overrideSorting = true`와 정렬 순서를 자동 적용합니다.
  - 새 UI는 레이어/스텝 규칙을 따르도록 `UISortingOrder` 컴포넌트를 부착하세요.
- 입력/포커스/모달 정책은 `ui_input_design.md`의 의사결정 안을 준수합니다(추후 세부 구현이 들어올 수 있음).

## UI 관리 서비스(IUIService)

- 역할: Realm별 UI 루트(Core/Panel/Modal)와 전역(System) 루트 관리, Panel/Modal 스택과 정렬 일관성 유지.
- 주요 API
  - 루트: `GetSystemRoot()`, `GetCoreRoot(realm)`, `GetPanelRoot(realm)`
  - 열기: `OpenPanel(realm, go, extraOffset=0)`, `OpenModal(realm, go, extraOffset=0)`
  - 닫기: `ClosePanel(realm, go)`, `CloseModal(realm, go)`
  - 조회: `GetPanelCount(realm)`, `GetModalCount(realm)`, `GetActivePanels(realm)`, `GetActiveModals(realm)`
  - 부착: `AttachToCore(realm, go)`, `AttachToSystem(go)`
- 정렬 규칙
  - Panel: `PanelBase + index * PanelStep`
  - Modal: `ModalBase + index * ModalStep`(항상 Panel 위 대역)
  - System은 최상단 전역, Core는 최하단 지속 UI

## Addressables 접근

- 정적 유틸 `AddressableAccess`를 통해 Addressables 자산을 동기/비동기로 로드합니다.
  - `LoadAsset<T>(key)`, `LoadAssetAsync<T>(key)`.
  - 아틀라스/스프라이트 헬퍼 제공: `LoadAtlas`, `LoadSpriteFromAtlas` 등.
- 에디터 지원
  - `AddressableAccess/AddressableAutoRegistrar`: `Assets/Contents/{Content}/{BuiltIn|CDN}/...` 경로의 자산을 자동 Addressables 그룹으로 등록합니다.
  - `AddressableAccess/Editor/*`: 주소/그룹 확인용 간단 툴 탭 제공.

## 오브젝트 풀과 스폰

- `ObjectPoolService`
  - 프리팹 기준 풀 맵 유지: `Spawn(GameObject prefab)`, `Despawn(GameObject instance)`.
  - 해제 시 풀 내 오브젝트를 안전 파괴하고 맵을 비웁니다.
  - 주소/에셋 로딩은 수행하지 않습니다(입력으로 받은 프리팹만 처리).
- `SpawnService`
  - 문자열 키를 받아 `AddressableAccess.LoadAsset<GameObject>(key)`로 프리팹을 로드하고, 가능하면 `ObjectPoolService`를 사용해 생성/반납합니다.
  - 주의: `ServiceSet`은 “인터페이스만” 등록합니다. 현재 `ObjectPoolService`는 클래스 타입만 노출하므로 `IServiceResolver`로는 조회되지 않을 수 있습니다.
    - 구현: `OnReady()`에서 `Services?.Get<ObjectPoolService>()` 시도 후, `GetComponentInParent<ObjectPoolService>` → `FindObjectOfType<ObjectPoolService>` 순서로 폴백합니다.
    - 권장: 빈 인터페이스(예: `IObjectPool`)를 도입해 인터페이스 기반 등록/조회로 전환하거나, 구성 루트에서 명시 주입을 사용하세요.

### 운영 팁(아바타 스폰 경량 정책)

- 초기에는 엔티티가 존재하지 않는 전제를 따릅니다. 초기 스냅샷 동기화는 수행하지 않습니다.
- 중복 생성 허용 정책에 따라 Added 이벤트마다 1개씩 생성합니다.
- 제거 시 전부 정리: 동일 엔티티 바인딩 아바타를 모두 반납합니다.

## 구현 규칙

- 의존성
  - 교차 서비스 의존은 인터페이스에 의존하고, 조회는 `OnReady()` 1회 + 캐시.
  - 필수 의존 부재 시 초기화 단계에서 로그/에러로 조기 발견. 선택 의존은 폴백 허용.
- 성능
  - 조회 경로는 O(1) 딕셔너리 기반을 유지합니다(`ServiceSet`). 리스트 선형 탐색 추가 금지.
- 스타일/네이밍
  - 컨벤션 문서(`Assets/DoughFramework/AI/Convention/`) 준수. 파라미터는 camelCase(`prefabPath` 대신 Addressables 키는 `key`).
- 안전성
  - `OnDestroy`에서 `Uninitialize()` 보장(중복 호출 가드). 널/파괴된 참조 방어 로직 유지.

## 에디터/도구

- `Editor/FrameworkViewerWindow`: 현재 Realm/Entity/Ability/Service 구조 텍스트 덤프.
- Addressables 관련 에디터 탭은 AddressableAccess 하위에 존재합니다.

## 체크리스트(새 기능 추가 시)

1) 이 레이어가 맞는가? Unity API 의존이 있다면 OK, 아니면 Core로.
2) `Service` 파생인가? 수명주기/Resolver 접근이 필요한 경우에만.
3) 인터페이스 등록 가이드 준수? Resolver로 노출할 것은 인터페이스를 정의/구현.
4) `OnReady()`에서 1회 조회/캐시? 핫패스 조회 금지.
5) Addressables 사용 시 `AddressableAccess`로 일관성 유지.
6) UI는 `UISortingOrder` 부착과 레이어 규칙 준수.
