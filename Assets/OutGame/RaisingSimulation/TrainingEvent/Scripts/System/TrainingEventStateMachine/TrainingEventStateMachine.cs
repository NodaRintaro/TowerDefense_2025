using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public enum TrainingEventStateType
{
    None,
    EventStartState,
    ReadScenario,
    EventBranch,
    RaidEvent,
    FinishEvent
}

#region ステートのベースクラス
/// <summary> ステートのベースクラス </summary>
public class TrainingEventStateBase : State<TrainingEventStateType>
{
    List<ITrainingEventStateOnEnterAction> _onEnterActionList = new();

    List<ITrainingEventStateOnExitAction> _onExitActionList = new();

    protected TrainingEventStateMachine _trainingEventStateMachine => _stateMachine as TrainingEventStateMachine;

    /// <summary> このステートに遷移した時に行う処理を追加する </summary>
    public void AddEnterAction(ITrainingEventStateOnEnterAction action)
    {
        _onEnterActionList.Add(action);
    }

    /// <summary> 別のステートへの遷移時に行う処理を追加する </summary>
    public void AddExitAction(ITrainingEventStateOnExitAction action)
    {
        _onExitActionList.Add(action);
    }

    /// <summary> このステートに遷移した時に行う処理 </summary>
    public async override UniTask OnEnter()
    {
        foreach (var action in _onEnterActionList)
        {
            await action.OnEnterAction();
        }
    }

    /// <summary> 別のステートへの遷移時に行う処理 </summary>
    public async override UniTask OnExit()
    {
        foreach(var action in _onExitActionList)
        {
            await action.OnExitAction();
        }
    }
}
#endregion

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
        _stateDict.Add(TrainingEventStateType.FinishEvent, new FinishEventState(this));
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
public class EventStartState : TrainingEventStateBase
{
    public EventStartState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }
}
#endregion

#region イベントシナリオを読んでいる間の処理
public class ReadScenarioState : TrainingEventStateBase
{
    public ReadScenarioState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
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
        
    }
}
#endregion

#region イベント終了時の処理
public class FinishEventState : TrainingEventStateBase
{
    public FinishEventState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }
}
#endregion

