using Rooton.Maple.Editor;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FontChanger : EditorWindow
{
    string _scenePath = "Assets/OutGame/Scenes/RaisingSimulation.unity";
    string _fontPath = "Assets/Fonts/NotoSerifJP-Black SDF.asset";

    [MenuItem("Tools/Change All TextMeshPro Fonts")]
    private static void Init()
    {
        CreateWindow<FontChanger>().Show();
    }

    public void OnGUI()
    {
        _scenePath = EditorGUILayout.TextField(_scenePath);
        _fontPath = EditorGUILayout.TextField(_fontPath);

        if (GUILayout.Button("ChangeFonts"))
        {
            ChangeFonts();
        }
    }

    public void ChangeFonts()
    {

        // フォントアセットを読み込みます
        TMP_FontAsset newFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(_fontPath);

        if (newFont == null)
        {
            Debug.LogError("Font asset not found at: " + _fontPath);
            return;
        }

        // 現在のシーンが変更されている場合は、保存を促します
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            bool save = EditorUtility.DisplayDialog("シーンが変更されています", "シーンを保存して続行しますか？", "はい", "いいえ");
            if (save)
            {
                EditorSceneManager.SaveOpenScenes();
            }
            else
            {
                Debug.Log("操作がキャンセルされました。");
                return;
            }
        }

        // シーンを開きます
        var scene = EditorSceneManager.OpenScene(_scenePath, OpenSceneMode.Single);
        if (!scene.IsValid())
        {
            Debug.LogError("Failed to open scene at: " + _scenePath);
            return;
        }

        // アクティブなシーン内のすべてのTextMeshProUGUIコンポーネントを取得します
        TextMeshProUGUI[] textComponents = FindObjectsOfType<TextMeshProUGUI>();

        int changedCount = 0;
        foreach (var textComponent in textComponents)
        {
            // フォントアセットを変更します
            textComponent.font = newFont;
            EditorUtility.SetDirty(textComponent);
            changedCount++;
        }

        if (changedCount > 0)
        {
            // 変更されたシーンを保存します
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"Changed the font for {changedCount} TextMeshProUGUI components in scene: {_scenePath}");
            EditorUtility.DisplayDialog("成功", $"{changedCount}個のTextMeshProUGUIコンポーネントのフォントを変更しました。", "OK");
        }
        else
        {
            Debug.Log("No TextMeshProUGUI components found in the scene.");
            EditorUtility.DisplayDialog("情報", "TextMeshProUGUIコンポーネントが見つかりませんでした。", "OK");
        }
    }
}
