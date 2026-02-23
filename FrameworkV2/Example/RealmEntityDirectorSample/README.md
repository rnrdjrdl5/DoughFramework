# RealmEntityDirectorSample

Realm → Entity → Director → Element 흐름과 Ability 연동을 함께 보여주는 예제입니다.

## 구조

- `RealmEntityDirectorSampleUsage.cs`
- `SampleDirector.cs`
- `SampleElements.cs`

## 흐름 요약

1. Realm 생성 후 초기화/준비 처리
2. Realm 하위에 Entity 생성 후 초기화/준비 처리
3. Entity에 `EventAbility` 추가
4. Entity에 `SampleDirector` 추가
5. Director가 Element를 구성하고 Ability를 연결
6. Element가 이벤트를 등록하고 실행 로그 출력

## 실행 방법

- 임의의 진입점에서 `RealmEntityDirectorSampleUsage.Run()` 호출
- 플레이 모드에서 로그 출력 확인

## 로그 기대값

- Element 준비 시 메시지 출력
- EventAbility 이벤트 실행 로그 출력
