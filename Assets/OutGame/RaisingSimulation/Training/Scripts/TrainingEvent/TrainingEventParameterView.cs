using VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TrainingEventParameterView : MonoBehaviour
{
    [SerializeField]private GameObject _parameterObj;
    private Transform _parameterOriginalTransform;

    [Header("　キャラクターの各パラメータUI ")]

    [SerializeField, Header("筋力")]
    private CharacterParameterUI _powerParameterUI = null;
    [SerializeField, Header("知力")]
    private CharacterParameterUI _intelligenceParameterUI = null;
    [SerializeField, Header("体力")]
    private CharacterParameterUI _physicalParameterUI = null;
    [SerializeField, Header("素早さ")]
    private CharacterParameterUI _speedParameterUI = null;

    private float _slideInCenterPos = 100f;
    private float _slideInDuration = 0.5f;

    private RaisingSimulationLifeTimeScope _raisingSimulationLifeTimeScope = null;
    private AddressableRankImageDataRepository _addressableRankImageDataRepository = null;

    public GameObject ParameterObj => _parameterObj;
    public CharacterParameterUI PowerParameterUI => _powerParameterUI;
    public CharacterParameterUI IntelligenceParameterUI => _intelligenceParameterUI;
    public CharacterParameterUI PhysicalParameterUI => _physicalParameterUI;
    public CharacterParameterUI SpeedParameterUI => _speedParameterUI;

    public void Awake()
    {
        _raisingSimulationLifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();
        _addressableRankImageDataRepository = _raisingSimulationLifeTimeScope.Container.Resolve<AddressableRankImageDataRepository>();
    }

    public void Start()
    {
        _parameterOriginalTransform = transform;
    }

    /// <summary> 元居た場所に戻る処理 </summary>
    public void SetOriginalTransform()
    {
        _parameterObj.transform.position = _parameterObj.transform.position;
    }

    public async UniTask TrainingBuffEvent(TrainingCharacterData trainingCharacterData)
    {
        ParameterObj.SetActive(true);
        await SlideInAnimation.SlideInGameObject(ParameterObj, _slideInCenterPos, _slideInDuration);

        await PowerParameterUI.SetParameter(trainingCharacterData.TotalPower,
            RankCalculator.GetCurrentRankMinNum(trainingCharacterData.TotalPower, CharacterParameterRankRateData.RankRateDict),
            RankCalculator.GetNextRankNum(trainingCharacterData.TotalPower, CharacterParameterRankRateData.RankRateDict),
            _addressableRankImageDataRepository.GetSprite(RankCalculator.GetCurrentRank(trainingCharacterData.TotalPower, CharacterParameterRankRateData.RankRateDict)));

        await IntelligenceParameterUI.SetParameter(trainingCharacterData.TotalIntelligence,
            RankCalculator.GetCurrentRankMinNum(trainingCharacterData.TotalIntelligence, CharacterParameterRankRateData.RankRateDict),
            RankCalculator.GetNextRankNum(trainingCharacterData.TotalIntelligence, CharacterParameterRankRateData.RankRateDict),
            _addressableRankImageDataRepository.GetSprite(RankCalculator.GetCurrentRank(trainingCharacterData.TotalIntelligence, CharacterParameterRankRateData.RankRateDict)));

        await PhysicalParameterUI.SetParameter(trainingCharacterData.TotalPhysical,
            RankCalculator.GetCurrentRankMinNum(trainingCharacterData.TotalPhysical, CharacterParameterRankRateData.RankRateDict),
            RankCalculator.GetNextRankNum(trainingCharacterData.TotalPhysical, CharacterParameterRankRateData.RankRateDict),
            _addressableRankImageDataRepository.GetSprite(RankCalculator.GetCurrentRank(trainingCharacterData.TotalPhysical, CharacterParameterRankRateData.RankRateDict)));

        await SpeedParameterUI.SetParameter(trainingCharacterData.TotalSpeed,
            RankCalculator.GetCurrentRankMinNum(trainingCharacterData.TotalSpeed, CharacterParameterRankRateData.RankRateDict),
            RankCalculator.GetNextRankNum(trainingCharacterData.TotalSpeed, CharacterParameterRankRateData.RankRateDict),
            _addressableRankImageDataRepository.GetSprite(RankCalculator.GetCurrentRank(trainingCharacterData.TotalSpeed, CharacterParameterRankRateData.RankRateDict)));

        await SlideInAnimation.SlideOutGameObject(ParameterObj, _slideInCenterPos, _slideInDuration);
        ParameterObj.SetActive(false);
        SetOriginalTransform();
    }
}
