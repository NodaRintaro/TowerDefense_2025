using UnityEngine;

public class TrainingUIController : MonoBehaviour
{
    [SerializeField, Header("トレーニング選択画面UIのViewClass")]
    private TrainingMenuView _trainingView;

    [SerializeField, Header("キャラクター選択画面UIのViewClass")]
    private CharacterPickView _characterPickView;

    private GameObject _currentActiveCanvas = null;

    private TrainingManager _trainingManager;

    private TrainingScreenChanger _screenChanger;

    public void Start()
    {
        _trainingManager = FindAnyObjectByType<TrainingManager>();
        _screenChanger = FindAnyObjectByType<TrainingScreenChanger>();
        _screenChanger.OnChangedScreenType += HandleScreenChanged;
    }

    /// <summary> Buttonに各種トレーニングを設定する </summary>
    private void SetTrainingMenu()
    {
        foreach(var trainingEvent in _trainingManager.TrainingMenuList)
        {
            SetTrainingButtonOnClickEvent(trainingEvent);
        }
    }

    private void SetTrainingButtonOnClickEvent(ITrainingMenu trainingMenu)
    {
        foreach(var buttonGUI in _trainingView.TrainingButtons)
        {
            if(trainingMenu.TrainingType == buttonGUI.TrainingType)
            {
                buttonGUI.TrainingButton.onClick.AddListener(() => trainingMenu.TrainingEvent(_trainingManager.CurrentTrainingCharacterData));
            }
        }
    }

    private void AddPickCharacterButtons()
    {

    }

    private void HandleScreenChanged(ScreenType screenType)
    {
        switch (screenType)
        {
            case ScreenType.CharacterPick:
                if (_currentActiveCanvas == null) _currentActiveCanvas = _characterPickView.ViewCanvasObj;
                else
                {
                    _currentActiveCanvas.SetActive(false);
                    _currentActiveCanvas = _characterPickView.ViewCanvasObj;
                }
                _currentActiveCanvas.SetActive(true);
                    break;
            case ScreenType.TrainingMenu:
                if (_currentActiveCanvas == null) _currentActiveCanvas = _trainingView.ViewCanvasObj;
                else
                {
                    _currentActiveCanvas.SetActive(false);
                    _currentActiveCanvas = _trainingView.ViewCanvasObj;
                }
                _currentActiveCanvas.SetActive(true);
                break;
        }
    }
}
