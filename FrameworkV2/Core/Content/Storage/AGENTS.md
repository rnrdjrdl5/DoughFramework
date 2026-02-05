# Storage Content Notes

이 폴더는 Core에서 Save/Load 요청을 외부로 위임하는 저장 Ability를 제공합니다.
저장 데이터(payload)는 Ability 내부 Dictionary에 보관되며, 외부는 Save/Load 이벤트에서
Dictionary를 사용해 저장하거나 갱신합니다. 실제 저장/로드 처리는 TBD 상태입니다.
Key는 기본적으로 타입의 FullName을 사용합니다.
