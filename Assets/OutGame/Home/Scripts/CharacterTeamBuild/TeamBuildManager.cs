using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// チーム編成を表示するクラス
/// </summary>
public class TeamBuildManager : MonoBehaviour
{
    [SerializeField] private HomeMenuLifeTimeScope _homeMenuLifeTimeScope;
    [SerializeField] private GameObject _selectButtonParent;
    
    private DataLoadCompleteNotifier _loadingNotifier;
    private JsonCharacterDeckDataRepository _jsonCharacterDeckDataRepository;
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    
    private List<int> _selectedCharacterIds = new List<int>();
    private List<GameObject> _selectButtonList = new List<GameObject>();
    private List<Image> _selectImageList = new List<Image>();
    
    private int _currentSelectDecIndex = 0;
    public int CurrentSelectDecIndex => _currentSelectDecIndex;

    private async void Awake()
    {
        _loadingNotifier = _homeMenuLifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();
        _jsonCharacterDeckDataRepository = _homeMenuLifeTimeScope.Container.Resolve<JsonCharacterDeckDataRepository>();
        _addressableCharacterImageDataRepository = _homeMenuLifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
        
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await _jsonCharacterDeckDataRepository.DataLoadAsync(cancellationTokenSource.Token);
        
        foreach (Transform selectButton in _selectButtonParent.transform.GetChild(_selectButtonParent.transform.childCount-1))
        {
            _selectButtonList.Add(selectButton.gameObject);
            _selectImageList.Add(selectButton.GetComponent<Image>());
        }
    }

    // private void OnEnable()
    // {
    //     _loadingNotifier.OnDataLoadComplete += TeamBuildView;
    // }
    //
    // private void OnDestroy()
    // {
    //     _loadingNotifier.OnDataLoadComplete -= TeamBuildView;
    // }

    // チーム編成の表示
    private void TeamBuildView()
    {
        //nullを後ろに持ってくるソート
        // Array.Sort(_jsonCharacterDeckDataRepository.RepositoryData.CharacterDeckHolder[_currentSelectDecIndex]
        //     .trainedCharacterDeck, (a, b) =>
        // {
        //     if(a == null && b == null) return 0;
        //     if(a == null) return 1;
        //     if(b == null) return -1;
        //     return a.CharacterID.CompareTo(b.CharacterID);
        // });
        Debug.Log(_jsonCharacterDeckDataRepository.RepositoryData.CharacterDeckHolder[1]);
        
        //編成キャラクターの表示
        for (int i = 0; i < _jsonCharacterDeckDataRepository.RepositoryData.CharacterDeckHolder[_currentSelectDecIndex].trainedCharacterDeck.Length; i++)
        {
            if(_jsonCharacterDeckDataRepository.RepositoryData.CharacterDeckHolder[_currentSelectDecIndex].trainedCharacterDeck[i] == null) break;
            _selectImageList[(int)_jsonCharacterDeckDataRepository.RepositoryData.CharacterDeckHolder[_currentSelectDecIndex].trainedCharacterDeck[i].CharacterID].sprite = _addressableCharacterImageDataRepository.GetSprite(
                _jsonCharacterDeckDataRepository.RepositoryData.CharacterDeckHolder[_currentSelectDecIndex].trainedCharacterDeck[i].CharacterID, CharacterSpriteType.MiniCard);
        }
    }
}
