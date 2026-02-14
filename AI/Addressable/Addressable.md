# Addressable - Overview

DoughFramework Addressable Asset auto-management system.

## Purpose

- Auto-register Addressable Keys when Assets are added
- Auto-create Groups
- Apply consistent Key naming rules

## Folder Structure

```
Assets/Contents/
├── [ContentName]/
│   ├── BuiltIn/        → Addressable Group (Local)
│   ├── CDN/            → Addressable Group (Remote)
│   └── ResourceBase/   → Excluded from Addressables (source storage)
```

### ResourceBase Role

- Not registered to Addressables
- Source resource storage
- Referenced from BuiltIn/CDN

## Auto-Registration Flow

```
Asset added (BuiltIn/ or CDN/)
    ↓
Check Group → auto-create if missing
    ↓
Auto-register Key
```

## Editor Window

- **Menu**: Dough/Addressable
- **Structure**: Tab-based, extensible
- **Common Tab**: Run sync (scan all assets → register/update)
- **Result Window**: Show sync results via `ResultWindow` (register/update/skip counts)

## Related Code

| Location | Role |
|------|------|
| `Assets/Script/Entry/Universe/Universe.Addressable.cs` | Load/Unload entry point |
| `Assets/Script/Addressable/AddressableAutoRegistrar.cs` | Auto-registration (AssetPostprocessor) |
| `Assets/Script/Addressable/Editor/` | Editor Window, Tab implementation |
