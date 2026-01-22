using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using System.Threading.Tasks;

/// <summary> プレイヤーからの入力を受けてトレーニングイベントを進行するClass </summary>
public class TrainingEventController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TrainingEventStateMachine _trainingEventStateMachine;

    [SerializeField] private TrainingNovelEventView _trainingNovelEventView;

    [SerializeField] private BranchEventSelectButtonView _branchEventSelectButtonView;

    [SerializeField] private TrainingEventParameterView _trainingEventParameterView;

    private void Awake()
    {
        _trainingEventStateMachine = FindFirstObjectByType<TrainingEventStateMachine>();
    }

    private void Start()
    {
        _trainingEventStateMachine.CurrentStateType.Subscribe(x => { OnClickEvent().Forget(); }).AddTo(this);
    }

    public async void OnPointerClick(PointerEventData eventData)
    {
        await OnClickEvent();
    }

    /// <summary> イベント進行時に呼ばれる処理 </summary>
    public async UniTask OnClickEvent()
    {
        switch (_trainingEventStateMachine.CurrentStateType.Value)
        {
            case TrainingEventStateType.EventLoading:
                //新たなイベントをロードする処理
                _trainingNovelEventView.SetScenario(_trainingEventStateMachine.CurrentScenario);
                await _trainingEventStateMachine.ChangeState(TrainingEventStateType.ReadingNovel);
                break;
            case TrainingEventStateType.ReadingNovel:
                //シナリオを読み進める処理
                await _trainingNovelEventView.OnScenarioReadingAction();
                break;
            case TrainingEventStateType.StaminaValueBranchEvent:

                break;
            case TrainingEventStateType.SelectBranchEventButton:

                break;
            case TrainingEventStateType.GetEventBonus:

                break;
        }
    }

    /// <summary> 現在のシナリオを読み終えた際の処理 </summary>
    public async UniTask FinishedReadScenario()
    {
        await _trainingEventStateMachine.ChangeState(TrainingEventStateType.FinishedReadScenario);
    }
}