using UnityEngine;

public enum Direction
{
    None,
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft,
}

public static class DirectionExtensions
{
    public static Vector2 ToVector2(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector2.up,
            Direction.UpRight => new Vector2(1f, 1f),
            Direction.Right => Vector2.right,
            Direction.DownRight => new Vector2(1f, -1f),
            Direction.Down => Vector2.down,
            Direction.DownLeft => new Vector2(-1f, -1f),
            Direction.Left => Vector2.left,
            Direction.UpLeft => new Vector2(-1f, 1f),
            _ => Vector2.zero,
        };
    }

    public static float ToAngle(this Direction direction)
    {
        return direction.ToVector2().ToAngle();
    }

    public static float ToAngle(this Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
