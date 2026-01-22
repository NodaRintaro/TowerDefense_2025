using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UniRx;
using VContainer;
using static UnityEngine.Rendering.DebugUI;

public enum TrainingEventStateType
{
    None,
    EventLoading,
    ReadingNovel,
    FinishedReadScenario,
    EventBranch,
    StaminaValueBranchEvent,
    SelectBranchEventButton,
    RaidEvent,
    GetEventBonus,
    CheckCurrentElapsedDays,
    BackTrainingSelectScreen,
    FinishRaisingSimulation
}

public abstract class TrainingEventStateBase : State<TrainingEventStateType>
{
    public TrainingEventStateMachine TrainingEventStateMachine => _stateMachine as TrainingEventStateMachine;
}

//トレーニングで行われるイベントを全て管理するステートマシン
public class TrainingEventStateMachine : StateMachine<TrainingEventStateType>
{
    //現在のState
    private TrainingEventStateType _currentStateType = TrainingEventStateType.None;

    //現在のイベントのID
    private uint _currentEventID = 0;

    //現在のシナリオデータ
    private ScenarioData _currentScenarioData;

    //現在の行われているイベントの種類
    private TrainingEventType _currentEventType = TrainingEventType.None;

    //現在進行中のイベント
    private TrainingEventData _currentEventData = null;

    //育成ゲーム全体のLifeTimeScope
    private RaisingSimulationLifeTimeScope _raisingSimulationLifeTimeScope;

    //シナリオデータのリポジトリ
    private AddressableTrainingEventScenarioDataRepository _trainingEventScenarioDataRepository;
    private AddressableCharacterEventScenarioDataRepository _characterEventScenarioDataRepository;
    private AddressableSupportCardEventScenarioDataRepository _supportCardEventScenarioDataRepository;

    public ReactiveProperty<TrainingEventStateType> CurrentStateType 
    {
        private set { value.Value = _currentStateType; }
        get { return CurrentStateType; }
    }

    public TrainingEventType CurrentEventType => _currentEventType;
    public TrainingEventData CurrentEventData => _currentEventData;
    public ScenarioData CurrentScenario => _currentScenarioData;
    public RaisingSimulationLifeTimeScope RaisingSimulationLifeTimeScope => _raisingSimulationLifeTimeScope;
    public TrainingEventPool EventPool => _raisingSimulationLifeTimeScope.Container.Resolve<TrainingEventPool>();
    public TrainingSaveData CurrentTrainingSaveData => _raisingSimulationLifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>().RepositoryData;

    private void Awake()
    {
        _raisingSimulationLifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _stateDict.Add(TrainingEventStateType.None, null);
        _stateDict.Add(TrainingEventStateType.EventLoading, new EventLoadingState(this));
        _stateDict.Add(TrainingEventStateType.ReadingNovel, new ReadingNovelState(this));
        _stateDict.Add(TrainingEventStateType.EventBranch, new EventBranchState(this));
        _stateDict.Add(TrainingEventStateType.StaminaValueBranchEvent, new StaminaValueBranchEventState(this));
        _stateDict.Add(TrainingEventStateType.SelectBranchEventButton, new SelectBranchEventButtonState(this));
        _stateDict.Add(TrainingEventStateType.RaidEvent, new RaidEventState(this));
        _stateDict.Add(TrainingEventStateType.GetEventBonus, new GetEventBonusState(this));
        _stateDict.Add(TrainingEventStateType.CheckCurrentElapsedDays, new CheckCurrentElapsedDaysState(this));
        _stateDict.Add(TrainingEventStateType.BackTrainingSelectScreen, new BackTrainingSelectScreenState(this));
        _stateDict.Add(TrainingEventStateType.FinishRaisingSimulation, new FinishRaisingSimulationState(this));
    }

    private async void OnEnable()
    {
        _currentState = null;
        SetCurrentEventType(TrainingEventType.TrainingEvent);
        await ChangeState(TrainingEventStateType.EventLoading);
    }

