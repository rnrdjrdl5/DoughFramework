using System;
using System.Collections.Generic;

// 게임 오브젝트 역할을 담당하는 Entity 기본 컴포넌트
public partial class Entity : AbilityHost
{
    public string Id => identity.Id;
    public IReadOnlyList<string> Aliases => aliases.Aliases;

    readonly Identity identity = new();
    readonly AliasSet aliases = new();

    // 별칭을 추가합니다.
    public void AddAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("Alias must be non-empty.", nameof(alias));
        }

        aliases.Add(alias);
    }

    // 별칭을 제거합니다.
    public bool RemoveAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return false;
        }

        return aliases.Remove(alias);
    }

    // 별칭 존재 여부를 확인합니다.
    public bool HasAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return false;
        }

        return aliases.Has(alias);
    }
}
