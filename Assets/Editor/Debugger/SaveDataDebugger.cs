using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class SaveDataDebugger : EditorWindow
{
    private bool _isDebug = false;
    private string debugText = "デバッグモード:オフ";

    [MenuItem("Tools/SaveDataDebugger")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SaveDataDebugger));
    }

    async void OnEnable()
    {
        CharacterCollectionData characterCollectionData = new CharacterCollectionData();
        if (await JsonDataSaveSystem.DataLoadAsync<CharacterCollectionData>(JsonCharacterCollectionDataRepository.SaveDataName) == default &&
            await JsonDataSaveSystem.DataLoadAsync<SupportCardCollectionData>(JsonSupportCardCollectionDataRepository.SaveDataName) == default)
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

        CharacterBaseDataRegistry characterBaseDataRegistry = 
            await AssetsLoader.LoadAssetAsync<CharacterBaseDataRegistry>(AAGCharacterData.kAssets_MasterData_ScriptableObject_CharacterData_CharacterDataRegistry);
        SupportCardDataRegistry supportCardDataRegistry = 
            await AssetsLoader.LoadAssetAsync<SupportCardDataRegistry>(AAGSupportCardData.kAssets_MasterData_ScriptableObject_SupportCard_SupportCardDataRegistry);

        if(characterBaseDataRegistry != null)
        {
            foreach (var characterData in characterBaseDataRegistry.DataHolder)
            {
                characterCollectionData.AddCollection(characterData.CharacterID);
            }

            await JsonDataSaveSystem.DataSave(characterCollectionData, JsonCharacterCollectionDataRepository.SaveDataName);
            Debug.Log("データの生成に成功しました");
        }

        if (supportCardDataRegistry != null)
        {
            foreach (var supportCard in supportCardDataRegistry.DataHolder)
            {
                supportCardCollectionData.AddCollection(supportCard.ID);
            }

            await JsonDataSaveSystem.DataSave(supportCardCollectionData, JsonSupportCardCollectionDataRepository.SaveDataName);
            Debug.Log("データの生成に成功しました");
        }

        
    }

    private void DebugEnd()
    {
        CharacterCollectionData characterCollectionData = new CharacterCollectionData();
        SupportCardCollectionData supportCardCollectionData = new SupportCardCollectionData();

        JsonDataSaveSystem.DataDelete(JsonCharacterCollectionDataRepository.SaveDataName);
        JsonDataSaveSystem.DataDelete(JsonSupportCardCollectionDataRepository.SaveDataName);
    }
}
#endif