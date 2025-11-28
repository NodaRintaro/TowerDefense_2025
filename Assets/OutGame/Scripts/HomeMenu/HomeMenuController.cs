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

    private const string _trainingGameSceneName = "TrainingDataSelect";

    private void Start()
    {
        SetOnClickEvents();
    }

    private void SetOnClickEvents()
    {
        _view.CharacterTrainingButton.OnClickAsObservable().Subscribe(x => OnTrainingGame()).AddTo(this);
    }

    public void OnTrainingGame()
    {
        SceneChanger.SceneChange(_trainingGameSceneName);
    }
}
