# Team Ability Summary

## Purpose
- `TeamAbility`: Realm-level team roster/event management
- `TeamMemberAbility`: Entity-level team data storage

## Assembly Rules
- Attach `TeamAbility` via `AddAbility` only in the Realms that need it.
- When creating an Entity, the external assembler must attach `TeamMemberAbility`.
- Inside Abilities, do not reference `Entity` or `Realm` types.

## Events
- `TeamAbility.MemberChanged` fires for join/leave/move.
- Payload includes `teamId`, `memberId`, `previousTeamId`, `reason`.

## Usage Flow
1. Attach `TeamAbility` to the Realm.
2. Attach `TeamMemberAbility` to the Entity.
3. Manage teams with `TeamAbility.TryAddMember/TryMoveMember/TryRemoveMember`.
   - Pass `memberId` and `TeamMemberAbility` when calling.
