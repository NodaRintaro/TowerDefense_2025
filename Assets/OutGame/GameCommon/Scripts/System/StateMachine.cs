using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<TStateType>
{
    protected StateMachine<TStateType> _stateMachine;

    public virtual UniTask OnEnter()
    {
        return UniTask.CompletedTask;
    }
    public virtual UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }
}


public abstract class StateMachine<TStateType> : MonoBehaviour
{
    protected State<TStateType> _currentState;

    protected Dictionary<TStateType, State<TStateType>> _stateDict = new Dictionary<TStateType, State<TStateType>>();

    /// <summary> 現在のステートの処理を終了し、次のステートへ移行する </summary>
    public abstract UniTask ChangeState(TStateType stateType);
}
