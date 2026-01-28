using RaisingSimulationGameFlowStateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class SelectedCharacterConfirmPagePresenter : MonoBehaviour, IPagePresenter
{
    [SerializeField] private SelectedSupportCardsConfirmView _selectedSupportCardsConfirmView;

    [SerializeField, Header("PageView")]
    private SelectPageView _pageView;

    private RaisingSimulationDataContainer _lifeTimeScope = null;
    private GameFlowStateMachine _gameFlowStateMachine;
    private TrainingEventPool _trainingEventPool;

    #region DataClass
    private JsonTrainingSaveDataRepository _trainingTargetSaveDataRepository;
    private AddressableCharacterTrainingScheduleRepository _trainingTargetCharacterTrainingScheduleRepository;
    private AddressableSupportCardImageDataRepository _addressableSupportCardImageDataRepository;
    private AddressableCharacterTrainingScheduleRepository _addressableCharacterTrainingScheduleRepository;
    #endregion


    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationDataContainer>();

        _trainingTargetCharacterTrainingScheduleRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterTrainingScheduleRepository>();
        _gameFlowStateMachine = _lifeTimeScope.Container.Resolve<GameFlowStateMachine>();
        _trainingEventPool = _lifeTimeScope.Container.Resolve<TrainingEventPool>();
        _trainingTargetSaveDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _addressableSupportCardImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableSupportCardImageDataRepository>();
        _addressableCharacterTrainingScheduleRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterTrainingScheduleRepository>();
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
    public async UniTask OnTrainingStart()
    {
        _trainingTargetSaveDataRepository.RepositoryData.SetCharacterSchedule(_trainingTargetCharacterTrainingScheduleRepository.RepositoryData.GetData(_trainingTargetSaveDataRepository.RepositoryData.TrainingCharacterData.CharacterID));
        await _trainingTargetSaveDataRepository.DataSave();

        _lifeTimeScope.Container.Resolve<TrainingEventSelector>().SetCharacterUniqueEvent();

        await _gameFlowStateMachine.ChangeState(ScreenType.TrainingEvent);
    }

    /// <summary> 現在のページからほかのページへ移行するイベントをボタンに登録する処理 </summary>
    public void SetOnClickTurnPageButtonEvent()
    {
        _pageView.BackPageButton.onClick.AddListener(async () => await _pageView.TurnPage(CharacterSelectPageType.SupportCardSelectPage));

        if(_trainingTargetSaveDataRepository.RepositoryData.TrainingCharacterData != null)
        {
            _pageView.SetTurnPageButtonsInteractable(true, true);
            _pageView.NextButton.onClick.AddListener(async () => await OnTrainingStart());
        }
        else
        {
            _pageView.SetTurnPageButtonsInteractable(false, true);
        }
    }

    /// <summary> トレーニング最初のイベントをあてはめる </summary>
    public void SetTrainingFirstEvent()
    {
        //キャラクターの育成スケジュールを設定
        CharacterTrainingSchedule characterTrainingSchedule = _addressableCharacterTrainingScheduleRepository.RepositoryData.GetData(_trainingTargetSaveDataRepository.RepositoryData.TrainingCharacterData.CharacterID);
        _trainingTargetSaveDataRepository.RepositoryData.SetCharacterSchedule(characterTrainingSchedule);


    }
}
