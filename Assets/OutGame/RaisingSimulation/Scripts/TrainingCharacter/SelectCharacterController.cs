using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

/// <summary>
/// 育成ゲームのキャラクター選択画面で選択中のキャラクターのController
/// </summary>
public class SelectCharacterController : MonoBehaviour
{
    [SerializeField, Header("選択したキャラクターのView")]
    private CharacterInformationView _selectCharacterView;

    [SerializeField, Header("キャラクターを選択するボタンのView")]
    private SelectButtonsView _buttonGenerater;

    private RaisingSimulationLifeTimeScope _lifeTimeScope = null;

    #region DataClass
    private JsonCharacterCollectionDataRepository _characterCollectionDataRepository;
    private JsonTrainingTargetSaveDataRepository _trainingTargetSaveDataRepository;
    private AddressableCharacterDataRepository _addressableCharacterDataRepository;
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    #endregion

    public void OnEnable()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _addressableCharacterDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();
        _addressableCharacterImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
        _trainingTargetSaveDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingTargetSaveDataRepository>();
        _characterCollectionDataRepository = _lifeTimeScope.Container.Resolve<JsonCharacterCollectionDataRepository>();

        GenerateCharacterSelectButtons();
    }

    public void OnDisable()
    {
        _buttonGenerater.ReleaseAllButtons();
    }

    /// <summary> 任意のキャラクター選択ボタンを押した際の処理 </summary>
    public void OnclickCharacterSelectEvent(uint id)
    {
        CharacterBaseData characterData = _addressableCharacterDataRepository.RepositoryData.GetData(id);
        _trainingTargetSaveDataRepository.RepositoryData.SetCharacterData(characterData);

        //Viewにキャラクターの情報を反映
        _selectCharacterView.SetName(characterData.CharacterName);
        _selectCharacterView.SetImage(id);
        _selectCharacterView.SetJob(characterData.CharacterRole);
        _selectCharacterView.SetParameter(characterData);
    }

    /// <summary> キャラクターの選択ボタンを生成 </summary>
    private void GenerateCharacterSelectButtons()
    {
        foreach(uint id in _characterCollectionDataRepository.RepositoryData.CollectionList)
        {
            string buttonName = _addressableCharacterDataRepository.GetCharacterData(id).CharacterName;
            Sprite buttonSprite = _addressableCharacterImageDataRepository.GetSprite(id, CharacterSpriteType.OverAllView);
            Button selectButton = _buttonGenerater.GenerateButton(buttonName, buttonSprite);

            //Buttonにクリック時のイベントを登録する
            selectButton.onClick.AddListener
            (
                () => OnclickCharacterSelectEvent(id)
            );
        }
    }
}
