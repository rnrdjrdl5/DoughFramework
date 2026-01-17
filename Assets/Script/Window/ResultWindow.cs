using UnityEditor;
using UnityEngine;

public class ResultWindow : EditorWindow
{
    string title;
    string message;

    public static void Show(string title, string message)
    {
        var window = GetWindow<ResultWindow>(true, title, true);
        window.title = title;
        window.message = message;
        window.minSize = new Vector2(300, 150);
        window.maxSize = new Vector2(500, 300);
        window.ShowUtility();
    }

    void OnGUI()
    {
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

        EditorGUILayout.Space(10);

        EditorGUILayout.HelpBox(message, MessageType.Info);

        EditorGUILayout.Space(10);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("확인", GUILayout.Height(25)))
        {
            Close();
        }

        EditorGUILayout.Space(10);
    }
}
