public interface IDefinitionProvider<TDefinition>
{
    bool TryResolveId(int id, out int resolvedId);
    bool TryGetDefinition(int id, out TDefinition definition);
}
