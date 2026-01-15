using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        _homeMenuLifeTimeScope = FindFirstObjectByType<HomeMenuLifeTimeScope>();
        _loadingNotifier = _homeMenuLifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();
    }

    private void OnEnable()
    {
        _loadingNotifier.OnDataLoadComplete += RandomCharacterPickUpView;
        if (_loadingNotifier.IsLoadCompleted)
        {
            RandomCharacterPickUpView();
        }
    }

    private void OnDisable()
    {
        _loadingNotifier.OnDataLoadComplete -= RandomCharacterPickUpView;
    }

    public void RandomCharacterPickUpView()
    {

    }
}
