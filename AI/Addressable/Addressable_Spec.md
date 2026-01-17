# Addressable_Spec - 상세 명세

## 폴더 구조 명세

### 대상 경로
```
Assets/Contents/[컨텐츠명]/BuiltIn/   → Addressable 등록 대상
Assets/Contents/[컨텐츠명]/CDN/       → Addressable 등록 대상
Assets/Contents/[컨텐츠명]/ResourceBase/ → 제외
```

### 컨텐츠 예시
```
Assets/Contents/Combat/BuiltIn/
Assets/Contents/Combat/CDN/
Assets/Contents/Lobby/BuiltIn/
Assets/Contents/Lobby/CDN/
```

## Group 명명 규칙

### 패턴
```
[컨텐츠명]_[폴더타입]
```

### 예시
| 경로 | Group명 |
|------|---------|
| `Assets/Contents/Combat/BuiltIn/` | `Combat_BuiltIn` |
| `Assets/Contents/Combat/CDN/` | `Combat_CDN` |
| `Assets/Contents/Lobby/BuiltIn/` | `Lobby_BuiltIn` |

### Group 설정
| 폴더타입 | Build Path | Load Path |
|----------|------------|-----------|
| BuiltIn | Local | Local |
| CDN | Remote | Remote |

## Key(Address) 명명 규칙

### 패턴
```
[컨텐츠명]/[하위디렉토리]/.../[Asset명].[확장자]
```

### 변환 규칙
1. `Assets/Contents/` 제거
2. `BuiltIn/` 또는 `CDN/` 제거
3. 나머지 경로 + 파일명 + 확장자

### 예시
| 실제 경로 | Key |
|-----------|-----|
| `Assets/Contents/Combat/BuiltIn/Character/Hero.prefab` | `Combat/Character/Hero.prefab` |
| `Assets/Contents/Combat/BuiltIn/Character/Hero.png` | `Combat/Character/Hero.png` |
| `Assets/Contents/Combat/CDN/UI/BattlePanel.prefab` | `Combat/UI/BattlePanel.prefab` |
| `Assets/Contents/Lobby/BuiltIn/NPC/Merchant.prefab` | `Lobby/NPC/Merchant.prefab` |

## 자동 등록 동작

### 트리거
- Asset이 `Assets/Contents/[컨텐츠명]/BuiltIn/` 또는 `CDN/`에 추가될 때

### 처리 순서
1. 경로에서 컨텐츠명, 폴더타입(BuiltIn/CDN) 추출
2. Group 존재 확인
   - 없으면: `[컨텐츠명]_[폴더타입]` Group 생성
   - 있으면: 기존 Group 사용
3. Key 생성 (명명 규칙 적용)
4. Addressable Entry 등록

### 제외 조건
- `ResourceBase/` 하위 Asset
- `.meta` 파일
- 폴더 자체

## 동기화 결과

### RegisterResult enum
| 값 | 설명 |
|-----|------|
| `Skipped` | 이미 등록됨 또는 대상 아님 |
| `Registered` | 새로 등록됨 |
| `Updated` | Key 규칙 불일치로 수정됨 |

### 결과창 표시
```
등록: X개
수정: Y개
스킵: Z개
```
