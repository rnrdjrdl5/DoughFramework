using System;

public class CommonBuilder : RealmBuilder
{
    public override Realm Build(Realm parent)
    {
        // Create and configure the new Realm instance.
        // Name policy: builder decides; duplicates are allowed.
        var realm = new Realm();

        // Configure realm here (aliases/abilities/entities) as needed.
        // e.g., realm.AddAlias("ingame");
        // e.g., realm.AddAbility(new SomeRealmAbility());

        return realm;
    }
}
