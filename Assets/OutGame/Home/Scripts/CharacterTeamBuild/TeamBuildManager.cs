using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using TowerDefenseDeckData;

/// <summary>
/// チーム編成を表示するクラス
/// </summary>
public class TeamBuildManager : MonoBehaviour
{
    [SerializeField] private HomeMenuLifeTimeScope _homeMenuLifeTimeScope;
    [SerializeField] private GameObject _selectButtonParent;
    [SerializeField] private GameObject _selectDeckParent;
    [SerializeField] private Sprite _emptySprite;

    private DataLoadCompleteNotifier _loadingNotifier;
    private JsonCharacterDeckDataRepository _jsonCharacterDeckDataRepository;
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    private AddressableCharacterDataRepository _addressableCharacterDataRepository;

    private List<uint> _selectedCharacterIds = new List<uint>();
    private List<Image> _selectImageList = new List<Image>();
    private List<Image> _selectDeckImageList = new List<Image>();

    private int _maxDeckIndex = 5;
    private int _currentSelectDecIndex = 0;
    public int CurrentSelectDecIndex => _currentSelectDecIndex;

    public List<uint> SelectedCharacterIds => _selectedCharacterIds;
    public List<Image> SelectImageList => _selectImageList;

    private async void Awake()
    {
        _loadingNotifier = _homeMenuLifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();
        _jsonCharacterDeckDataRepository = _homeMenuLifeTimeScope.Container.Resolve<JsonCharacterDeckDataRepository>();
        _addressableCharacterImageDataRepository =
            _homeMenuLifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
        _addressableCharacterDataRepository =
            _homeMenuLifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await _jsonCharacterDeckDataRepository.DataLoadAsync(cancellationTokenSource.Token);
        foreach (Transform selectButton in _selectButtonParent.transform)
        {
            //Debug.Log("selectButton:" + selectButton.name);
            _selectImageList.Add(selectButton.GetComponent<Image>());
        }

        foreach (Transform item in _selectDeckParent.transform)
        {
            _selectDeckImageList.Add(item.GetComponent<Image>());
        }

        TeamBuildView();
    }

    private void OnEnable()
    {
        //_loadingNotifier.OnDataLoadComplete += TeamBuildView;
    }


    private void OnDestroy()
    {
        _loadingNotifier.OnDataLoadComplete -= TeamBuildView;
    }

    private void Initialize()
    {
        if (_loadingNotifier.IsDataLoadComplete == false) return;
        _selectedCharacterIds.Clear();
        foreach (TowerDefenseCharacterData t in _jsonCharacterDeckDataRepository.RepositoryData
                     .CharacterDeckHolder[CurrentSelectDecIndex].trainedCharacterDeck)
        {
            if (_addressableCharacterDataRepository.RepositoryData.DataHolder.Any(x => x.CharacterID == t.CharacterID))
                _selectedCharacterIds.Add(t.CharacterID);
        }
    }

    // チーム編成の表示
    public void TeamBuildView()
    {
        if (_loadingNotifier.IsDataLoadComplete == false) return;
        Initialize();
        //編成キャラクターの表示
        Debug.Log("_selectedCharacterIds.Count:" + _selectedCharacterIds.Count);
        Debug.Log("_currentSelectDecIndex:" + _selectImageList.Count);
        for (int i = 0; i < _selectedCharacterIds.Count; i++)
        {
            _selectImageList[i].sprite =
                _addressableCharacterImageDataRepository.GetSprite(_selectedCharacterIds[i],
                    CharacterSpriteType.MiniCard);
        }

        for (int i = _selectedCharacterIds.Count; i < CharacterDeckData.DeckLength; i++)
        {
            _selectImageList[i].sprite = _emptySprite;
        }
        
        DeckDataLoader.SetDeck(_jsonCharacterDeckDataRepository.RepositoryData.GetData(_currentSelectDecIndex));
    }

    public void ChangeDeck(int num)
    {
        _currentSelectDecIndex = num;
        SelectDeckView();
    }

    public void PlusDeckIndex()
    {
        _currentSelectDecIndex = (_currentSelectDecIndex + 1) % _maxDeckIndex;
        SelectDeckView();
    }

    public void MinusDeckIndex()
    {
        _currentSelectDecIndex = (_currentSelectDecIndex - 1 + _maxDeckIndex) % _maxDeckIndex;
        SelectDeckView();
    }

    private void SelectDeckView()
    {
        if (_loadingNotifier.IsDataLoadComplete == false) return;
        for (int i = 0; i < _maxDeckIndex; i++)
        {
            if (i == _currentSelectDecIndex)
            {
                _selectDeckImageList[i].DOFade(1f, 0f);
            }
            else
            {
                _selectDeckImageList[i].DOFade(0f, 0f);
            }
        }

        TeamBuildView();
    }
}