using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CharacterChanger : MonoBehaviour
{
    [SerializeField] private Image _characterImage;

    private HomeMenuLifeTimeScope _homeMenuLifeTimeScope;
    private DataLoadCompleteNotifier _loadingNotifier;
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
    private JsonCharacterCollectionDataRepository _jsonCharacterCollectionDataRepository;

    private int _nowCharacterImageId = 0;

    private int _maxCharacterImageId = 1;

    private async void Awake()
    {
        _homeMenuLifeTimeScope = FindFirstObjectByType<HomeMenuLifeTimeScope>();
        _loadingNotifier = _homeMenuLifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        _jsonCharacterCollectionDataRepository =
            _homeMenuLifeTimeScope.Container.Resolve<JsonCharacterCollectionDataRepository>();
        _addressableCharacterImageDataRepository =
            _homeMenuLifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();

        await _jsonCharacterCollectionDataRepository.DataLoadAsync(cancellationTokenSource.Token);
        await _addressableCharacterImageDataRepository.DataLoadAsync(cancellationTokenSource.Token);
        _maxCharacterImageId = _jsonCharacterCollectionDataRepository.RepositoryData.CollectionList.Count;
    }

    private void OnEnable()
    {
        _loadingNotifier.OnDataLoadComplete += RandomCharacterPickUpView;
    }

    private void OnDisable()
    {
        _loadingNotifier.OnDataLoadComplete -= RandomCharacterPickUpView;
    }

    // キャラクターを順番に表示する
    public void RandomCharacterPickUpView()
    {
        if (_maxCharacterImageId == 0) return;
        while (!_jsonCharacterCollectionDataRepository.RepositoryData.TryGetCollection(
                   (uint)((_nowCharacterImageId % _maxCharacterImageId) + 1)))
        {
            _nowCharacterImageId++;
        }

        _characterImage.sprite =
            _addressableCharacterImageDataRepository.GetSprite(
                (uint)((_nowCharacterImageId % _maxCharacterImageId) + 1),
                CharacterSpriteType.OverAllView);
        
        _characterImage.rectTransform.pivot = new Vector2(
            _characterImage.sprite.pivot.x / _characterImage.sprite.rect.width,
            _characterImage.sprite.pivot.y / _characterImage.sprite.rect.height);
    }

    public void OnClickNextCharacter()
    {
        _nowCharacterImageId++;
        RandomCharacterPickUpView();
    }
}