using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "SupportCardResource", menuName = "ScriptableObject/SupportCardResource")]
public class SupportCardSprite : ScriptableObject
{
    [SerializeField, Header("ID")]
    private uint _cardID = 0;

    [SerializeField,Header("カードのスプライト")]
    private Sprite _cardSprite = null;

    public uint CardID => _cardID;

    public Sprite CardSprite => _cardSprite;
}
