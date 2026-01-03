using System.Collections;
using System.Collections.Generic;
using HomeMenuView;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HomeMenuController : MonoBehaviour
{
    [SerializeField]
    private ViewData _view;

    private const string _trainingGameSceneName = "RaisingSimulation";

    private void Start()
    {
        SetOnClickEvents();
    }

    private void SetOnClickEvents()
    {
        _view.CharacterTrainingButton.OnClickAsObservable().Subscribe(x => OnTrainingGameSceneChange()).AddTo(this);
    }

    public void OnTrainingGameSceneChange()
    {
        SceneChanger.SceneChange(_trainingGameSceneName);
    }
}
