# Convention_History - 변경 이력

## 2026-01-15

### 최초 작성
- 코드베이스 분석을 통해 코딩 스타일 문서화
- 분석 대상: Assets/Script/ 디렉토리 112개 C# 파일

### 주요 발견 사항
- PascalCase/camelCase 네이밍 일관성 높음
- Allman 스타일 중괄호 사용
- 제네릭 패턴 광범위 활용
- UniTask 비동기 처리 표준
- `// NOTE :`, `// TBD` 주석 형식 사용
- XML 문서 주석 거의 미사용
- 언더스코어 prefix 미사용

## 2026-01-24

### 멤버 변수 선언 규칙 추가
- private 키워드 비표기 원칙 도입(암시적 private)
- 멤버 변수 선언 순서: public → private(표기 생략)
- 컨테이너(Dictionary, List 등) → 기본형(int, string, float 등) 정렬
- public 그룹과 private 그룹 사이 공백 한 줄 유지
 - 멤버 변수 초기화 시 타겟-타입 `new()` 강제(`new Type()` 금지)

### 언더스코어 prefix 금지 명시
- 모든 변수/필드/파라미터에서 선행 언더스코어(`_`) 사용 금지 규칙을 문서에 명시
- FrameworkV2 전역 리네이밍 적용(`_field` → `field`)
