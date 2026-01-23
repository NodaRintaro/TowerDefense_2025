using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;
using VContainer;
using TMPro;

/// <summary>
/// プレイヤーの入力によって行われる現在の処理
/// </summary>
public enum EventInputActionType
{
    Inactive,
    WaitForEvent,
    ReadScenarioEvent,
    EventSelect,
    FinishEvent
}

/// <summary> プレイヤーからの入力を受けてトレーニングイベントを進行するClass </summary>
public class TrainingEventController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TrainingEventScreen _trainingEventScreen;

    [SerializeField] private TrainingEventStateMachine _trainingEventStateMachine;

    [SerializeField] private TrainingNovelEventView _trainingNovelEventView;

    [SerializeField] private BranchEventSelectButtonView _branchEventSelectButtonView;

    [SerializeField] private TrainingEventParameterView _trainingEventBonusParameterView;

    private RaisingSimulationLifeTimeScope _lifeTimeScope;
    private TrainingSaveData _trainingSaveData;
    private TrainingEventDataGenerator _trainingEventDataGenerator;

    private EventInputActionType _currentAction = EventInputActionType.Inactive;

    private void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();
        _trainingEventStateMachine = FindFirstObjectByType<TrainingEventStateMachine>();
        DataLoadCompleteNotifier dataLoadCompleteNotifier = _lifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();

        if (dataLoadCompleteNotifier.IsDataLoadComplete)
        {
            DataResolve();
        }
        else
        {
            dataLoadCompleteNotifier.OnDataLoadComplete += DataResolve;
        }
    }

    private void OnEnable()
    {
        _currentAction = EventInputActionType.WaitForEvent;
    }

    private void OnDisable()
    {
        _currentAction = EventInputActionType.Inactive;
    }

    public async void OnPointerClick(PointerEventData eventData)
    {
        await OnClickEventAction();
        Debug.Log("Click");
    }

    private void DataResolve()
    {
        _trainingSaveData = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>().RepositoryData;
        _trainingEventDataGenerator = _lifeTimeScope.Container.Resolve<TrainingEventDataGenerator>();
    }

    public async UniTask StartTraining()
    {
        Debug.Log("トレーニング開始");
        await _trainingEventStateMachine.ChangeState(TrainingEventStateType.EventLoadingState);
    }

    /// <summary> プレーヤーの入力によって行う処理を変更 </summary>
    public void ChangeEventAction(EventInputActionType eventActionType)
    {
        _currentAction = eventActionType;
    }

    /// <summary>現在のシナリオをViewに反映する処理</summary>
    public void SetReadScenario(ScenarioData scenarioData)
    {
        _trainingNovelEventView.SetScenario(scenarioData);
        _trainingNovelEventView.ChangeNovelEventPlayerActions(TrainingNovelEventView.NovelEventPlayerActions.ReadScenario);
    }

    /// <summary> イベント進行時に呼ばれる処理 </summary>
    public async UniTask OnClickEventAction()
    {
        switch (_currentAction)
        {
            case EventInputActionType.ReadScenarioEvent:
                await ReadScenario();
                break;
        }
    }

    /// <summary> ノベルイベントの際の処理 </summary>
    public async UniTask ReadScenario()
    {
        await _trainingNovelEventView.OnScenarioReadingAction();
    }

    /// <summary> 現在のシナリオを読み終えた際の処理 </summary>
    public async UniTask FinishedReadScenario()
    {
        await _trainingEventStateMachine.OnFinishReadScenario();
    }

    /// <summary> 分岐イベントの処理 </summary>
    public async UniTask EventBranch()
    {
        switch (_trainingEventStateMachine.CurrentEventData.BranchType)
        {
            case EventBranchType.StaminaValue:
                Debug.Log("イベント分岐：スタミナ");
                await EventBranchWithStaminaValueStaminaValue();
                break;
            case EventBranchType.Button:
                GenerateBranchEventSelectButton();
                break;
        }


    }

    private async UniTask EventBranchWithStaminaValueStaminaValue()
    {
        List<TrainingEventData> branchDataList = _trainingEventDataGenerator.GenerateBranchEvent
            (_trainingEventStateMachine.CurrentEventType, _trainingEventStateMachine.CurrentEventData.EventID);

        Debug.Log(branchDataList.Count);
        switch (TrainingSuccessDecider.TrySuccessTrainingEvent(_trainingSaveData.CurrentStamina))
        {
            case EventBranchType.TrainingFailed:
                foreach(var data in branchDataList)
                {
                    if(data.BranchType == EventBranchType.TrainingFailed)
                    {
                        _trainingEventStateMachine.SetCurrentTrainingEvent(data);

                        Debug.Log(data.ScenarioID);
                        Debug.Log(_trainingEventStateMachine.CurrentEventType);
                        ScenarioData scenarioData = _trainingEventDataGenerator.GenerateScenarioData
                            (_trainingEventStateMachine.CurrentEventType, data.ScenarioID);

                        _trainingEventStateMachine.SetCurrentScenarioData(scenarioData);
                        SetReadScenario(scenarioData);

                        Debug.Log("シナリオセット完了");
                    }    
                }
                break;
            case EventBranchType.TrainingSuccess:
                foreach (var data in branchDataList)
                {
                    if (data.BranchType == EventBranchType.TrainingSuccess)
                    {
                        _trainingEventStateMachine.SetCurrentTrainingEvent(data);

                        Debug.Log(data.ScenarioID);
                        Debug.Log(_trainingEventStateMachine.CurrentEventType);
                        ScenarioData scenarioData = _trainingEventDataGenerator.GenerateScenarioData
                            (_trainingEventStateMachine.CurrentEventType, data.ScenarioID);

                        _trainingEventStateMachine.SetCurrentScenarioData(scenarioData);
                        SetReadScenario(scenarioData);
                        Debug.Log("シナリオセット完了");
                    }
                }
                break;
        }

        await _trainingEventStateMachine.ChangeState(TrainingEventStateType.ReadScenario);
    }

    /// <summary> 分岐イベントの選択ボタンを生成する </summary>
    private void GenerateBranchEventSelectButton()
    {
        List<TrainingEventData> trainingEventDataList = _trainingEventDataGenerator.GenerateBranchEvent
            (_trainingEventStateMachine.CurrentEventType, _trainingEventStateMachine.CurrentEventData.EventID);

        foreach (TrainingEventData eventData in trainingEventDataList)
        {
            Button selectButton = _branchEventSelectButtonView.GenerateSelectButton(eventData.EventName);
            selectButton.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text = eventData.EventName;
            selectButton.onClick.AddListener(async () =>
            {
                await OnClickSelectBranchButtonEvent(eventData);
            });
        }
    }

    /// <summary> ボタンによる分岐イベントの選択時の処理 </summary>
    private async UniTask OnClickSelectBranchButtonEvent(TrainingEventData eventData)
    {
        _trainingEventStateMachine.SetCurrentTrainingEvent(eventData);
        _trainingEventStateMachine.SetCurrentScenarioData(_trainingEventDataGenerator.GenerateScenarioData
            (_trainingEventStateMachine.CurrentEventType, eventData.ScenarioID));

        await UniTask.Delay(200);

        _branchEventSelectButtonView.ButtonGenerator.ReleaseAllButtons();
        await _trainingEventStateMachine.ChangeState(TrainingEventStateType.ReadScenario);
    }

    /// <summary> イベント終了時の報酬の受け取り </summary>
    public async UniTask GetEventBonus()
    {
        await _trainingEventBonusParameterView.TrainingBonusBuffEvent(_trainingEventStateMachine.CurrentTrainingSaveData.TrainingCharacterData);
    }

    public async UniTask ChangeScenarioFadeIn()
    {
        await _trainingEventScreen.FadeInScreen();
    }

    public async UniTask ChangeScenarioFadeOut()
    {
        await _trainingEventScreen.FadeOutScreen();
    }
}