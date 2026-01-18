using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TestManager : MonoBehaviour
{
    [SerializeField] private GameObject _screen;

    private CharacterTeamBuildLifeTimeScope _lifeTimeScope;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<CharacterTeamBuildLifeTimeScope>();

         DataLoadCompleteNotifier notifier = _lifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();

        notifier.OnDataLoadComplete += ScreenUp;
        if (notifier.IsLoadCompleted)
            ScreenUp();
    }

    public void ScreenUp()
    {
        _screen.SetActive(true);
    }
}
