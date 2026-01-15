using Cysharp.Threading.Tasks;
using NovelData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// Playerからの入力を受けとって
/// </summary>
public class NovelEventPersenter : MonoBehaviour
{
    [Header("ViewClass")]
    [SerializeField]private NovelEventView _novelView;
    [SerializeField]private BranchEventSelectButtonView _branchEventSelectButtonView;

    [Header("トレーニングイベントの進行管理クラス")]
    [SerializeField] private TrainingEventFlowController _trainingEventProgressor;

    [SerializeField]
    private TrainingSuccessDecider _trainingSuccessDecider = new TrainingSuccessDecider();

    //現在のプレイヤーが可能なアクション
    private NovelEventPlayerAction _trainingEventPlayerAction;

    //現在のシナリオデータ
    private ITrainingEventData _currentTrainingEventData = null;
    private NovelEventData _currentNovelEventData = null;

    //現在のページ
    private uint _currentPage = 0;

    //トレーニングキャラクターのID
    private uint _trainingCharacterID = 0;

    //LifeTimeScope
    private RaisingSimulationLifeTimeScope _lifeTimeScope;

    private ParameterBuffCalculator _trainingCharacterParameterTotalBuffCalculator;

    private JsonTrainingSaveDataRepository _jsonTrainingSaveData;
    private AddressableTrainingEventDataRepository _addressableTrainingEventDataRepository;
    private AddressableNovelEventDataRepository _addressableNovelEventDataRepository;
    private AddressableBranchTrainingEventDataRepository _trainingBranchEventDataRepository;
    private AddressableCharacterDataRepository _addressableCharacterDataRepository;
    private AddressableCharacterImageDataRepository _characterImageDataRepository;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _trainingCharacterParameterTotalBuffCalculator = _lifeTimeScope.Container.Resolve<ParameterBuffCalculator>();
        _jsonTrainingSaveData = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _addressableTrainingEventDataRepository = _lifeTimeScope.Container.Resolve<AddressableTrainingEventDataRepository>();
        _addressableNovelEventDataRepository = _lifeTimeScope.Container.Resolve<AddressableNovelEventDataRepository>();
        _trainingBranchEventDataRepository = _lifeTimeScope.Container.Resolve<AddressableBranchTrainingEventDataRepository>();
        _addressableCharacterDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();
        _characterImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();

