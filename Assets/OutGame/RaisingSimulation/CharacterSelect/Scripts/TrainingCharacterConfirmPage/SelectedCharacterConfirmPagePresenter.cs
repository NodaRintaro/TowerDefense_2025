using RaisingSimulationGameFlowStateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class SelectedCharacterConfirmPagePresenter : MonoBehaviour, IPagePresenter
{
    [SerializeField] private SelectedSupportCardsConfirmView _selectedSupportCardsConfirmView;

    [SerializeField, Header("PageView")]
    private PageView _pageView;

    private RaisingSimulationLifeTimeScope _lifeTimeScope = null;

    #region DataClass
    private GameFlowStateMachine _gameflowStateMachine;
    private JsonTrainingSaveDataRepository _trainingTargetSaveDataRepository;
    private AddressableSupportCardDataRepository _addressableSupportCardDataRepository;
    private AddressableSupportCardImageDataRepository _addressableSupportCardImageDataRepository;
    #endregion


    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _gameflowStateMachine = _lifeTimeScope.Container.Resolve<GameFlowStateMachine>();
        _trainingTargetSaveDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _addressableSupportCardDataRepository = _lifeTimeScope.Container.Resolve<AddressableSupportCardDataRepository>();
        _addressableSupportCardImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableSupportCardImageDataRepository>();
    }

    public void OnEnable()
    {
        SetSupportCardView();
        SetOnClickTurnPageButtonEvent();
    }

    /// <summary> トレーニングで使うサポートカードの画像データをViewに反映する処理 </summary>
    public void SetSupportCardView()
    {
        int index = 0;
        foreach(var card in _trainingTargetSaveDataRepository.RepositoryData.TrainingCardDeckData.CardDeckData)
        {
            if (card != null)
            {
                Sprite cardSprite = _addressableSupportCardImageDataRepository.GetSprite(card.ID);
                _selectedSupportCardsConfirmView.SelectedSupportCardImages[index].sprite = cardSprite;
                index++;
            }
        }
    }

    /// <summary> 育成ゲームへ移行する処理 </summary>
    public async UniTask GameStart()
    {
        await _trainingTargetSaveDataRepository.DataSave();
        _gameflowStateMachine.ChangeState(ScreenType.TrainingSelectMenu);
    }

    /// <summary> 現在のページからほかのページへ移行するイベントをボタンに登録する処理 </summary>
    public void SetOnClickTurnPageButtonEvent()
    {
        _pageView.SetTurnPageButtonsInteractable(false, true);
        _pageView.BackPageButton.onClick.AddListener(async () => await _pageView.TurnPage(CharacterSelectPageType.SupportCardSelectPage));

        if(_trainingTargetSaveDataRepository.RepositoryData.TrainingCharacterData != null)
        {
            _pageView.SetStartTrainingButtonInteractable(true);
            _pageView.StartTrainingButton.onClick.AddListener(async () => await GameStart());
        }
        else
        {
            _pageView.SetStartTrainingButtonInteractable(false);
        }
    }
}
