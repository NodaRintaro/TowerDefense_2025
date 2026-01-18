using System.Collections;
using System.Collections.Generic;
using TowerDefenseDeckData;
using UnityEngine;
using VContainer.Unity;

public class TowerDefenseCharacterDeckPresenter : MonoBehaviour
{
    private CharacterTeamBuildLifeTimeScope _lifeTimeScope;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<CharacterTeamBuildLifeTimeScope>();
    }

    private void OnEnable()
    {
        
    }

    public void CharacterSelectMenu()
    {

    }
}
