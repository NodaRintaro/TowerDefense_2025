using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingSelectButtonsView : MonoBehaviour
{
    [Header("選んだトレーニングを開始するボタン")]
    [SerializeField] private Button _trainingStartButton;

    [Header("各種トレーニングを行うボタン")]
    [SerializeField, Header("模擬戦闘")] private Button _powerTrainingButton;
    [SerializeField, Header("読書")] private Button _intelligenceTrainingButton;
    [SerializeField, Header("マラソン")] private Button _physicalTrainingButton;
    [SerializeField, Header("狩猟")] private Button _speedTrainingButton;
    [SerializeField, Header("休息")] private Button _takeBreakButton;

    #region 参照プロパティ
    public Button TrainingStartButton => _trainingStartButton;
    public Button PowerTrainingButton => _powerTrainingButton;
    public Button IntelligenceTrainingButton => _intelligenceTrainingButton;
    public Button PhysicalTrainingButton => _physicalTrainingButton;
    public Button SpeedTrainingButton => _speedTrainingButton;
    public Button TakeBreakButton => _takeBreakButton;
    #endregion

    public void Awake()
    {
        SetButtonAnimation();
    }

    public void SetButtonAnimation()
    {
        ButtonAnimation.SetupPointerEnterAnimationEvents(_powerTrainingButton);
        ButtonAnimation.SetupPointerEnterAnimationEvents(_intelligenceTrainingButton);
        ButtonAnimation.SetupPointerEnterAnimationEvents(_physicalTrainingButton);
        ButtonAnimation.SetupPointerEnterAnimationEvents(_speedTrainingButton);
        ButtonAnimation.SetupPointerEnterAnimationEvents(_takeBreakButton);
        ButtonAnimation.SetupPointerEnterAnimationEvents(_trainingStartButton);
    }
}
