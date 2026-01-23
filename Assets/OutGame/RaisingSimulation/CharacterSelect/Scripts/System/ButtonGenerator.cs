using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// ボタンの動的生成Class
/// </summary>
public class ButtonGenerator : MonoBehaviour
{
    [SerializeField, Header("ボタンにつけるテキストのオブジェクト")] private GameObject _textObj = null;

    private Transform _generateButtonParent;

    //生成するボタンのBaseObject
    private GameObject _baseButtonObj = null;

    //生成したボタンのオブジェクトプール
    private List<Button> _buttonPool = new List<Button>();

    public void Awake()
    {
        if(_baseButtonObj == null)
        {
            InitBaseButtonObj();
        }
    }

    public void OnEnable()
    {
        if(_buttonPool == null)
        {
            _buttonPool = new List<Button>();
        }
    }

    private void InitBaseButtonObj()
    {
        _baseButtonObj = new();
        GameObject textObj = new();

        // RectTransformを追加
        RectTransform rectTransform = _baseButtonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 50);

        // 各種必要なコンポーネントを追加（ボタンの背景）
        CanvasRenderer canvasRenderer = _baseButtonObj.AddComponent<CanvasRenderer>();
        EventTrigger eventTrigger = _baseButtonObj.AddComponent<EventTrigger>();
        Button button = _baseButtonObj.AddComponent<Button>();
        Image image = _baseButtonObj.AddComponent<Image>();
    }

    /// <summary> 新たにButtonを生成 </summary>
    public Button GenerateButton(string buttonName, Sprite buttonSprite)
    {
        GameObject generatedButtonObj = SpawnButtonObj();
        generatedButtonObj.transform.SetParent(_generateButtonParent, false);

        generatedButtonObj.name = buttonName + "_SelectButton";
        generatedButtonObj.GetComponent<Image>().sprite = buttonSprite;
        Button generatedButton = generatedButtonObj.GetComponent<Button>();
        ButtonAnimation.SetupPointerEnterAnimationEvents(generatedButton);
        _buttonPool.Add(generatedButton);

        return generatedButton;
    }

    /// <summary> 生成したButtonを回収 </summary>
    public void ReleaseButton(Button button)
    {
        GameObject buttonObj = button.gameObject;
        ButtonAnimation.RemoveAnimationEvent(button);
        buttonObj.SetActive(false);
        button.onClick.RemoveAllListeners();
    }

    /// <summary> 生成した全てのButtonを回収 </summary>
    public void ReleaseAllButtons()
    {
        if (_buttonPool != null)
        {
            foreach (var button in _buttonPool)
            {
                ReleaseButton(button);
            }
        }
    }

    /// <summary> 生成するButtonの親を設定 </summary>
    public void SetGenerateButtonParent(Transform generateButtonParent)
    {
        _generateButtonParent = generateButtonParent;
    }

    /// <summary> Pool内の使われていないボタンを再利用、なければ新たに生成 </summary>
    private GameObject SpawnButtonObj()
    {
        GameObject buttonObj;
        GameObject textObj;
        foreach (var button in _buttonPool)
        {
            if (!button.gameObject.activeSelf)
            {
                button.interactable = true;
                buttonObj = button.gameObject;
                buttonObj.SetActive(true);
                return buttonObj;
            }
        }
        buttonObj = Instantiate(_baseButtonObj);
        textObj = Instantiate(_textObj);
        textObj.gameObject.transform.parent = buttonObj.transform;

        return buttonObj;
    }
}
