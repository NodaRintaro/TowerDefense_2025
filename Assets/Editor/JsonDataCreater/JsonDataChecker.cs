using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEditor;

public class JsonDataChecker : EditorWindow
{
    private string _jsonDataName;
    private byte[] _encodeJson;
    private Vector2 _scrollPos;
    private string _json;

    [MenuItem("Tools/JsonDataCreator/JsonDataChecker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(JsonDataChecker));
    }

    async void OnGUI()
    {
        //TextFieldで自動生成するScriptableObjectの名前を入力
        _jsonDataName = EditorGUILayout.TextField("JsonDataの名前", _jsonDataName);

        if (GUILayout.Button("JsonDataの中身を見る"))
        {
            string filePath = Application.streamingAssetsPath + "/" + _jsonDataName + ".json";

            if (File.Exists(filePath))
            {
                // 非同期でバイト配列を読み込む
                _encodeJson = await File.ReadAllBytesAsync(filePath);
                _json = JsonDataSaveSystem.DecodeBytes(_encodeJson);
            }
        }

        // テキストエリアを表示（スクロール可能）
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        // 第2引数で高さを指定。GUILayout.ExpandHeight(true)でウィンドウいっぱいに広げる
        _json = EditorGUILayout.TextArea(_json, GUILayout.ExpandHeight(true));

        EditorGUILayout.EndScrollView();
    }
}
