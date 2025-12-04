using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ボタンの動的生成Class
/// </summary>
public class SelectButtonCreater
{
    private GameObject _buttonObjPrefab;

    private Dictionary<string, GameObject> _createbuttonDict = new();

    public SelectButtonCreater()
    {
        // ボタンのGameObjectを作成
        _buttonObjPrefab = new GameObject();

        // RectTransformを追加
        RectTransform rectTransform = _buttonObjPrefab.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 50);

        // Imageコンポーネントを追加（ボタンの背景）
        Button button = _buttonObjPrefab.AddComponent<Button>();
        Image image = _buttonObjPrefab.AddComponent<Image>();
    }

    /// <summary>
    /// 生成するButton
    /// </summary>
    public GameObject CreateButton(string buttonName, Sprite buttonSprite)
    {
        _buttonObjPrefab.name = "Button_" + buttonName;

        Image buttonImage = _buttonObjPrefab.GetComponent<Image>();
        buttonImage.sprite = buttonSprite;

        return _buttonObjPrefab;
    }
}
