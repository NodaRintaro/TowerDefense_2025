using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// キャラクター一覧を表示するクラス
/// </summary>
public class CharacterListManager : MonoBehaviour
{
    [SerializeField] private HomeMenuLifeTimeScope _homeMenuLifeTimeScope;
    [SerializeField] private GameObject _characterListParent;
    [SerializeField] private GameObject _characterView;

    [Header("キャラ詳細表示用UI")] [SerializeField]
    private Image _characterViewImage;

    [SerializeField] private TMP_Text _characterNameText;
    [Header("パラメーター")] [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _blockCountText;
    [SerializeField] private Image _jobIconImage;
    [Header("パワー")] [SerializeField] private TMP_Text _powerText;
    [SerializeField] private TMP_Text _maxPowerText;
    [SerializeField] private Slider _powerSlider;
    [SerializeField] private Image _powerRankImage;
    [Header("知力")] [SerializeField] private TMP_Text _intelligenceText;
    [SerializeField] private TMP_Text _maxIntelligenceText;
    [SerializeField] private Slider _intelligenceSlider;
    [SerializeField] private Image _intelligenceRankImage;
    [Header("体力")] [SerializeField] private TMP_Text _physicalText;
    [SerializeField] private TMP_Text _maxPhysicalText;
    [SerializeField] private Slider _physicalSlider;
    [SerializeField] private Image _physicalRankImage;
    [Header("素早さ")] [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _maxSpeedText;
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private Image _speedRankImage;

    private DataLoadCompleteNotifier _loadingNotifier;
    private JsonCharacterCollectionDataRepository _jsonCharacterCollectionDataRepository;
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    private TowerDefenseCharacterDataBase _towerDefenseCharacterDataBase;
    private AddressableRankImageDataRepository _rankImageDataRegistry;
    private AddressableCharacterJobImageDataRepository _characterJobImageDataRegistry;
    private AddressableCharacterDataRepository _characterBaseDataRegistry;
    List<GameObject> _characterListView = new List<GameObject>();

    private async void Awake()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        _loadingNotifier = _homeMenuLifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();
        _jsonCharacterCollectionDataRepository =
            _homeMenuLifeTimeScope.Container.Resolve<JsonCharacterCollectionDataRepository>();
        _addressableCharacterImageDataRepository =
            _homeMenuLifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
        _towerDefenseCharacterDataBase =
            _homeMenuLifeTimeScope.Container.Resolve<TowerDefenseCharacterDataBase>();
        _rankImageDataRegistry =
            _homeMenuLifeTimeScope.Container.Resolve<AddressableRankImageDataRepository>();
        _characterJobImageDataRegistry =
            _homeMenuLifeTimeScope.Container.Resolve<AddressableCharacterJobImageDataRepository>();
        _characterBaseDataRegistry =
            _homeMenuLifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();

        await _jsonCharacterCollectionDataRepository.DataLoadAsync(cancellationTokenSource.Token);
        await _addressableCharacterImageDataRepository.DataLoadAsync(cancellationTokenSource.Token);
        await _characterJobImageDataRegistry.DataLoadAsync(cancellationTokenSource.Token);
        await _characterBaseDataRegistry.DataLoadAsync(cancellationTokenSource.Token);
        
    }

    private void OnEnable()
    {
        _loadingNotifier.OnDataLoadComplete += CreateCharacterListView;
    }

    private void OnDestroy()
    {
        _loadingNotifier.OnDataLoadComplete -= CreateCharacterListView;
    }

    //所持キャラのリストを作成
    private void CreateCharacterListView()
    {
        foreach (var characterID in _jsonCharacterCollectionDataRepository.RepositoryData.CollectionList)
        {
            GameObject obj = Instantiate(_characterView, _characterListParent.transform);
            obj.GetComponent<Image>().sprite = _addressableCharacterImageDataRepository.GetSprite(
                characterID, CharacterSpriteType.MiniCard);
            obj.GetComponent<CharacterViewButton>().OnClick += () =>
            {
                ViewDetail(characterID);
            };
            _characterListView.Add(obj);
        }
        ViewDetail(1);
    }

    //キャラクターを選択した際、詳細を表示する
    private void ViewDetail(uint characterId)
    {
        CharacterBaseData characterData;
        TowerDefenseCharacterData currentCharacterData;
        //育成データが存在しない場合は基本データのみ表示
        if (_towerDefenseCharacterDataBase.TryGetCharacterDict(out currentCharacterData, characterId, 0) == false)
        {
            characterData = _characterBaseDataRegistry.RepositoryData.GetData(characterId);
        }
        else
        {
            _towerDefenseCharacterDataBase.TryGetCharacterDict(out currentCharacterData, characterId,
                (uint)(_towerDefenseCharacterDataBase.TowerDefenseCharacterDataDict[characterId].Length - 1));
            characterData = currentCharacterData;
        }


        //キャラクター画像
        _characterViewImage.sprite = _addressableCharacterImageDataRepository.GetSprite(
            characterId, CharacterSpriteType.OverAllView);
        _characterViewImage.rectTransform.pivot = new Vector2(
            _characterViewImage.sprite.pivot.x / _characterViewImage.sprite.rect.width,
            _characterViewImage.sprite.pivot.y / _characterViewImage.sprite.rect.height);

        //キャラクター名
        _characterNameText.text = characterData.CharacterName;

        //ジョブ
        _jobIconImage.sprite =
            _characterJobImageDataRegistry.GetSprite(_characterBaseDataRegistry.RepositoryData.GetData(characterId)
                .CharacterRole);

        //コスト
        _costText.text = characterData.Cost.ToString();

        //ブロック数


        //パワー
        RankType powerRank =
            RankCalculator.GetCurrentRank(characterData.TotalPower, CharacterParameterRankRateData.RankRateDict);
        uint maxPower = CharacterParameterRankRateData.RankRateDict[powerRank + 1];
        _powerText.text = characterData.TotalPower.ToString();
        _maxPowerText.text = "/" + maxPower;
        _powerSlider.value = (float)characterData.TotalPower / maxPower;
        _powerRankImage.sprite = _rankImageDataRegistry.GetSprite(powerRank);

        //知力
        RankType intelligenceRank = RankCalculator.GetCurrentRank(characterData.TotalIntelligence,
            CharacterParameterRankRateData.RankRateDict);
        uint maxIntelligence = CharacterParameterRankRateData.RankRateDict[intelligenceRank + 1];
        _intelligenceText.text = characterData.TotalIntelligence.ToString();
        _maxIntelligenceText.text = "/" + maxIntelligence;
        _intelligenceSlider.value = (float)characterData.TotalIntelligence / maxIntelligence;
        _intelligenceRankImage.sprite = _rankImageDataRegistry.GetSprite(powerRank);

        //体力
        RankType physicalRank = RankCalculator.GetCurrentRank(characterData.TotalPhysical,
            CharacterParameterRankRateData.RankRateDict);
        uint maxPhysical = CharacterParameterRankRateData.RankRateDict[physicalRank + 1];
        _physicalText.text = characterData.TotalPhysical.ToString();
        _maxPhysicalText.text = "/" + maxPhysical;
        _physicalSlider.value = (float)characterData.TotalPhysical / maxPhysical;
        _physicalRankImage.sprite = _rankImageDataRegistry.GetSprite(powerRank);
        //素早さ
        RankType speedRank =
            RankCalculator.GetCurrentRank(characterData.TotalSpeed, CharacterParameterRankRateData.RankRateDict);
        uint maxSpeed = CharacterParameterRankRateData.RankRateDict[speedRank + 1];
        _speedText.text = characterData.TotalSpeed.ToString();
        _maxSpeedText.text = "/" + maxSpeed;
        _speedSlider.value = (float)characterData.TotalSpeed / maxSpeed;
        _speedRankImage.sprite = _rankImageDataRegistry.GetSprite(powerRank);
    }
}