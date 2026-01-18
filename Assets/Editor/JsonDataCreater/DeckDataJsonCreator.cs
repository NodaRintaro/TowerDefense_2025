using Cysharp.Threading.Tasks;
using TowerDefenseDeckData;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class DeckDataJsonCreator : EditorWindow
{
    private CharacterDeckDataBase _dataBase = new();

    [MenuItem("Tools/JsonDataCreator/CharacterDeckDataJsonDataCreator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DeckDataJsonCreator));
    }

    private void OnEnable()
    {
        
    }

    async void OnGUI()
    {
        if (GUILayout.Button("データをセーブ"))
        {
            await CreateJsonData();
        }
    }

    private async UniTask CreateJsonData()
    {
        await JsonDataSaveSystem.DataSaveStreamingAssets(_dataBase, CharacterDeckDataBase.SaveDataName);
    }
}
