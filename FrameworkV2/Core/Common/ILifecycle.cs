public interface ILifecycle
{
    void Initialize();
    void Ready();
    void Uninitialize();

    bool IsInitialized { get; }
    bool IsReady { get; }
}

