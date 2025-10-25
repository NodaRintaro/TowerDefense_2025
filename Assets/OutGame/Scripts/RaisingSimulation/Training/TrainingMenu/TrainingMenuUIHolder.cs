using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TrainingMenuUIHolder : IView
{
    [SerializeField, Header("Canvasのオブジェクト")]
    GameObject _viewCanvas;

    [SerializeField, Header("各種トレーニングのBottonのList")]
    private TrainingButtonGUI[] _trainingButtons = default;

    [SerializeField, Header("レイドイベントまでのカウントダウンText")]
    private TextMeshProUGUI _raidEventGUI = default;

    [SerializeField, Header("スタミナの残量を表すSlider")]
    private Slider _staminaGage = default;

    public TrainingButtonGUI[] TrainingButtons => _trainingButtons;

    public GameObject ViewCanvasObj => _viewCanvas;

    public void SetRaidCountDownGUI(string eventCountDownText)
    {
        _raidEventGUI.text = eventCountDownText;
    }

    public void SetStaminaBarGUI(uint staminaNum)
    {
        _staminaGage.value = staminaNum;
    }
}

[System.Serializable]
public class TrainingButtonGUI
{
    [SerializeField, Header("トレーニングの内容")]
    private TrainingType _trainingType;

    [SerializeField, Header("各種トレーニングのボタンUI")]
    private Button _trainingButton;

    [SerializeField, Header("各種トレーニングのテキストUI")]
    private TextMeshProUGUI _trainingText;

    public Button TrainingButton => _trainingButton;

    public TextMeshProUGUI TrainingText => _trainingText;

    public TrainingType TrainingType => _trainingType;
}
