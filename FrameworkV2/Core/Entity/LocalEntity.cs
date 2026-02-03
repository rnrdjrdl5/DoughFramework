using System;

public class LocalEntity : Entity
{
    public static LocalEntity Create()
    {
        var localEntity =  new LocalEntity();
        return localEntity;
    }
    
    public LocalEntity() : base()
    {
    }
}
