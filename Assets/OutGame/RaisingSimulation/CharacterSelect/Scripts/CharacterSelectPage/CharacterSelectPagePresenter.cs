using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

/// <summary>
/// 育成ゲームのキャラクター選択画面のPresenter
/// </summary>
public class CharacterSelectPagePresenter : MonoBehaviour, IPagePresenter
{
    [SerializeField, Header("選択したキャラクターのView")]
    private CharacterInformationView _selectedCharacterView;

    [SerializeField, Header("キャラクターを選択するボタンの生成クラス")]
    private ButtonGenerator _buttonGenerator;

    [SerializeField, Header("PageView")]
    private PageView _pageView;

    [SerializeField, Header("サポートカード選択ボタンの親オブジェクト")]
    private Transform _characterSelectButtonParent;

    private RaisingSimulationLifeTimeScope _lifeTimeScope = null;

    #region DataClass
    private JsonCharacterCollectionDataRepository _characterCollectionDataRepository;
    private JsonTrainingSaveDataRepository _trainingTargetSaveDataRepository;
    private AddressableCharacterDataRepository _addressableCharacterDataRepository;
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    #endregion

    private void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _addressableCharacterDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();
        _addressableCharacterImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
        _trainingTargetSaveDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _characterCollectionDataRepository = _lifeTimeScope.Container.Resolve<JsonCharacterCollectionDataRepository>();
    }

    private void OnEnable()
    {
        //セーブデータが残っていなければ新たに作る
        if(_trainingTargetSaveDataRepository.RepositoryData == null)
            _trainingTargetSaveDataRepository.SetData(new TrainingData());

        //キャラクター選択ボタンを生成
        _buttonGenerator.SetGenerateButtonParent(_characterSelectButtonParent);
        GenerateCharacterSelectButtons();

        //現在のキャラクター選択ページからほかのページに移るボタンの初期化
        _pageView.SetStartTrainingButtonInteractable(false);
        _pageView.SetTurnPageButtonsInteractable(true, false);
        SetOnClickTurnPageButtonEvent();
    }

    private void OnDisable()
    {
        _buttonGenerator.ReleaseAllButtons();
    }

    /// <summary> 任意のキャラクター選択ボタンを押した際の処理 </summary>
    public void OnclickCharacterSelectEvent(uint id)
    {
        CharacterBaseData characterData = _addressableCharacterDataRepository.RepositoryData.GetData(id);
        _trainingTargetSaveDataRepository.RepositoryData.SetCharacterBaseData(characterData);

        //Viewにキャラクターの情報を反映
        _selectedCharacterView.SetName(characterData.CharacterName);
        _selectedCharacterView.SetImage(id);
        _selectedCharacterView.SetJob(characterData.CharacterRole);
        _selectedCharacterView.SetParameter(characterData);
    }

    /// <summary> キャラクターの選択ボタンを生成 </summary>
    private void GenerateCharacterSelectButtons()
    {
        foreach(uint id in _characterCollectionDataRepository.RepositoryData.CollectionList)
        {
            string buttonName = _addressableCharacterDataRepository.GetCharacterData(id).CharacterName;
            Sprite buttonSprite = _addressableCharacterImageDataRepository.GetSprite(id, CharacterSpriteType.MiniCard);
            Button selectButton = _buttonGenerator.GenerateButton(buttonName, buttonSprite);

            //Buttonにクリック時のイベントを登録する
            selectButton.onClick.AddListener
            (
                () => OnclickCharacterSelectEvent(id)
            );
        }
    }


    public void SetOnClickTurnPageButtonEvent()
    {
        _pageView.NextPageButton.onClick.AddListener(async () => await _pageView.TurnPage(CharacterSelectPageType.SupportCardSelectPage));
    }
}
