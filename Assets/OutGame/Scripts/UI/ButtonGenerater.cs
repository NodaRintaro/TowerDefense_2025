using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボタンの動的生成Class
/// </summary>
public class ButtonGenerater : MonoBehaviour 
{
    protected readonly GameObject _baseButtonObj;

    protected List<GameObject> _buttonPool = new List<GameObject>();

    public ButtonGenerater()
    {
        InitBaseButton(_baseButtonObj);
    }

    public virtual void InitBaseButton(GameObject baseButtonObj)
    {
        // ボタンのGameObjectを作成
        baseButtonObj = new GameObject();

        // RectTransformを追加
        RectTransform rectTransform = _baseButtonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 50);

        // Imageコンポーネントを追加（ボタンの背景）
        Button button = _baseButtonObj.AddComponent<Button>();
        Image image = _baseButtonObj.AddComponent<Image>();
    }

    /// <summary>
    /// Buttonを生成する
    /// </summary>
    public virtual GameObject GenerateButton(Transform parent, string buttonName, Sprite buttonSprite)
    {
        GameObject generatedButtonObj = SpawnButtonObj();
        generatedButtonObj.transform.SetParent(parent, false);

        generatedButtonObj.name = buttonName + "_SelectButton";
        buttonSprite = generatedButtonObj.GetComponent<Image>().sprite;

        return generatedButtonObj;
    }

    public void ReleaseButton(Button button)
    {
        GameObject buttonObj = button.gameObject;
        buttonObj.SetActive(false);
        _buttonPool.Add(buttonObj);
        button.onClick.RemoveAllListeners();
    }

    public GameObject SpawnButtonObj()
    {
        foreach (var button in _buttonPool)
        {
            if (!button.activeSelf)
            {
                GameObject buttonObj = button.gameObject;
                buttonObj.SetActive(true);
                return buttonObj;
            }
        }

        return Instantiate(_baseButtonObj);
    }
}
