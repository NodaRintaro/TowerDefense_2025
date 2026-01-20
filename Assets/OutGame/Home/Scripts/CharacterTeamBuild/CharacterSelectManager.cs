using System.Collections.Generic;
using System.Linq;
using TowerDefenseDeckData;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// キャラクターを選択する画面の管理を行うクラス
/// </summary>
public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private HomeMenuLifeTimeScope _lifeTimeScope;
    [SerializeField] private CharacterListManager _characterListManager;
    [SerializeField] private TeamBuildManager _teamBuildManager;

    private DataLoadCompleteNotifier _loadingNotifier;
    private JsonCharacterDeckDataRepository _jsonCharacterDeckDataRepository;
    private TowerDefenseCharacterDataBase _towerDefenseCharacterDataBase;
    private AddressableCharacterDataRepository _addressableCharacterDataRepository;

    private TowerDefenseCharacterData _noneData = new TowerDefenseCharacterData();
    private CharacterBaseData _noneBaseData = new CharacterBaseData();
    
    private Dictionary<uint, Image> _selectedCharacterViews = new Dictionary<uint, Image>();

    private void Awake()
    {
        _loadingNotifier = _lifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();
        _jsonCharacterDeckDataRepository = _lifeTimeScope.Container.Resolve<JsonCharacterDeckDataRepository>();
        _towerDefenseCharacterDataBase = _lifeTimeScope.Container.Resolve<TowerDefenseCharacterDataBase>();
        _addressableCharacterDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();
        _noneBaseData.InitData(999, "None", 1, 1, 1, 1, 1, "", 1);
        _noneData.SetBaseData(_noneBaseData);
    }

    private void OnEnable()
    {
        _characterListManager.OnInitialize += Initialize;
        InitializeView();
    }
    
    private void OnDestroy()
    {
        _characterListManager.OnInitialize -= Initialize;
    }

    private void Initialize()
    {
        if(_loadingNotifier.IsDataLoadComplete == false) return;
        // キャラクターリストの各ボタンにクリックイベントを登録する
        Debug.Log(_characterListManager.CharacterListView.Count);
        foreach (var key in _characterListManager.CharacterListView.Keys)
        {
            _characterListManager.CharacterListView[key].GetComponent<CharacterViewButton>().OnClick += () =>
            {
                EditDeck(key);
            };
        }
        
        foreach (var item in _characterListManager.CharacterListView)
        {
            _selectedCharacterViews.Add(item.Key, item.Value.GetComponent<Image>());
        }
    }

    // デッキデータにキャラクターをセットする
    private void EditDeck(uint characterId)
    {
        if (CheckDeck(characterId))
        {
            RemoveDeck(characterId);
        }
        else
        {
            AddDeck(characterId);
        }

        SelectCharacterView(characterId);
    }

    private void AddDeck(uint characterId)
    {
        if (_teamBuildManager.SelectedCharacterIds.Contains(characterId)) return;
        _teamBuildManager.SelectedCharacterIds.Add(characterId);
    }

    private void RemoveDeck(uint characterId)
    {
        _teamBuildManager.SelectedCharacterIds.Remove(characterId);
    }

    //デッキデータを上書き
    public async void SaveDeck()
    {
        if(_loadingNotifier.IsDataLoadComplete == false) return;
        for (int i = 0;
             i < _teamBuildManager.SelectedCharacterIds.Count; i++)
        {
                if (_towerDefenseCharacterDataBase.TryGetCharacterDict(
                        out TowerDefenseCharacterData currentCharacterData,
                        _teamBuildManager.SelectedCharacterIds[i], 0) == false)
                {
                    //トレーニングデータがない場合、基本データを取得する
                    var data = _addressableCharacterDataRepository.GetCharacterData(
                        _teamBuildManager.SelectedCharacterIds[i]);
                    currentCharacterData = new TowerDefenseCharacterData();
                    currentCharacterData.SetBaseData(data);
                }
                else
                {
                    //トレーニングデータがあった場合、最新のデータを取得する
                    _towerDefenseCharacterDataBase.TryGetCharacterDict(out currentCharacterData,
                        _teamBuildManager.SelectedCharacterIds[i],
                        (uint)(_towerDefenseCharacterDataBase
                            .TowerDefenseCharacterDataDict[_teamBuildManager.SelectedCharacterIds[i]].Length - 1));
                }

                _jsonCharacterDeckDataRepository.RepositoryData
                    .CharacterDeckHolder[_teamBuildManager.CurrentSelectDecIndex].SetData(i, currentCharacterData);
        }

        for (int i = _teamBuildManager.SelectedCharacterIds.Count;
             i < CharacterDeckData.DeckLength; i++)
        {
            _jsonCharacterDeckDataRepository.RepositoryData
                .CharacterDeckHolder[_teamBuildManager.CurrentSelectDecIndex].SetData(i, _noneData);
            
        }

        await _jsonCharacterDeckDataRepository.DataSaveAsync();
    }

    // デッキにキャラクターが存在するか確認する
    private bool CheckDeck(uint characterId)
    {
        return _teamBuildManager.SelectedCharacterIds.Contains(characterId);
    }

    //選んだキャラの表示を変える
    private void SelectCharacterView(uint characterId)
    {
        if (_teamBuildManager.SelectedCharacterIds.Contains(characterId))
        {
            _selectedCharacterViews[characterId].color = Color.cyan;
        }
        else
        {
            _selectedCharacterViews[characterId].color = Color.white;
        }
    }

    public void InitializeView()
    {
        foreach (var obj in _selectedCharacterViews.Values)
        {
            obj.color = Color.white;
        }
        foreach (var id in _teamBuildManager.SelectedCharacterIds)
        {
            _selectedCharacterViews[id].color = Color.cyan;
        }
    }
}