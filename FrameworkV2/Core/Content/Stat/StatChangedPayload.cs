public readonly struct StatChangedPayload
{
    public int Id { get; }
    public int Previous { get; }
    public int Delta { get; }
    public int Current { get; }

    public StatChangedPayload(int id, int previous, int delta, int current)
    {
        Id = id;
        Previous = previous;
        Delta = delta;
        Current = current;
    }
}
