using System;
using System.Collections.Generic;
using System.Text;

// 현재 Realm/Entity/Ability 구조를 텍스트로 덤프합니다.
public static class FrameworkStructureText
{
    // 덤프 옵션 설정을 제공합니다.
    public sealed class Options
    {
        public bool IncludeAliases = true;
        public bool IncludeAbilities = true;
        public bool IncludeEntities = true;
        public int MaxDepth = int.MaxValue; 
    }

    // Realm 트리 구조를 문자열로 반환합니다.
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

    // 단일 Realm을 텍스트로 기록합니다.
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

    // Realm의 Entity 목록을 기록합니다.
    static void DumpEntities(StringBuilder sb, SpawnEntityAbility reg, int depth, Options opt)
    {
        var indent = Indent(depth);

        var list = reg.Entities;
        if (list != null && list.Count > 0)
        {
            sb.Append(indent).Append("Entities (count=").Append(list.Count).Append(")\n");
            for (int i = 0; i < list.Count; i++)
            {
                DumpEntity(sb, list[i], depth + 1, opt);
            }
        }
    }

    // 단일 Entity를 기록합니다.
    static void DumpEntity(StringBuilder sb, Entity e, int depth, Options opt)
    {
        var indent = Indent(depth);

        sb.Append(indent).Append("Entity ").Append(e.Id);
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

    // 별칭 정보를 출력합니다.
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

    // Ability 타입명을 출력합니다.
    static void AppendTypes(StringBuilder sb, IReadOnlyList<Ability> abilities)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(abilities[i]?.GetType()?.Name ?? "<null>");
        }
    }

    // 들여쓰기 문자열을 생성합니다.
    static string Indent(int depth)
    {
        if (depth <= 0) return string.Empty;
        return new string(' ', depth * 2);
    }
}
