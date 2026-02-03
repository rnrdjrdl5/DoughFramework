using UnityEditor;
using UnityEngine;

public class FrameworkViewerWindow : EditorWindow
{
    GameRoot gameRoot;
    Vector2 scroll;

    bool includeAliases = true;
    bool includeAbilities = true;
    bool includeEntities = true;
    int maxDepth = 1000;

    string dumpText = "";

    [MenuItem("Dough/Framework Viewer")] 
    public static void Open()
    {
        var win = GetWindow<FrameworkViewerWindow>(false, "Framework Viewer", true);
        win.minSize = new Vector2(480, 320);
        win.Show();
    }

    void OnEnable()
    {
        TryAutoFindGameRoot();
        RefreshDump();
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("GameRoot", GUILayout.Width(80));
            using (var cc = new EditorGUI.ChangeCheckScope())
            {
                gameRoot = (GameRoot)EditorGUILayout.ObjectField(gameRoot, typeof(GameRoot), true);
                if (cc.changed)
                {
                    RefreshDump();
                }
            }

            if (GUILayout.Button("Find", GUILayout.Width(60)))
            {
                TryAutoFindGameRoot();
                RefreshDump();
            }
            if (GUILayout.Button("Refresh", GUILayout.Width(70)))
            {
                RefreshDump();
            }
            if (GUILayout.Button("Copy", GUILayout.Width(60)))
            {
                EditorGUIUtility.systemCopyBuffer = dumpText ?? string.Empty;
            }
        }

        EditorGUILayout.Space();
        using (var cc = new EditorGUI.ChangeCheckScope())
        {
            includeAliases = EditorGUILayout.Toggle("Include Aliases", includeAliases);
            includeAbilities = EditorGUILayout.Toggle("Include Abilities", includeAbilities);
            includeEntities = EditorGUILayout.Toggle("Include Entities", includeEntities);
            maxDepth = Mathf.Max(0, EditorGUILayout.IntField("Max Depth", maxDepth));
            if (cc.changed)
            {
                RefreshDump();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dump", EditorStyles.boldLabel);
        using (var scrollScope = new EditorGUILayout.ScrollViewScope(scroll))
        {
            scroll = scrollScope.scrollPosition;
            var style = new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = false
            };
            EditorGUILayout.TextArea(dumpText ?? string.Empty, style, GUILayout.ExpandHeight(true));
        }
    }

    void TryAutoFindGameRoot()
    {
        if (gameRoot != null) return;
        gameRoot = FindObjectOfType<GameRoot>();
    }

    void RefreshDump()
    {
        if (gameRoot == null || gameRoot.RootRealm == null)
        {
            dumpText = "<no GameRoot or RootRealm>";
            return;
        }

        var opt = new FrameworkStructureText.Options
        {
            IncludeAliases = includeAliases,
            IncludeAbilities = includeAbilities,
            IncludeEntities = includeEntities,
            MaxDepth = maxDepth
        };
        dumpText = FrameworkStructureText.Dump(gameRoot.RootRealm, opt);
        Repaint();
    }
}

