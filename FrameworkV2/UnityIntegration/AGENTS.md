# UnityIntegration Notes

이 문서는 `FrameworkV2/UnityIntegration` 범위의 현재 구조/의도/사용 패턴을 요약합니다.

## 핵심

- Core의 Realm/Entity/Ability는 `MonoBehaviour` 기반입니다.
- UnityIntegration은 서비스/에디터/입력/유틸 계층을 담당합니다.
- 서비스 기능은 Ability로 제공되며 RootRealm에 부착해 사용합니다.

## 서비스(Ability)

- `UIAbility(IUIAbility)`: Realm별 Core/Panel/Modal과 전역 System UI 관리.
- `InputAbility(IInputAbility)`: UI 히트 여부 판단 후 월드 입력 라우팅.
- `SpawnAbility(ISpawnAbility)`: 프리팹 스폰 및 UI 루트 부착 제공.
- `ObjectPoolAbility`: 간단 오브젝트 풀.

## UI

- UI 루트는 Realm 기준으로 분리됩니다.
- `UIAbility`는 Panel/Modal 스택 정렬과 활성 Realm 상태를 관리합니다.

## 입력

- `InputAbility`는 UI 히트 여부를 판단하여 월드 입력을 라우팅합니다.

## GameRoot

- 루트 Realm 생성, RootAbility 부착, `Initialize/Ready` 호출을 담당합니다.

## 에디터

- `FrameworkViewerWindow`: `Inspection/FrameworkStructureText`를 이용해 구조 텍스트를 표시합니다.
