using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

public abstract class EntityDataStorageBase
{
    protected static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        Converters = { new StringEnumConverter() },
        NullValueHandling = NullValueHandling.Include,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        FloatParseHandling = FloatParseHandling.Double
    };

    protected string LogPrefix => GetType().Name;

    public virtual string ToJson(Entity entity)
    {
        if (entity == null)
        {
            Debug.LogWarning($"{LogPrefix}.ToJson failed: entity is null.");
            return null;
        }

        var payload = new Dictionary<string, object>();
        foreach (var data in entity.EntityDatas)
        {
            if (data == null)
                continue;

            var type = data.GetType();
            var typeKey = type.FullName;
            if (string.IsNullOrEmpty(typeKey))
            {
                Debug.LogWarning($"{LogPrefix}.ToJson skipped data with empty FullName: {type}");
                continue;
            }

            if (payload.ContainsKey(typeKey))
            {
                Debug.LogWarning($"{LogPrefix}.ToJson duplicate type key detected: {typeKey}. Overwriting.");
            }

            payload[typeKey] = data;
        }

        return JsonConvert.SerializeObject(payload, Formatting.Indented, Settings);
    }

    public virtual bool LoadFromJson(Entity entity, string json)
    {
        if (entity == null)
        {
            Debug.LogWarning($"{LogPrefix}.LoadFromJson failed: entity is null.");
            return false;
        }

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogWarning($"{LogPrefix}.LoadFromJson failed: json is null or empty.");
            return false;
        }

        try
        {
            var root = JObject.Parse(json);
            foreach (var data in entity.EntityDatas)
            {
                if (data == null)
                    continue;

                var typeKey = data.GetType().FullName;
                if (string.IsNullOrEmpty(typeKey))
                    continue;

                if (root.TryGetValue(typeKey, out var token))
                {
                    JsonConvert.PopulateObject(token.ToString(Formatting.None), data, Settings);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{LogPrefix}.LoadFromJson failed: {ex.Message}");
            return false;
        }
    }

    protected string BuildPath(string saveKey, string relativeDirectory = null)
    {
        var fileName = $"{saveKey}.json";
        var basePath = Application.persistentDataPath;
        var directory = string.IsNullOrEmpty(relativeDirectory)
            ? basePath
            : Path.Combine(basePath, relativeDirectory);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return Path.Combine(directory, fileName);
    }
}
