using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TrainingView : MonoBehaviour
{
    [SerializeField, Header("各種パラメータのテキストUI")]
    private List<TextMeshProUGUI> _trainingParameters = default;

    [SerializeField, Header("各種トレーニングのBottonのList")]
    private List<TrainingButtonView> _trainingButtons = default;

    [SerializeField, Header("レイドイベントまでのカウントダウンText")]
    private TextMeshProUGUI _raidEventGUI = default;

    [SerializeField, Header("スタミナの残量を表すSlider")]
    private Slider _staminaGage = default;

    public void SetTrainingEvent()
    {

    }

    public void SetRaidEventGUIText(string eventCountDownText)
    {
        _raidEventGUI.text = eventCountDownText;
    }

    public void SetStaminaView(uint staminaNum)
    {
        _staminaGage.value = staminaNum;
    }
}

[System.Serializable]
public class TrainingButtonView
{
    [SerializeField, Header("トレーニングの内容")]
    private TrainingType _trainingType;

    [SerializeField, Header("各種トレーニングのボタンUI")]
    private Button _trainingButton;

    [SerializeField, Header("各種トレーニングのテキストUI")]
    private TextMeshProUGUI _trainingText;

    [SerializeField, Header("トレーニングの名前")]
    private string _trainingName;

    public Button TrainingButton => _trainingButton;

    public TextMeshProUGUI TrainingText => _trainingText;

    public TrainingType TrainingType => _trainingType;

    public string Name => _trainingName;
}
