using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SupportCardSelectPagePresenter : MonoBehaviour, IPagePresenter
{
    [SerializeField, Header("サポートカードを選択するボタンの生成クラス")]
    private ButtonGenerator _buttonGenerator;

    [SerializeField, Header("サポートカードのデッキのView")] 
    private SupportCardDeckView _deckView;

    [SerializeField, Header("PageView")]
    private SelectPageView _pageView;

    [SerializeField, Header("サポートカード選択ボタンの親オブジェクト")]
    private Transform _supportCardSelectButtonParent;

    private RaisingSimulationDataContainer _lifeTimeScope = null;

    /// <summary> 現在選択中のボタンの配列番号 </summary>
    private uint _currentDeckNum;
    /// <summary> 現在選択中のボタン </summary>
    private Button _currentSelectButton;

    /// <summary> 既に選択済みのカードID </summary>
    private uint[] _selectedCardId = new uint[_deckNum];
    private const int _deckNum = 4;

    #region DataClass
    private JsonSupportCardCollectionDataRepository _supportCardCollectionDataRepository;
    private JsonTrainingSaveDataRepository _trainingTargetSaveDataRepository;
    private AddressableSupportCardDataRepository _addressableSupportCardDataRepository;
    private AddressableSupportCardImageDataRepository _addressableSupportCardImageDataRepository;
    #endregion

    private void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationDataContainer>();

        _addressableSupportCardDataRepository = _lifeTimeScope.Container.Resolve<AddressableSupportCardDataRepository>();
        _addressableSupportCardImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableSupportCardImageDataRepository>();
        _trainingTargetSaveDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _supportCardCollectionDataRepository = _lifeTimeScope.Container.Resolve<JsonSupportCardCollectionDataRepository>();

        SetOnclickSelectDeckButtonEvent();
    }

    private void OnEnable()
    {
        //このページで使用可能なボタンの選別
        SetOnClickTurnPageButtonEvent();

        //最初はメインのサポートカードを選択できるようにしておく
        OnclickSelectDeckChangeEvent(0);

        //サポートカード選択ボタンを生成
        _buttonGenerator.SetGenerateButtonParent(_supportCardSelectButtonParent);
        GenerateSupportCardSelectButtons();
    }

    private void OnDisable()
    {
        _buttonGenerator.ReleaseAllButtons();
    }

    private void SetOnclickSelectDeckButtonEvent()
    {
        TrainingSupportCardDeckData trainingSupportCardDeckData = _trainingTargetSaveDataRepository.RepositoryData.TrainingCardDeckData;
        for (uint deckCount = 0; trainingSupportCardDeckData.CardDeckData.Length > deckCount; deckCount++)
        {
            uint deckIndex = deckCount;
            _deckView.GetDeckButton(deckIndex).onClick.AddListener(() => OnclickSelectDeckChangeEvent(deckIndex));
        }
    }

    /// <summary> 選択中のデッキの変更 </summary>
    public void OnclickSelectDeckChangeEvent(uint deckNum)
    {
        _currentDeckNum = deckNum;
        _currentSelectButton = _deckView.GetDeckButton(deckNum);
    }

    /// <summary> 任意のサポートカード選択ボタンを押した際にデッキにサポートカードを登録する関数 </summary>
    public void OnclickSupportCardSelectEvent(uint id, Button selectButton)
    {
        //サポートカードを登録
        SupportCardData supportCardData = _addressableSupportCardDataRepository.RepositoryData.GetData(id);
        _trainingTargetSaveDataRepository.RepositoryData.SetCardData(_currentDeckNum, supportCardData);

        //サポートカードのイメージを反映
        _currentSelectButton.image.sprite = _addressableSupportCardImageDataRepository.RepositoryData.GetData(id);

        //選んだ対象のボタンを選べなくする
        _deckView.SetSelectedButtons(_currentDeckNum, selectButton);
        _selectedCardId[_currentDeckNum] = id;

        //次の選択対象の枠があれば選択対象を次の枠にする
        if (_currentDeckNum < _trainingTargetSaveDataRepository.RepositoryData.TrainingCardDeckData.CardDeckData.Length - 1)
        {
            _currentDeckNum++;
            OnclickSelectDeckChangeEvent(_currentDeckNum);
        }
    }

    /// <summary> キャラクターの選択ボタンを生成 </summary>
    private void GenerateSupportCardSelectButtons()
    {
        foreach (uint id in _supportCardCollectionDataRepository.RepositoryData.CollectionList)
        {
            string buttonName = _addressableSupportCardDataRepository.GetSupportCardData(id).CardName;
            Sprite buttonSprite = _addressableSupportCardImageDataRepository.GetSprite(id);
            Button selectButton = _buttonGenerator.GenerateButton(buttonName, buttonSprite);

            //Buttonにクリック時のイベントを登録する
            selectButton.onClick.AddListener
            (
                () => 
                {
                    OnclickSupportCardSelectEvent(id , selectButton);
                }
            );

            if(_selectedCardId.Contains(id))
            {
                selectButton.interactable = false;
            }
        }
    }

    public void SetOnClickTurnPageButtonEvent()
    {
        _pageView.SetTurnPageButtonsInteractable(true, true);

        _pageView.BackPageButton.onClick.AddListener(async () => await _pageView.TurnPage(CharacterSelectPageType.CharacterSelectPage));
        _pageView.NextButton.onClick.AddListener(async () => await _pageView.TurnPage(CharacterSelectPageType.SelectedCharacterConfirmPage));
    }
}
