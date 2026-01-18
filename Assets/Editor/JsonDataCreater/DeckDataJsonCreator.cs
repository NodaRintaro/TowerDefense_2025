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
            CharacterDeckData characterDeckData = new CharacterDeckData();
            for (int j = 0; j < 5; j++)
            {
                //ここでデッキに入れるキャラクターデータを設定する
                TowerDefenseCharacterData data = new TowerDefenseCharacterData();


                data.InitData((uint)(j + 1), "", 2, 1, 1, 1, 1, "", 1);
                characterDeckData.SetData(j, data);
            }

            _dataBase.SetData(i, characterDeckData);
        }

        Debug.Log(_dataBase.CharacterDeckHolder[0].trainedCharacterDeck[1].CharacterID);
        await JsonDataSaveSystem.DataSaveStreamingAssets(_dataBase, CharacterDeckDataBase.SaveDataName);
    }
}