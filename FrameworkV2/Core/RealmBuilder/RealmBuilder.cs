using UnityEngine;

// Realm 생성 규약을 정의하는 빌더 베이스
public abstract class RealmBuilder
{
    // 새로운 Realm을 생성하고 설정합니다(부모 부착 금지).
    public abstract Realm Build(Realm parent);
}
