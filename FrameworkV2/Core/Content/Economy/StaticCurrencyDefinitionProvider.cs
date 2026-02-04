using System;
using System.Collections.Generic;

// 정적 통화 정의를 보관하는 구현체
public sealed class StaticCurrencyDefinitionProvider : ICurrencyDefinitionProvider
{
    readonly Dictionary<int, CurrencyDefinition> definitionById = new();

    // 통화 정의를 등록한다
    public void Register(CurrencyDefinition definition)
    {
        if (definition == null)
        {
            // null 정의는 허용하지 않는다
            throw new ArgumentNullException(nameof(definition));
        }

        var id = definition.Id;
        if (id <= 0)
        {
            // 잘못된 통화 식별자
            throw new ArgumentException("Currency id must be positive.", nameof(definition));
        }

        definitionById[id] = definition;
    }

    // 여러 통화 정의를 일괄 등록한다
    public void RegisterRange(IEnumerable<CurrencyDefinition> definitions)
    {
        if (definitions == null)
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Register(definition);
        }
    }

    // 통화 정의를 조회한다
    public bool TryGetDefinition(int id, out CurrencyDefinition definition)
    {
        if (id <= 0)
        {
            definition = null;
            return false;
        }

        return definitionById.TryGetValue(id, out definition);
    }

    // 통화 id를 검증하고 반환한다
    public bool TryResolveId(int id, out int resolvedId)
    {
        resolvedId = 0;
        if (id <= 0)
        {
            return false;
        }

        if (!definitionById.ContainsKey(id))
        {
            return false;
        }

        resolvedId = id;
        return true;
    }
}