        _trainingCharacterID = _jsonTrainingSaveData.RepositoryData.TrainingCharacterData.CharacterID;
    }

    public void Init()
    {
        _novelView.DialogueText.Init();
        _novelView.SetNameText(" ");
        _novelView.SetCharacterImage(null);
    }

    public void SetNovelEvent(TrainingEventData trainingEventData)
    {
        _currentTrainingEventData = trainingEventData;
        _currentNovelEventData = _addressableNovelEventDataRepository.RepositoryData.GetData(_currentTrainingEventData.NovelEventID);
        _currentPage = 0;
        _trainingEventPlayerAction = NovelEventPlayerAction.ReadScenario;
    }

    /// <summary> 画面がクリックされたときに実行される処理 </summary>
    public async UniTask EventScenarioReading()
    {
        switch (_trainingEventPlayerAction)
        {
            case NovelEventPlayerAction.SkipScenario:
                _novelView.DialogueText.SkipText();
                _trainingEventPlayerAction = NovelEventPlayerAction.ReadScenario;
                break;

            case NovelEventPlayerAction.ReadScenario:
                if (IsBranchEvent(_currentPage))
                {
                    EventBranch();
                }
                else
                {
                    await ScenarioReading(_currentPage);
                }
                _currentPage++;
                if (_currentNovelEventData.NovelData.Length <= _currentPage)
                {
                    _trainingEventPlayerAction = NovelEventPlayerAction.FinishedScenario;
                }
                break;

            case NovelEventPlayerAction.FinishedScenario:
                _trainingEventPlayerAction = NovelEventPlayerAction.None;
                _trainingCharacterParameterTotalBuffCalculator.SetTrainingEventBuff(_currentTrainingEventData);
                _trainingEventProgressor.FinishReadNovel();
                break;
        }
    }

    /// <summary> シナリオを読み進める処理 </summary>
    public async UniTask ScenarioReading(uint page)
    {
        _trainingEventPlayerAction = NovelEventPlayerAction.SkipScenario;

        _novelView.SetNameText(_currentNovelEventData.NovelData[page].TalkCharacterName);

        _novelView.SetCharacterImage(
            GetCharacterSprite(_currentNovelEventData.NovelData[_currentPage].CharacterCenter), NovelPageCharacters());
        

        await _novelView.DialogueText.ShowTextAsync(_currentNovelEventData.NovelData[page].ScenarioData);

        _trainingEventPlayerAction = NovelEventPlayerAction.ReadScenario;
    }

    public Sprite[] NovelPageCharacters()
    {
        List<Sprite> spriteDataList = new();

        spriteDataList.Add(GetCharacterSprite(_currentNovelEventData.NovelData[_currentPage].CharacterLeftBottom));
        spriteDataList.Add(GetCharacterSprite(_currentNovelEventData.NovelData[_currentPage].CharacterRightBottom));
        spriteDataList.Add(GetCharacterSprite(_currentNovelEventData.NovelData[_currentPage].CharacterLeftTop));

        return spriteDataList.ToArray();
    }

    public Sprite GetCharacterSprite(string name)
    {
        if(name == "TrainingCharacter")
            return _characterImageDataRepository.RepositoryData.GetCharacterSprite(_trainingCharacterID, CharacterSpriteType.OverAllView);
        else if (name == "Null") return null;

        uint id = _addressableCharacterDataRepository.GetCharacterID(name);
        if(id == 0) return null;

        return _characterImageDataRepository.RepositoryData.GetCharacterSprite(id, CharacterSpriteType.OverAllView);
    }

    public bool IsBranchEvent(uint page)
    {
        if(_currentNovelEventData.NovelData[page].TalkCharacterName == "EventBranch")
        {
            return true;
        }
        return false;
    }

    /// <summary> 分岐イベント選択処理 </summary>
    private void EventBranch()
    {
        switch (_currentTrainingEventData.BranchType)
        {
            case EventBranchWay.StaminaValue:
                BranchTrainingEvent();
                _trainingEventPlayerAction = NovelEventPlayerAction.ReadScenario;
                break;
            case EventBranchWay.Button:
                BranchEventSelectButtonGenerate();
                _trainingEventPlayerAction = NovelEventPlayerAction.FinishedScenario;
                break;
        }
    }

    /// <summary> トレーニングイベントの分岐 </summary>
    private void BranchTrainingEvent()
    {
        EventBranchType trainingResult = _trainingSuccessDecider.TrySuccessTrainingEvent(_jsonTrainingSaveData.RepositoryData.CurrentStamina);
        List<BranchTrainingEventData> branchTrainingEventDataList = _trainingBranchEventDataRepository.RepositoryData.GetBranchEvents(_currentTrainingEventData.EventID);

        StartBranchNovelEvent(FindBranchTrainingData(trainingResult, branchTrainingEventDataList));
    }

    /// <summary> 分岐先のイベントを探す処理 </summary>
    private BranchTrainingEventData FindBranchTrainingData(EventBranchType trainingBranch, List<BranchTrainingEventData> branchTrainingList)
    {
        foreach (var trainingEvent in branchTrainingList)
        {
            if (trainingEvent.TrainingEventBranchType == trainingBranch)
            {
                return trainingEvent;
            }
        }
        return null;
    }

    /// <summary> 分岐選択ボタンの生成 </summary>
    private void BranchEventSelectButtonGenerate()
    {
        List<BranchTrainingEventData> branchTrainingEventDataList = _trainingBranchEventDataRepository.RepositoryData.GetBranchEvents(_currentTrainingEventData.EventID);
        foreach (var trainingEvent in branchTrainingEventDataList)
        {
            Button button = _branchEventSelectButtonView.GenerateSelectButton(trainingEvent.EventName);
            button.onClick.AddListener(() => OnclickEventSelectButtonEvent(trainingEvent));
        }
    }

    /// <summary> 分岐選択ボタンを押した際の処理 </summary>
    private void OnclickEventSelectButtonEvent(ITrainingEventData trainingEvent)
    {
        StartBranchNovelEvent(trainingEvent);
        _branchEventSelectButtonView.OnSelected();
    }

    /// <summary> 分岐先のノベルイベントの開始処理 </summary>
    private void StartBranchNovelEvent(ITrainingEventData trainingEvent)
    {
        _currentTrainingEventData = trainingEvent;
        _currentNovelEventData = _addressableNovelEventDataRepository.RepositoryData.GetData(_currentTrainingEventData.NovelEventID);
        _currentPage = 0;
    }

    public enum NovelEventPlayerAction
    {
        None,
        FinishedScenario,
        SkipScenario,
        ReadScenario,
    }
}
