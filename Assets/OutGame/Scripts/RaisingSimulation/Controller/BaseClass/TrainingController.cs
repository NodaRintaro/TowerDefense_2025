using System;
using UnityEngine;

public class TrainingControllerBase : MonoBehaviour
{
    [SerializeField, Header("トレーニング選択画面UIのViewClass")]
    private TrainingMenuUIHolder _trainingMenuView;

    [SerializeField, Header("キャラクター選択画面UIのViewClass")]
    private CharacterPickUIHolder _characterPickView;

    [SerializeField, Header("トレーニングイベントUI")]
    private CharacterPickUIHolder _trainingEventView;
    
    
    private GameObject _currentActiveCanvas = null;

    /// <summary> ModelClassの変数 </summary>
    private CharacterPickController _characterPickHandler;
    private TrainingMenuController _trainingEventHandler;


    private ScreenController _screenChanger;

    public void Awake()
    {
        _characterPickHandler = FindAnyObjectByType<CharacterPickController>();
        _trainingEventHandler = FindAnyObjectByType<TrainingMenuController>();
        _screenChanger = FindAnyObjectByType<ScreenController>();
    }

    public void OnEnable()
    {
        _screenChanger.OnChangedScreenType += ChangeView;
    }

    public void OnDisable()
    {
        _screenChanger.OnChangedScreenType -= ChangeView;
    }

    private void ChangeView(ScreenType screenType)
    {
        if(_currentActiveCanvas != null)
            _currentActiveCanvas.SetActive(false);

        switch (screenType) 
        {
            case ScreenType.CharacterPick:
                _characterPickView.ViewCanvasObj.SetActive(true);
                break;
            case ScreenType.TrainingMenu:
                _trainingMenuView.ViewCanvasObj.SetActive(true);
                break;
            case ScreenType.TrainingEvent:
                _trainingEventView.ViewCanvasObj.SetActive(true);
                break;       
        }
    }
}
public enum TrainingType
{
    Physical,
    Power,
    Intelligence,
    Speed,
    TakeBreak
}
