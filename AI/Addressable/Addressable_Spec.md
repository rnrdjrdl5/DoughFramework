# Addressable_Spec - Detailed Specification

## Folder Structure Specification

### Target Paths
```
Assets/Contents/[ContentName]/BuiltIn/   → Addressable registration target
Assets/Contents/[ContentName]/CDN/       → Addressable registration target
Assets/Contents/[ContentName]/ResourceBase/ → Excluded
```

### Content Examples
```
Assets/Contents/Combat/BuiltIn/
Assets/Contents/Combat/CDN/
Assets/Contents/Lobby/BuiltIn/
Assets/Contents/Lobby/CDN/
```

## Group Naming Rules

### Pattern
```
[ContentName]_[FolderType]
```

### Examples
| Path | Group Name |
|------|---------|
| `Assets/Contents/Combat/BuiltIn/` | `Combat_BuiltIn` |
| `Assets/Contents/Combat/CDN/` | `Combat_CDN` |
| `Assets/Contents/Lobby/BuiltIn/` | `Lobby_BuiltIn` |

### Group Settings
| Folder Type | Build Path | Load Path |
|----------|------------|-----------|
| BuiltIn | Local | Local |
| CDN | Remote | Remote |

## Key (Address) Naming Rules

### Pattern
```
[ContentName]/[SubDirectory]/.../[AssetName].[Extension]
```

### Conversion Rules
1. Remove `Assets/Contents/`
2. Remove `BuiltIn/` or `CDN/`
3. Use the remaining path + file name + extension

### Examples
| Actual Path | Key |
|-----------|-----|
| `Assets/Contents/Combat/BuiltIn/Character/Hero.prefab` | `Combat/Character/Hero.prefab` |
| `Assets/Contents/Combat/BuiltIn/Character/Hero.png` | `Combat/Character/Hero.png` |
| `Assets/Contents/Combat/CDN/UI/BattlePanel.prefab` | `Combat/UI/BattlePanel.prefab` |
| `Assets/Contents/Lobby/BuiltIn/NPC/Merchant.prefab` | `Lobby/NPC/Merchant.prefab` |

## Auto-Registration Behavior

### Trigger
- When an Asset is added under `Assets/Contents/[ContentName]/BuiltIn/` or `CDN/`

### Processing Order
1. Extract content name and folder type (BuiltIn/CDN) from the path
2. Check whether the Group exists
   - If missing: create `[ContentName]_[FolderType]` Group
   - If present: use the existing Group
3. Generate Key (apply naming rules)
4. Register Addressable Entry

### Exclusion Conditions
- Assets under `ResourceBase/`
- `.meta` files
- Folders themselves

## Sync Results

### RegisterResult enum
| Value | Description |
|-----|------|
| `Skipped` | Already registered or not a target |
| `Registered` | Newly registered |
| `Updated` | Fixed due to Key rule mismatch |

### Result Window Display
```
Registered: X
Updated: Y
Skipped: Z
```
