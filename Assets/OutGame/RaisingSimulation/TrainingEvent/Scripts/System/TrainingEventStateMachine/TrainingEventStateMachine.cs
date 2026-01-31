using Cysharp.Threading.Tasks;
using UnityEngine;

public enum TrainingEventStateType
{
    None,
    EventStartState,
    ReadScenario,
    EventBranch,
    RaidEvent,
    EventResult
}

/// <summary>
/// トレーニング中のイベントの生成と管理を行うステートマシン
/// </summary>
public class TrainingEventStateMachine : StateMachine<TrainingEventStateType>
{
    [SerializeField] private NovelEventPresenter _trainingEventController;

    /// <summary> 現在のステート </summary>
    private TrainingEventStateType _currentStateType;

    public TrainingEventStateType CurrentStateType => _currentStateType;

    private void Awake()
    {
        _currentStateType = TrainingEventStateType.None;

        _stateDict.Add(TrainingEventStateType.EventStartState, new EventStartState(this));
        _stateDict.Add(TrainingEventStateType.ReadScenario, new ReadScenarioState(this));
        _stateDict.Add(TrainingEventStateType.EventBranch, new EventBranchState(this));
        _stateDict.Add(TrainingEventStateType.EventResult, new EventResultState(this));
    }

    public override async UniTask ChangeState(TrainingEventStateType trainingEventState)
    {
        if (_currentState != null)
        {
            await _currentState.OnExit();
        }

        Debug.Log("現在のステート" + trainingEventState);
        //次のステートに切り替える
        _currentState = _stateDict[trainingEventState];
        _currentStateType = trainingEventState;
        await _currentState.OnEnter();
    }
}

#region イベント開始時のステート
public class EventStartState : State<TrainingEventStateType>
{
    public EventStartState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }
}
#endregion

#region イベントシナリオを読んでいる間の処理
public class ReadScenarioState : State<TrainingEventStateType>
{
    public ReadScenarioState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }
}
#endregion

#region イベント分岐時の処理
public class EventBranchState : State<TrainingEventStateType>
{
    public EventBranchState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }
}
#endregion

#region イベント終了時の処理
public class EventResultState : State<TrainingEventStateType>
{
    public EventResultState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }
}
#endregion

