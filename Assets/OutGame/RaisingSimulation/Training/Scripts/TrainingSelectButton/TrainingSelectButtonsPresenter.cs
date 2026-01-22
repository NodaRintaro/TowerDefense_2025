using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RaisingSimulationGameFlowStateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TrainingSelectButtonsPresenter : MonoBehaviour
{
    [SerializeField] private TrainingSelectButtonsView _buttonView;
    [SerializeField] private TrainingCharacterView _trainingCharacterView;
    [SerializeField] private StaminaSlider _staminaSlider;

    [SerializeField, Header("模擬戦ID")] private int _powerTrainingEventID;
    [SerializeField, Header("読書ID")] private int _intelligenceTrainingEventID;
    [SerializeField, Header("マラソンID")] private int _physicalTrainingEventID;
    [SerializeField, Header("狩猟ID")] private int _speedTrainingEventID;
    [SerializeField, Header("休憩ID")] private int _takeBreakTrainingEventID;

    //現在選択中のトレーニング
    private int _currentSelectedTrainingEventID;

    private TrainingEventPool _trainingEventPool;
    private GameFlowStateMachine _gameFlowStateMachine;

    private RaisingSimulationLifeTimeScope _lifeTimeScope;
    private JsonTrainingSaveDataRepository _trainingDataRepository;
    private AddressableTrainingEventDataRepository _trainingEventDataRepository;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();
        _gameFlowStateMachine = FindFirstObjectByType<GameFlowStateMachine>();
        _trainingDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _trainingEventPool = _lifeTimeScope.Container.Resolve<TrainingEventPool>();
        _trainingEventDataRepository = _lifeTimeScope.Container.Resolve<AddressableTrainingEventDataRepository>();
    }

    public void OnEnable()
    {
        _buttonView.TrainingStartButton.interactable = false;
    }

    public void Start()
    {
        SetOnclickTrainingSelectButtonEvents();
        SetOnclickTrainingStartButtonEvent();
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
        //_trainingEventPool.EnqueueData((uint)_currentSelectedTrainingEventID);

        await _gameFlowStateMachine.ChangeState(ScreenType.TrainingEvent);
    }

    /// <summary> トレーニング選択ボタンのイベント </summary>
    public void OnclickTrainingSelectEvent(int id)
    {
        ITrainingEventData eventData = null;

        //if (_trainingEventDataRepository.RepositoryData.GetData(id).IsBranch)
        //    eventData = FindSuccessTrainingBranchEvent((uint)id);
        //else
        //    eventData = _trainingEventDataRepository.RepositoryData.GetData(id);

        if (eventData.StaminaBaseBuff > 0)
            _staminaSlider.ShowIncreasePrediction((uint)eventData.StaminaBaseBuff);
        else if (eventData.StaminaBaseBuff < 0)
            _staminaSlider.ShowDecreasePrediction((uint)Mathf.Abs(eventData.StaminaBaseBuff));

        _trainingCharacterView.SetParameterBuffText(eventData.PowerBaseBuff, eventData.IntelligenceBaseBuff, eventData.PhysicalBaseBuff, eventData.SpeedBaseBuff);
        _currentSelectedTrainingEventID = id;
        _buttonView.TrainingStartButton.interactable = true;
    }

    /// <summary> トレーニングイベントの成功時のデータを返す </summary>
    private ITrainingEventData FindSuccessTrainingBranchEvent(uint id)
    {
        //TrainingEventData trainingEventData = _trainingEventDataRepository.RepositoryData.GetData((int)id);

        //if (trainingEventData.IsBranch)
        //{
        //    List<TrainingEventData> branchTrainingEventDataList = _branchTrainingEventDataRepository.RepositoryData.GetBranchRouteEvent(id);

        //    foreach(var branchEvent in  branchTrainingEventDataList)
        //        if(branchEvent.EventBranchType == EventBranchType.TrainingSuccess)
        //            return branchEvent;
        //}

        //return trainingEventData;

        return null;
    }
}
