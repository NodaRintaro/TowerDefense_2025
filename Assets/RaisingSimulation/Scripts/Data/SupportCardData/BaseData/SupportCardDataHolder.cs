using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SupportCardDataHolder", menuName = "ScriptableObject/SupportCardDataHolder")]
public class SupportCardDataHolder : ScriptableObject
{
    [SerializeField, Header("SupportCardのデータリスト")]
    private List<SupportCardData> _dataList = new();

    public List<SupportCardData> DataList => _dataList;

    public void AddData(SupportCardData cardData)
    {
        _dataList.Add(cardData);
    }
}

