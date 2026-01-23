using Cysharp.Threading.Tasks;
using UnityEngine;
using RaisingSimulationGameFlowStateMachine;
using VContainer;

public enum TrainingEventStateType
{
    None,
    EventLoadingState,
    ReadScenario,
    EventBranch,
    RaidEvent,
    FinishEvent
}

public abstract class TrainingEventStateBase : State<TrainingEventStateType>
{
    public TrainingEventStateMachine TrainingEventStateMachine => _stateMachine as TrainingEventStateMachine;
}

/// <summary>
/// トレーニング中のイベントの生成と管理を行うステートマシン
/// </summary>
public class TrainingEventStateMachine : StateMachine<TrainingEventStateType>
{
    [SerializeField] private TrainingEventController _trainingEventController;

    //現在のState
    private TrainingEventStateType _currentStateType;

    //現在のイベントのID
    private uint _currentEventID = 0;

    //現在進行中のイベントデータ
    private TrainingEventData _currentEventData = null;
    private ScenarioData _currentScenarioData;
    private TrainingEventType _currentEventType = TrainingEventType.None;

    //育成ゲーム全体のLifeTimeScope
    private RaisingSimulationLifeTimeScope _raisingSimulationLifeTimeScope;

    public TrainingEventController TrainingEventController => _trainingEventController;

    public TrainingEventStateType CurrentStateType => _currentStateType;
    public TrainingEventType CurrentEventType => _currentEventType;
    public TrainingEventData CurrentEventData => _currentEventData;
    public ScenarioData CurrentScenario => _currentScenarioData;
    public RaisingSimulationLifeTimeScope RaisingSimulationLifeTimeScope => _raisingSimulationLifeTimeScope;
    public GameFlowStateMachine GameFlowStateMachine => _raisingSimulationLifeTimeScope.Container.Resolve<GameFlowStateMachine>();
    public TrainingEventPool EventPool => _raisingSimulationLifeTimeScope.Container.Resolve<TrainingEventPool>();
    public TrainingSaveData CurrentTrainingSaveData => _raisingSimulationLifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>().RepositoryData;
    public TrainingEventDataGenerator TrainingEventDataGenerator => _raisingSimulationLifeTimeScope.Container.Resolve<TrainingEventDataGenerator>();

    private void Awake()
    {
        _raisingSimulationLifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _currentStateType = TrainingEventStateType.None;

        _stateDict.Add(TrainingEventStateType.EventLoadingState, new EventLoadingState(this));
        _stateDict.Add(TrainingEventStateType.ReadScenario, new ReadScenarioState(this));
        _stateDict.Add(TrainingEventStateType.EventBranch, new EventBranchState(this));
        _stateDict.Add(TrainingEventStateType.FinishEvent, new FinishEventState(this));
    }

    public override async UniTask ChangeState(TrainingEventStateType trainingEventState)
    {
        if (_currentState != null)
        {
            await _currentState.OnExit();
        }

        Debug.Log("現在のステート" +  trainingEventState);
        //次のステートに切り替える
        _currentState = _stateDict[trainingEventState];
        _currentStateType = trainingEventState;
        await _currentState.OnEnter();
    } 

    public void SetCurrentEventType(TrainingEventType trainingEventType) => _currentEventType = trainingEventType;

    public void SetCurrentTrainingEvent(TrainingEventData trainingEventData) => _currentEventData = trainingEventData;

    public void SetCurrentScenarioData(ScenarioData scenarioData) 
    { 
        _currentScenarioData = scenarioData; 
    }

    public async UniTask OnFinishReadScenario()
    {
        if (_currentEventData.IsBranch)
            await ChangeState(TrainingEventStateType.EventBranch);
        else
            await ChangeState(TrainingEventStateType.FinishEvent);
    }

    public async UniTask FinishAllEvents()
    {
        if(CurrentTrainingSaveData.CurrentCharacterSchedule.TrainingEventSchedule.Length > CurrentTrainingSaveData.CurrentElapsedDays)
        {
            await GameFlowStateMachine.ChangeState(ScreenType.TrainingSelectMenu);
        }
        else
        {
            await GameFlowStateMachine.ChangeState(ScreenType.Result);
        }
    }
}

#region トレーニング開始時の処理
public class EventLoadingState : TrainingEventStateBase
{
    public EventLoadingState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public async override UniTask OnEnter()
    {
        if(TrainingEventStateMachine.CurrentEventType == TrainingEventType.None)
            TrainingEventStateMachine.SetCurrentEventType(TrainingEventType.TrainingEvent);

        if (await TrainingEventLoading())
        {
            await TrainingEventStateMachine.ChangeState(TrainingEventStateType.ReadScenario);
        }
        else
        {
            await TrainingEventStateMachine.FinishAllEvents();
        }
    }

