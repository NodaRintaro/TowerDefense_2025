using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace RaisingSimulationGameFlowStateMachine
{
    public enum StateType
    {
        CharacterSelect,
        TrainingSelectMenu,
        TrainingEvent,
        Result
    }


    /// <summary>
    /// 育成ゲームの進行状況を管理するステートマシン
    /// </summary>
    public class GameFlowStateMachine : StateMachine<StateType>
    {
        [SerializeField] private RaisingSimulationLifeTimeScope _lifeTimeScope;

        public void Start()
        {
            _stateDict.Add(StateType.CharacterSelect, new CharacterSelectState());
            _stateDict.Add(StateType.TrainingSelectMenu, new TrainingSelectMenuState());
            _stateDict.Add(StateType.TrainingEvent, new TrainingEventState());
            _stateDict.Add(StateType.Result, new ResultState());

            ChangeState(StateType.CharacterSelect);
        }

        public override void ChangeState(StateType stateType)
        {
            if (_stateDict[stateType] != null)
            {
                if (_currentState != null)
                {
                    _currentState.OnExit();
                }

                _currentState = _stateDict[stateType];
                _currentState.OnEnter();
            }
            else
            {
                Debug.Log("ステートの切り替えに失敗しました");
            }
        }
    }

    public class CharacterSelectState : IState
    {
        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }
    }

    public class TrainingSelectMenuState : IState
    {
        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }
    }

    public class TrainingEventState : IState
    {
        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }
    }

    public class ResultState : IState
    {
        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }
    }
}


