using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "CharacterResource", menuName = "ScriptableObject/CharacterResource")]
public class CharacterResource : ScriptableObject
{
    [SerializeField, Header("ID")]
    private uint _characterID = 0;

    [SerializeField, Header("キャラクターの画像データ")]
    ChracterSpriteData[] _chracterSpriteData;

    public uint CharacterID => _characterID;

    public ChracterSpriteData[] ChracterSpriteDatas => _chracterSpriteData;

    public Sprite GetCharacterSprite(SpriteType spriteType)
    {
        foreach(var data  in ChracterSpriteDatas)
        {
            if(spriteType == data.CharacterSpriteType)
            {
                return data.CharacterSprite;
            }
        }

        return null;
    }
}

[System.Serializable]
public struct ChracterSpriteData
{
    [SerializeField, Header("登録してあるSpriteのタイプ")] private SpriteType _spriteType;

    [SerializeField, Header("キャラクターの立ち絵")] private Sprite _characterSprite;

    public SpriteType CharacterSpriteType => _spriteType;
    public Sprite CharacterSprite => _characterSprite;
}

public enum SpriteType
{
    [InspectorName("タイプが登録されていません")] None,
    [InspectorName("キャラクターの全体像")] OverAllView,
    [InspectorName("キャラクターのアイコン")] Icon
}


