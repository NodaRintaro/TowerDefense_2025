using System;
using UnityEngine;

/// <summary>
/// キャラクターの画像データ保管クラス
/// </summary>
[Serializable]
public class CharacterImageData
{
    [SerializeField]
    private CharacterSprite[] _spriteData;

    [SerializeField]
    private uint _id;

    public CharacterSprite[] SpriteData => _spriteData;
    public uint ID => _id;

    public Sprite GetSprite(CharacterSpriteType spriteType)
    {
        foreach (var spriteData in _spriteData)
        {
            if(spriteType == spriteData.SpriteType)
            {
                return spriteData.SpriteData;
            }
        }
        return null;
    }
}