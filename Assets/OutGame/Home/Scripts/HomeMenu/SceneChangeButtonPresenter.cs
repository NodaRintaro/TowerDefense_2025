using System.Collections;
using System.Collections.Generic;
using HomeMenuView;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SceneChangeButtonPresenter : MonoBehaviour
{
    [SerializeField]
    private ViewData _view;

    [SerializeField] private HomeScreen _homeScreen;

    private const string _trainingGameSceneName = "RaisingSimulation";

    private async void Start()
    {
        await _homeScreen.FadeInScreen();
        SetOnClickEvents();
    }

    private void SetOnClickEvents()
    {
        _view.CharacterTrainingButton.OnClickAsObservable().Subscribe(x => OnTrainingGameSceneChange()).AddTo(this);
    }

    public async void OnTrainingGameSceneChange()
    {
        await _homeScreen.FadeOutScreen();
        SceneChanger.SceneChange(_trainingGameSceneName);
    }
}
