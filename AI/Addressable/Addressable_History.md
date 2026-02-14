# Addressable_History - Change Log

## 2026-01-15

### Initial Design
- Designed the Addressable auto-registration system
- Defined folder structure:
  - `Assets/Contents/[ContentName]/BuiltIn/` - Local build
  - `Assets/Contents/[ContentName]/CDN/` - Remote build
  - `Assets/Contents/[ContentName]/ResourceBase/` - Source storage (excluded from Addressables)
- Group naming rule: `[ContentName]_[BuiltIn|CDN]`
- Key naming rule: `[ContentName]/[SubPath]/[AssetName].[Extension]`
- Decided to include extensions in Keys (avoid collisions across asset types)

### Implementation
- Created `Assets/Script/Addressable/AddressableAutoRegistrar.cs`
- Created `Assets/Script/Addressable/DoughFramework.Addressable.Editor.asmdef`
  - Editor-only Assembly Definition
  - References Unity.Addressables, Unity.Addressables.Editor
- Implemented AssetPostprocessor-based auto-registration
- Features:
  - Auto-detect asset add/move
  - Auto-create Groups (BuiltIn: Local, CDN: Remote)
  - Auto-generate and register Keys
  - Exclude ResourceBase folder

### Editor Window Added
- Created `Assets/Script/Addressable/Editor/AddressableEditorWindow.cs`
  - Window name: DoughFramework Addressable
  - Menu: Dough/Addressable
  - Tab-based extensible structure
- Created `Assets/Script/Addressable/Editor/AddressableTab.cs`
  - Tab base class
- Created `Assets/Script/Addressable/Editor/CommonTab.cs`
  - Common Tab: sync feature
  - Scan all `Assets/Contents/` → register unregistered assets, fix mismatched Keys

### Sync Result Window Added
- Added `AddressableAutoRegistrar.RegisterResult` enum (Skipped, Registered, Updated)
- Changed `TryRegisterAddressable()` return type (void → RegisterResult)
- Added result aggregation in `CommonTab.SyncAllAssets()`
- Show results via `ResultWindow.Show()`
- Added `DoughFramework.Window.Editor` reference to `DoughFramework.Addressable.Editor.asmdef`
