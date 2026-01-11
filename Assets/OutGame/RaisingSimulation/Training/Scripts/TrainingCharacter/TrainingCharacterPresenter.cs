using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class TrainingCharacterPresenter : MonoBehaviour
{
    [SerializeField] private TrainingCharacterView _characterView;

    private RaisingSimulationLifeTimeScope _lifeTimeScope;

    #region DataClass
    private JsonTrainingSaveDataRepository _trainingTargetSaveDataRepository;
    private AddressableCharacterDataRepository _addressableCharacterDataRepository;
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    #endregion

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();

        _trainingTargetSaveDataRepository = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        _addressableCharacterDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterDataRepository>();
        _addressableCharacterImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
    }

    public void OnEnable()
    {
        SetCharacterView(_trainingTargetSaveDataRepository.RepositoryData);
    }

    /// <summary> キャラクターの情報をViewに反映する処理 </summary>
    public void SetCharacterView(TrainingData trainingData)
    {
        //キャラクターのイメージをViewに反映
        uint trainingCharacterID = trainingData.TrainingCharacterData.CharacterID;
        _characterView.SetImage(_addressableCharacterImageDataRepository.GetSprite(trainingCharacterID, CharacterSpriteType.OverAllView));

        //キャラクターのパラメータをセットする
        _characterView.SetTrainingCharacterParameter(trainingData.TrainingCharacterData);
    }
}
