using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IState
{
    public void OnEnter();
    public void OnExit();
}


public abstract class StateMachine<TState> : MonoBehaviour
{
    protected IState _currentState;

    protected Dictionary<TState, IState> _stateDict = new Dictionary<TState, IState>();

    /// <summary> 現在のステートの処理を終了し、次のステートへ移行する </summary>
    /// <param name="stateType"></param>
    public abstract void ChangeState(TState stateType);
}
