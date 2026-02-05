// 아이템 정의를 조회/해석하는 공급자 인터페이스
public interface IItemDefinitionProvider
{
    // 아이템 정의를 조회한다
    bool TryGetDefinition(int id, out ItemDefinition definition);
    // 아이템 id를 검증하고 반환한다
    bool TryResolveId(int id, out int resolvedId);
}
