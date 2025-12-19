using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using CharacterData;

#if UNITY_EDITOR
public class SaveDataDebugger : EditorWindow
{
    private bool _isDebug = false;
    private string debugText = "デバッグモード:オフ";

    [MenuItem("Tools/SaveDataDebugger")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SaveDataDebugger));
    }

    void OnEnable()
    {
        CharacterCollectionData characterCollectionData = new CharacterCollectionData();
        if (JsonDataSaveSystem.DataLoad<CharacterCollectionData>("SaveData_" + "CharacterCollectionData") == default)
        {
            _isDebug = false;
        }
        else
        {
            _isDebug = true;
        }
    }

    void OnGUI()
    {
        if (_isDebug)
        {
            debugText = "デバッグモード:オン";
        }
        else
        {
            debugText = "デバッグモード:オフ";
        }

        if (GUILayout.Button(debugText))
        {
            if (!_isDebug)
            {
                DebugStart();
                _isDebug = true;
            }
            else
            {
                DebugEnd();
                _isDebug = false;
            }
        }
    }

    private async void DebugStart()
    {
        CharacterCollectionData characterCollectionData = new CharacterCollectionData();
        SupportCardCollectionData supportCardCollectionData = new SupportCardCollectionData();
        AAGSupportCardData supportCardDataPath = new AAGSupportCardData();
        string charaPath = AAGCharacterData.kAssets_MasterData_CharacterData_CharacterData;
        string supportCardPath = AAGSupportCardData.kAssets_MasterData_SupportCard_SupportCard;

        CharacterBaseDataRegistry characterBaseDataRegistry = await AssetsLoader.LoadAssetAsync<CharacterBaseDataRegistry>(charaPath);
        SupportCardDataRegistry supportCardDataRegistry = await AssetsLoader.LoadAssetAsync<SupportCardDataRegistry>(supportCardPath);

        foreach(var characterData in characterBaseDataRegistry.DataHolder)
        {
            characterCollectionData.AddCollection(characterData.CharacterID);
        }
        foreach(var supportCard in supportCardDataRegistry.DataHolder)
        {
            supportCardCollectionData.AddCollection(supportCard.ID);
        }

        characterCollectionData.DataSave();
        supportCardCollectionData.DataSave();
    }

    private void DebugEnd()
    {
        CharacterCollectionData characterCollectionData = new CharacterCollectionData();
        SupportCardCollectionData supportCardCollectionData = new SupportCardCollectionData();

        characterCollectionData.DataDelete();
        supportCardCollectionData.DataDelete();
    }
}
#endif