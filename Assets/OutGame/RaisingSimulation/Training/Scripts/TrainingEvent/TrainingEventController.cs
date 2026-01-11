using UnityEngine;
using UnityEngine.EventSystems;
using NovelData;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public enum EventScreenAdvanceType
{
    None,
    WriteScenario,
    ReadScenario,
    GetTrainingBonus
}

public class TrainingEventController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] NovelEventView _novelView;
    [SerializeField] TrainingEventBonusView _trainingBonusView;

    private TrainingEventData _currentTrainingEventData;
    private NovelEventData _currentNovelEventData;

    private RaisingSimulationLifeTimeScope _lifeTimeScope;
    private AddressableTrainingEventDataRepository _trainingEventDataRepository;
    private AddressableNovelEventDataRepository _novelEventDataRepository;
    private AddressableRankImageDataRepository _rankImageDataRepository;
    private AddressableCharacterImageDataRepository _characterImageDataRepository;
    private TrainingEventPool _eventPool;

    //現在の画面の状況
    private EventScreenAdvanceType _currentEventAdvance = EventScreenAdvanceType.None;

    //イベントの進行度合い
    private int _novelPageCount = 0;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _trainingEventDataRepository = _lifeTimeScope.Container.Resolve<AddressableTrainingEventDataRepository>();
        _novelEventDataRepository = _lifeTimeScope.Container.Resolve<AddressableNovelEventDataRepository>();
        _characterImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
        _rankImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableRankImageDataRepository>();
        _eventPool = _lifeTimeScope.Container.Resolve<TrainingEventPool>();
    }

    public async void OnEnable()
    {
        _currentEventAdvance = EventScreenAdvanceType.None;
        uint eventID = _eventPool.DequeueData();
        _currentTrainingEventData = _trainingEventDataRepository.RepositoryData.GetData(eventID);
        _currentNovelEventData = _novelEventDataRepository.RepositoryData.GetData(_currentTrainingEventData.NovelEventID);
        await EventOnEnter();
    }

    /// <summary> 画面がクリックされたときに実行される処理 </summary>
    public async void OnPointerClick(PointerEventData eventData)
    {
        switch(_currentEventAdvance)
        {
            case EventScreenAdvanceType.WriteScenario:
                _novelView.DialogueText.SkipText();
                break;
            case EventScreenAdvanceType.ReadScenario:
                await EventAdvance();
                break;
            case EventScreenAdvanceType.GetTrainingBonus:
                EventExit();
                break;
        }
    }

    /// <summary> イベント開始時の処理 </summary>
    public async UniTask EventOnEnter()
    {
        _currentEventAdvance = EventScreenAdvanceType.WriteScenario;
        await _novelView.DialogueText.ShowTextAsync(_currentNovelEventData.NovelData[_novelPageCount].ScenarioData);
        _currentEventAdvance = EventScreenAdvanceType.ReadScenario;
        _novelPageCount++;
    }

    /// <summary> イベントを進める処理 </summary>
    public async UniTask EventAdvance()
    {
        if (_currentNovelEventData.NovelData.Length < _novelPageCount)
        {
            _currentEventAdvance = EventScreenAdvanceType.GetTrainingBonus;
            GetTrainingBonus();
        }
        else
        {
            _currentEventAdvance = EventScreenAdvanceType.WriteScenario;
            await _novelView.DialogueText.ShowTextAsync(_currentNovelEventData.NovelData[_novelPageCount].ScenarioData);
            _currentEventAdvance = EventScreenAdvanceType.ReadScenario;
        }
    }

    /// <summary> イベント終了時の処理 </summary>
    public void EventExit()
    {
        _novelPageCount = 0;
        _currentNovelEventData = null;
        _currentTrainingEventData = null;

        if (_eventPool.IsEventQueueEmpty())
        {
            BackTrainingSelectScreen();
        }
        else
        {
            NextEvent();
        }
    }

    /// <summary> 次のEventへ移る処理 </summary>
    public void NextEvent()
    {

    }

    /// <summary> シナリオ終了後にキャラクターの強化やSkillのかくとくをおこなう </summary>
    public void GetTrainingBonus()
    {

    }

    /// <summary> トレーニング選択画面へ移る処理 </summary>
    public void BackTrainingSelectScreen()
    {

    }
}
