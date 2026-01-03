
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// 育成ゲームのキャラクター選択画面で選択中のキャラクターのView
/// </summary>
public class CharacterInformationView : MonoBehaviour 
{
    [SerializeField] private Image _characterImage;

    [SerializeField] private GameObject _characterUI;

    [SerializeField] private Image _jobImage = null;

    [SerializeField] private TMP_Text _nameText = null;

    [SerializeField,Header("キャラクターのパラメータのUI")] 
    private CharacterParameterUI _characterParameterUI = null;

    //VContainerのLifeTimeScope継承クラス
    private RaisingSimulationLifeTimeScope _lifeTimeScope = null;

    #region ImageDataClass
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    private AddressableCharacterJobImageDataRepository _addressableCharacterJobImageDataRepository;
    private AddressableRankImageDataRepository _addressableRankImageDataRepository;
    #endregion

    public GameObject CharacterUI => _characterUI;
    public CharacterParameterUI CharacterParameterUI => _characterParameterUI;

    public void OnEnable()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();
        _addressableCharacterImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
        _addressableCharacterJobImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterJobImageDataRepository>();
        _addressableRankImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableRankImageDataRepository>();
    }

    public void SetImage(uint id)
    {
        _characterImage.sprite = _addressableCharacterImageDataRepository.RepositoryData.GetCharacterSprite(id, CharacterSpriteType.OverAllView);
    }

    public void SetJob(JobType jobType)
    {
        _jobImage.sprite = _addressableCharacterJobImageDataRepository.RepositoryData.GetData(jobType);
    }

    public void SetName(string name)
    {
        _nameText.text = name; 
    }

    /// <summary> 各パラメータのUIにキャラクターのパラメータを反映する </summary>
    public void SetParameter(CharacterBaseData character)
    {
        for(int arrayCount = 0; _characterParameterUI.ParameterUIHolder.Length > arrayCount; arrayCount++)
        {
            switch (_characterParameterUI.ParameterUIHolder[arrayCount].ParamType)
            {
                case ParameterType.Physical:
                    SetParameterUI(ref _characterParameterUI.ParameterUIHolder[arrayCount], character.BasePhysical);
                    break;
                case ParameterType.Power:
                    SetParameterUI(ref _characterParameterUI.ParameterUIHolder[arrayCount], character.BasePower);
                    break;
                case ParameterType.Intelligence:
                    SetParameterUI(ref _characterParameterUI.ParameterUIHolder[arrayCount], character.BaseIntelligence);
                    break;
                case ParameterType.Speed:
                    SetParameterUI(ref _characterParameterUI.ParameterUIHolder[arrayCount], character.BaseSpeed);
                    break;
            }
        }
    }

    private void SetParameterUI(ref CharacterParameterUI.ParameterUI parameterUI, uint currentParam)
    {
        RankType currentRank = RankCalculator.GetCurrentRank(currentParam, CharacterParameterRankRateData.RankRateDict);
        uint nextRankValue = RankCalculator.GetNextRankNum(currentParam, CharacterParameterRankRateData.RankRateDict);

        //現在のパラメータのRankを反映
        _characterParameterUI.SetRankImage(ref parameterUI.ParamRankImage, _addressableRankImageDataRepository.GetSprite(currentRank));

        //スライダーゲージに現在のパラメータを反映
        _characterParameterUI.SetParameterSlider(ref parameterUI.Slider, currentParam, nextRankValue);

        //テキストに現在のパラメータを入力
        _characterParameterUI.SetParameterText(ref parameterUI.ParamText, currentParam, nextRankValue);
    }
}
