using Cysharp.Threading.Tasks;
using RaisingSimulationGameFlowStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TrainingEventBonusController : MonoBehaviour
{
    [SerializeField] private TrainingEventParameterView _trainingBonusView = null;
    [SerializeField] private TrainingEventFlowController _trainingEventController = null;

    private float _slideInCenterPos = 100f;
    private float _slideInDuration = 0.5f;

    private TrainingCharacterData _trainingCharacterData = null;

    //LifeTimeScope
    private RaisingSimulationLifeTimeScope _lifeTimeScope;

    private JsonTrainingSaveDataRepository _trainingSaveDataRepository;
    private AddressableRankImageDataRepository _addressableRankImageDataRepository;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _trainingSaveDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _addressableRankImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableRankImageDataRepository>();
    }

    public void Start()
    {
        _trainingCharacterData = _trainingSaveDataRepository.RepositoryData.TrainingCharacterData;
    }

    public async UniTask TrainingBuffEvent()
    {
        _trainingBonusView.ParameterObj.SetActive(true);
        await SlideInAnimation.SlideInGameObject(_trainingBonusView.ParameterObj, _slideInCenterPos, _slideInDuration);

        await _trainingBonusView.PowerParameterUI.SetParameter(_trainingCharacterData.TotalPower,
            RankCalculator.GetCurrentRankMinNum(_trainingCharacterData.TotalPower, CharacterParameterRankRateData.RankRateDict),
            RankCalculator.GetNextRankNum(_trainingCharacterData.TotalPower, CharacterParameterRankRateData.RankRateDict), 
            _addressableRankImageDataRepository.GetSprite(RankCalculator.GetCurrentRank(_trainingCharacterData.TotalPower, CharacterParameterRankRateData.RankRateDict)));

        await _trainingBonusView.IntelligenceParameterUI.SetParameter(_trainingCharacterData.TotalIntelligence,
            RankCalculator.GetCurrentRankMinNum(_trainingCharacterData.TotalIntelligence, CharacterParameterRankRateData.RankRateDict),
            RankCalculator.GetNextRankNum(_trainingCharacterData.TotalIntelligence, CharacterParameterRankRateData.RankRateDict),
            _addressableRankImageDataRepository.GetSprite(RankCalculator.GetCurrentRank(_trainingCharacterData.TotalIntelligence, CharacterParameterRankRateData.RankRateDict)));

        await _trainingBonusView.PhysicalParameterUI.SetParameter(_trainingCharacterData.TotalPhysical,
            RankCalculator.GetCurrentRankMinNum(_trainingCharacterData.TotalPhysical, CharacterParameterRankRateData.RankRateDict),
            RankCalculator.GetNextRankNum(_trainingCharacterData.TotalPhysical, CharacterParameterRankRateData.RankRateDict),
            _addressableRankImageDataRepository.GetSprite(RankCalculator.GetCurrentRank(_trainingCharacterData.TotalPhysical, CharacterParameterRankRateData.RankRateDict)));

        await _trainingBonusView.SpeedParameterUI.SetParameter(_trainingCharacterData.TotalSpeed,
            RankCalculator.GetCurrentRankMinNum(_trainingCharacterData.TotalSpeed, CharacterParameterRankRateData.RankRateDict),
            RankCalculator.GetNextRankNum(_trainingCharacterData.TotalSpeed, CharacterParameterRankRateData.RankRateDict),
            _addressableRankImageDataRepository.GetSprite(RankCalculator.GetCurrentRank(_trainingCharacterData.TotalSpeed, CharacterParameterRankRateData.RankRateDict)));

        await SlideInAnimation.SlideOutGameObject(_trainingBonusView.ParameterObj, _slideInCenterPos, _slideInDuration);
        _trainingBonusView.ParameterObj.SetActive(false);
        _trainingBonusView.SetOriginalTransform();
    }
}
