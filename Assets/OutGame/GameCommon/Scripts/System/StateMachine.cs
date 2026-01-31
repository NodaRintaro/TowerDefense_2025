using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<TStateType>
{
    protected StateMachine<TStateType> _stateMachine;

    List<IStateOnEnterAction> _onEnterActionList = new();

    List<IStateOnExitAction> _onExitActionList = new();

    /// <summary> このステートに遷移した時に行う処理を追加する </summary>
    public void AddEnterAction(IStateOnEnterAction action)
    {
        _onEnterActionList.Add(action);
    }

    /// <summary> 別のステートへの遷移時に行う処理を追加する </summary>
    public void AddExitAction(IStateOnExitAction action)
    {
        _onExitActionList.Add(action);
    }

    /// <summary> このステートに遷移した時に行う処理 </summary>
    public async UniTask OnEnter()
    {
        foreach (var action in _onEnterActionList)
        {
            await action.OnEnterAction();
        }
    }

    /// <summary> 別のステートへの遷移時に行う処理 </summary>
    public async UniTask OnExit()
    {
        foreach (var action in _onExitActionList)
        {
            await action.OnExitAction();
        }
    }
}


public abstract class StateMachine<TStateType> : MonoBehaviour
{
    protected State<TStateType> _currentState;

    protected Dictionary<TStateType, State<TStateType>> _stateDict = new Dictionary<TStateType, State<TStateType>>();

    /// <summary> 現在のステートの処理を終了し、次のステートへ移行する </summary>
    public abstract UniTask ChangeState(TStateType stateType);
}
