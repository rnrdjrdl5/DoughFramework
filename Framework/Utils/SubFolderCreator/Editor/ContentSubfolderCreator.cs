using System.IO;
using UnityEditor;
using UnityEngine;

public static class ContentSubfolderCreator
{
    private const string MenuPath = "Assets/Dough/Create Content Subfolders";

    [MenuItem(MenuPath, false, 2000)]
    private static void CreateContentSubfolders()
    {
        var selectedObject = Selection.activeObject;
        if (selectedObject == null)
        {
            return;
        }

        var selectedPath = AssetDatabase.GetAssetPath(selectedObject);
        if (!AssetDatabase.IsValidFolder(selectedPath))
        {
            return;
        }

        CreateFolderIfMissing(selectedPath, "BuiltIn");
        CreateFolderIfMissing(selectedPath, "CDN");
        CreateFolderIfMissing(selectedPath, "Script");
        CreateFolderIfMissing(selectedPath, "ResourceBase");

        AssetDatabase.Refresh();
    }

    [MenuItem(MenuPath, true)]
    private static bool ValidateCreateContentSubfolders()
    {
        var selectedObject = Selection.activeObject;
        if (selectedObject == null)
        {
            return false;
        }

        var selectedPath = AssetDatabase.GetAssetPath(selectedObject);
        return AssetDatabase.IsValidFolder(selectedPath);
    }

    private static void CreateFolderIfMissing(string parentPath, string folderName)
    {
        var combinedPath = Path.Combine(parentPath, folderName).Replace("\\", "/");
        if (AssetDatabase.IsValidFolder(combinedPath))
        {
            return;
        }

        AssetDatabase.CreateFolder(parentPath, folderName);
        Debug.Log($"Created folder: {combinedPath}");
    }
}
