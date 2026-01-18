using System.Collections;
using System.Collections.Generic;
using TowerDefenseDeckData;
using UnityEngine;
using VContainer;

public class TowerDefenseDeckSelectPresenter : MonoBehaviour
{
    [SerializeField] private GameObject[] _selectDeckUI;

    private CharacterTeamBuildLifeTimeScope _lifeTimeScope;

    private JsonCharacterDeckDataRepository _jsonCharacterDeckDataRepository;
    private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<CharacterTeamBuildLifeTimeScope>();

        _jsonCharacterDeckDataRepository = _lifeTimeScope.Container.Resolve<JsonCharacterDeckDataRepository>();
        _addressableCharacterImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterImageDataRepository>();
    }


}
