using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class TowerDefenseJsonDataCreator : EditorWindow
{
    private TowerDefenseCharacterData _towerDefenseCharacterData;

    private uint _characterId = 0;

    private uint _addPowerParam = 0;
    private uint _addIntelligenceParam = 0;
    private uint _addPhysicalParam = 0;
    private uint _addSpeedParam = 0;

    private const uint _spaceSize = 10;

    private JsonTowerDefenseCharacterDataRepository _towerDefenseCharacterDataRepository;
    private AddressableCharacterDataRepository _addressableCharacterDataRepository;

    [MenuItem("Tools/JsonDataCreator/TowerDefenseJsonDataCreator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TowerDefenseJsonDataCreator));
    }

    private async void OnEnable()
    {
        _towerDefenseCharacterData = new TowerDefenseCharacterData();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        await _towerDefenseCharacterDataRepository.DataLoadAsync(cancellationTokenSource.Token);
        await _addressableCharacterDataRepository.DataLoadAsync(cancellationTokenSource.Token);
    }

    private void OnDisable()
    {
        _towerDefenseCharacterData = null;
    }

    void OnGUI()
    {
        //TextFieldで自動生成するScriptableObjectの名前を入力
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("自動生成するキャラクターのID");
        _characterId = (uint)EditorGUILayout.IntField("BaseCharacterID", (int)_characterId);

        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("筋力の強化値");
        _addPowerParam = (uint)EditorGUILayout.IntField("筋力の強化値", (int)_addPowerParam);

        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("知力の強化値");
        _addIntelligenceParam = (uint)EditorGUILayout.IntField("知力の強化値", (int)_addIntelligenceParam);

        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("体力の強化値");
        _addPhysicalParam = (uint)EditorGUILayout.IntField("体力の強化値", (int)_addPhysicalParam);

        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("素早さの強化値");
        _addSpeedParam = (uint)EditorGUILayout.IntField("素早さの強化値", (int)_addSpeedParam);
    }

    private void CreateJsonData()
    {
        
    }
}
