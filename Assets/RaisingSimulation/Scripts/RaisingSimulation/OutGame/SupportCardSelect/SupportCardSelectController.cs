using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SupportCardSelectController : MonoBehaviour
{
    [SerializeField, Header("カードの選択クラス")]
    private SupportCardSelecter _supportCardSelecter;

    [SerializeField, Header("UIをまとめるViewClass")] 
    private SupportCardSelectUIHolder _supportCardSelectUIHolder = new();

    [SerializeField,Header("カードのデータを読み込むクラス")]
    private SupportCardDataBase _supportCardDataBase;

    private const string _raisingSimulationSceneName = "RaisingSimulation";

    private TrainingDataHolder _trainingDataHolder;
    private CharacterSelectScreenChanger _screenChanger;
    private SceneChanger _sceneChanger;
    private CharacterPickController _characterPickController;
    private TrainingDataSelectLifeTimeScope _trainingDataSelectLifeTimeScope;
    private TrainingDataManager _trainingCharacterSaveDataManager;

    public SupportCardSelectUIHolder SupportCardSelectUIHolder => _supportCardSelectUIHolder;

    public async void Start()
    {
        _trainingDataSelectLifeTimeScope = FindAnyObjectByType<TrainingDataSelectLifeTimeScope>();
        _characterPickController = _trainingDataSelectLifeTimeScope.Container.Resolve<CharacterPickController>();
        _supportCardDataBase = _trainingDataSelectLifeTimeScope.Container.Resolve<SupportCardDataBase>();
        _supportCardSelecter = _trainingDataSelectLifeTimeScope.Container.Resolve<SupportCardSelecter>();
        _screenChanger = _trainingDataSelectLifeTimeScope.Container.Resolve<CharacterSelectScreenChanger>();
        _sceneChanger = _trainingDataSelectLifeTimeScope.Container.Resolve<SceneChanger>();
        _trainingDataHolder = _trainingDataSelectLifeTimeScope.Container.Resolve<TrainingDataHolder>();
        _trainingCharacterSaveDataManager = _trainingDataSelectLifeTimeScope.Container.Resolve<TrainingDataManager>();

        await _supportCardDataBase.CardDataLoad();
        CreateCardSelectButtons();
        SetCardDeckButtonAction();
        SetBackCharacterSelectButton();
        SetStartRaisingSimulation();
    }

    /// <summary>
    /// 編成に加えるカードを選択
    /// </summary>
    private void CreateCardSelectButtons()
    {
        foreach(var card in _supportCardDataBase.SupportCardDataHolder.DataList)
        {
            foreach(var resource in _supportCardDataBase.SupportCardResources)
            {
                if(card.ID == resource.CardID && card.IsGetting == true)
                {
                    Button selectButton = CardSelectButtonInstantiate(resource.CardSprite);
                    selectButton.OnClickAsObservable().Subscribe(_ => OnClickCardButtonAction(card.ID)).AddTo(this);
                }
            }
        }
    }

    private Button CardSelectButtonInstantiate(Sprite sprite)
    {
        GameObject buttonObj = Instantiate(_supportCardSelectUIHolder.SupportCardSelectButtonPrefab);
        buttonObj.transform.parent = _supportCardSelectUIHolder.CardSelectButtonsLayout.gameObject.gameObject.transform;

        Image buttonImage = buttonObj.GetComponent<Image>();
        buttonImage.sprite = sprite;

        return buttonObj.GetComponent<Button>();
    }

    private void SetCardDeckButtonAction()
    {
        foreach(var button in _supportCardSelectUIHolder.DeckButtonList)
        {
            button.Button.OnClickAsObservable().Subscribe(_ => OnClickCardDeckButtonAction(button.ButtonDeckNum)).AddTo(this);
        }
    }

    private void SetBackCharacterSelectButton()
    {
        _supportCardSelectUIHolder.BackCharacterSelectButton.OnClickAsObservable().Subscribe(_ => OnClickBackCharacterSelectScreen()).AddTo(this);
    }

    private void SetStartRaisingSimulation()
    {
        _supportCardSelectUIHolder.StartRaisingSiomulationButton.OnClickAsObservable().Subscribe(_ => OnClickStartRaisingSimulation()).AddTo(this);
    }

    #region OnClickActions

    private void OnClickCardDeckButtonAction(uint deckNum)
    {
        _supportCardSelecter.SetSelectDeckNum(deckNum);
    }

    private void OnClickCardButtonAction(uint id)
    {
        SupportCardData supportCardData = _supportCardDataBase.GetSupportCardData(id);

        if (_supportCardSelecter.SelectCardDeckNum != default && !_supportCardSelecter.IsSelectedCard(supportCardData))
        {
            Sprite sprite = _supportCardDataBase.GetCardResource(id).CardSprite;

            _supportCardSelecter.SelectSupportCard(supportCardData);
            CardSelectButton cardDeckButton = _supportCardSelectUIHolder.DeckButtonList[_supportCardSelecter.SelectCardDeckNum - 1];
            cardDeckButton.Image.sprite = sprite;

            _supportCardSelectUIHolder.ViewStatus(supportCardData);
        }
    }

    /// <summary>
    /// キャラクターセレクト画面へ戻る処理
    /// </summary>
    private void OnClickBackCharacterSelectScreen()
    {
        _screenChanger.ShowCharacterPickScreen();
        Debug.Log("Click");
    }

    /// <summary>
    /// トレーニングシーンへの移行時の処理
    /// </summary>
    private void OnClickStartRaisingSimulation()
    {
        _trainingDataHolder.SetSupportCardsData(_supportCardSelecter.SupportCardDeckData);
        _trainingCharacterSaveDataManager.DataSave(_trainingDataHolder);
        _sceneChanger.SceneChange(_raisingSimulationSceneName);
    }

    #endregion
}
