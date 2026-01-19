using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    private int _currentSelectIndex = 0;

    private void Awake()
    {
        _loadingNotifier = _lifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();
        _jsonCharacterDeckDataRepository = _lifeTimeScope.Container.Resolve<JsonCharacterDeckDataRepository>();
        _towerDefenseCharacterDataBase = _lifeTimeScope.Container.Resolve<TowerDefenseCharacterDataBase>();
        _addressableCharacterDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();
    }

    private void OnEnable()
    {
        _loadingNotifier.OnDataLoadComplete += Initialize;
    }

    private void OnDestroy()
    {
        _loadingNotifier.OnDataLoadComplete -= Initialize;
    }

    private void Initialize()
    {
        // キャラクターリストの各ボタンにクリックイベントを登録する
        foreach (var key in _characterListManager.CharacterListView.Keys)
        {
            _characterListManager.CharacterListView[key].GetComponent<CharacterViewButton>().OnClick += () =>
            {
                SetDeckIndex(key);
            };
        }
        
        
    }

    // デッキデータにキャラクターをセットする
    private void SetDeckIndex(uint characterId)
    {
        TowerDefenseCharacterData currentCharacterData;
        if (_towerDefenseCharacterDataBase.TryGetCharacterDict(out currentCharacterData, characterId, 0) == false)
        {
            currentCharacterData =
                _addressableCharacterDataRepository.GetCharacterData(characterId) as TowerDefenseCharacterData;
        }
        else
        {
            _towerDefenseCharacterDataBase.TryGetCharacterDict(out currentCharacterData, characterId,
                (uint)(_towerDefenseCharacterDataBase.TowerDefenseCharacterDataDict[characterId].Length - 1));
        }

        // _jsonCharacterDeckDataRepository.RepositoryData
        //         .CharacterDeckHolder[_teamBuildManager.CurrentSelectDecIndex].trainedCharacterDeck[index] =
        //     currentCharacterData;
    }

    private void SelectCharacterView()
    {
        
    }
}