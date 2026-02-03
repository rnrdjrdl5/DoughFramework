using System;
using System.Collections.Generic;

public class AliasSet
{
    public IReadOnlyList<string> Aliases => aliases;
    
    readonly List<string> aliases = new();

    public void Add(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("Alias must be non-empty.", nameof(alias));
        }

        aliases.Add(alias);
    }

    public bool Remove(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return false;
        }

        return aliases.Remove(alias);
    }

    public bool Has(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return false;
        }

        return aliases.Contains(alias);
    }
}
