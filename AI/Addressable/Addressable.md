# Addressable - 개요

DoughFramework의 Addressable Asset 자동 관리 시스템.

## 목적

- Asset 추가 시 Addressable Key 자동 등록
- Group 자동 생성
- 일관된 Key 명명 규칙 적용

## 폴더 구조

```
Assets/Contents/
├── [컨텐츠명]/
│   ├── BuiltIn/        → Addressable Group (Local)
│   ├── CDN/            → Addressable Group (Remote)
│   └── ResourceBase/   → Addressable 제외 (원본 저장소)
```

### ResourceBase 역할

- Addressable에 등록되지 않음
- 원본 리소스 저장소
- BuiltIn/CDN에서 참조로 연결

## 자동 등록 흐름

```
Asset 추가 (BuiltIn/ 또는 CDN/)
    ↓
Group 확인 → 없으면 자동 생성
    ↓
Key 자동 등록
```

## Editor Window

- **메뉴**: Dough/Addressable
- **구조**: Tab 기반 확장 가능
- **Common Tab**: 동기화 실행 (전체 에셋 스캔 → 등록/수정)
- **결과창**: `ResultWindow`로 동기화 결과 표시 (등록/수정/스킵 수)

## 관련 코드

| 위치 | 역할 |
|------|------|
| `Assets/Script/Entry/Universe/Universe.Addressable.cs` | Load/Unload 진입점 |
| `Assets/Script/Addressable/AddressableAutoRegistrar.cs` | 자동 등록 (AssetPostprocessor) |
| `Assets/Script/Addressable/Editor/` | Editor Window, Tab 구현 |
