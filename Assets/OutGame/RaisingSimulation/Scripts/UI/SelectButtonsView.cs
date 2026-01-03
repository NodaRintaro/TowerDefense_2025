using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// ボタンの動的生成Class
/// </summary>
public class SelectButtonsView : MonoBehaviour 
{
    [SerializeField] private Transform _generateButtonParent;

    //生成するボタンのBaseObject
    private GameObject _baseButtonObj = null;

    //生成したボタンのオブジェクトプール
    private List<Button> _buttonPool = new List<Button>();

    public List<Button> ButtonPool => _buttonPool;

    public void Awake()
    {
        InitBaseButtonObj();
    }

    public void OnEnable()
    {
        if(_buttonPool == null)
        {
            _buttonPool = new List<Button>();
        }
    }

    public void OnDisable()
    {
        _buttonPool = null;
    }

    private void InitBaseButtonObj()
    {
        _baseButtonObj = new();

        // RectTransformを追加
        RectTransform rectTransform = _baseButtonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 50);

        // 各種必要なコンポーネントを追加（ボタンの背景）
        CanvasRenderer canvasRenderer = _baseButtonObj.AddComponent<CanvasRenderer>();
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
        _buttonPool.Add(generatedButton);

        return generatedButton;
    }

    /// <summary> 生成したButtonを回収 </summary>
    public void ReleaseButton(Button button)
    {
        GameObject buttonObj = button.gameObject;
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

    /// <summary> Pool内の使われていないボタンを再利用、なければ新たに生成 </summary>
    private GameObject SpawnButtonObj()
    {
        GameObject buttonObj;
        foreach (var button in _buttonPool)
        {
            if (!button.gameObject.activeSelf)
            {
                buttonObj = button.gameObject;
                buttonObj.SetActive(true);
                return buttonObj;
            }
        }
        buttonObj = Instantiate(_baseButtonObj);

        return buttonObj;
    }
}
