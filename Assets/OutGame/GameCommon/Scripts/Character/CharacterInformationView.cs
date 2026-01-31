using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// 育成ゲームのキャラクター選択画面で選択中のキャラクターのView
/// </summary>
public class CharacterInformationView : MonoBehaviour 
{
    [SerializeField, Header("キャラクターのイメージ")] private Image _characterImage;

    [SerializeField, Header("キャラクターの役割")] private Image _jobImage = null;

    [SerializeField, Header("キャラクターの名前")] private TMP_Text _nameText = null;

    [Header("　キャラクターの各パラメータUI ")]

    [SerializeField, Header("筋力")] 
    private CharacterParameterUI _powerParameterUI = null;
    [SerializeField, Header("知力")]
    private CharacterParameterUI _intelligenceParameterUI = null;
    [SerializeField, Header("体力")]
    private CharacterParameterUI _physicalParameterUI = null;
    [SerializeField, Header("素早さ")]
    private CharacterParameterUI _speedParameterUI = null;

    //VContainerのLifeTimeScope継承クラス
    private RaisingSimulationDataContainer _lifeTimeScope = null;

    private const float _maxAlpha = 1.0f;

    #region ImageDataClass
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    private AddressableCharacterJobImageDataRepository _addressableCharacterJobImageDataRepository;
    private AddressableRankImageDataRepository _addressableRankImageDataRepository;
    #endregion

    public CharacterParameterUI CharacterParameterUI => _powerParameterUI;

    public void Start()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationDataContainer>();
        _addressableCharacterImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
        _addressableCharacterJobImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterJobImageDataRepository>();
        _addressableRankImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableRankImageDataRepository>();

        _powerParameterUI.Init();
        _intelligenceParameterUI.Init();
        _physicalParameterUI.Init();
        _speedParameterUI.Init();
        InitView();
    }

    private void InitView()
    {
        if (_characterImage != null) 
            ChangeImageAlpha(_characterImage, 0f);

        if (_jobImage != null)
            ChangeImageAlpha(_jobImage, 0f);
    }

    public void SetImage(uint id)
    {
        if (_characterImage.color.a != _maxAlpha) ChangeImageAlpha(_characterImage, _maxAlpha);

        _characterImage.sprite = _addressableCharacterImageDataRepository.RepositoryData.GetCharacterSprite(id, CharacterSpriteType.OverAllView);
    }

    public void SetJob(JobType jobType)
    {
        if (_jobImage.color.a != _maxAlpha) ChangeImageAlpha(_jobImage, _maxAlpha);

        _jobImage.sprite = _addressableCharacterJobImageDataRepository.RepositoryData.GetData(jobType);
    }

    public void SetName(string name)
    {
        _nameText.text = name; 
    }

    /// <summary> 各パラメータのUIにキャラクターのパラメータを反映する </summary>
    public void SetParameter(CharacterBaseData character)
    {
        SetParameterUI(_physicalParameterUI, character.BasePhysical);
        SetParameterUI(_powerParameterUI, character.BasePower);
        SetParameterUI(_intelligenceParameterUI, character.BaseIntelligence);
        SetParameterUI(_speedParameterUI, character.BaseSpeed);
    }

    /// <summary> パラメータのUIに現在のパラメータを反映する処理 </summary>
    private async void SetParameterUI(CharacterParameterUI parameterUI, uint currentParam)
    {
        RankType currentRank = RankCalculator.GetCurrentRank(currentParam, CharacterParameterRankRateData.RankRateDict);
        uint currentRankMinValue = RankCalculator.GetCurrentRankMinNum(currentParam, CharacterParameterRankRateData.RankRateDict);
        uint nextRankValue = RankCalculator.GetNextRankNum(currentParam, CharacterParameterRankRateData.RankRateDict);

        //スライダーゲージに現在のパラメータを反映
        await parameterUI.SetParameter(currentParam, currentRankMinValue, nextRankValue, _addressableRankImageDataRepository.GetSprite(currentRank));
    }

    private void ChangeImageAlpha(Image image, float alphaNum)
    {
        var color = image.color;
        color.a = alphaNum;
        image.color = color;
    }
}
