using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
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
public class NovelEventPresenter : MonoBehaviour, IStateOnEnterAction, IStateOnExitAction
{
    [SerializeField] private TrainingEventScreen _trainingEventScreen = null;

    [SerializeField] private TrainingEventStateMachine _trainingEventStateMachine = null;

    [SerializeField] private TrainingNovelEventView _trainingNovelEventView = null;

    [SerializeField] private BranchEventSelectButtonView _branchEventSelectButtonView = null;

    [SerializeField] private ParameterEventBonusView _trainingEventBonusParameterView = null;

    private RaisingSimulationDataContainer _lifeTimeScope = null;
    private TrainingSaveData _trainingSaveData = null;
    private TrainingEventDataGenerator _trainingEventDataGenerator = null;
    private DataLoadCompleteNotifier _dataLoadCompleteNotifier = null;

    private EventInputActionType _currentAction = EventInputActionType.Inactive;

    private void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationDataContainer>();
        _trainingEventStateMachine = FindFirstObjectByType<TrainingEventStateMachine>();
        _dataLoadCompleteNotifier = _lifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();

        //リポジトリデータのロード完了を確認してデータを取得する
        if (_dataLoadCompleteNotifier.IsDataLoadComplete)
            DataResolve();
        else _dataLoadCompleteNotifier.OnDataLoadComplete += DataResolve;
    }

    public async UniTask OnEnterAction()
    {
        
    }

    public async UniTask OnExitAction()
    {
        
    }

    public void SetActionStateType()
    {

    }

    private void DataResolve()
    {
        _trainingSaveData = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>().RepositoryData;
        _trainingEventDataGenerator = _lifeTimeScope.Container.Resolve<TrainingEventDataGenerator>();

        _dataLoadCompleteNotifier.OnDataLoadComplete -= DataResolve;
    }

    /// <summary> プレーヤーの入力によって行う処理を変更 </summary>
    public void ChangeEventAction(EventInputActionType eventActionType)
    {
        _currentAction = eventActionType;
    }

    /// <summary>現在のシナリオをViewに反映する処理</summary>
    public void SetReadScenarioData(ScenarioData scenarioData)
    {
        _trainingNovelEventView.SetScenarioData(scenarioData);
        _trainingNovelEventView.ChangeNovelEventPlayerActions(TrainingNovelEventView.NovelEventPlayerActions.ReadScenario);
    }

    /// <summary> 現在のシナリオを読み終えた際の処理 </summary>
    public async UniTask OnFinishedReadScenarioEvent()
    {
        
    }
}