using UnityEngine;

public sealed class ExamplePlayerAvatar : Avatar
{
    [SerializeField] float speed = 5f;

    [HandlesOutputCommand(typeof(PositionUpdated))]
    void OnPositionUpdated(PositionUpdated cmd)
    {
        transform.position = new Vector3(cmd.WorldPosition.X, cmd.WorldPosition.Y, cmd.WorldPosition.Z);
    }

    [HandlesOutputCommand(typeof(HealthChanged))]
    void OnHealthChanged(HealthChanged cmd)
    {
        Debug.Log($"HP: {cmd.Current} / {cmd.Max}");
    }

    void Update()
    {
        var world = Entity;
        if (world == null) return;

        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(x) > 0.001f || Mathf.Abs(z) > 0.001f)
        {
            world.Execute(new MoveCommand
            {
                Direction = new Vec3(x, 0f, z),
                Speed = speed,
                DeltaTime = Time.deltaTime
            });
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            world.Execute(new ApplyDamageCommand { Amount = 5 });
        }
    }
}
