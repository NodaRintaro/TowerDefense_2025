using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RaisingSimulationGameFlowStateMachine;
using UnityEngine;
using VContainer;

public class TrainingSelectButtonsPresenter : MonoBehaviour
{
    [SerializeField] private TrainingSelectButtonsView _buttonView;
    [SerializeField] private TrainingCharacterView _trainingCharacterView;
    [SerializeField] private StaminaSlider _staminaSlider;
    [SerializeField] private TrainingRaidCountDownView _trainingRaidCountDownView;

    [SerializeField, Header("模擬戦ID")] private uint _powerTrainingEventID;
    [SerializeField, Header("読書ID")] private uint _intelligenceTrainingEventID;
    [SerializeField, Header("マラソンID")] private uint _physicalTrainingEventID;
    [SerializeField, Header("狩猟ID")] private uint _speedTrainingEventID;
    [SerializeField, Header("休憩ID")] private uint _takeBreakTrainingEventID;

    //現在選択中のトレーニング
    private uint _currentSelectedTrainingEventID;

    private int _staminaBuff = 0;

    private TrainingEventPool _trainingEventPool;
    private GameFlowStateMachine _gameFlowStateMachine;

    private RaisingSimulationLifeTimeScope _lifeTimeScope;
    private TrainingSaveData _trainingSaveData;
    private TrainingEventSelector _trainingEventSelector;
    private JsonTrainingSaveDataRepository _trainingDataRepository;
    private AddressableTrainingEventDataRepository _trainingEventDataRepository;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();
        _gameFlowStateMachine = FindFirstObjectByType<GameFlowStateMachine>();
        _trainingSaveData = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>().RepositoryData;
        _trainingDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _trainingEventPool = _lifeTimeScope.Container.Resolve<TrainingEventPool>();
        _trainingEventDataRepository = _lifeTimeScope.Container.Resolve<AddressableTrainingEventDataRepository>();
        _trainingEventSelector = _lifeTimeScope.Container.Resolve<TrainingEventSelector>();
    }

    public void OnEnable()
    {
        _staminaBuff = 0;
        _buttonView.TrainingStartButton.interactable = false;
        CheckSchedule();
    }

    public void Start()
    {
        SetOnclickTrainingSelectButtonEvents();
        SetOnclickTrainingStartButtonEvent();
    }

    private void CheckSchedule()
    {
        _trainingSaveData.AddElapsedDays();

        int count = 0;

        for(int i = (int)_trainingSaveData.CurrentElapsedDays; !_trainingSaveData.CurrentCharacterSchedule.TrainingEventSchedule[i].IsRaid; i++)
        {
            count++;
        }

        if (count != 0)
            _trainingRaidCountDownView.CountDown(count);
        else
            SetRaidEventMenu();
    }

    /// <summary> トレーニング開始ボタンのイベントを設定 </summary>
    public void SetOnclickTrainingStartButtonEvent()
    {
        _buttonView.TrainingStartButton.onClick.AddListener(async () => await OnclickTrainingStartEvent());
    }

    /// <summary> トレーニング選択ボタンのイベントを設定 </summary>
    public void SetOnclickTrainingSelectButtonEvents()
    {
        _buttonView.PowerTrainingButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_powerTrainingEventID));
        _buttonView.PhysicalTrainingButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_physicalTrainingEventID));
        _buttonView.IntelligenceTrainingButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_intelligenceTrainingEventID));
        _buttonView.SpeedTrainingButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_speedTrainingEventID));
        _buttonView.TakeBreakButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_takeBreakTrainingEventID));
    }

    /// <summary> トレーニング開始ボタンのイベント </summary>
    public async UniTask OnclickTrainingStartEvent()
    {
        _trainingEventSelector.SetTrainingCommonEvent(_currentSelectedTrainingEventID);
        _trainingEventSelector.SetCharacterUniqueEvent();
        //_trainingEventSelector.SetSupportCardEvent();

        if(_staminaBuff < 0)
        {
            uint decreaseStamina = (uint)Math.Abs(_staminaBuff);
            _staminaSlider.ConsumeStamina(decreaseStamina);
            _trainingSaveData.UseStamina(decreaseStamina);
        }

        await UniTask.Delay(700);

        await _gameFlowStateMachine.ChangeState(ScreenType.TrainingEvent);
    }

    /// <summary> トレーニング選択ボタンのイベント </summary>
    public void OnclickTrainingSelectEvent(uint id)
    {
        TrainingEventData eventData = null;

        if (_trainingEventDataRepository.GetCsvEventData(id) != null)
        {
            eventData = _trainingEventDataRepository.GetCsvEventData(id);

            if (eventData.IsBranch)
                eventData = FindSuccessTrainingBranchEvent(id);
        }

        if (eventData.StaminaBaseBuff > 0)
            _staminaSlider.ShowIncreasePrediction((uint)eventData.StaminaBaseBuff);
            
        else if (eventData.StaminaBaseBuff < 0)
            _staminaSlider.ShowDecreasePrediction((uint)Mathf.Abs(eventData.StaminaBaseBuff));

        _trainingCharacterView.SetParameterBuffText(eventData.PowerBaseBuff, eventData.IntelligenceBaseBuff, eventData.PhysicalBaseBuff, eventData.SpeedBaseBuff);
        _currentSelectedTrainingEventID = id;
        _buttonView.TrainingStartButton.interactable = true;
    }

    /// <summary> トレーニングイベントの成功時のデータを返す </summary>
    private TrainingEventData FindSuccessTrainingBranchEvent(uint id)
    {
        List <TrainingEventData> branchTrainingEventDataList = _trainingEventDataRepository.GetBranchEventData(id);

        foreach (var branchEvent in branchTrainingEventDataList)
            if (branchEvent.BranchType == EventBranchType.TrainingSuccess)
                return branchEvent;

        Debug.LogError("トレーニング結果が見れませんでした");
        return null;
    }

    private void SetRaidEventMenu()
    {

    }
}
