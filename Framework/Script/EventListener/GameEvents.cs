
public abstract class GameEvent<T> where T : class
{
    public static string Prefix => typeof(T).Name;

    public static int EventCode(string eventName) => $"{Prefix}_{eventName}".GetHashCode();
}

public class CommonEvent : GameEvent<CommonEvent>
{

}