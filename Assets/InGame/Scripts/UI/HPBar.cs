using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Image HpBarObject; //HPバー
    [SerializeField] private Canvas HpBarCanvas; //HPバーのCanvas
    private float MaxHpBar;       //HPバーの最大値
    private float CurrentHpBar;   //Hp1当たりのHPバーの長さ

    public void Init(float maxHp)
    {
        MaxHpBar = HpBarCanvas.GetComponent<RectTransform>().rect.width;
        CurrentHpBar = MaxHpBar / maxHp;
    }

    public void UpdateHp(float hp)
    {
        float hpBarLength = hp * CurrentHpBar;
        Debug.Log($"MaxHpBar:{MaxHpBar}\nhpBarLemgth:{hpBarLength}\nCurrentHpBar:{CurrentHpBar}\nPosition.x:{HpBarObject.rectTransform.position.x}\nhp:{hp}");
        HpBarObject.rectTransform.sizeDelta = new Vector2(hpBarLength, HpBarObject.rectTransform.sizeDelta.y);
        HpBarObject.rectTransform.position = new Vector2((hpBarLength / 2f)/100, HpBarObject.rectTransform.position.y);
    }
}
