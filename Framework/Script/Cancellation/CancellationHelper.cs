using System;
using System.Threading;

public struct CancellationHelper : IDisposable
{
    public CancellationToken Token { get; }
    
    CancellationTokenSource source;

    public void Initialize()
    {
        source = new();
    }

    public void Uninitialize()
    {
        Dispose();
    }

    public void Dispose()
    {
        Release();
    }

    public void Release()
    {
        source.Cancel();
        source.Dispose();
    }

    CancellationToken GetCancellationToken()
    {
        if (source == null)
        {
            InitializeSource();
        }

        return source.Token;
    }

    void InitializeSource()
    {
        source = new();
    }
}