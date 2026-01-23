using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// ノベルゲームのView
/// </summary>
public class TrainingNovelEventView : MonoBehaviour
{
    [Header("シナリオを読んでいる際View")]
    [SerializeField] private NovelEventUI _novelUI;

    [Header("トレーニングイベントのControllerClass")]
    [SerializeField] private TrainingEventController _trainingEventController;

    private ScenarioData _currentReadScenario;
    private NovelEventPlayerActions _currentAction;

    //この名前が出てきたらシナリオを読むのを中断し分岐イベントに移る
    private string _eventBranchName = "EventBranch";

    private NovelPageData _currentPageData;

    //LifeTimeScope
    private RaisingSimulationLifeTimeScope _lifeTimeScope;

    private ParameterBuffCalculator _trainingCharacterParameterTotalBuffCalculator;

    private JsonTrainingSaveDataRepository _jsonTrainingSaveData;
    private AddressableCharacterDataRepository _addressableCharacterDataRepository;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _trainingCharacterParameterTotalBuffCalculator = _lifeTimeScope.Container.Resolve<ParameterBuffCalculator>();
        _jsonTrainingSaveData = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _addressableCharacterDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();
    }

    public void OnEnable()
    {
        _novelUI.Init();
    }

    /// <summary> シナリオデータをセットする </summary>
    public void SetScenario(ScenarioData scenarioData)
    {
        _currentReadScenario = scenarioData;
    }

    /// <summary> 現在表示されているシナリオを読み進める処理 </summary>
    public async UniTask OnScenarioReadingAction()
    {
        switch(_currentAction)
        {
            case NovelEventPlayerActions.ReadScenario:
                if (_currentReadScenario == null) break;

                bool isFinishScenarioReading = _currentReadScenario.TryGetNextPage(out _currentPageData);

                if (!isFinishScenarioReading || _currentPageData.TalkCharacterName == _eventBranchName)
                {
                    ChangeNovelEventPlayerActions(NovelEventPlayerActions.FinishedRead);
                    await OnScenarioReadingAction();
                    break;
                }

                await ReadNovel();
                break;
            case NovelEventPlayerActions.SkipScenario:
                _novelUI.DialogueText.SkipText();
                break;
            case NovelEventPlayerActions.FinishedRead:
                ChangeNovelEventPlayerActions(NovelEventPlayerActions.Inactive);
                await _trainingEventController.FinishedReadScenario();
                break;
        }
    }

    public void ChangeNovelEventPlayerActions(NovelEventPlayerActions novelEventPlayerActions)
    {
        _currentAction = novelEventPlayerActions;
    }

    /// <summary> 送られてきたデータをもとに画面を整える処理 </summary>
    public async UniTask ReadNovel()
    {
        Debug.Log("シナリオをひょうじ");

        _novelUI.SetNameText(_currentPageData.TalkCharacterName);

        CharacterBaseData centerCharacter = _addressableCharacterDataRepository.GetCharacterDataByName(_currentPageData.CharacterCenter);
        CharacterBaseData leftBottomCharacter = _addressableCharacterDataRepository.GetCharacterDataByName(_currentPageData.CharacterLeftBottom);
        CharacterBaseData rightBottomCharacter = _addressableCharacterDataRepository.GetCharacterDataByName(_currentPageData.CharacterRightBottom);
        CharacterBaseData leftTopCharacter = _addressableCharacterDataRepository.GetCharacterDataByName(_currentPageData.CharacterLeftTop);
        CharacterBaseData rightTopCharacter = _addressableCharacterDataRepository.GetCharacterDataByName(_currentPageData.CharacterRightTop);

        Sprite centerSprite = null, leftBottomSprite = null, rightBottomSprite = null, leftTopSprite = null , rightTopSprite = null;

        if (centerCharacter != null) centerSprite = centerCharacter.CharacterImageData.GetSprite(CharacterSpriteType.OverAllView);
        if (leftBottomCharacter != null) leftBottomSprite = leftBottomCharacter.CharacterImageData.GetSprite(CharacterSpriteType.OverAllView);
        if (rightBottomCharacter != null) rightBottomSprite = rightBottomCharacter.CharacterImageData.GetSprite(CharacterSpriteType.OverAllView);
        if (leftTopCharacter != null) leftTopSprite = leftTopCharacter.CharacterImageData.GetSprite(CharacterSpriteType.OverAllView);
        if (rightTopCharacter != null) rightTopSprite = rightTopCharacter.CharacterImageData.GetSprite(CharacterSpriteType.OverAllView);

        //キャラクター立ち絵をViewに反映
        _novelUI.SetCharacterImage(centerSprite, leftBottomSprite, rightBottomSprite, leftTopSprite, rightTopSprite);

        //テキストがフェードインしている間はアニメーションをいつでもスキップできるようにする
        _currentAction = NovelEventPlayerActions.SkipScenario;
        await _novelUI.DialogueText.ShowTextAsync(_currentPageData.ScenarioData);

        //テキストがすべて表示されたら次のページへ進めるようにする
        if(_currentAction == NovelEventPlayerActions.SkipScenario)
            _currentAction = NovelEventPlayerActions.ReadScenario;
    }


    public enum NovelEventPlayerActions
    {
        Inactive = 0,
        FinishedRead,
        SkipScenario,
        ReadScenario,
    }
}
