using System;
using System.Collections.Generic;
using System.Text;

public static class FrameworkStructureText
{
    public sealed class Options
    {
        public bool IncludeAliases = true;
        public bool IncludeAbilities = true;
        public bool IncludeEntities = true;
        public int MaxDepth = int.MaxValue; 
    }

    public static string Dump(Realm root, Options options = null)
    {
        if (root == null)
        {
            return "<no realm>";
        }

        options ??= new Options();
        var sb = new StringBuilder(1024);
        DumpRealm(sb, root, 0, options);
        return sb.ToString();
    }

    static void DumpRealm(StringBuilder sb, Realm realm, int depth, Options opt)
    {
        if (depth > opt.MaxDepth)
        {
            return;
        }

        var indent = Indent(depth);
        sb.Append(indent).Append("Realm ").Append(realm.Id);
        if (opt.IncludeAliases)
        {
            AppendAliases(sb, realm.Aliases);
        }
        sb.Append('\n');

        if (opt.IncludeAbilities && realm.Abilities != null && realm.Abilities.Count > 0)
        {
            sb.Append(indent).Append("  Abilities (Realm): ");
            AppendTypes(sb, realm.Abilities);
            sb.Append('\n');
        }

        if (opt.IncludeEntities)
        {
            var reg = realm.GetAbility<SpawnEntityAbility>();
            if (reg != null)
            {
                DumpEntities(sb, reg, depth + 1, opt);
            }
        }

        var children = realm.Children;
        if (children != null)
        {
            for (int i = 0; i < children.Count; i++)
            {
                DumpRealm(sb, children[i], depth + 1, opt);
            }
        }
    }

    static void DumpEntities(StringBuilder sb, SpawnEntityAbility reg, int depth, Options opt)
    {
        var indent = Indent(depth);

        var locals = reg.LocalEntities;
        if (locals != null && locals.Count > 0)
        {
            sb.Append(indent).Append("LocalEntities (count=").Append(locals.Count).Append(")\n");
            for (int i = 0; i < locals.Count; i++)
            {
                DumpEntity(sb, locals[i], depth + 1, opt, label: "Local");
            }
        }

        var worlds = reg.WorldEntities;
        if (worlds != null && worlds.Count > 0)
        {
            sb.Append(indent).Append("WorldEntities (count=").Append(worlds.Count).Append(")\n");
            for (int i = 0; i < worlds.Count; i++)
            {
                DumpEntity(sb, worlds[i], depth + 1, opt, label: "World");
            }
        }
    }

    static void DumpEntity(StringBuilder sb, Entity e, int depth, Options opt, string label)
    {
        var indent = Indent(depth);

        sb.Append(indent).Append(label).Append("Entity ").Append(e.Id);
        if (opt.IncludeAliases)
        {
            AppendAliases(sb, e.Aliases);
        }
        sb.Append('\n');

        if (opt.IncludeAbilities && e.Abilities != null && e.Abilities.Count > 0)
        {
            sb.Append(indent).Append("  Abilities (Entity): ");
            AppendTypes(sb, e.Abilities);
            sb.Append('\n');
        }
    }

    static void AppendAliases(StringBuilder sb, IReadOnlyList<string> aliases)
    {
        if (aliases == null || aliases.Count == 0)
        {
            return;
        }
        sb.Append(" [aliases: ");
        for (int i = 0; i < aliases.Count; i++)
        {
            if (i > 0) sb.Append(',');
            sb.Append(aliases[i]);
        }
        sb.Append(']');
    }

    static void AppendTypes(StringBuilder sb, IReadOnlyList<Ability> abilities)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(abilities[i]?.GetType()?.Name ?? "<null>");
        }
    }

    static string Indent(int depth)
    {
        if (depth <= 0) return string.Empty;
        return new string(' ', depth * 2);
    }
}
