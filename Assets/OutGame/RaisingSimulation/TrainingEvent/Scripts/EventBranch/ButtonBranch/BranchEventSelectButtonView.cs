using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 分岐イベント選択ボタンのView
/// </summary>
public class BranchEventSelectButtonView : MonoBehaviour
{
    [SerializeField] Transform _selectButtonParent;

    [SerializeField] Sprite _selectButtonSprite;

    private ButtonGenerator _buttonGenerator;

    public ButtonGenerator ButtonGenerator => _buttonGenerator;

    public void OnEnable()
    {
        _buttonGenerator = FindFirstObjectByType<ButtonGenerator>();
        _buttonGenerator.SetGenerateButtonParent(_selectButtonParent);
    }

    /// <summary> 選択ボタンの生成 </summary>
    public Button GenerateSelectButton(string buttonName)
    {
        Button generateButton = _buttonGenerator.GenerateButton(buttonName, _selectButtonSprite);
        generateButton.gameObject.transform.parent = _selectButtonParent;
        return generateButton;
    }

    /// <summary> 選択後の処理 </summary>
    public void OnSelected()
    {
        _buttonGenerator.ReleaseAllButtons();
    }

    /// <summary> 分岐イベントの選択ボタンを生成する </summary>
    private void GenerateBranchEventSelectButton()
    {
        //List<TrainingEventData> trainingEventDataList = _trainingEventDataGenerator.GenerateBranchEvent
        //    (_trainingEventStateMachine.CurrentEventType, _trainingEventStateMachine.CurrentEventData.EventID);

        //foreach (TrainingEventData eventData in trainingEventDataList)
        //{
        //    Button selectButton = _branchEventSelectButtonView.GenerateSelectButton(eventData.EventName);
        //    selectButton.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text = eventData.EventName;
        //    selectButton.onClick.AddListener(async () =>
        //    {
        //        await OnClickSelectBranchButtonEvent(eventData);
        //    });
        //}
    }
}
