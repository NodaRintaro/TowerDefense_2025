using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefenseDeckData;
using UnityEngine;
using VContainer;

public class RaidManager : MonoBehaviour
{
    [SerializeField] private RaisingSimulationLifeTimeScope _raisingSimulationLifeTimeScope;
    [SerializeField] private GameObject _selectButtonParent;
    [SerializeField] private GameObject _selectDeckParent;


    private DataLoadCompleteNotifier _loadingNotifier;
    private JsonCharacterDeckDataRepository _jsonCharacterDeckDataRepository;
    private AddressableRaidDataRepository _addressableRaidDataRepository;
    private AddressableStageDataRepository _addressableStageDataRepository;

    CharacterDeckData _currentDeckData = new CharacterDeckData();

    private void Awake()
    {
        _jsonCharacterDeckDataRepository =
            _raisingSimulationLifeTimeScope.Container.Resolve<JsonCharacterDeckDataRepository>();
        _addressableRaidDataRepository =
            _raisingSimulationLifeTimeScope.Container.Resolve<AddressableRaidDataRepository>();
        _addressableStageDataRepository =
            _raisingSimulationLifeTimeScope.Container.Resolve<AddressableStageDataRepository>();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void StartRaid()
    {
    }
}