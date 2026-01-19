using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RaisingSimulationGameFlowStateMachine
{
    public enum ScreenType
    {
        CharacterSelect,
        TrainingSelectMenu,
        TrainingEvent,
        Result
    }

    public abstract class ScreenState : State<ScreenType>
    {
        protected ScreenBase _screen;

        public override async UniTask OnEnter()
        {
            _screen.gameObject.SetActive(true);
            await _screen.FadeInScreen();
        }

        public override async UniTask OnExit()
        {
            await _screen.FadeOutScreen();
            _screen.gameObject.SetActive(false);
        }
    }


    /// <summary> 育成ゲームの進行状況を管理するステートマシン </summary>
    public class GameFlowStateMachine : StateMachine<ScreenType>
    {
        [SerializeField] private RaisingSimulationLifeTimeScope _lifeTimeScope;

        [Header("各種スクリーン")]
        [SerializeField] private CharacterSelectScreen _characterSelectScreen;
        [SerializeField] private TrainingSelectScreen _trainingSelectScreen;
        [SerializeField] private TrainingEventScreen _trainingEventScreen;
        [SerializeField] private ResultScreen _resultScreen;

        [SerializeField, Header("シーン遷移してすぐのフェードアウト中の画面")]
        private GameObject _fadeScreen;

        private DataLoadCompleteNotifier _loadingNotifier;
        private ScreenType _currentScreen;

        public void Awake()
        {
            //最初に全てのスクリーンの状態を非アクティブにしておく
            _characterSelectScreen.gameObject.SetActive(false);
            _trainingSelectScreen.gameObject.SetActive(false);
            _trainingEventScreen.gameObject.SetActive(false);
            _resultScreen.gameObject.SetActive(false);

            //DataLoadが済んだタイミングでゲームを動かし始める
            _fadeScreen.SetActive(true);
            _loadingNotifier = _lifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();
            _loadingNotifier.OnDataLoadComplete += StartGameFlow;
        }

        public void OnDisable()
        {
            _loadingNotifier.OnDataLoadComplete -= StartGameFlow;
        }

        public async void StartGameFlow()
        {
            Debug.Log("ゲームスタート");
            _stateDict.Add(ScreenType.CharacterSelect, new CharacterSelectScreenState(_characterSelectScreen, this));
            _stateDict.Add(ScreenType.TrainingSelectMenu, new TrainingSelectScreenState(_trainingSelectScreen, this));
            _stateDict.Add(ScreenType.TrainingEvent, new TrainingEventScreenState(_trainingEventScreen, this));
            _stateDict.Add(ScreenType.Result, new ResultScreenState(_resultScreen, this));

            JsonTrainingSaveDataRepository tSaveData = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();

            //セーブデータを確認してデータが残っていれば途中の画面からスタート
            tSaveData.SetData(new TrainingData());
            _currentScreen = ScreenType.CharacterSelect;
            await ChangeState(ScreenType.CharacterSelect);
            Debug.Log(_currentScreen);

        }

        public override async UniTask ChangeState(ScreenType stateType)
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
    public class CharacterSelectScreenState : ScreenState
    {
        public CharacterSelectScreenState(CharacterSelectScreen characterSelectScreen, StateMachine<ScreenType> stateMachine)
        {
            _screen = characterSelectScreen;
            _stateMachine = stateMachine;
        }
    }

    /// <summary> トレーニング選択画面のステート </summary>
    public class TrainingSelectScreenState : ScreenState
    {
        public TrainingSelectScreenState(TrainingSelectScreen trainingSelectScreen, StateMachine<ScreenType> stateMachine)
        {
            _screen = trainingSelectScreen;
            _stateMachine = stateMachine;
        }
    }

    /// <summary> トレーニングイベント画面のステート </summary>
    public class TrainingEventScreenState : ScreenState
    {
        public TrainingEventScreenState(TrainingEventScreen trainingEventScreen, StateMachine<ScreenType> stateMachine)
        {
            _screen = trainingEventScreen;
            _stateMachine = stateMachine;
        }
    }

    /// <summary> キャラクター育成結果表示画面のステート </summary>
    public class ResultScreenState : ScreenState
    {
        public ResultScreenState(ResultScreen resultScreen, StateMachine<ScreenType> stateMachine)
        {
            _screen = resultScreen;
            _stateMachine = stateMachine;
        }
    }
}


