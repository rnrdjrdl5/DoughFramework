using System.IO;
using UnityEditor;
using UnityEngine;

public class CommonTab : AddressableTab
{
    public override string TabName => "Common";

    public override void OnGUI()
    {
        EditorGUILayout.LabelField("Addressable 동기화", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox(
            "Assets/Contents/ 하위의 모든 에셋을 스캔하여\n" +
            "미등록 에셋은 등록하고, Key 규칙이 다른 에셋은 수정합니다.",
            MessageType.Info);
        EditorGUILayout.Space(10);

        if (GUILayout.Button("동기화 실행", GUILayout.Height(30)))
        {
            SyncAllAssets();
        }
    }

    void SyncAllAssets()
    {
        var contentsPath = AddressableAutoRegistrar.ContentsPath;

        if (!Directory.Exists(contentsPath))
        {
            ResultWindow.Show("동기화 실패", $"Contents 폴더가 존재하지 않습니다.\n{contentsPath}");
            return;
        }

        int registeredCount = 0;
        int updatedCount = 0;
        int skippedCount = 0;

        var assetGuids = AssetDatabase.FindAssets("", new[] { contentsPath });

        foreach (var guid in assetGuids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var result = AddressableAutoRegistrar.TryRegisterAddressable(assetPath);

            switch (result)
            {
                case AddressableAutoRegistrar.RegisterResult.Registered:
                    registeredCount++;
                    break;
                case AddressableAutoRegistrar.RegisterResult.Updated:
                    updatedCount++;
                    break;
                case AddressableAutoRegistrar.RegisterResult.Skipped:
                    skippedCount++;
                    break;
            }
        }

        AssetDatabase.SaveAssets();

        var message = $"등록: {registeredCount}개\n수정: {updatedCount}개\n스킵: {skippedCount}개";
        ResultWindow.Show("동기화 완료", message);
    }
}
