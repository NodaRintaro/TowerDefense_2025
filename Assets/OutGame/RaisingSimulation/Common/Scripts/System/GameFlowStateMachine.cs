using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RaisingSimulationGameFlowStateMachine
{
    public enum ScreenStateType
    {
        CharacterSelect,
        TrainingSelectMenu,
        TrainingEvent,
        Result
    }

    /// <summary> 育成ゲームの進行状況を管理するステートマシン </summary>
    public class GameFlowStateMachine : StateMachine<ScreenStateType>
    {
        public void Start()
        {
            Debug.Log("ゲームスタート");
            _stateDict.Add(ScreenStateType.CharacterSelect, new CharacterSelectState(this));
            _stateDict.Add(ScreenStateType.TrainingSelectMenu, new TrainingSelectState(this));
            _stateDict.Add(ScreenStateType.TrainingEvent, new TrainingEventState(this));
            _stateDict.Add(ScreenStateType.Result, new ResultState(this));
        }

        public override async UniTask ChangeState(ScreenStateType stateType)
        {
            if (_stateDict[stateType] != null)
            {
                if (_currentState != null)
                {
                    await _currentState.OnExit();
                }

                _currentState = _stateDict[stateType];
                await _currentState.OnEnter();
            }
        }
    }

    /// <summary> キャラクター選択画面のステート </summary>
    public class CharacterSelectState : State<ScreenStateType>
    {
        public CharacterSelectState(StateMachine<ScreenStateType> stateMachine)
        {
            _stateMachine = stateMachine;
        }
    }

    /// <summary> トレーニング選択画面のステート </summary>
    public class TrainingSelectState : State<ScreenStateType>
    {
        public TrainingSelectState(StateMachine<ScreenStateType> stateMachine)
        {
            _stateMachine = stateMachine;
        }
    }

    /// <summary> トレーニングイベント画面のステート </summary>
    public class TrainingEventState : State<ScreenStateType>
    {
        public TrainingEventState(StateMachine<ScreenStateType> stateMachine)
        {
            _stateMachine = stateMachine;
        }
    }

    /// <summary> キャラクター育成結果表示画面のステート </summary>
    public class ResultState : State<ScreenStateType>
    {
        public ResultState(StateMachine<ScreenStateType> stateMachine)
        {
            _stateMachine = stateMachine;
        }
    }
}


