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

    private NovelPageData _currentPageData;

    private ScenarioData _currentReadScenario;
    private NovelEventPlayerActions _currentAction;

    //この名前が出てきたらシナリオを読むのを中断し分岐イベントに移る
    private string _eventBranchName = "EventBranch";

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

    /// <summary> シナリオデータをセットする </summary>
    public void SetScenario(ScenarioData scenarioData)
    {
        _currentReadScenario = scenarioData;
        _currentReadScenario.TryGetNextPage(out _currentPageData);
    }

    /// <summary> 現在表示されているシナリオを読み進める処理 </summary>
    public async UniTask OnScenarioReadingAction()
    {
        switch(_currentAction)
        {
            case NovelEventPlayerActions.ReadScenario:
                bool isScenarioFinish = _currentReadScenario.TryGetNextPage(out _currentPageData);

                if (isScenarioFinish || _currentPageData.TalkCharacterName == _eventBranchName)
                {
                    _currentAction = NovelEventPlayerActions.FinishedRead;
                    await OnScenarioReadingAction();
                    break;
                }

                await SetScenarioView();
                break;
            case NovelEventPlayerActions.SkipScenario:
                _novelUI.DialogueText.SkipText();
                break;
            case NovelEventPlayerActions.FinishedRead:
                _trainingEventController.FinishedReadScenario();
                break;
        }
    }

    /// <summary> 送られてきたデータをもとに画面を整える処理 </summary>
    public async UniTask SetScenarioView()
    {
        _novelUI.SetNameText(_currentPageData.TalkCharacterName);

        //キャラクター立ち絵をViewに反映
        _novelUI.SetCharacterImage
            (_addressableCharacterDataRepository.GetCharacterDataByName //真ん中のキャラクター
            (_currentPageData.CharacterCenter).CharacterImageData.GetSprite(CharacterSpriteType.OverAllView),
            _addressableCharacterDataRepository.GetCharacterDataByName //左下のキャラクター
            (_currentPageData.CharacterLeftBottom).CharacterImageData.GetSprite(CharacterSpriteType.OverAllView),
            _addressableCharacterDataRepository.GetCharacterDataByName //右下のキャラクター
            (_currentPageData.CharacterRightBottom).CharacterImageData.GetSprite(CharacterSpriteType.OverAllView),
            _addressableCharacterDataRepository.GetCharacterDataByName //左上のキャラクター
            (_currentPageData.CharacterLeftTop).CharacterImageData.GetSprite(CharacterSpriteType.OverAllView),
            _addressableCharacterDataRepository.GetCharacterDataByName //右上のキャラクター
            (_currentPageData.CharacterRightTop).CharacterImageData.GetSprite(CharacterSpriteType.OverAllView)
            );

        //テキストがフェードインしている間はアニメーションをいつでもスキップできるようにする
        _currentAction = NovelEventPlayerActions.SkipScenario;
        await _novelUI.DialogueText.ShowTextAsync(_currentPageData.ScenarioData);

        //テキストがすべて表示されたら次のページへ進めるようにする
        if(_currentAction == NovelEventPlayerActions.SkipScenario)
            _currentAction = NovelEventPlayerActions.ReadScenario;
    }


    public enum NovelEventPlayerActions
    {
        FinishedRead,
        SkipScenario,
        ReadScenario,
    }
}
