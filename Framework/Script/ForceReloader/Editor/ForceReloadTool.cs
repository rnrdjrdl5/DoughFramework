using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ForceReloadTool
{
    [MenuItem("Dough/Tools/Force Reload/Reload Domain %#d")] // Ctrl/Cmd + Shift + D
    private static void ForceDomainReload()
    {
        Debug.Log("⚡ Forcing Domain Reload...");
        EditorUtility.RequestScriptReload();
    }

    [MenuItem("Dough/Tools/Force Reload/Reload Scene %#s")] // Ctrl/Cmd + Shift + S
    private static void ForceSceneReload()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.Log("⚡ Forcing Scene Reload (Play Mode)...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("⚡ Forcing Scene Reload (Edit Mode)...");
            EditorSceneManager.OpenScene(SceneManager.GetActiveScene().path);
        }
    }

    [MenuItem("Dough/Tools/Force Reload/Reload Both %#b")] // Ctrl/Cmd + Shift + B
    private static void ForceBothReload()
    {
        Debug.Log("⚡ Forcing Domain + Scene Reload...");
        EditorUtility.RequestScriptReload();
        if (EditorApplication.isPlaying)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            EditorSceneManager.OpenScene(SceneManager.GetActiveScene().path);
    }
}