    public override async UniTask ChangeState(TrainingEventStateType trainingEventState)
    {
        //現在のStateがNoneになった場合すべてのイベントを終了したとみなしScreenを切り替える
        if (_stateDict[trainingEventState] == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        await _currentState.OnExit();

        //次のステートに切り替える
        _currentState = _stateDict[trainingEventState];
        _currentStateType = trainingEventState;
        await _currentState.OnEnter();
    } 

    public void SetCurrentEventType(TrainingEventType trainingEventType)
    {
        _currentEventType = trainingEventType;
    }

    public void SetCurrentTrainingEvent(TrainingEventData trainingEventData)
    {
        _currentEventData = trainingEventData;
    }

    public void SetCurrentScenarioData(ScenarioData scenarioData)
    {
        _currentScenarioData = scenarioData;
    }
}

#region EventLoadingState
public class EventLoadingState : TrainingEventStateBase
{
    //イベントデータのリポジトリ
    private AddressableTrainingEventDataRepository _addressableTrainingEventDataRepository;
    private AddressableCharacterEventDataRepository _addressableCharacterEventDataRepository;
    private AddressableSupportCardEventDataRepository _addressableSupportCardEventDataRepository;

    //シナリオデータのリポジトリ
    private AddressableTrainingEventScenarioDataRepository _trainingEventScenarioDataRepository;
    private AddressableCharacterEventScenarioDataRepository _characterEventScenarioDataRepository;
    private AddressableSupportCardEventScenarioDataRepository _supportCardEventScenarioDataRepository;

    public EventLoadingState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;

        _addressableTrainingEventDataRepository = TrainingEventStateMachine.RaisingSimulationLifeTimeScope.Container.Resolve<AddressableTrainingEventDataRepository>();
        _addressableCharacterEventDataRepository = TrainingEventStateMachine.RaisingSimulationLifeTimeScope.Container.Resolve<AddressableCharacterEventDataRepository>();
        _addressableSupportCardEventDataRepository = TrainingEventStateMachine.RaisingSimulationLifeTimeScope.Container.Resolve<AddressableSupportCardEventDataRepository>();
        _trainingEventScenarioDataRepository = TrainingEventStateMachine.RaisingSimulationLifeTimeScope.Container.Resolve<AddressableTrainingEventScenarioDataRepository>();
        _characterEventScenarioDataRepository = TrainingEventStateMachine.RaisingSimulationLifeTimeScope.Container.Resolve<AddressableCharacterEventScenarioDataRepository>();
        _supportCardEventScenarioDataRepository = TrainingEventStateMachine.RaisingSimulationLifeTimeScope.Container.Resolve<AddressableSupportCardEventScenarioDataRepository>();
    }

    public override async UniTask OnEnter()
    {
        await LoadTrainingEvent();
    }

    private async UniTask LoadTrainingEvent()
    {
        if (TrainingEventStateMachine.EventPool.IsEventQueueEmpty(TrainingEventStateMachine.CurrentEventType))
        {
            switch(TrainingEventStateMachine.CurrentEventType)
            {
                case TrainingEventType.TrainingEvent:
                    TrainingEventStateMachine.SetCurrentEventType(TrainingEventType.CharacterUniqueEvent);
                    await LoadTrainingEvent();
                    break;
                case TrainingEventType.CharacterUniqueEvent:
                    TrainingEventStateMachine.SetCurrentEventType(TrainingEventType.SupportCardEvent);
                    await LoadTrainingEvent();
                    break;
                case TrainingEventType.SupportCardEvent:
                    TrainingEventStateMachine.SetCurrentEventType(TrainingEventType.None);
                    await TrainingEventStateMachine.ChangeState(TrainingEventStateType.CheckCurrentElapsedDays);
                    break;
            }
        }
        else
        {
            TrainingEventStateMachine.SetCurrentTrainingEvent(GenerateTrainingEvent(TrainingEventStateMachine.EventPool.DequeueData(TrainingEventStateMachine.CurrentEventType)));
            TrainingEventStateMachine.SetCurrentScenarioData(GenerateScenarioData(TrainingEventStateMachine.CurrentEventData.NovelEventID));
        }
    }

    private TrainingEventData GenerateTrainingEvent(uint eventID)
    {
        switch(TrainingEventStateMachine.CurrentEventType)
        {
            case TrainingEventType.TrainingEvent:
                return TrainingEventDataGenerator.GenerateEventData(_addressableTrainingEventDataRepository.GetCsvEventData(eventID));
            
            case TrainingEventType.CharacterUniqueEvent:
                return TrainingEventDataGenerator.GenerateEventData(_addressableCharacterEventDataRepository.GetCsvEventData(TrainingEventStateMachine.CurrentTrainingSaveData.TrainingCharacterData.CharacterID, eventID));

            case TrainingEventType.SupportCardEvent:
                return TrainingEventDataGenerator.GenerateEventData(_addressableSupportCardEventDataRepository.GetCsvEventData(eventID));
        }
        return null;
    }

