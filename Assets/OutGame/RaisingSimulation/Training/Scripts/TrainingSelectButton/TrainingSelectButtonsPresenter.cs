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

    [SerializeField, Header("模擬戦ID")] private uint _powerTrainingEventID;
    [SerializeField, Header("読書ID")] private uint _intelligenceTrainingEventID;
    [SerializeField, Header("マラソンID")] private uint _physicalTrainingEventID;
    [SerializeField, Header("狩猟ID")] private uint _speedTrainingEventID;
    [SerializeField, Header("休憩ID")] private uint _takeBreakTrainingEventID;

    //現在選択中のトレーニング
    private uint _currentSelectedTrainingEventID;

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

    public void Start()
    {
        SetOnclickTrainingSelectButtonEvents();
        SetOnclickTrainingStartButtonEvent();
    }

    public void SetOnclickTrainingStartButtonEvent()
    {
        _buttonView.TrainingStartButton.onClick.AddListener(async () => await OnclickTrainingStartEvent());
    }

    public void SetOnclickTrainingSelectButtonEvents()
    {
        _buttonView.PowerTrainingButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_powerTrainingEventID));
        _buttonView.PhysicalTrainingButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_physicalTrainingEventID));
        _buttonView.IntelligenceTrainingButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_intelligenceTrainingEventID));
        _buttonView.SpeedTrainingButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_speedTrainingEventID));
        _buttonView.TakeBreakButton.onClick.AddListener(() => OnclickTrainingSelectEvent(_takeBreakTrainingEventID));
    }

    public async UniTask OnclickTrainingStartEvent()
    {
        _trainingEventPool.EnqueueData(_currentSelectedTrainingEventID);

        await _gameFlowStateMachine.ChangeState(ScreenType.TrainingEvent);
    }

    public void OnclickTrainingSelectEvent(uint id)
    {
        TrainingEventData eventData = _trainingEventDataRepository.RepositoryData.GetData(id);

        if (eventData.StaminaBaseBuff > 0)
            _staminaSlider.ShowIncreasePrediction((uint)eventData.StaminaBaseBuff);
        else if (eventData.StaminaBaseBuff < 0)
            _staminaSlider.ShowDecreasePrediction((uint)Mathf.Abs(eventData.StaminaBaseBuff));

            _trainingCharacterView.SetParameterBuffText(eventData.PowerBaseBuff, eventData.IntelligenceBaseBuff, eventData.PhysicalBaseBuff, eventData.SpeedBaseBuff);
        _currentSelectedTrainingEventID = id;
    }
}
