# Team Ability 요약

## 목적
- `TeamAbility`: Realm 단위 팀 로스터/이벤트 관리
- `TeamMemberAbility`: Entity 단위 팀 정보 보관

## 조립 규칙
- `TeamAbility`는 필요한 Realm에서만 `AddAbility`로 부착한다.
- `TeamMemberAbility`는 Entity 생성 시 외부 조립자가 반드시 부착한다.
- Ability 내부에서는 `Entity`, `Realm` 타입을 참조하지 않는다.

## 이벤트
- `TeamAbility.MemberChanged`는 join/leave/move 모두 발행한다.
- 페이로드에는 `teamId`, `memberId`, `previousTeamId`, `reason`이 포함된다.

## 사용 흐름
1. Realm에 `TeamAbility` 부착
2. Entity에 `TeamMemberAbility` 부착
3. `TeamAbility.TryAddMember/TryMoveMember/TryRemoveMember`로 팀 관리
   - 호출 시 `memberId`와 `TeamMemberAbility`를 전달한다
