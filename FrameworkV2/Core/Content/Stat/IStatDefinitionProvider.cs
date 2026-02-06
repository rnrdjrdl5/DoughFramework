public interface IStatDefinitionProvider
{
    bool TryResolveId(int id, out int resolvedId);
    bool TryGetDefinition(int id, out StatDefinition definition);
}
