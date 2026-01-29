using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

public class TowerDefenseJsonDataCreator : EditorWindow
{
    private TowerDefenseCharacterDataBase _dataBase;
    private TowerDefenseCharacterData _towerDefenseCharacterData;

    private uint _characterId = 0;

    private uint _addPowerParam = 0;
    private uint _addIntelligenceParam = 0;
    private uint _addPhysicalParam = 0;
    private uint _addSpeedParam = 0;

    private const uint _spaceSize = 10;

    private JsonTowerDefenseCharacterDataRepository _towerDefenseCharacterDataRepository = new();
    private AddressableCharacterDataRepository _addressableCharacterDataRepository = new();

    [MenuItem("Tools/JsonDataCreator/TowerDefenseJsonDataCreator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TowerDefenseJsonDataCreator));
    }

    private async void OnEnable()
    {
        _dataBase = await JsonDataSaveSystem.DataLoadAsyncStreamingAssetsAsync<TowerDefenseCharacterDataBase>(JsonTowerDefenseCharacterDataRepository.SaveDataName);
        if (_dataBase == default) _dataBase = new TowerDefenseCharacterDataBase();

        _towerDefenseCharacterData = new TowerDefenseCharacterData();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        await _towerDefenseCharacterDataRepository.DataLoadAsync(cancellationTokenSource.Token);
        await _addressableCharacterDataRepository.DataLoadAsync(cancellationTokenSource.Token);
    }

    private void OnDisable()
    {
        _towerDefenseCharacterData = null;
    }

    async void OnGUI()
    {
        //TextFieldで自動生成するScriptableObjectの名前を入力
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("自動生成するキャラクターのID");
        _characterId = (uint)EditorGUILayout.IntField("BaseCharacterID", (int)_characterId);

        EditorGUILayout.Space(_spaceSize);
        _addPowerParam = (uint)EditorGUILayout.IntField("筋力の強化値", (int)_addPowerParam);

        EditorGUILayout.Space(_spaceSize);
        _addIntelligenceParam = (uint)EditorGUILayout.IntField("知力の強化値", (int)_addIntelligenceParam);

        EditorGUILayout.Space(_spaceSize);
        _addPhysicalParam = (uint)EditorGUILayout.IntField("体力の強化値", (int)_addPhysicalParam);

        EditorGUILayout.Space(_spaceSize);
        _addSpeedParam = (uint)EditorGUILayout.IntField("素早さの強化値", (int)_addSpeedParam);

        if (GUILayout.Button("新たなデータを追加"))
        {
            _towerDefenseCharacterData = new();
            _towerDefenseCharacterData.SetBaseData(_addressableCharacterDataRepository.GetCharacterDataByID(_characterId));
            _towerDefenseCharacterData.SetCharacterTrainedParameterData(_addPhysicalParam, _addPowerParam, _addIntelligenceParam, _addSpeedParam);
            
            _towerDefenseCharacterData.SetCharacterRank(
                RankCalculator.GetCurrentRank(_towerDefenseCharacterData.TotalParameter, TowerDefenseCharacterRankRateData.RankRateDict));
            
            if(!_dataBase.TryAddCharacterDict(_characterId, _towerDefenseCharacterData))
                Debug.Log("データの追加に失敗しました");
            else Debug.Log("データの追加に成功しました");


            _characterId = 0;
            _addPowerParam = 0;
            _addIntelligenceParam = 0;
            _addPhysicalParam = 0;
            _addSpeedParam = 0;
        }

        if (GUILayout.Button("データをセーブ"))
        {
            await CreateJsonData();
            Debug.Log("データの保存に成功しました");
        }
    }

    private async UniTask CreateJsonData()
    {
        await JsonDataSaveSystem.DataSaveStreamingAssetsAsync(_dataBase, JsonTowerDefenseCharacterDataRepository.SaveDataName);
    }
}
