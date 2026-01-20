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
        for (int i = 0; i < 5; i++)
        {
            CharacterDeckData deckData = new CharacterDeckData();
            for (int j = 0; j < 12; j++)
            {
                TowerDefenseCharacterData characterData = new TowerDefenseCharacterData();
                characterData.InitData((uint)(j + 1), $"Character{j + 1}", 1, 1, 1, 1, 1, "Warrior", 1);
                deckData.SetData(j, characterData);
            }

            _dataBase.SetData(i, deckData);
            Debug.Log(_dataBase.GetData(i));
        }

        await JsonDataSaveSystem.DataSaveStreamingAssets(_dataBase, JsonCharacterDeckDataRepository.SaveDataName);
    }
}