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

    public void OnEnable()
    {
        _buttonGenerator = FindFirstObjectByType<ButtonGenerator>();
        _buttonGenerator.SetGenerateButtonParent(_selectButtonParent);
    }

    /// <summary> 選択ボタンの生成 </summary>
    public Button GenerateSelectButton(string buttonName)
    {
        Button generateButton = _buttonGenerator.GenerateButton(buttonName, _selectButtonSprite);
        return generateButton;
    }

    /// <summary> 選択後の処理 </summary>
    public void OnSelected()
    {
        _buttonGenerator.ReleaseAllButtons();
    }
}
