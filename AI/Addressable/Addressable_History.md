# Addressable_History - 변경 이력

## 2026-01-15

### 최초 설계
- Addressable 자동 등록 시스템 설계
- 폴더 구조 정의:
  - `Assets/Contents/[컨텐츠명]/BuiltIn/` - Local 빌드
  - `Assets/Contents/[컨텐츠명]/CDN/` - Remote 빌드
  - `Assets/Contents/[컨텐츠명]/ResourceBase/` - 원본 저장소 (Addressable 제외)
- Group 명명 규칙: `[컨텐츠명]_[BuiltIn|CDN]`
- Key 명명 규칙: `[컨텐츠명]/[하위경로]/[Asset명].[확장자]`
- Key에 확장자 포함 결정 (동일 이름 다른 타입 에셋 충돌 방지)

### 구현
- `Assets/Script/Addressable/AddressableAutoRegistrar.cs` 생성
- `Assets/Script/Addressable/DoughFramework.Addressable.Editor.asmdef` 생성
  - Editor 전용 Assembly Definition
  - Unity.Addressables, Unity.Addressables.Editor 참조
- AssetPostprocessor 기반 자동 등록 구현
- 기능:
  - Asset 추가/이동 시 자동 감지
  - Group 자동 생성 (BuiltIn: Local, CDN: Remote)
  - Key 자동 생성 및 등록
  - ResourceBase 폴더 제외 처리

### Editor Window 추가
- `Assets/Script/Addressable/Editor/AddressableEditorWindow.cs` 생성
  - Window 이름: DoughFramework Addressable
  - 메뉴: Dough/Addressable
  - Tab 기반 확장 가능 구조
- `Assets/Script/Addressable/Editor/AddressableTab.cs` 생성
  - Tab 베이스 클래스
- `Assets/Script/Addressable/Editor/CommonTab.cs` 생성
  - Common Tab: 동기화 기능
  - Assets/Contents/ 전체 스캔 → 미등록 에셋 등록, Key 불일치 수정

### 동기화 결과창 추가
- `AddressableAutoRegistrar.RegisterResult` enum 추가 (Skipped, Registered, Updated)
- `TryRegisterAddressable()` 반환 타입 변경 (void → RegisterResult)
- `CommonTab.SyncAllAssets()` 결과 집계 기능 추가
- `ResultWindow.Show()` 호출로 결과 표시
- `DoughFramework.Addressable.Editor.asmdef`에 `DoughFramework.Window.Editor` 참조 추가
