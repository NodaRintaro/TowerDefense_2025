using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 育成ゲームのキャラクター選択画面で選択中のキャラクターのPresenter
/// </summary>
public class TrainingCharacterSelectionPresenter : MonoBehaviour 
{
    [SerializeField] private TrainingSelectCharacterView _characterView;
    [SerializeField] private CharacterParameterView[] _characterParamView;

    private RaisingSimulationLifeTimeScope _lifeTimeScope;

    private void Start()
    {
        _lifeTimeScope = FindAnyObjectByType<RaisingSimulationLifeTimeScope>();
    }
}
