public sealed class MoveCommand : ICommand
{
    public Vec3 Direction;   // Desired direction
    public float Speed;      // Units per second
    public float DeltaTime;  // Seconds since last step
}

