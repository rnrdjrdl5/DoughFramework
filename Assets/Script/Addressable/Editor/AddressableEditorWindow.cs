using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddressableEditorWindow : EditorWindow
{
    List<AddressableTab> tabs = new();
    int selectedTabIndex;
    string[] tabNames;

    [MenuItem("Dough/Addressable")]
    public static void ShowWindow()
    {
        var window = GetWindow<AddressableEditorWindow>();
        window.titleContent = new GUIContent("DoughFramework Addressable");
        window.Show();
    }

    void OnEnable()
    {
        InitializeTabs();
    }

    void InitializeTabs()
    {
        tabs.Clear();
        tabs.Add(new CommonTab());

        tabNames = new string[tabs.Count];
        for (int i = 0; i < tabs.Count; i++)
        {
            tabNames[i] = tabs[i].TabName;
        }
    }

    void OnGUI()
    {
        if (tabs.Count == 0)
            return;

        selectedTabIndex = GUILayout.Toolbar(selectedTabIndex, tabNames);

        EditorGUILayout.Space(10);

        tabs[selectedTabIndex].OnGUI();
    }
}
