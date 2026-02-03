// Simple headless usage demonstrating WorldEntity + HeadlessPresence
public static class HeadlessExampleUsage
{
    public static (Vec3? pos, (int current, int max)? hp) Run()
    {
        var entity = new WorldEntity();
        // Attach sample abilities that emit output commands
        entity.AddAbility(new OutputMovementAbility());
        entity.AddAbility(new OutputHealthAbility());

        // Bind a headless presence to collect outputs
        var presence = new ExampleHeadlessPresence();
        presence.Initialize();
        presence.BindEntity(entity);
        presence.Ready();

        // Issue some input commands to the entity
        entity.Execute(new MoveCommand { Direction = new Vec3(1, 0, 0), Speed = 5f, DeltaTime = 1.0f });
        entity.Execute(new ApplyDamageCommand { Amount = 7 });

        // Ensure outputs are drained in non-Unity loop
        presence.Tick();

        // Return captured outputs for testing
        return (presence.LastPosition, presence.LastHealth);
    }
}
