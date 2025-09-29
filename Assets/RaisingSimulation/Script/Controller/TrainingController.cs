using System;
using UnityEngine;

public class TrainingController : MonoBehaviour
{
    [SerializeField, Header("トレーニング選択画面UIのViewClass")]
    private TrainingMenuView _trainingMenuView;

    [SerializeField, Header("キャラクター選択画面UIのViewClass")]
    private CharacterPickView _characterPickView;

    [SerializeField, Header("トレーニングイベントUI")]
    private CharacterPickView _trainingEventView;
    

    private GameObject _currentActiveCanvas = null;

    /// <summary> ModelClassの変数 </summary>
    private CharacterPickHandler _characterPickHandler;
    private TrainingMenuHandler _trainingEventHandler;


    private TrainingScreenController _screenChanger;

    public void Awake()
    {
        _characterPickHandler = FindAnyObjectByType<CharacterPickHandler>();
        _trainingEventHandler = FindAnyObjectByType<TrainingMenuHandler>();
        _screenChanger = FindAnyObjectByType<TrainingScreenController>();
    }

    public void OnEnable()
    {
        _screenChanger.OnChangedScreenType += ChangeView;
    }

    public void OnDisable()
    {
        _screenChanger.OnChangedScreenType -= ChangeView;
    }

    #region キャラクター選択画面の処理

    #endregion
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
