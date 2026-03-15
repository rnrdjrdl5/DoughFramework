public enum MessageOriginType
{
    UI,
    Entity,
    EntityData,
    Popup
}

public interface IMessageOrigin
{
    MessageOriginType Origin { get; }
}
