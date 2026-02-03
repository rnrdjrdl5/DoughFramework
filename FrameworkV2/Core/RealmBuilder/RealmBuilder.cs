using System;

public abstract class RealmBuilder
{
    // Creates and configures a new Realm. Do not attach to parent here.
    public abstract Realm Build(Realm parent);
}

