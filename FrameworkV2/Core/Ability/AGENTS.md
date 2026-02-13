# Ability 디렉토리 요약

이 디렉토리는 Ability 시스템의 기본 구조를 제공합니다.

## 핵심 사항

- `Ability`는 순수 C# 객체이며 `ILifecycle` 기반입니다.
- `AbilityHost`가 Ability 등록/조회/수명 관리를 담당합니다.
- `AbilityAttribute`를 통해 Host 타입에 기본 Ability를 자동 부착할 수 있습니다.
- Ability 수명은 Unity 이벤트가 아니라 Host의 `Initialize/Ready/Uninitialize`로 제어합니다.
- 매 프레임 호출이 필요한 Ability는 `IAbilityTick`을 구현합니다.

## 사용 규칙

- Ability를 추가할 때는 Host의 `AddAbility<T>()` 또는 `AddAbility(Ability)`를 사용합니다.
- Ability는 Host 소유의 GameObject를 주입받습니다.
- 상위 컨텍스트 조회는 `UpstreamAbilityResolver`를 통해 수행합니다.
