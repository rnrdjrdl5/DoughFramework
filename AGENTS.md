# AGENTS

- Read `AI/Convention/Convention.MD` before making code changes.
- Follow the conventions described there.
- `AI/Convention/Convention.MD`의 함수 구조 예시는 초기화 흐름 설계 참고용입니다.
- 오케스트레이션 원칙: 진입점은 고수준 흐름만 정의하고, 하위 함수로 구체 로직을 위임합니다.
- 작업 후 테스트케이스: 변경한 문서/코드에 대해 사용자가 직접 확인할 수 있는 테스트 목록을 제공합니다.
- Ability 작업 규칙: 요청한 Ability 작업 후 해당 디렉토리에 Ability 요약 `AGENTS.md`를 작성합니다.
- Ability 예제 규칙: 요청한 Ability 작업 후 해당 디렉토리에 사용법을 담은 `Example{AbilityName}` 파일을 작성합니다. (접두사 `Example` 필수, 이름은 중복 회피를 위해 자유롭게 결정)
- Example 파일 참고 규칙: 작업 대상 디렉토리에 `Example{Name}.cs`가 있으면 사용법 참고용으로 읽되, 동일 구현으로 복사하지 않습니다.
