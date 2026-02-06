# Stat Content Notes

## Summary
- `StatAbility`: 스탯 값 저장/조회/변경 담당, 계산 공식은 없음.
- `IStatDefinitionProvider`: 스탯 정의(기본값/범위) 및 ID 검증 제공.
- `StatDefinition`: 기본값과 min/max 범위를 보관.
- `StatChangedPayload`: 변경 이벤트 페이로드.

## Usage
- 외부에서 `IStatDefinitionProvider`를 구현하고 `StatAbility.Configure(...)`로 주입합니다.
- 변경 이벤트는 `OnChangedStat`에서 수신합니다.

## Example
- `ExampleStatAbility.cs` 참고.
