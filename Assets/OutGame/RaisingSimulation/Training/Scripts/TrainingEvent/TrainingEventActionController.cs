using Cysharp.Threading.Tasks;
using NovelData;
using RaisingSimulationGameFlowStateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public enum EventPhase
{
    None,
    ReadNovel,
    GetBonus,
    ChangeEvent,
    StartEvent,
    FinishEvent
}

/// <summary> プレイヤーからの入力を受けてトレーニングイベントを進行するClass </summary>
public class TrainingEventFlowController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private NovelEventPersenter _novelEventPresenter = null;
    [SerializeField] private TrainingEventBonusController _trainingEventBonusController = null;
    [SerializeField] private TrainingEventFadeView _trainingEventFadeView = null;

    private EventPhase _currentEventPhase = EventPhase.None;

    private bool _isClickAction = false;

    //LifeTimeScope
    private RaisingSimulationLifeTimeScope _lifeTimeScope;

    private TrainingEventPool _trainingEventPool;
    private ParameterBuffCalculator _trainingCharacterParameterTotalBuffCalculator;
    private GameFlowStateMachine _gameFlowStateMachine;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _trainingEventPool = _lifeTimeScope.Container.Resolve<TrainingEventPool>();
        _trainingCharacterParameterTotalBuffCalculator = _lifeTimeScope.Container.Resolve<ParameterBuffCalculator>();
        _gameFlowStateMachine = _lifeTimeScope.Container.Resolve<GameFlowStateMachine>();
    }

    private async void OnEnable()
    {
        await StartEventFlow();
        _isClickAction = true;
    }

    private void OnDisable()
    {
        _currentEventPhase = EventPhase.None;
        _isClickAction = false;
    }

    public async void OnPointerClick(PointerEventData eventData)
    {
        if(_isClickAction) 
            await AdvanceTrainingEvent();
    }

    private async UniTask StartEventFlow()
    {
        //完全にフェードイン仕切るのを待つ分の時間
        int fadeInAfterDelayTime = 17500;

        //
        _novelEventPresenter.Init();
        await _trainingEventFadeView.FadeIn();

        //フェードインしきったらイベントを開始する
        await UniTask.Delay(fadeInAfterDelayTime);
        _currentEventPhase = EventPhase.StartEvent;
        await AdvanceTrainingEvent();
    }

    public async UniTask AdvanceTrainingEvent()
    {
        switch (_currentEventPhase)
        {
            //新たにイベントを開始する
            case EventPhase.StartEvent:

                if (_trainingEventPool.IsEventQueueEmpty())
                {
                    //取得したIDが0(Null)だった場合トレーニング選択画面に移行する
                    _currentEventPhase = EventPhase.FinishEvent;
                }
                else
                {
                    //取得したIDが0以外だった場合イベントを行う
                    _novelEventPresenter.SetNovelEvent(_trainingEventPool.DequeueData());
                    _currentEventPhase = EventPhase.ReadNovel;
                    await AdvanceTrainingEvent();
                }
                break;

            //シナリオを読む
            case EventPhase.ReadNovel:
                await _novelEventPresenter.EventScenarioReading();
                break;

            //トレーニングによって発生した報酬の受け取り
            case EventPhase.GetBonus:
                await GetTrainingBonus();

                //まだ行っていないイベントがあればイベントを切り替え、なければトレーニング選択画面へ戻る
                if (_trainingEventPool.IsEventQueueEmpty())
                    _currentEventPhase = EventPhase.FinishEvent;
                else　_currentEventPhase = EventPhase.ChangeEvent;

                await AdvanceTrainingEvent();
                break;

            //次のイベントへ向かう
            case EventPhase.ChangeEvent:
                _novelEventPresenter.SetNovelEvent(_trainingEventPool.DequeueData());
                await _trainingEventFadeView.FadeOut();
                _currentEventPhase = EventPhase.StartEvent;
                await AdvanceTrainingEvent();
                break;

            //トレーニング選択画面へ戻る
            case EventPhase.FinishEvent:
                await _gameFlowStateMachine.ChangeState(ScreenType.TrainingSelectMenu);
                break;
        }
    }

    /// <summary> シナリオが読み終わった際に次のフェーズへ移行する処理</summary>
    public void FinishReadNovel()
    {
        _currentEventPhase = EventPhase.GetBonus;
    }

    /// <summary> 次のイベントの有無の判定 </summary>
    private void IsNextEventEmpty()
    {
        _trainingEventPool.IsEventQueueEmpty();
    }


    /// <summary> トレーニングによって発生したボーナスを受け取る処理 </summary>
    private async UniTask GetTrainingBonus()
    {
        _trainingCharacterParameterTotalBuffCalculator.OnTrainingEventBuff();
        _trainingCharacterParameterTotalBuffCalculator.InitBuff();

        _isClickAction = false;
        await _trainingEventBonusController.TrainingBuffEvent();
        _isClickAction = true;
    }
}