    public async UniTask<bool> TrainingEventLoading()
    {
        switch (TrainingEventStateMachine.CurrentEventType)
        {
            case TrainingEventType.TrainingEvent:
                if (TrainingEventStateMachine.EventPool.IsEventQueueEmpty(TrainingEventType.TrainingEvent))
                {
                    Debug.Log(TrainingEventStateMachine.CurrentEventType + "のイベントは空っぽ");
                    TrainingEventStateMachine.SetCurrentEventType(TrainingEventType.CharacterUniqueEvent);
                    return await TrainingEventLoading();
                }
                else
                {
                    SetCurrentEventData();
                    return true;
                }
            case TrainingEventType.CharacterUniqueEvent:
                if (TrainingEventStateMachine.EventPool.IsEventQueueEmpty(TrainingEventType.CharacterUniqueEvent))
                {
                    Debug.Log(TrainingEventStateMachine.CurrentEventType + "のイベントは空っぽ");
                    TrainingEventStateMachine.SetCurrentEventType(TrainingEventType.SupportCardEvent);
                    return await TrainingEventLoading();
                }
                else
                {
                    SetCurrentEventData();
                    return true;
                }
            case TrainingEventType.SupportCardEvent:
                if (TrainingEventStateMachine.EventPool.IsEventQueueEmpty(TrainingEventType.SupportCardEvent))
                {

                    Debug.Log(TrainingEventStateMachine.CurrentEventType + "のイベントは空っぽ");
                    TrainingEventStateMachine.SetCurrentEventType(TrainingEventType.None);
                    await TrainingEventStateMachine.FinishAllEvents();
                    return false;
                }
                else
                {
                    SetCurrentEventData();
                    return true;
                }
        }
        return false;
    }

    private void SetCurrentEventData()
    {
        uint dequeueData = TrainingEventStateMachine.EventPool.DequeueData(TrainingEventStateMachine.CurrentEventType);

        TrainingEventStateMachine.SetCurrentTrainingEvent(TrainingEventStateMachine.TrainingEventDataGenerator.GenerateTrainingEvent
            (TrainingEventStateMachine.CurrentEventType, dequeueData));

        TrainingEventStateMachine.SetCurrentScenarioData(TrainingEventStateMachine.TrainingEventDataGenerator.GenerateScenarioData
            (TrainingEventStateMachine.CurrentEventType, TrainingEventStateMachine.CurrentEventData.ScenarioID));

    }
}
#endregion

#region シナリオを読んでいる間の処理
public class ReadScenarioState : TrainingEventStateBase
{
    public ReadScenarioState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public async override UniTask OnEnter()
    {
        TrainingEventStateMachine.TrainingEventController.SetReadScenario(TrainingEventStateMachine.CurrentScenario);
        TrainingEventStateMachine.TrainingEventController.ChangeEventAction(EventInputActionType.ReadScenarioEvent);
        await TrainingEventStateMachine.TrainingEventController.OnClickEventAction();
    }
}
#endregion

#region イベント分岐時の処理
public class EventBranchState : TrainingEventStateBase
{
    public EventBranchState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public async override UniTask OnEnter()
    {
        await TrainingEventStateMachine.TrainingEventController.EventBranch();
    }
}
#endregion

public class FinishEventState : TrainingEventStateBase
{
    public FinishEventState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public async override UniTask OnEnter()
    {
        if (TrainingEventStateMachine.CurrentEventData.IsBranch)
        {
            await TrainingEventStateMachine.ChangeState(TrainingEventStateType.EventBranch);
        }
        else
        {
            await GetEventBonus();
            await TrainingEventStateMachine.ChangeState(TrainingEventStateType.EventLoadingState);
        }
    }

    /// <summary> イベントボーナスを受け取る処理 </summary>
    public async UniTask GetEventBonus()
    {
        ParameterBuffCalculator parameterBuffCalculator = TrainingEventStateMachine.RaisingSimulationLifeTimeScope.Container.Resolve<ParameterBuffCalculator>();
        parameterBuffCalculator.SetTrainingEventBuff(TrainingEventStateMachine.CurrentEventData);
        parameterBuffCalculator.OnTrainingEventBuff();

        if(!parameterBuffCalculator.IsTotalBuffZero())
            await TrainingEventStateMachine.TrainingEventController.GetEventBonus();
    }
}

