using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AddressableAutoRegistrar : AssetPostprocessor
{
    public const string ContentsPath = "Assets/Contents/";
    public const string BuiltInFolder = "BuiltIn";
    public const string CDNFolder = "CDN";
    public const string ResourceBaseFolder = "ResourceBase";

    public enum RegisterResult
    {
        Skipped,
        Registered,
        Updated
    }

    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var assetPath in importedAssets)
        {
            TryRegisterAddressable(assetPath);
        }

        foreach (var assetPath in movedAssets)
        {
            TryRegisterAddressable(assetPath);
        }
    }

    public static RegisterResult TryRegisterAddressable(string assetPath)
    {
        if (!IsValidAssetPath(assetPath))
            return RegisterResult.Skipped;

        var pathInfo = ParseAssetPath(assetPath);
        if (pathInfo == null)
            return RegisterResult.Skipped;

        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogWarning("[AddressableAutoRegistrar] Addressable Settings not found.");
            return RegisterResult.Skipped;
        }

        var group = GetOrCreateGroup(settings, pathInfo.Value.contentName, pathInfo.Value.folderType);
        if (group == null)
            return RegisterResult.Skipped;

        var address = GenerateAddress(assetPath, pathInfo.Value.contentName, pathInfo.Value.folderType);
        return RegisterAsset(settings, group, assetPath, address);
    }

    public static bool IsValidAssetPath(string assetPath)
    {
        if (!assetPath.StartsWith(ContentsPath))
            return false;

        if (assetPath.EndsWith(".meta"))
            return false;

        if (AssetDatabase.IsValidFolder(assetPath))
            return false;

        if (assetPath.Contains($"/{ResourceBaseFolder}/"))
            return false;

        return assetPath.Contains($"/{BuiltInFolder}/") || assetPath.Contains($"/{CDNFolder}/");
    }

    static (string contentName, string folderType)? ParseAssetPath(string assetPath)
    {
        var relativePath = assetPath.Substring(ContentsPath.Length);
        var parts = relativePath.Split('/');

        // BuiltIn 또는 CDN 폴더를 찾기
        int folderTypeIndex = -1;
        string folderType = null;

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == BuiltInFolder || parts[i] == CDNFolder)
            {
                folderTypeIndex = i;
                folderType = parts[i];
                break;
            }
        }

        if (folderTypeIndex == -1 || folderTypeIndex == 0)
            return null;

        // BuiltIn/CDN의 바로 위 폴더를 contentName으로 사용
        var contentName = parts[folderTypeIndex - 1];

        return (contentName, folderType);
    }

    static AddressableAssetGroup GetOrCreateGroup(
        AddressableAssetSettings settings,
        string contentName,
        string folderType)
    {
        var groupName = $"{contentName}_{folderType}";
        var group = settings.FindGroup(groupName);

        if (group != null)
            return group;

        group = settings.CreateGroup(groupName, false, false, false, null, typeof(BundledAssetGroupSchema));

        var schema = group.GetSchema<BundledAssetGroupSchema>();
        if (schema != null)
        {
            if (folderType == BuiltInFolder)
            {
                schema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kLocalBuildPath);
                schema.LoadPath.SetVariableByName(settings, AddressableAssetSettings.kLocalLoadPath);
            }
            else if (folderType == CDNFolder)
            {
                schema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kRemoteBuildPath);
                schema.LoadPath.SetVariableByName(settings, AddressableAssetSettings.kRemoteLoadPath);
            }
        }

        Debug.Log($"[AddressableAutoRegistrar] Group created: {groupName}");
        return group;
    }

    static string GenerateAddress(string assetPath, string contentName, string folderType)
    {
        // BuiltIn 또는 CDN 폴더 이후의 경로만 추출
        var builtInIndex = assetPath.IndexOf($"/{BuiltInFolder}/");
        var cdnIndex = assetPath.IndexOf($"/{CDNFolder}/");
        var folderIndex = builtInIndex != -1 ? builtInIndex : cdnIndex;

        if (folderIndex == -1)
            return assetPath;

        var folderTypeLength = folderType == BuiltInFolder ? BuiltInFolder.Length : CDNFolder.Length;
        var addressPath = assetPath.Substring(folderIndex + folderTypeLength + 2); // +2 for the two slashes

        // 확장자 제거
        var lastDotIndex = addressPath.LastIndexOf('.');
        if (lastDotIndex > 0)
            addressPath = addressPath.Substring(0, lastDotIndex);

        return $"{contentName}/{addressPath}";
    }

    static RegisterResult RegisterAsset(
        AddressableAssetSettings settings,
        AddressableAssetGroup group,
        string assetPath,
        string address)
    {
        var guid = AssetDatabase.AssetPathToGUID(assetPath);
        var entry = settings.FindAssetEntry(guid);

        if (entry != null)
        {
            if (entry.address != address)
            {
                entry.address = address;
                Debug.Log($"[AddressableAutoRegistrar] Address updated: {address}");
                return RegisterResult.Updated;
            }
            return RegisterResult.Skipped;
        }

        entry = settings.CreateOrMoveEntry(guid, group);
        entry.address = address;

        Debug.Log($"[AddressableAutoRegistrar] Registered: {address} -> {group.Name}");
        return RegisterResult.Registered;
    }
}