    private ScenarioData GenerateScenarioData(uint eventID)
    {
        switch (TrainingEventStateMachine.CurrentEventType)
        {
            case TrainingEventType.TrainingEvent:
                return _trainingEventScenarioDataRepository.GetScenarioData(eventID);

            case TrainingEventType.CharacterUniqueEvent:
                return _characterEventScenarioDataRepository.GetScenarioData
                    (TrainingEventStateMachine.CurrentTrainingSaveData.TrainingCharacterData.CharacterID, eventID);

            case TrainingEventType.SupportCardEvent:
                return _supportCardEventScenarioDataRepository.GetScenarioData(eventID);
        }

        return null;
    }
}
#endregion

#region ReadingNovelState
public class ReadingNovelState : TrainingEventStateBase
{
    public ReadingNovelState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }
}
#endregion

#region ReadingNovelState
public class FinishedReadScenarioState : TrainingEventStateBase
{
    public FinishedReadScenarioState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public override async UniTask OnEnter()
    {
        if (TrainingEventStateMachine.CurrentEventData.IsBranch)
        {
            await TrainingEventStateMachine.ChangeState(TrainingEventStateType.EventBranch);
        }
        else
        {
            await TrainingEventStateMachine.ChangeState(TrainingEventStateType.GetEventBonus);
        }
    }
}
#endregion

#region EventBranchState
public class EventBranchState : TrainingEventStateBase
{
    public EventBranchState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public override async UniTask OnEnter()
    {
        switch(TrainingEventStateMachine.CurrentEventData.BranchType)
        {
            case EventBranchType.StaminaValue:
                await TrainingEventStateMachine.ChangeState(TrainingEventStateType.StaminaValueBranchEvent);
                break;
            case EventBranchType.Button:
                await TrainingEventStateMachine.ChangeState(TrainingEventStateType.SelectBranchEventButton);
                break;
            case EventBranchType.RaidResult:
                await TrainingEventStateMachine.ChangeState(TrainingEventStateType.RaidEvent);
                break;
        }
    }
}
#endregion

#region StaminaValueBranchEventState
public class StaminaValueBranchEventState : TrainingEventStateBase
{
    public StaminaValueBranchEventState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }


    public override async UniTask OnEnter()
    {

    }
}
#endregion

#region SelectBranchEventButtonState
public class SelectBranchEventButtonState : TrainingEventStateBase
{
    public SelectBranchEventButtonState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }


    public override async UniTask OnEnter()
    {

    }

    public override async UniTask OnExit()
    {

    }
}
#endregion

#region GetEventBonusState
public class GetEventBonusState : TrainingEventStateBase
{
    public GetEventBonusState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public override async UniTask OnEnter()
    {

    }

    public override async UniTask OnExit()
    {

    }
}
#endregion

#region RaidEventState
public class RaidEventState : TrainingEventStateBase
{
    public RaidEventState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public override async UniTask OnEnter()
    {

    }

    public override async UniTask OnExit()
    {

    }
}
#endregion

#region CheckCurrentElapsedDaysState
public class CheckCurrentElapsedDaysState : TrainingEventStateBase
{
    public CheckCurrentElapsedDaysState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }


    public override async UniTask OnEnter()
    {

    }

    public override async UniTask OnExit()
    {

    }
}
#endregion

#region BackTrainingSelectScreen
public class BackTrainingSelectScreenState : TrainingEventStateBase
{
    public BackTrainingSelectScreenState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public override async UniTask OnEnter()
    {

    }

    public override async UniTask OnExit()
    {

    }
}
#endregion

#region FinishRaisingSimulationState
public class FinishRaisingSimulationState : TrainingEventStateBase
{
    public FinishRaisingSimulationState(TrainingEventStateMachine trainingEventStateMachine)
    {
        _stateMachine = trainingEventStateMachine;
    }

    public override async UniTask OnEnter()
    {

    }

    public override async UniTask OnExit()
    {

    }
}
#endregion



