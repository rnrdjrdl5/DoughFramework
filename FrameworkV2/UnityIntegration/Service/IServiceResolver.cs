public interface IServiceResolver
{
    bool Has<T>() where T : class;
    bool TryGet<T>(out T service) where T : class;
    T Get<T>() where T : class;
}

