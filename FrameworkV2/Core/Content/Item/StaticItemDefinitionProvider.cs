using System;
using System.Collections.Generic;

// 정적 아이템 정의를 보관하는 구현체
public sealed class StaticItemDefinitionProvider : IItemDefinitionProvider
{
    readonly Dictionary<int, ItemDefinition> definitionById = new();

    // 아이템 정의를 등록한다
    public void Register(ItemDefinition definition)
    {
        if (definition == null)
        {
            // null 정의는 허용하지 않는다
            throw new ArgumentNullException(nameof(definition));
        }

        var id = definition.Id;
        if (id <= 0)
        {
            // 잘못된 아이템 식별자
            throw new ArgumentException("Item id must be positive.", nameof(definition));
        }

        definitionById[id] = definition;
    }

    // 여러 아이템 정의를 일괄 등록한다
    public void RegisterRange(IEnumerable<ItemDefinition> definitions)
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

    // 아이템 정의를 조회한다
    public bool TryGetDefinition(int id, out ItemDefinition definition)
    {
        if (id <= 0)
        {
            definition = null;
            return false;
        }

        return definitionById.TryGetValue(id, out definition);
    }

    // 아이템 id를 검증하고 반환한다
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
