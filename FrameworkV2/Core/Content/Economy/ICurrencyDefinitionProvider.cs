// 통화 정의를 조회/해석하는 공급자 인터페이스
public interface ICurrencyDefinitionProvider
{
    // 통화 정의를 조회한다
    bool TryGetDefinition(int id, out CurrencyDefinition definition);
    // 통화 id를 검증하고 반환한다
    bool TryResolveId(int id, out int resolvedId);
}
